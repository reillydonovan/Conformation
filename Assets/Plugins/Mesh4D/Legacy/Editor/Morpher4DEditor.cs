using UnityEngine;
using System.Collections;
using UnityEditor;
using M4DLib.Legacy;

namespace M4DLib.Editors.Legacy
{


[CustomEditor(typeof(Mesh4DMorpher), true)]
[CanEditMultipleObjects]
public class Morpher4DEditor : Editor
{
    SerializedProperty MeshL;
    SerializedProperty MeshR;
    SerializedProperty wOffset;
    SerializedProperty wRotation;
    SerializedProperty wScale;
    SerializedProperty wPivot;
    SerializedProperty wPivotShape;
    SerializedProperty wPivotRadius;

    protected void OnEnable()
    {
        MeshL = serializedObject.FindProperty("m_MeshL");
        MeshR = serializedObject.FindProperty("m_MeshR");
        wOffset = serializedObject.FindProperty("m_wOffset");
        wRotation = serializedObject.FindProperty("m_wRotation");
        wScale = serializedObject.FindProperty("m_wScale");
        wPivot = serializedObject.FindProperty("m_wPivot");
        wPivotShape = serializedObject.FindProperty("m_morphPivotShape");
        wPivotRadius = serializedObject.FindProperty("m_morphPivotRadius");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(MeshL);
        EditorGUILayout.PropertyField(MeshR);
        if (EditorGUI.EndChangeCheck())
            BakeMesh(true);
		EditorGUI.BeginChangeCheck();
        if (MeshL.objectReferenceValue != null && MeshR.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(wOffset);
            EditorGUILayout.PropertyField(wRotation);
            EditorGUILayout.PropertyField(wScale);
            EditorGUILayout.PropertyField(wPivot);
        	EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(wPivotShape);
            EditorGUILayout.PropertyField(wPivotRadius);
        	if (EditorGUI.EndChangeCheck())
            	BakeMesh(true);
       }
        if (EditorGUI.EndChangeCheck())
            BakeMesh(false);
        serializedObject.ApplyModifiedProperties();
    }

    void BakeMesh(bool bake)
    {
		((Mesh4DMorpher)(serializedObject.targetObject)).SetDirty(bake);
    }
}

[CustomPropertyDrawer(typeof(Vector3Lerp))]
public class Vector3LerpEditor : UnityEditor.PropertyDrawer
{
    bool extended = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return extended ? (Screen.width < 333 ? 64 : 48) : 16;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect RR = EditorGUI.PrefixLabel(position, label);
        EditorGUIUtility.labelWidth = 14f;
        if (!extended)
        {
            RR.width /= 2.5f;
            float pL = EditorGUI.FloatField(RR, "L", MeshUtilities.DefaultOrNull(property.FindPropertyRelative("L").vector3Value));
            if (!float.IsNaN(pL))
                property.FindPropertyRelative("L").vector3Value = Vector3.one * pL;
            RR.x += RR.width;
            pL = EditorGUI.FloatField(RR, "R", MeshUtilities.DefaultOrNull(property.FindPropertyRelative("R").vector3Value));
            if (!float.IsNaN(pL))
                property.FindPropertyRelative("R").vector3Value = Vector3.one * pL;
            RR.x += RR.width + 2;
            RR.width = RR.width / 2 - 2;
            extended = GUI.Toggle(RR, extended, "E", EditorStyles.miniButton);
        }
        else
        {
            if (Screen.width < 333)
            {
                EditorGUI.indentLevel++;
                RR = EditorGUI.IndentedRect(position);
                RR.y += 16;
            }
            EditorGUI.PropertyField(RR, property.FindPropertyRelative("L"), GUIContent.none);
            RR.y += 16;
            EditorGUI.PropertyField(RR, property.FindPropertyRelative("R"), GUIContent.none);
            RR.x += 2;
            RR.y += 16;
            RR.width = RR.width / 3f - 2;
            RR.height = 16;
            EditorGUI.PropertyField(RR, property.FindPropertyRelative("CurveX"), GUIContent.none);
            RR.x += RR.width + 2;
            EditorGUI.PropertyField(RR, property.FindPropertyRelative("CurveY"), GUIContent.none);
            RR.x += RR.width + 2;
            EditorGUI.PropertyField(RR, property.FindPropertyRelative("CurveZ"), GUIContent.none);
            if (Screen.width < 333)
                RR.y -= 49;
            else
                RR.x -= RR.width * 3 + 5;
            extended = GUI.Toggle(RR, extended, "Extend", EditorStyles.miniButton);
        }
        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(FloatLerp))]
public class FloatLerpEditor : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);
        position.width /= 2.5f;
        EditorGUIUtility.labelWidth = 14f;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("L"));
        position.x += position.width;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("R"));
        position.x += position.width + 2;
        position.width = position.width / 2 - 2;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("Curve"), GUIContent.none);
        EditorGUI.EndProperty();
    }
}
}