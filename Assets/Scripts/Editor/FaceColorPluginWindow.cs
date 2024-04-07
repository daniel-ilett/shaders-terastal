#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

// Based on: https://forum.unity.com/threads/save-rendertexture-or-texture2d-as-image-file-utility.1325130/
public class FaceColorPluginWindow : EditorWindow
{
    private ObjectField mesh;
    private TextField filepath;
    private Vector2IntField size;
    private Button button;

    [MenuItem("Tools/Convert Mesh to Terastal Texture")]
    public static void ShowWindow()
    {
        var window = GetWindow<FaceColorPluginWindow>();
        window.minSize = new Vector2(300, 100);
        window.titleContent = new GUIContent("Mesh -> Terastal Texture");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        mesh = new ObjectField("Mesh") { objectType = typeof(Mesh) };
        root.Add(mesh);
        filepath = new TextField("Filepath") { value = "Assets/texture.png" };
        root.Add(filepath);
        size = new Vector2IntField("Size") { value = new Vector2Int(1024, 1024) };
        root.Add(size);
        button = new Button(Save) { text = "Save" };
        root.Add(button);
    }

    private void Save()
    {
        if(size.value.x <= 0 || size.value.y <= 0)
        {
            Debug.LogError("Size must be positive.");
        }

        FaceColorPlugin.GenerateColors(filepath.value, size.value.x, size.value.y, mesh.value as Mesh);
    }
}
#endif
