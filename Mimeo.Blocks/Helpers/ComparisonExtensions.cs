using System.Collections.Generic;
using System.Linq;

namespace Mimeo.Blocks.Helpers
{
    public static class ComparisonExtensions
    {
        public static bool SameIgnoringOrder(this List<long> input, List<long> other)
        {
            if (input.Count != other.Count)
            {
                return false;
            }

            var inputOrdered = input.OrderBy(x => x).ToList();
            var otherOrder = input.OrderBy(x => x).ToList();

            for (var index = 0; index < inputOrdered.Count; index++)
            {
                if (inputOrdered[index] != otherOrder[index])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
