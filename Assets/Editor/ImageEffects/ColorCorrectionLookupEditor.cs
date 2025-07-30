using System;
using UnityEditor;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [CustomEditor(typeof(ColorCorrectionLookup))]
    class ColorCorrectionLookupEditor : Editor
    {
        SerializedObject serObj;
        private Texture2D tempClutTex2D;

        void OnEnable()
        {
            serObj = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            EditorGUILayout.LabelField("Converts textures into color lookup volumes (for grading)", EditorStyles.miniLabel);

            tempClutTex2D = EditorGUILayout.ObjectField("Based on", tempClutTex2D, typeof(Texture2D), false) as Texture2D;

            if (tempClutTex2D == null)
            {
                var texPath = ((ColorCorrectionLookup)target).basedOnTempTex;
                if (!string.IsNullOrEmpty(texPath))
                {
                    tempClutTex2D = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
                }
            }

            if (tempClutTex2D)
            {
                var path = AssetDatabase.GetAssetPath(tempClutTex2D);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;

                bool needsImport = false;

                if (importer != null)
                {
                    if (!importer.isReadable || importer.mipmapEnabled)
                    {
                        needsImport = true;
                        importer.isReadable = true;
                        importer.mipmapEnabled = false;
                        importer.textureCompression = TextureImporterCompression.Uncompressed;
                        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    }
                }

                if ((target as ColorCorrectionLookup).ValidDimensions(tempClutTex2D))
                {
                    if (GUILayout.Button("Convert and Apply"))
                    {
                        (target as ColorCorrectionLookup).Convert(tempClutTex2D, path);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Invalid texture dimensions! Expected e.g. 256x16.", MessageType.Warning);
                }
            }

            if (!string.IsNullOrEmpty(((ColorCorrectionLookup)target).basedOnTempTex))
            {
                EditorGUILayout.HelpBox("Using: " + ((ColorCorrectionLookup)target).basedOnTempTex, MessageType.Info);
                var previewTex = AssetDatabase.LoadAssetAtPath<Texture2D>(((ColorCorrectionLookup)target).basedOnTempTex);
                if (previewTex)
                {
                    Rect r = GUILayoutUtility.GetAspectRect((float)previewTex.width / previewTex.height);
                    GUI.DrawTexture(r, previewTex, ScaleMode.ScaleToFit);
                }
            }

            serObj.ApplyModifiedProperties();
        }
    }
}