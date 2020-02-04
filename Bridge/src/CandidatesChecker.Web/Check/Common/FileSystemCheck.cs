using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CandidatesChecker.Web.Check.Common
{
    public class FileSystemCheck : IFileSystemCheck, IDisposable
    {
        private const string _unkownAuthor = "unknown";

        private const int _cachingDelay = 10_000;

        private readonly string _directory;

        private readonly CancellationTokenSource _filesCachingCancellation;

        private bool _disposed;

        private CandidateFileData[] _namesInFolder = Array.Empty<CandidateFileData>();

        public FileSystemCheck(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Cannot be empty", nameof(directory));
            }

            _directory = directory;
            _filesCachingCancellation = new CancellationTokenSource();

            _ = StartFilesCaching(_filesCachingCancellation.Token);
        }

        public bool DirectoryContainsFileWithName(string name, out DateTime creationDate, out string author)
        {
            creationDate = DateTime.MinValue;
            author = string.Empty;

            try
            {
                var file = FindMostRecentMatchingFile(name);

                if (file != null)
                {
                    string fullAuthor = file.GetAccessControl().GetOwner(typeof(NTAccount))?.Value ?? _unkownAuthor;
                    creationDate = file.CreationTime;
                    author = Path.GetFileName(fullAuthor);

                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _filesCachingCancellation.Cancel();
            _filesCachingCancellation.Dispose();

            _disposed = true;
        }

        private FileInfo? FindMostRecentMatchingFile(string name)
        {
            name = name.RemoveDiacritics();

            return Array
                .FindAll(_namesInFolder, f => StringContainsName(f.CandidateName, name))
                .Select(f => new FileInfo(f.FileName))
                .OrderByDescending(f => f.CreationTime)
                .FirstOrDefault();
        }

        private bool StringContainsName(string s, string name)
        {
            return name
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                .All(e => Regex.IsMatch(s, @$"\b{Regex.Escape(e)}\b", RegexOptions.IgnoreCase));
        }

        private async Task StartFilesCaching(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _namesInFolder = Directory
                        .GetFiles(_directory, "*", SearchOption.AllDirectories)
                        .Select(f => new CandidateFileData(f))
                        .ToArray();
                }
                catch (Exception)
                {
                }

                await Task.Delay(_cachingDelay, cancellationToken).ConfigureAwait(false);
            }
        }

        ~FileSystemCheck()
        {
            Dispose(false);
        }

        private class CandidateFileData
        {
            public string CandidateName { get; }

            public string FileName { get; }

            public CandidateFileData(string fileName)
            {
                FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
                CandidateName = Path.GetFileNameWithoutExtension(FileName).RemoveDiacritics();
            }
        }
    }
}
