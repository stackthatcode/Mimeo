using Mimeo.Blocks.CommonDtos;

namespace Mimeo.Services.File
{
    // We're stubbing this out to run on local file system first
    //
    public class MockFileService
    {
        private readonly string _baseDirectory;

        public MockFileService(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }

        // We'll assume we're using namespace-ish file organization on Azure Blob once we
        // .. start putting tings up theree
        //
        public string[] GetFiles()
        {
            return System.IO.Directory.GetFiles(_baseDirectory);
        }

        public InMemoryFile GetFile(string fileName)
        {
            var path = System.IO.Path.Combine(_baseDirectory, fileName);
            var data = System.IO.File.ReadAllBytes(path);
            return new InMemoryFile(data, fileName);
        }
    }
}
