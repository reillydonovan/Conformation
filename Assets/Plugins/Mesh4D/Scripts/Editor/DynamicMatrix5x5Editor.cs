
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace M4DLib.Editors {

[CustomPropertyDrawer(typeof(DynamicMatrix5x5))]
public class DynamicMatrix5x5Editor : PropertyDrawer
{
    public GUIContent header;
    protected ReorderableList list;
    DynamicMatrix5x5 matrix;

    public DynamicMatrix5x5Editor ()
    {
        CreateReorderable();
    }

    void CreateReorderable ()
    {
        list = new ReorderableList (null, typeof(DynamicMatrix5x5.Operation));
        list.headerHeight = 16f;
        list.elementHeight = 18;
        list.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate (OnHeaderDraw);
        list.drawElementCallback = new ReorderableList.ElementCallbackDelegate (OnElementDraw);
        list.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate (OnAddCallback);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        if (property.hasMultipleDifferentValues)
        {
            EditorGUI.LabelField(position, "DynamicMatrix has different values");   
            return;
        }

        var r = EditorGUI.IndentedRect (position);
        EditorGUI.BeginChangeCheck();
        list.DoList (r);
        if (EditorGUI.EndChangeCheck()) {
            property.SetValueAll(matrix);
            property.serializedObject.Update();
            // THIS IS WEIRD WORKAROUND TO NOTIFY CHANGES IN EDITOR
            var _dum = property.FindPropertyRelative("_dummy");
            _dum.intValue = _dum.intValue == 0 ? 1 : 0;
            property.serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (list == null)
            CreateReorderable();

        header = label;
        matrix = property.GetValue<DynamicMatrix5x5>();
        list.list = matrix.operations;
        list.elementHeight = 18 * (EditorGUIUtility.wideMode ? 1 : 2);

        return list.GetHeight();
    }

    void OnHeaderDraw (Rect r)
    {
        GUI.Label (r, header);
    }

    static bool op_uniform = false;

    void OnElementDraw (Rect r, int idx, bool active, bool focus)
    {
        var wide = EditorGUIUtility.wideMode;
        var oriW = EditorGUIUtility.labelWidth;
        var indent = EditorGUI.indentLevel;
        var op = matrix.operations [idx];
        EditorGUI.indentLevel = 0;
        var rW = r.width;
        if (wide)
            r.width = EditorGUIUtility.labelWidth;
        else
            r.height /= 2f;

        op.mode = EditorGUI.IntPopup (r, matrix.operations [idx].mode, Styles.kMatrixChoices, Styles.kMatrixValues);
        if (wide) {
            r.x += r.width + 4;
            r.width = (rW - r.width) / 4f - 4f;
        } else {
            r.y += r.height;
            r.width = r.width / 4f - 4f;
        }

        EditorGUIUtility.labelWidth = 16f;

        bool uniform = false;
        var val = op.value;

        float isScale = op.mode == 3 ? 1f : 0;
        if (isScale == 1f) {
            uniform = Mathf.Approximately (val.x, val.y) && Mathf.Approximately (val.y, val.z)  && Mathf.Approximately (val.z, val.w);
            EditorGUI.BeginDisabledGroup (!uniform);
            var rR = r;
            rR.width = 16f;
            op_uniform = GUI.Toggle (rR, op_uniform, GUIContent.none);
            EditorGUI.EndDisabledGroup ();
            r.x += 20;
            r.width -= 20 / 4f;
            //if (!uniform && alreadyUniform)
            //GUI.changed = false;
        }
        val.x = EditorGUI.FloatField(r, "X", val.x);
        r.x += r.width + 4;
        val.y = EditorGUI.FloatField(r, "Y", val.y);
        r.x += r.width + 4;
        val.z = EditorGUI.FloatField(r, "Z", val.z);
        r.x += r.width + 4;
        if (op.mode == 0 || op.mode == 3)
        {
            val.w = EditorGUI.FloatField(r, "W", val.w);
            r.x += r.width + 4;
        }

        if (uniform && op_uniform) {
            bool alreadyUniform = Mathf.Approximately (val.x, val.y) && Mathf.Approximately (val.y, val.z) && Mathf.Approximately (val.z, val.w);
            if (!alreadyUniform) {
                if (Mathf.Approximately (val.x, val.y) && Mathf.Approximately (val.y, val.z)) {
                    val.x = val.w;
                    val.y = val.w;
                    val.z = val.w;
                } else if (Mathf.Approximately (val.y, val.z) && Mathf.Approximately (val.z, val.w)) {
                    val.y = val.x;
                    val.z = val.x;
                    val.w = val.x;
                } else if (Mathf.Approximately (val.x, val.z) && Mathf.Approximately (val.z, val.w)) {
                    val.x = val.y;
                    val.z = val.y;
                    val.w = val.y;
                } else {
                    val.x = val.z;
                    val.y = val.z;
                    val.w = val.z;
                }
            }
        }
        op.value = val;
        matrix.operations [idx] = op;
        EditorGUIUtility.labelWidth = oriW;
        EditorGUI.indentLevel = indent;
    
    }

    void OnAddCallback (Rect r, ReorderableList l)
    {
        GUI.changed = false;
        EditorUtility.DisplayCustomMenu (r, Styles.kMatrixChoices, -1, new EditorUtility.SelectMenuItemFunction (OnAddPopup), null); 
    }

    void OnAddPopup (object u, string[] opt, int sel)
    {
        matrix.Add (sel, sel == 3 ? Vector4.one : Vector4.zero);
        GUI.changed = true;
    }

    static public class Styles
    {
        public static readonly GUIContent[] kMatrixChoices = new  GUIContent[] {
            new GUIContent ("Translate"),
            new GUIContent ("Rotate 3D"),
            new GUIContent ("Rotate 4D"),
            new GUIContent ("Scale")
        };
        public static readonly int[] kMatrixValues = new int[] { 0, 1, 2, 3 };
    }
}

}