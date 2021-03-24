using UnityEngine;
using UnityEditor;

namespace ColorStudio {

    public static class TextureExtensions {

        public static void EnsureTextureIsReadable(this Texture texture) {
            if (Application.isPlaying) return;
            string path = AssetDatabase.GetAssetPath(texture);
            if (!string.IsNullOrEmpty(path)) {
                TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                if (imp != null && !imp.isReadable) {
                    imp.isReadable = true;
                    imp.SaveAndReimport();
                }
            }
        }

        public static void EnsureTextureCanBeEdited(this Texture texture) {
            if (Application.isPlaying) return;
            string path = AssetDatabase.GetAssetPath(texture);
            if (!string.IsNullOrEmpty(path)) {
                TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
                if (imp != null && (!imp.isReadable || imp.textureCompression != TextureImporterCompression.Uncompressed)) {
                    imp.isReadable = true;
                    imp.textureCompression = TextureImporterCompression.Uncompressed;
                    imp.SaveAndReimport();
                }
            }
        }

    }
}