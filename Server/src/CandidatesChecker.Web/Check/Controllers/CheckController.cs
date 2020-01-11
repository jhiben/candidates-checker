using CandidatesChecker.Web.Check.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Web;

namespace CandidatesChecker.Web.Check.Controllers
{
    [Route("api/check")]
    public class CheckController : ControllerBase
    {
        [HttpGet("{rawName}")]
        public ActionResult<CheckResult> CheckCandidate(string rawName)
        {
            rawName = HttpUtility.UrlDecode(rawName);

            return !string.IsNullOrWhiteSpace(rawName) ? CheckNameInFiles(rawName) : new CheckResult();
        }

        private CheckResult CheckNameInFiles(string name)
        {
            try
            {
                string? fileName = Directory
                    .GetFiles(@"C:\Users\jonathan\source\repos\CandidatesChecker\Server\fake_documents", "*", SearchOption.AllDirectories)
                    .FirstOrDefault(fn => FileNameContainsName(name, Path.GetFileNameWithoutExtension(fn) ?? string.Empty));

                var file = new FileInfo(fileName);

                return new CheckResult(file.CreationTime, "unknown");
            }
            catch (Exception)
            {
                return new CheckResult();
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
