using System;

namespace CandidatesChecker.Web.Check.Common
{
    public interface IFileSystemCheck
    {
        bool DirectoryContainsFileWithName(string name, out DateTime creationDate, out string author);
    }
}