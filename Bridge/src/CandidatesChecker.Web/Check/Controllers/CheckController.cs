using CandidatesChecker.Web.Check.Common;
using CandidatesChecker.Web.Check.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CandidatesChecker.Web.Check.Controllers
{
    [Route("api/check")]
    public class CheckController : ControllerBase
    {
        private readonly IFileSystemCheck _fileSystemCheck;

        private readonly ILogger<CheckController> _logger;

        public CheckController(IFileSystemCheck fileSystemCheck, ILogger<CheckController> logger)
        {
            _fileSystemCheck = fileSystemCheck ?? throw new ArgumentNullException(nameof(fileSystemCheck));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{rawName}")]
        public ActionResult<CheckResult> CheckCandidate(string rawName)
        {
            if (!string.IsNullOrWhiteSpace(rawName))
            {
                _logger.LogInformation($"Checking for '{rawName}'...");

                if (_fileSystemCheck.DirectoryContainsFileWithName(rawName, out var date, out string author))
                {
                    _logger.LogInformation($"'{rawName}' found! Your past self did a good job ;)");

                    return new CheckResult(date, author);
                }

                _logger.LogInformation($"'{rawName}' not found.");
            }

            return new CheckResult();
        }
    }
}
