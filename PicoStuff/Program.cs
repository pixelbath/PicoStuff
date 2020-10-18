using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PicoStuff
{
    class Program
    {
        static string datPath = @"C:\Program Files (x86)\PICO-8\pico8.dat";
        static string outPath = @"C:\temp\picodat";

        static void Main(string[] args) {
            UnpackPod(datPath, null, true);
        }

        static void UnpackPod(string path, string subfolder = null, bool recurse = true) {
            Console.WriteLine($"Opening {path}");

            var fileBytes = File.ReadAllBytes(path);

            for (int i = 0; i < fileBytes.Length; i++) {
                if (fileBytes[i] == 'C') {
                    if (fileBytes[i + 1] == 'P' && fileBytes[i + 2] == 'O' && fileBytes[i + 3] == 'D') {
                        int chunkLength = BitConverter.ToInt32(fileBytes, i + 8);

                        Console.WriteLine($"CPOD @ {i:X4}, length={chunkLength} ({chunkLength:X4})");
                        i += chunkLength;
                    }
                    if (fileBytes[i + 1] == 'F' && fileBytes[i + 2] == 'I' && fileBytes[i + 3] == 'L') {
                        int chunkLength = BitConverter.ToInt32(fileBytes, i + 8);
                        string filename = Encoding.UTF8.GetString(fileBytes, i + 12, 64).Replace("\0", string.Empty).Trim();

                        // thanks zep
                        if (filename == ".") continue;

                        // folder structure eh
                        if (chunkLength == 0) {
                            if (!Directory.Exists(filename)) {
                                Directory.CreateDirectory(filename);
                                continue;
                            }
                        }

                        FileChunk fileChunk = new FileChunk() {
                            FilePath = subfolder != null ? Path.Combine(outPath, subfolder, filename) : Path.Combine(outPath, filename),
                            Size = chunkLength,
                            Bytes = fileBytes.Skip(i + 76).Take(Convert.ToInt32(chunkLength)).ToArray(),
                        };

                        Console.Write($"CFIL @ {i:X4}, length={fileChunk.Size} ({fileChunk.Size:X4}): writing {fileChunk.FilePath}...");
                        fileChunk.WriteToDisk();
                        Console.WriteLine("DONE");

                        if (recurse && Path.GetExtension(fileChunk.FilePath) == ".pod") {
                            UnpackPod(fileChunk.FilePath, Path.GetFileNameWithoutExtension(fileChunk.FilePath), recurse);
                        }

                        i += fileChunk.Size;
                    }
                }
            }
        }
    }
}
