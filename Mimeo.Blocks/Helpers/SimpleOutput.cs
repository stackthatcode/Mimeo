using System.Collections.Generic;

namespace Mimeo.Blocks.Helpers
{
    public class SimpleOutput
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public List<string> Messages { get; set; }


        public SimpleOutput Fail(string message = null)
        {
            if (message != null)
            {
                Messages.Add(message);
            }

            Success = false;
            return this;
        }

        public SimpleOutput Merge(SimpleOutput input)
        {
            Messages.AddRange(input.Messages);
            Success = this.Success && input.Success;
            return this;
        }

        public SimpleOutput()
        {
            Messages = new List<string>();
        }

        

        public static SimpleOutput Succeeded(object data = null, string message = null)
        {
            return new SimpleOutput()
            {
                Success = true,
                Data = data,
                Messages = message != null ? new List<string>() { message } : new List<string>()
            };
        }

        public static SimpleOutput Failure(string message = "")
        {
            return new SimpleOutput()
            {
                Success = false,
                Messages = new List<string>() { message }
            };
        }

        public static SimpleOutput Failure(List<string> messages)
        {
            return new SimpleOutput()
            {
                Success = false,
                Messages = messages,
            };
        }

    }
}
