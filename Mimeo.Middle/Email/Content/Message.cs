using System.Collections.Generic;
using Mimeo.Middle.Config;

namespace Mimeo.Middle.Email.Content
{
    public class Message
    {
        public MimeoAppSettings AppSettings { get; set; }
        public ContentImage Logo { get; set; }
        public List<IContentBlock> ContentBlocks { get; set; }

        public Message()
        {
            ContentBlocks = new List<IContentBlock>();
        }
    }
}
