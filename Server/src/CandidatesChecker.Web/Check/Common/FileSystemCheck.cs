using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CandidatesChecker.Web.Check.Common
{
    public class FileSystemCheck : IFileSystemCheck
    {
        public bool DirectoryContainsFileWithName(string name, out DateTime creationDate, out string author)
        {
            creationDate = DateTime.MinValue;
            author = string.Empty;

            try
            {
                string? fileName = Directory
                    .GetFiles(@"C:\Users\jonathan\source\repos\CandidatesChecker\Server\fake_documents", "*", SearchOption.AllDirectories)
                    .FirstOrDefault(fn => FileNameContainsName(name, Path.GetFileNameWithoutExtension(fn) ?? string.Empty));

                var file = new FileInfo(fileName);

                creationDate = file.CreationTime;
                author = "unkown";

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool FileNameContainsName(string name, string fileName)
        {
            return name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .All(e => Regex.IsMatch(fileName, @$"\b{Regex.Escape(e)}\b", RegexOptions.IgnoreCase));
        }
    }
}
