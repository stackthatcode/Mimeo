using Mimeo.Communications.Html.Content.Fragments;
using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Blocks
{
    public class SingleBlock : IContentBlock
    {
        public List<IFragment> Content { get; set; }
        public List<ImageEnvelope> ImageRefs => Content.ImageRefs();

        public StyleBag Style { get; set; } = new StyleBag(); //{ { "text-align", "center" }, };


        public SingleBlock(List<IFragment> content, StyleBag styleBag = null)
        {
            Content = content;
            Style.MaybeSetRange(styleBag);

        }

        public SingleBlock(IFragment contentFragment, StyleBag styleBag = null)
            : this(new List<IFragment>() { contentFragment }, styleBag)
        {
        }

        public SingleBlock() : this(new List<IFragment>())
        {
        }
    }
}

