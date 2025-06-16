using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse_Conversion.Materials;

namespace CUE4Parse_Conversion.Meshes
{
    public class Mesh : ExporterBase
    {
        public readonly string FileName;
        public readonly byte[] FileData;
        public readonly List<UMaterialInterface> MaterialObjects;

        public Mesh(string fileName, byte[] fileData, List<UMaterialInterface> materials)
        {
            FileName = fileName;
            FileData = fileData;
            MaterialObjects = materials;
        }

        private readonly object _material = new ();
        public override bool TryWriteToDir(DirectoryInfo baseDirectory, List<UObject> ObjectQueue, out string label, out string savedFilePath)
        {
            label = string.Empty;
            savedFilePath = string.Empty;
            if (FileData.Length <= 0) return false;

            foreach (var obj in MaterialObjects)
            {
                ObjectQueue.Add(obj);
            }
            
            savedFilePath = FixAndCreatePath(baseDirectory, FileName);
            File.WriteAllBytesAsync(savedFilePath, FileData);
            label = Path.GetFileName(savedFilePath);
            return File.Exists(savedFilePath);
        }

        public override bool TryWriteToZip(out byte[] zipFile)
        {
            throw new NotImplementedException();
        }

        public override void AppendToZip()
        {
            throw new NotImplementedException();
        }
    }
}
