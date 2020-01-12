using CandidatesChecker.Web.Check.Common;
using CandidatesChecker.Web.Check.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;

namespace CandidatesChecker.Web.Check.Controllers
{
    [Route("api/check")]
    public class CheckController : ControllerBase
    {
        private readonly IFileSystemCheck _fileSystemCheck;

        public CheckController(IFileSystemCheck fileSystemCheck)
        {
            _fileSystemCheck = fileSystemCheck ?? throw new ArgumentNullException(nameof(fileSystemCheck));
        }

        [HttpGet("{rawName}")]
        public ActionResult<CheckResult> CheckCandidate(string rawName)
        {
            rawName = HttpUtility.UrlDecode(rawName);

            if (!string.IsNullOrWhiteSpace(rawName))
            {
                if (_fileSystemCheck.DirectoryContainsFileWithName(rawName, out var date, out string author))
                {
                    return new CheckResult(date, author);
                }
            }

            return new CheckResult();
        }
    }
}
