using UnityEditor;
using UnityEngine;

/*

[CustomEditor(typeof(M4Renderer)), CanEditMultipleObjects]
public class M4RendererEditor : Editor
{
    static class Styles {
        static public GUIStyle Header = EditorStyles.largeLabel;
    }

    SerializedProperty baseMesh, defaultOffset, defaultSwapAxis, defaultExtraRot;
    SerializedProperty  renderMode, buildMode, faceMode, postTransformation, preTransformation;
    DynamicMatrix4x4Editor postDynamicEditor = new DynamicMatrix4x4Editor(null);
    

    void OnEnable () {
        // renderers = serializedObject.FindProperty("renderers");
        baseMesh = serializedObject.FindProperty("baseMesh");
        defaultSwapAxis = serializedObject.FindProperty("defaultSwapAxis");
        defaultOffset = serializedObject.FindProperty("defaultOffset");
        defaultExtraRot = serializedObject.FindProperty("defaultExtraRot");
        buildMode = serializedObject.FindProperty("buildMode");
        renderMode = serializedObject.FindProperty("renderMode");
        faceMode = serializedObject.FindProperty("faceMode");
        postTransformation = serializedObject.FindProperty("postTransformation");
        postDynamicEditor.header = "Post-Transformation";
    }
    
    public override void OnInspectorGUI () {
        serializedObject.Update();
        //EditorGUILayout.LabelField("4D Transformation", Styles.Header, GUILayout.Height(20));
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(baseMesh);
        EditorGUILayout.PropertyField(buildMode);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(defaultOffset);
        EditorGUILayout.PropertyField(defaultSwapAxis);
        EditorGUILayout.PropertyField(defaultExtraRot);
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
            foreach (var i in serializedObject.targetObjects)
            {
                ((M4Renderer)i).SetDirty(true);
            } 
        }
        
        if (!postTransformation.hasMultipleDifferentValues) {
            var x  = ((M4Renderer)target).postTransformation;
            EditorGUI.BeginChangeCheck();
            postDynamicEditor.matrix = x;
            postDynamicEditor.OnGUI();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                x = postDynamicEditor.matrix;
                foreach (var i in serializedObject.targetObjects)
                {
                    ((M4Renderer)i).postTransformation.CopyFrom(x);   
                    ((M4Renderer)i).SetDirty(true);
                }
            }
        }
        EditorGUI.indentLevel--;
        
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(renderMode);
        EditorGUILayout.PropertyField(faceMode);
        if (!serializedObject.isEditingMultipleObjects) {
            if (GUILayout.Button("Go To Viewer"))
                Selection.activeGameObject = ((M4Renderer)target).manager.gameObject;
        }
        serializedObject.ApplyModifiedProperties();
    }
}

*/