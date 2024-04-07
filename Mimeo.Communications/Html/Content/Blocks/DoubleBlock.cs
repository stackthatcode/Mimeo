using Mimeo.Communications.Html.Content.Fragments;
using Mimeo.Communications.Html.Content.Images;


namespace Mimeo.Communications.Html.Content.Blocks
{
    public class DoubleBlock : IContentBlock
    {
        public List<IFragment> Content1 { get; set; }
        public List<IFragment> Content2 { get; set; }

        public List<ImageEnvelope> ImageRefs
        {
            get
            {
                var imageRefs1 = Content1.ImageRefs();
                var imageRefs2 = Content2.ImageRefs();
                var output = new List<ImageEnvelope>();
                output.AddRange(imageRefs1);
                output.AddRange(imageRefs2);
                return output.DistinctBy(x => x.UniqueId).ToList();
            }
        }
        
        public StyleBag Style { get; set; } = new StyleBag();   // { { "text-align", "center" }, };


        public DoubleBlock(
                List<IFragment> content1, List<IFragment> content2, StyleBag styleBag = null)
        {
            Content1 = content1;
            Content2 = content2;
            Style.MaybeSetRange(styleBag);
        }

        public DoubleBlock(
            IFragment contentFragment1, IFragment contentFragment2,
            StyleBag styleBag = null)
            : this(
                new List<IFragment> { contentFragment1 },
                new List<IFragment> { contentFragment2 },
                styleBag)
        {
        }

        // *** NOTE - Indeed, I'll create those overloads as soon as time permits
    }
}

