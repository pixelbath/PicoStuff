using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PicoStuff
{
    public class FileChunk
    {
        public string FilePath { get; set; }
        public int Size { get; set; }
        public byte[] Bytes { get; set; }

        public void WriteToDisk() {
            if (!Directory.Exists(Path.GetDirectoryName(this.FilePath))) {
                // try because GetDirectoryName() isn't perfect
                try {
                    Directory.CreateDirectory(Path.GetDirectoryName(this.FilePath));
                } catch {

                }
            }
            File.WriteAllBytes(this.FilePath, this.Bytes);
        }
    }
}
