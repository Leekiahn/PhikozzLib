using UnityEditor;
using UnityEngine;

namespace PhikozzLib.Editor
{
    public class BootstrapConfigEditorWindow : BaseEditorWindow
    {
        private const string ResourcesFolderPath = "Assets/Resources";
        private const string ResourceAssetPath = ResourcesFolderPath + "/BootstrapConfig.asset";

        private BootstrapConfig _config;

        [MenuItem("PhikozzLib/Bootstrap Config Editor")]
        private static void OpenWindow()
        {
            Open<BootstrapConfigEditorWindow>("Bootstrap Config");
        }

        protected override void DrawGUI()
        {
            TitleLabel("Bootstrap Config Editor");
            Space();

            _config = ObjectField("Config", _config, false);

            Space();

            if (_config == null)
            {
                Warning("BootstrapConfig 에셋을 지정하세요.");
                return;
            }

            var so = new SerializedObject(_config);
            var managersProperty = so.FindProperty("_managers");

            so.Update();
            EditorGUILayout.PropertyField(managersProperty, true);
            so.ApplyModifiedProperties();

            Space();

            if (Button("Place In Resources"))
            {
                PlaceInResources(_config);
            }

            Label("Target Path", ResourceAssetPath);
        }

        private static void PlaceInResources(BootstrapConfig source)
        {
            if (source == null)
            {
                Debug.LogError("BootstrapConfig is null.");
                return;
            }

            EnsureFolder(ResourcesFolderPath);

            var target = AssetDatabase.LoadAssetAtPath<BootstrapConfig>(ResourceAssetPath);

            if (target == null)
            {
                target = CreateInstance<BootstrapConfig>();
                AssetDatabase.CreateAsset(target, ResourceAssetPath);
            }

            EditorUtility.CopySerialized(source, target);
            EditorUtility.SetDirty(target);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);

            Debug.Log($"BootstrapConfig saved to {ResourceAssetPath}");
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            int index = path.LastIndexOf('/');
            string parent = path.Substring(0, index);
            string folderName = path.Substring(index + 1);

            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}