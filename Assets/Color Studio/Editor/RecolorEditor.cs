/* Color Studio by Ramiro Oliva (Kronnect)   /
/  Premium assets for Unity on kronnect.com */


using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ColorStudio {

    [CustomEditor(typeof(Recolor))]
    public class RecolorEditor : Editor {

        SerializedProperty palette, applyPalette, mode, materialIndex, colorMatch, threshold, showOriginalTexture, colorOperations;
        SerializedProperty enableColorAdjustments, colorAdjustments, lutProp;
        Texture2D originalTexture;
        List<Color> originalColors;

        private void OnEnable() {
            palette = serializedObject.FindProperty("palette");
            applyPalette = serializedObject.FindProperty("applyPalette");
            mode = serializedObject.FindProperty("mode");
            materialIndex = serializedObject.FindProperty("materialIndex");
            colorMatch = serializedObject.FindProperty("colorMatch");
            threshold = serializedObject.FindProperty("threshold");
            colorOperations = serializedObject.FindProperty("_colorOperations");
            showOriginalTexture = serializedObject.FindProperty("showOriginalTexture");
            enableColorAdjustments = serializedObject.FindProperty("enableColorAdjustments");
            colorAdjustments = serializedObject.FindProperty("colorAdjustments");
            lutProp = colorAdjustments.FindPropertyRelative("LUT");
        }

        public override void OnInspectorGUI() {

            bool requireRefresh = false;

            Recolor rc = (Recolor)target;
            if (rc.GetComponent<Renderer>() == null) {
                EditorGUILayout.HelpBox("Recolor script requires an GameObject with a MeshRenderer or SpriteRenderer component.", MessageType.Warning);
                return;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(this.palette);

            CSPalette palette = (CSPalette)this.palette.objectReferenceValue;
            if (palette != null) {

                if (palette.material == null || palette.material.GetColorArray("_Colors") == null) {
                    palette.UpdateMaterial();
                }

                EditorGUILayout.BeginVertical(GUI.skin.box);

                Rect space = EditorGUILayout.BeginVertical();
                GUILayout.Space(64);
                EditorGUILayout.EndVertical();

                palette.material.SetVector("_CursorPos", Vector3.left);
                EditorGUI.DrawPreviewTexture(space, Texture2D.whiteTexture, palette.material);

                if (GUILayout.Button("Open in Color Studio")) {
                    CSWindow cs = CSWindow.ShowWindow();
                    cs.LoadPalette(palette);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.PropertyField(applyPalette);
            } else {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Open Color Studio")) {
                    CSWindow.ShowWindow();
                }
                if (GUILayout.Button("Help")) {
                    EditorUtility.DisplayDialog("Quick Help", "This Recolor script changes colors of the gameobject or sprite at runtime.\n\nIf you assign a palette created with Color Studio, Recolor will transform the colors of the original texture to the nearest colors of the palette.\n\nYou can also specify custom color operations, like preserving or replacing individual colors from the original texture.", "Ok");
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(mode, new GUIContent("Recolor Mode"));
            EditorGUILayout.PropertyField(colorMatch);
            EditorGUILayout.PropertyField(threshold, new GUIContent("Color Threshold"));
            EditorGUILayout.PropertyField(materialIndex);

            if (mode.intValue != (int)RecolorMode.MainColorOnly) {

                if (originalTexture == null) {
                    originalTexture = rc.GetOriginalTexture();
                    if (originalTexture != null) {
                        originalTexture = Instantiate<Texture2D>(originalTexture);
                        originalTexture.filterMode = FilterMode.Point;
                    }
                    originalColors = rc.GetOriginalUniqueColors();
                }

                EditorGUILayout.PropertyField(showOriginalTexture);
                if (showOriginalTexture.boolValue) {
                    if (originalTexture != null) {
                        EditorGUILayout.BeginVertical(GUI.skin.box);

                        Rect space = EditorGUILayout.BeginVertical();
                        GUILayout.Space(128);
                        EditorGUILayout.EndVertical();

                        EditorGUI.DrawPreviewTexture(space, originalTexture);
                        EditorGUILayout.EndVertical();

                    }
                }
            }

            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(colorOperations, new GUIContent("Per Color Operations"), true);

            if (originalColors != null && originalColors.Count < 64 && GUILayout.Button("Add All Texture Colors")) {
                colorOperations.isExpanded = true;
                List<ColorEntry> cc = new List<ColorEntry>();
                if (rc.colorOperations != null) {
                    for (int k = 0; k < rc.colorOperations.Length; k++) {
                        int index = originalColors.IndexOf(rc.colorOperations[k].color);
                        if (index >= 0) {
                            originalColors.RemoveAt(index);
                        }
                        cc.Add(rc.colorOperations[k]);
                    }
                }
                for (int k = 0; k < originalColors.Count; k++) {
                    ColorEntry ce = new ColorEntry { color = originalColors[k], operation = ColorOperation.Preserve, replaceColor = originalColors[k] };
                    cc.Add(ce);
                }
                rc.colorOperations = cc.ToArray();
                EditorUtility.SetDirty(rc);
                serializedObject.Update();
                requireRefresh = true;
            }

            if (mode.intValue != (int)RecolorMode.MainColorOnly) {
                if (!rc.isSprite && originalTexture != null && GUILayout.Button("Add Main Texture Colors")) {
                    colorOperations.isExpanded = true;
                    List<ColorEntry> cc = new List<ColorEntry>();
                    List<Color> mainColors = rc.GetOriginalTextureMainColors();
                    if (mainColors != null) {
                        if (rc.colorOperations != null) {
                            for (int k = 0; k < rc.colorOperations.Length; k++) {
                                int index = mainColors.IndexOf(rc.colorOperations[k].color);
                                if (index >= 0) {
                                    mainColors.RemoveAt(index);
                                }
                                cc.Add(rc.colorOperations[k]);
                            }
                        }

                        for (int k = 0; k < mainColors.Count; k++) {
                            ColorEntry ce = new ColorEntry { color = mainColors[k], operation = ColorOperation.Preserve, replaceColor = mainColors[k] };
                            cc.Add(ce);
                        }
                        rc.colorOperations = cc.ToArray();
                        EditorUtility.SetDirty(rc);
                        serializedObject.Update();
                        requireRefresh = true;

                    }
                }
                if (!rc.isSprite && mode.intValue == (int)RecolorMode.VertexColors && GUILayout.Button("Add Vertex Colors")) {
                    colorOperations.isExpanded = true;
                    List<ColorEntry> cc = new List<ColorEntry>();
                    List<Color> mainColors = rc.GetOriginalVertexColors();
                    if (rc.colorOperations != null) {
                        for (int k = 0; k < rc.colorOperations.Length; k++) {
                            int index = mainColors.IndexOf(rc.colorOperations[k].color);
                            if (index >= 0) {
                                mainColors.RemoveAt(index);
                            }
                            cc.Add(rc.colorOperations[k]);
                        }
                    }

                    for (int k = 0; k < mainColors.Count; k++) {
                        ColorEntry ce = new ColorEntry { color = mainColors[k], operation = ColorOperation.Preserve, replaceColor = mainColors[k] };
                        cc.Add(ce);
                    }
                    rc.colorOperations = cc.ToArray();
                    EditorUtility.SetDirty(rc);
                    serializedObject.Update();
                    requireRefresh = true;

                }
            }

            // Color adjustments
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(enableColorAdjustments, new GUIContent("Color Correction"), true);
            if (enableColorAdjustments.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(colorAdjustments, true);
                EditorGUI.indentLevel--;
            }

            CheckLUTSettings((Texture2D)lutProp.objectReferenceValue);

            if (rc.enabled) {
                if (GUILayout.Button("Refresh")) {
                    rc.Refresh();
                }
            }

            if (serializedObject.ApplyModifiedProperties() || rc.dirty || requireRefresh) {

                rc.dirty = false;
                if (rc.enabled) {
                    rc.Refresh();
                }
            }
        }

        public void CheckLUTSettings(Texture2D tex) {
            if (Application.isPlaying || tex == null)
                return;
            string path = AssetDatabase.GetAssetPath(tex);
            if (string.IsNullOrEmpty(path))
                return;
            TextureImporter imp = (TextureImporter)AssetImporter.GetAtPath(path) as TextureImporter;
            if (imp == null)
                return;
            if (!imp.isReadable || imp.textureType != TextureImporterType.Default || imp.sRGBTexture || imp.mipmapEnabled || imp.textureCompression != TextureImporterCompression.Uncompressed || imp.wrapMode != TextureWrapMode.Clamp || imp.filterMode != FilterMode.Bilinear) {
                EditorGUILayout.HelpBox("Texture has invalid import settings.", MessageType.Warning);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Fix Texture Import Settings", GUILayout.Width(200))) {
                    imp.isReadable = true;
                    imp.textureType = TextureImporterType.Default;
                    imp.sRGBTexture = false;
                    imp.mipmapEnabled = false;
                    imp.textureCompression = TextureImporterCompression.Uncompressed;
                    imp.wrapMode = TextureWrapMode.Clamp;
                    imp.filterMode = FilterMode.Bilinear;
                    imp.anisoLevel = 0;
                    imp.SaveAndReimport();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }


    }
}

