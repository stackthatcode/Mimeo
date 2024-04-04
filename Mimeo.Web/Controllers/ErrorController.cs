using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mimeo.Blocks.Logging;
using Mimeo.Web.Models;

namespace Mimeo.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly MimeoLogger _logger;

        public ErrorController(MimeoLogger logger)
        {
            _logger = logger;
        }

        public IActionResult Http500()
        {
            return Dispatch(500);
        }


        //
        // TODO - confirm that this Activity ID is the same as the Serilog ID
        //
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        //


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Dispatch([Bind(Prefix = "id")] int statusCode = 0)
        {
            var description = StatusDescription(statusCode);

            var model = new ErrorViewModel();
            model.Title = description;
            model.Message = "(blank message for now)";
            model.StatusCode = statusCode;
            model.RequestId = CurrentRequestId();

            if (RequiresJsonResponse())
            {
                return new JsonResult(model);
            }
            else
            {
                return View(model);
            }
        }

        public string StatusDescription(int statusCode = 0)
        {
            if (statusCode == 0)
            {
                return $"(Undetermined Error - Empty Status Code)";
            }
            try
            {
                return ReasonPhrases.GetReasonPhrase(statusCode);
            }
            catch
            {
                return $"(Undetermined Error - Status Code: {statusCode})";
            }
        }

        public string CurrentRequestId()
        {
            return HttpContext.TraceIdentifier;
        }

        public bool RequiresJsonResponse()
        {
            foreach (var header in HttpContext.Request.Headers)
            {
                _logger.Info(header.Value);
            }

            var matchText = "XMLHTTPREQUEST";
            return HttpContext
                .Request
                .Headers
                .Any(t => t.Value.Any(y => y.ToUpper() == matchText));
        }
    }
}

