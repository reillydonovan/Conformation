using UnityEngine;
using System.Collections;
using UnityEditor;
using M4DLib.Legacy;

namespace M4DLib.Editors.Legacy
{


[CustomEditor(typeof(MorphHelper),true)]
[CanEditMultipleObjects]
public class MorphHelperEditor : Editor
{
	private SerializedProperty phase;
	private SerializedProperty Offset;
	private SerializedProperty Rotation;
	private SerializedProperty Scale ;
	private SerializedProperty Pivot;
	private SerializedProperty allLinear;
	private SerializedProperty keepScale;
	private SerializedProperty scaleFix;
	private SerializedProperty autoUpdate;
	private SerializedProperty scaleType;
	private SerializedProperty keepRotation;
	private SerializedProperty rotationFix;
	// Use this for initialization
	void OnEnable ()
	{
		phase = serializedObject.FindProperty ("m_phase");
		Offset = serializedObject.FindProperty ("m_Offset");
		Rotation = serializedObject.FindProperty ("m_Rotation");
		Scale = serializedObject.FindProperty ("m_Scale");
		Pivot = serializedObject.FindProperty ("m_Pivot");
		allLinear = serializedObject.FindProperty ("m_allLinear");
		keepScale = serializedObject.FindProperty ("m_keepScale");
		scaleFix = serializedObject.FindProperty ("m_scaleFix");
		scaleType = serializedObject.FindProperty ("m_scaleType");
		keepRotation = serializedObject.FindProperty ("m_keepRotation");
		rotationFix = serializedObject.FindProperty ("m_rotationFix");
	}

	public  override void OnInspectorGUI ()
	{
		serializedObject.Update ();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField (phase);
		EditorGUILayout.PropertyField (Offset);
		EditorGUILayout.PropertyField (Rotation);
		EditorGUILayout.PropertyField (Scale);
		EditorGUILayout.PropertyField (Pivot);
		EditorGUILayout.PropertyField (keepRotation);
		if (keepRotation.boolValue)
			EditorGUILayout.PropertyField (rotationFix);
		EditorGUILayout.PropertyField (scaleType);
		EditorGUILayout.PropertyField (keepScale);
		if (keepScale.boolValue)
			EditorGUILayout.PropertyField (scaleFix);
		EditorGUILayout.PropertyField (allLinear);
		if (EditorGUI.EndChangeCheck())
            ((MorphHelper)target).SetDirty();
        serializedObject.ApplyModifiedProperties ();	
	}
	
}

}