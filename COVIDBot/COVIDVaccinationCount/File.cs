using System.Collections.Generic;
using System.IO;

namespace COVIDVaccinationCount
{
    class File
    {
        private string path;
        public File(string path)
        {
            this.path = path;
        }

        public void Update(string text)
        {
            using StreamWriter writer = System.IO.File.CreateText(this.path);
            writer.WriteLine(text);
        }

        public IEnumerable<string> ReadLines()
        {
            return System.IO.File.ReadLines(this.path);
        }
    }
}
