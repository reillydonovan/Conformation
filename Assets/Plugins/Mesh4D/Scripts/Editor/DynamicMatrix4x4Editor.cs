
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace M4DLib.Editors {

[CustomPropertyDrawer(typeof(DynamicMatrix4x4))]
public class DynamicMatrix4x4Editor : PropertyDrawer
{
    public GUIContent header;
    protected ReorderableList list;
    DynamicMatrix4x4 matrix;

	public DynamicMatrix4x4Editor ()
	{
        CreateReorderable();
	}

    void CreateReorderable ()
    {
        list = new ReorderableList (null, typeof(DynamicMatrix4x4.Operation));
        list.headerHeight = 16f;
        list.elementHeight = 18;
        list.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate (OnHeaderDraw);
        list.drawElementCallback = new ReorderableList.ElementCallbackDelegate (OnElementDraw);
        list.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate (OnAddCallback);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

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
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (list == null)
            CreateReorderable();

        header = label;
        matrix = property.GetValue<DynamicMatrix4x4>();
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
			r.width = (rW - r.width) / 3f - 4f;
		} else {
			r.y += r.height;
			r.width = r.width / 3f - 4f;
		}

		EditorGUIUtility.labelWidth = 16f;

		bool uniform = false;
		var val = op.value;

		float isScale = op.mode == 2 ? 1f : 0;
		if (isScale == 1f) {
			//if (GUI.changed)
			//	Debug.LogWarning("APRIL FOOL");
			uniform = Mathf.Approximately (val.x, val.y) && Mathf.Approximately (val.y, val.z);
			EditorGUI.BeginDisabledGroup (!uniform);
			var rR = r;
			rR.width = 16f;
			op_uniform = GUI.Toggle (rR, op_uniform, GUIContent.none);
			EditorGUI.EndDisabledGroup ();
			r.x += 20;
			r.width -= 20 / 3f;
			//if (!uniform && alreadyUniform)
			//GUI.changed = false;
		}
		val.x = EditorGUI.FloatField(r, "X", val.x);
		r.x += r.width + 4;
		val.y = EditorGUI.FloatField(r, "Y", val.y);
		r.x += r.width + 4;
		val.z = EditorGUI.FloatField(r, "Z", val.z);
		r.x += r.width + 4;

		if (uniform && op_uniform) {
			bool alreadyUniform = Mathf.Approximately (val.x, val.y) && Mathf.Approximately (val.y, val.z);
			if (!alreadyUniform) {
				if (Mathf.Approximately (val.x, val.y)) {
					val.x = val.z;
					val.y = val.z;
				} else if (Mathf.Approximately (val.y, val.z)) {
					val.y = val.x;
					val.z = val.x;
				} else {
					val.x = val.y;
					val.z = val.y;
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
		matrix.Add (sel, sel == 2 ? Vector3.one : Vector3.zero);
		GUI.changed = true;
	}

	static public class Styles
	{
		public static readonly GUIContent[] kMatrixChoices = new  GUIContent[] {
			new GUIContent ("Translate"),
			new GUIContent ("Rotate"),
			new GUIContent ("Scale")
		};
		public static readonly int[] kMatrixValues = new int[] { 0, 1, 2 };
	}
}

}