using System;
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

        private string[] _files = Array.Empty<string>();

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
                string? fileName = Array.Find(_files, fn => FileNameContainsName(name, Path.GetFileNameWithoutExtension(fn) ?? string.Empty));

                var file = new FileInfo(fileName);

                creationDate = file.CreationTime;
                string fullAuthor = file.GetAccessControl().GetOwner(typeof(NTAccount))?.Value ?? _unkownAuthor;
                author = Path.GetFileName(fullAuthor);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        private bool FileNameContainsName(string name, string fileName)
        {
            return name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .All(e => Regex.IsMatch(fileName, @$"\b{Regex.Escape(e)}\b", RegexOptions.IgnoreCase));
        }

        private async Task StartFilesCaching(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _files = Directory.GetFiles(_directory, "*", SearchOption.AllDirectories);

                await Task.Delay(_cachingDelay, cancellationToken).ConfigureAwait(false);
            }
        }

        ~FileSystemCheck()
        {
            Dispose(false);
        }
    }
}
