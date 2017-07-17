using UnityEngine;
using System.Collections;
using UnityEditor;
using M4DLib.Legacy;

namespace M4DLib.Editors.Legacy
{

[CustomEditor(typeof(Mesh4DEngine), true)]
[CanEditMultipleObjects]
public class Mesh4DEditor : Editor
{
    SerializedProperty baseMesh;
    SerializedProperty wOffset;
    SerializedProperty wRotation;
    SerializedProperty wScale;
    SerializedProperty wPivot;
    SerializedProperty useWFaces;
    SerializedProperty wFaces;
    SerializedProperty smallScale;
    SerializedProperty bigScale;
    SerializedProperty bakeReport;
    protected void OnEnable()
    {
        baseMesh = serializedObject.FindProperty("m_baseMesh");
        wOffset = serializedObject.FindProperty("m_wOffset");
        wRotation = serializedObject.FindProperty("m_wRotation");
        wScale = serializedObject.FindProperty("m_wScale");
        wPivot = serializedObject.FindProperty("m_wPivot");
        useWFaces = serializedObject.FindProperty("m_useWFaces");
        wFaces = serializedObject.FindProperty("m_wFaces");
        smallScale = serializedObject.FindProperty("m_smallScale");
        bigScale = serializedObject.FindProperty("m_bigScale");
        bakeReport = serializedObject.FindProperty("wBakeStat");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(baseMesh);
        if (EditorGUI.EndChangeCheck())
            BakeMesh(true);
        EditorGUI.BeginChangeCheck();
        if (baseMesh.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(wOffset);
            EditorGUILayout.PropertyField(wRotation);
            EditorGUILayout.PropertyField(wScale);
            EditorGUILayout.PropertyField(wPivot);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useWFaces);
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
                BakeMesh(true);
            }
            if (useWFaces.intValue == 2 && !useWFaces.hasMultipleDifferentValues)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(wFaces);
                EditorGUILayout.PropertyField(smallScale);
                EditorGUILayout.PropertyField(bigScale);
				if (EditorGUI.EndChangeCheck()) {
					serializedObject.ApplyModifiedProperties();
                    BakeMesh(true);
                }
                DisplayHelpBox();
                EditorGUI.indentLevel--;
            }
        }
		if (EditorGUI.EndChangeCheck())
            BakeMesh(false);
        serializedObject.ApplyModifiedProperties();
    }
	
	void DisplayHelpBox() {
		float wRep = bakeReport.floatValue;
		if (wRep == -1)
			EditorGUILayout.HelpBox("Please Assign W Face Connector", MessageType.None);
		else if (wRep == 1)
			EditorGUILayout.HelpBox("Face Connector is Fully Matched", MessageType.Info);
		else if (wRep == 0)
		{
			Mesh4DEngine mmm = (Mesh4DEngine)(serializedObject.targetObject);
			if (mmm.wFaces == null)
				EditorGUILayout.HelpBox("Face Connector is Fully Unmatched, \n Base Mesh Bounds is " + mmm.baseMesh.bounds.max.ToString(), MessageType.Error);
			else
				EditorGUILayout.HelpBox("Face Connector is Fully Unmatched, \n Base Mesh Bounds is " + mmm.baseMesh.bounds.max.ToString() +
						" \n wFaces Bounds is " + mmm.wFaces.bounds.max.ToString(), MessageType.Error);
		}
		else if (wRep < 1)
			EditorGUILayout.HelpBox("Face Connector is Partially Matched (" + (wRep * 100).ToString("0") + "%)", MessageType.Warning);
	
	}

    void BakeMesh(bool bake)
    {
        ((Mesh4DEngine)(serializedObject.targetObject)).SetDirty(bake);
    }
    //		void CalculateMesh()
    //		{
    //		((Mesh4DEngine)(serializedObject.targetObject)).CalculateMesh ();
    //	}
}

}