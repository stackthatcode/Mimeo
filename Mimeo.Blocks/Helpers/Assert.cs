using System;

namespace Mimeo.Blocks.Helpers
{
    public static class Assert
    {
        public static void IsTrue(Func<bool> condition, string failMessage)
        {
            if (!condition())
            {
                throw new Exception(failMessage);
            }
        }

        public static void IsTrue(bool condition, string failMessage)
        {
            if (!condition)
            {
                throw new Exception(failMessage);
            }
        }

        public static void IsFalse(bool condition, string failMessage)
        {
            if (condition)
            {
                throw new Exception(failMessage);
            }
        }
    }
}
