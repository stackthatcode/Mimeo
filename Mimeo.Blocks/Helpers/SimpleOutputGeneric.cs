using System;
using System.Collections.Generic;

namespace Mimeo.Blocks.Helpers
{
    public class SimpleOutput<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Messages { get; set; }

        public SimpleOutput()
        {
            Messages = new List<string>();
        }

        public SimpleOutput<T> Fail(string message = null)
        {
            if (message != null)
            {
                Messages.Add(message);
            }
            Success = false;

            return this;
        }

        public SimpleOutput<T> Merge(SimpleOutput<T> input)
        {
            Messages.AddRange(input.Messages);
            Success = this.Success && input.Success;
            return this;
        }


        public static SimpleOutput<T> Succeeded(T data, string message = "")
        {
            return new SimpleOutput<T>()
            {
                Success = true,
                Data = data,
                Messages = new List<string>() { message }
            };
        }

        public static SimpleOutput<T> Failed(string message = "")
        {
            return new SimpleOutput<T>()
            {
                Success = false,
                Messages = new List<string>() { message }
            };
        }

        public static SimpleOutput<T> Failed(List<string> messages)
        {
            return new SimpleOutput<T>()
            {
                Success = false,
                Messages = messages,
            };
        }

    }
}
