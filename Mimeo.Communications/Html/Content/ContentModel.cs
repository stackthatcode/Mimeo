using Mimeo.Communications.Html.Content.Blocks;
using Mimeo.Communications.Html.Content.Images;


namespace Mimeo.Communications.Html.Content
{
    public class ContentModel
    {
        public ExtendedProperties ExtendedProperties { get; set; } = new();
        public SingleBlock Header { get; set; } = new SingleBlock();

        public List<IContentBlock> ContentBlocks { get; set; } = new();
        public SingleBlock Footer { get; set; } = new SingleBlock();

        
        public List<ImageEnvelope> ImageReferences
            => ContentBlocks
                .SelectMany(x => x.ImageRefs)
                .ToList()
                .Merge(Header.ImageRefs)
                .Merge(Footer.ImageRefs);

        public ContentModel AddContent(IContentBlock contentBlock)
        {
            ContentBlocks.Add(contentBlock);
            return this;
        }

        public ContentModel AddContent(List<IContentBlock> contentBlocks)
        {
            ContentBlocks.AddRange(contentBlocks);
            return this;
        }

        public ContentModel ForceImageTransferMedium(ImageTransferMedium input)
        {
            this.ImageReferences.ForEach(x => x.TransferMedium = input);
            return this;
        }
    }
}

