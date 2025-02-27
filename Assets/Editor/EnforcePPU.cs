using UnityEngine;
using UnityEditor;

public class EnforcePPU : EditorWindow {
    private int targetPPU = 100;

    [MenuItem("Tools/Enforce PPU")]
    public static void ShowWindow() {
        GetWindow<EnforcePPU>("Enforce PPU");
    }

    void OnGUI() {
        targetPPU = EditorGUILayout.IntField("Target PPU", targetPPU);

        if (GUILayout.Button("Apply to All Sprites")) {
            ApplyPPUToAllSprites(targetPPU);
        }
    }

    private void ApplyPPUToAllSprites(int ppu) {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D");

        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (textureImporter != null && textureImporter.spritePixelsPerUnit != ppu) {
                textureImporter.spritePixelsPerUnit = ppu;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log($"Applied PPU = {ppu} to all sprites.");
    }
}
