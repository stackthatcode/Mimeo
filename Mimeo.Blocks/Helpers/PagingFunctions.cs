namespace Mimeo.Blocks.Helpers
{
    public class PagingFunctions
    {

        public static int NumberOfPages(int page_size, int count)
        {
            return (count + page_size - 1) / page_size;
        }

    }
}
