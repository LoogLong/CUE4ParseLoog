using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CUE4Parse_Conversion.Textures;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Exports.Texture;
using Newtonsoft.Json;
using SkiaSharp;
using CUE4Parse.UE4.Assets.Exports;
using System.Collections.Concurrent;
using SharpGLTF.Schema2;

namespace CUE4Parse_Conversion.Textures
{
    public class TextureExporter : ExporterBase
    {
        private readonly string _internalFilePath;
        private readonly UTexture2D? _unrealTexture;

        public TextureExporter(ExporterOptions options)
        {
            Options = options;
            _internalFilePath = string.Empty;
            _unrealTexture = null;
        }

        public TextureExporter(UTexture2D? unrealTexture, ExporterOptions options) : this(options)
        {
            if (unrealTexture == null) return;
            _unrealTexture = unrealTexture;
        }

        public override bool TryWriteToDir(DirectoryInfo baseDirectory, List<UObject> ObjectQueue, out string label, out string savedFilePath)
        {
            label = string.Empty;
            savedFilePath = string.Empty;
            if (!baseDirectory.Exists) return false;

            if (_unrealTexture is not UTexture2D t || t.Decode(Options.Platform) is not { } bitmap)
                return false;
            var ext = Options.TextureFormat switch
            {
                ETextureFormat.Png => "png",
                ETextureFormat.Tga => "tga",
                ETextureFormat.Dds => "dds",
                _ => "png"
            };

            var texturePath = FixAndCreatePath(baseDirectory, t.Owner?.Name ?? t.Name, ext);
            using var fs = new FileStream(texturePath, FileMode.Create, FileAccess.Write);
            using var data = bitmap.Encode(Options.TextureFormat, 100);
            using var stream = data.AsStream();
            stream.CopyTo(fs);
            return true;
        }

        public override bool TryWriteToZip(out byte[] zipFile)
        {
            throw new System.NotImplementedException();
        }

        public override void AppendToZip()
        {
            throw new System.NotImplementedException();
        }
    }
}
