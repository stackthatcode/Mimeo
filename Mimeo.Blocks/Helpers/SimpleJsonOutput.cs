using Microsoft.AspNetCore.Mvc;

namespace Mimeo.Blocks.Helpers
{
    public static class SimpleJsonOutput
    {
        public static JsonResult Succeed(object data = null)
        {
            return new JsonResult(new
            {
                Success = true, Data = data,
            });
        }

        public static JsonResult Fail(string message = "")
        {
            return new JsonResult(new
            {
                Success = false, Data = (object)null, Message = message
            });
        }
    }
}

