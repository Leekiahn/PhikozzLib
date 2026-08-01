#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace PhikozzLib.Editor
{
    public abstract class BaseEditorWindow : EditorWindow
    {
        // ─────────────────────────────────────────────
        // Window
        // ─────────────────────────────────────────────

        protected virtual void DrawGUI() { }

        private void OnGUI() => DrawGUI();

        protected void RepaintWindow() => Repaint();
        protected void FocusWindow() => Focus();
        protected void RemoveFocusWindow()
        {
            GUI.FocusControl(null);
            EditorGUIUtility.editingTextField = false;
        }
        protected void CloseWindow() => Close();
        protected void ShowWindow() => Show();
        protected void ShowUtilityWindow() => ShowUtility();

        protected void ShowPopupWindow(Rect activatorRect, Vector2 size)
            => ShowAsDropDown(activatorRect, size);

        protected Rect WindowPosition
        {
            get => position;
            set => position = value;
        }

        protected Vector2 WindowSize
        {
            get => position.size;
            set => position = new Rect(position.position, value);
        }

        protected Vector2 MinSize
        {
            get => minSize;
            set => minSize = value;
        }

        protected Vector2 MaxSize
        {
            get => maxSize;
            set => maxSize = value;
        }

        protected GUIContent TitleContent
        {
            get => titleContent;
            set => titleContent = value;
        }

        protected string Title
        {
            get => titleContent.text;
            set => titleContent = new GUIContent(value);
        }

        protected bool WantsMouseMove
        {
            get => wantsMouseMove;
            set => wantsMouseMove = value;
        }

        protected bool AutoRepaintOnSceneChange
        {
            get => autoRepaintOnSceneChange;
            set => autoRepaintOnSceneChange = value;
        }

        // ─────────────────────────────────────────────
        // Layout
        // ─────────────────────────────────────────────

        protected void Space(float pixels = 4f)
            => GUILayout.Space(pixels);

        protected void FlexibleSpace()
            => GUILayout.FlexibleSpace();

        protected void BeginHorizontal(params GUILayoutOption[] options)
            => EditorGUILayout.BeginHorizontal(options);

        protected void EndHorizontal()
            => EditorGUILayout.EndHorizontal();

        protected void BeginVertical(params GUILayoutOption[] options)
            => EditorGUILayout.BeginVertical(options);

        protected void EndVertical()
            => EditorGUILayout.EndVertical();

        protected void BeginBox(params GUILayoutOption[] options)
            => EditorGUILayout.BeginVertical(EditorStyles.helpBox, options);

        protected void EndBox()
            => EditorGUILayout.EndVertical();

        protected Vector2 BeginScrollView(
            Vector2 scroll,
            params GUILayoutOption[] options)
            => EditorGUILayout.BeginScrollView(scroll, options);

        protected void EndScrollView()
            => EditorGUILayout.EndScrollView();

        // ─────────────────────────────────────────────
        // Label
        // ─────────────────────────────────────────────

        protected void Label(string text)
            => EditorGUILayout.LabelField(text);

        protected void Label(string label, string value)
            => EditorGUILayout.LabelField(label, value);

        protected void Label(string text, GUIStyle style)
            => EditorGUILayout.LabelField(text, style);

        protected void BoldLabel(string text)
            => EditorGUILayout.LabelField(text, EditorStyles.boldLabel);

        protected void TitleLabel(string text)
            => EditorGUILayout.LabelField(text, EditorStyles.largeLabel);

        // ─────────────────────────────────────────────
        // Button
        // ─────────────────────────────────────────────

        protected bool Button(
            string text,
            params GUILayoutOption[] options)
            => GUILayout.Button(text, options);

        protected bool Button(
            GUIContent content,
            params GUILayoutOption[] options)
            => GUILayout.Button(content, options);

        protected bool MiniButton(string text)
            => GUILayout.Button(text, EditorStyles.miniButton);

        protected bool ToolbarButton(string text)
            => GUILayout.Button(text, EditorStyles.toolbarButton);

        // ─────────────────────────────────────────────
        // Toggle
        // ─────────────────────────────────────────────

        protected bool Toggle(bool value)
            => EditorGUILayout.Toggle(value);

        protected bool Toggle(string label, bool value)
            => EditorGUILayout.Toggle(label, value);

        protected bool ToggleLeft(string label, bool value)
            => EditorGUILayout.ToggleLeft(label, value);

        // ─────────────────────────────────────────────
        // Text
        // ─────────────────────────────────────────────

        protected string TextField(string value)
            => EditorGUILayout.TextField(value);

        protected string TextField(string label, string value)
            => EditorGUILayout.TextField(label, value);

        protected string DelayedTextField(string value)
            => EditorGUILayout.DelayedTextField(value);

        protected string DelayedTextField(string label, string value)
            => EditorGUILayout.DelayedTextField(label, value);

        protected string PasswordField(string label, string value)
            => EditorGUILayout.PasswordField(label, value);

        // ─────────────────────────────────────────────
        // Numeric
        // ─────────────────────────────────────────────

        protected int IntField(int value)
            => EditorGUILayout.IntField(value);

        protected int IntField(string label, int value)
            => EditorGUILayout.IntField(label, value);

        protected float FloatField(float value)
            => EditorGUILayout.FloatField(value);

        protected float FloatField(string label, float value)
            => EditorGUILayout.FloatField(label, value);

        protected double DoubleField(double value)
            => EditorGUILayout.DoubleField(value);

        protected double DoubleField(string label, double value)
            => EditorGUILayout.DoubleField(label, value);

        protected int IntSlider(
            string label,
            int value,
            int min,
            int max)
            => EditorGUILayout.IntSlider(label, value, min, max);

        protected float Slider(
            string label,
            float value,
            float min,
            float max)
            => EditorGUILayout.Slider(label, value, min, max);

        // ─────────────────────────────────────────────
        // Vector / Color / Rect
        // ─────────────────────────────────────────────

        protected Vector2 Vector2Field(
            string label,
            Vector2 value)
            => EditorGUILayout.Vector2Field(label, value);

        protected Vector3 Vector3Field(
            string label,
            Vector3 value)
            => EditorGUILayout.Vector3Field(label, value);

        protected Vector4 Vector4Field(
            string label,
            Vector4 value)
            => EditorGUILayout.Vector4Field(label, value);

        protected Color ColorField(
            string label,
            Color value)
            => EditorGUILayout.ColorField(label, value);

        protected Rect RectField(
            string label,
            Rect value)
            => EditorGUILayout.RectField(label, value);

        // ─────────────────────────────────────────────
        // Object
        // ─────────────────────────────────────────────

        protected T ObjectField<T>(
            string label,
            T value,
            bool allowSceneObjects = true)
            where T : UnityEngine.Object
        {
            return (T)EditorGUILayout.ObjectField(
                label,
                value,
                typeof(T),
                allowSceneObjects);
        }

        protected T ObjectField<T>(
            T value,
            bool allowSceneObjects = true)
            where T : UnityEngine.Object
        {
            return (T)EditorGUILayout.ObjectField(
                value,
                typeof(T),
                allowSceneObjects);
        }

        // ─────────────────────────────────────────────
        // Enum
        // ─────────────────────────────────────────────

        protected T EnumField<T>(T value)
            where T : Enum
            => (T)EditorGUILayout.EnumPopup(value);

        protected T EnumField<T>(
            string label,
            T value)
            where T : Enum
            => (T)EditorGUILayout.EnumPopup(label, value);

        // ─────────────────────────────────────────────
        // Popup
        // ─────────────────────────────────────────────

        protected int Popup(
            int selected,
            string[] options)
            => EditorGUILayout.Popup(selected, options);

        protected int Popup(
            string label,
            int selected,
            string[] options)
            => EditorGUILayout.Popup(label, selected, options);

        protected int IntPopup(
            string label,
            int selected,
            string[] displayedOptions,
            int[] optionValues)
            => EditorGUILayout.IntPopup(
                label,
                selected,
                displayedOptions,
                optionValues);

        // ─────────────────────────────────────────────
        // Foldout
        // ─────────────────────────────────────────────

        protected bool Foldout(
            bool expanded,
            string label,
            bool toggleOnLabelClick = true)
            => EditorGUILayout.Foldout(
                expanded,
                label,
                toggleOnLabelClick);

        // ─────────────────────────────────────────────
        // HelpBox
        // ─────────────────────────────────────────────

        protected void Info(string message)
            => EditorGUILayout.HelpBox(
                message,
                MessageType.Info);

        protected void Warning(string message)
            => EditorGUILayout.HelpBox(
                message,
                MessageType.Warning);

        protected void Error(string message)
            => EditorGUILayout.HelpBox(
                message,
                MessageType.Error);

        protected void HelpBox(
            string message,
            MessageType type)
            => EditorGUILayout.HelpBox(message, type);

        // ─────────────────────────────────────────────
        // Toolbar
        // ─────────────────────────────────────────────

        protected int Toolbar(
            int selected,
            string[] options)
            => GUILayout.Toolbar(selected, options);

        protected int Toolbar(
            int selected,
            GUIContent[] options)
            => GUILayout.Toolbar(selected, options);

        // ─────────────────────────────────────────────
        // EditorGUI
        // ─────────────────────────────────────────────

        protected void BeginChangeCheck()
            => EditorGUI.BeginChangeCheck();

        protected bool EndChangeCheck()
            => EditorGUI.EndChangeCheck();

        protected void BeginDisabled(bool disabled = true)
            => EditorGUI.BeginDisabledGroup(disabled);

        protected void EndDisabled()
            => EditorGUI.EndDisabledGroup();

        protected void BeginIndent(int level = 1)
            => EditorGUI.indentLevel += level;

        protected void EndIndent(int level = 1)
            => EditorGUI.indentLevel -= level;

        protected Rect GetControlRect(
            float height = 18f,
            params GUILayoutOption[] options)
            => EditorGUILayout.GetControlRect(
                GUILayout.Height(height));

        // ─────────────────────────────────────────────
        // EditorUtility
        // ─────────────────────────────────────────────

        protected void SetDirty(UnityEngine.Object target)
        {
            if (target != null)
                EditorUtility.SetDirty(target);
        }

        protected bool Confirm(
            string title,
            string message,
            string ok = "OK",
            string cancel = "Cancel")
            => EditorUtility.DisplayDialog(
                title,
                message,
                ok,
                cancel);

        protected void Progress(
            string title,
            string info,
            float progress)
            => EditorUtility.DisplayProgressBar(
                title,
                info,
                progress);

        protected void ClearProgress()
            => EditorUtility.ClearProgressBar();

        // ─────────────────────────────────────────────
        // AssetDatabase
        // ─────────────────────────────────────────────

        protected T LoadAsset<T>(string path)
            where T : UnityEngine.Object
            => AssetDatabase.LoadAssetAtPath<T>(path);

        protected T Find<T>(string guid)
            where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(guid))
                return null;

            string path = AssetDatabase.GUIDToAssetPath(guid);

            return string.IsNullOrEmpty(path)
                ? null
                : AssetDatabase.LoadAssetAtPath<T>(path);
        }

        protected string[] FindAssets(string filter)
            => AssetDatabase.FindAssets(filter);

        protected string[] FindAssets(
            string filter,
            params string[] folders)
            => AssetDatabase.FindAssets(filter, folders);

        protected string GUIDToPath(string guid)
            => AssetDatabase.GUIDToAssetPath(guid);

        protected string PathToGUID(string path)
            => AssetDatabase.AssetPathToGUID(path);

        // ─────────────────────────────────────────────
        // EditorApplication
        // ─────────────────────────────────────────────

        protected void StartUpdate()
            => EditorApplication.update += Repaint;

        protected void StopUpdate()
            => EditorApplication.update -= Repaint;

        protected void Delay(Action action)
        {
            if (action != null)
                EditorApplication.delayCall += () => action();
        }

        // ─────────────────────────────────────────────
        // Event
        // ─────────────────────────────────────────────

        protected Event CurrentEvent
            => Event.current;

        protected UnityEngine.EventType CurrentEventType
            => Event.current.type;

        protected bool IsRepaint
            => Event.current.type == UnityEngine.EventType.Repaint;

        protected bool IsLayout
            => Event.current.type == UnityEngine.EventType.Layout;

        protected bool IsMouseDown
            => Event.current.type == UnityEngine.EventType.MouseDown;

        protected bool IsMouseUp
            => Event.current.type == UnityEngine.EventType.MouseUp;

        protected bool IsKeyDown
            => Event.current.type == UnityEngine.EventType.KeyDown;

        protected bool IsKeyUp
            => Event.current.type == UnityEngine.EventType.KeyUp;

        // ─────────────────────────────────────────────
        // GUIContent
        // ─────────────────────────────────────────────

        protected GUIContent Content(string text)
            => new(text);

        protected GUIContent Content(
            string text,
            string tooltip)
            => new(text, tooltip);

        protected GUIContent Content(
            string text,
            Texture image)
            => new(text, image);

        protected GUIContent Content(
            string text,
            Texture image,
            string tooltip)
            => new(text, image, tooltip);

        // ─────────────────────────────────────────────
        // Window Factory
        // ─────────────────────────────────────────────

        protected static T Open<T>(string title = null)
            where T : BaseEditorWindow
        {
            T window = GetWindow<T>();

            if (!string.IsNullOrEmpty(title))
                window.titleContent = new GUIContent(title);

            window.Show();

            return window;
        }

        protected static T OpenUtility<T>(string title = null)
            where T : BaseEditorWindow
        {
            T window = GetWindow<T>();

            if (!string.IsNullOrEmpty(title))
                window.titleContent = new GUIContent(title);

            window.ShowUtility();

            return window;
        }

        protected static T FindWindow<T>()
            where T : BaseEditorWindow
            => GetWindow<T>();

        // ─────────────────────────────────────────────
        // Styles
        // ─────────────────────────────────────────────

        protected GUIStyle BoxStyle
            => EditorStyles.helpBox;

        protected GUIStyle BoldStyle
            => EditorStyles.boldLabel;

        protected GUIStyle ToolbarStyle
            => EditorStyles.toolbar;

        protected GUIStyle ToolbarButtonStyle
            => EditorStyles.toolbarButton;

        protected GUIStyle MiniButtonStyle
            => EditorStyles.miniButton;

        protected GUIStyle FoldoutStyle
            => EditorStyles.foldout;
    }
}

#endif