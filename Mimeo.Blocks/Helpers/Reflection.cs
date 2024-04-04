namespace Mimeo.Blocks.Helpers
{
    public static class ReflectionHelpers
    {
        public static byte[] GetResource(this object assemblyObjectRef, string resourceName)
        {
            using (var templateStream 
                   = assemblyObjectRef
                       .GetType()
                       .Assembly
                       .GetManifestResourceStream(resourceName))
            {
                if (templateStream != null)
                {
                    var ba = new byte[templateStream.Length];
                    templateStream.Read(ba, 0, ba.Length);
                    return ba;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

