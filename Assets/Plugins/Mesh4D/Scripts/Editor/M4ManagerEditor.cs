using UnityEditor;
using UnityEngine;

/*

[CustomEditor(typeof(M4Viewer)), CanEditMultipleObjects]
public class M4ViewerEditor : Editor
{
    static class Styles {
        static public GUIStyle Header = EditorStyles.largeLabel;
        
        public static GUIStyle Opt = EditorStyles.miniButton;
        public static GUIStyle OptLeft = EditorStyles.miniButtonLeft;
        public static GUIStyle OptMid = EditorStyles.miniButtonMid;
        public static GUIStyle OptRight = EditorStyles.miniButtonRight;        
    }
    
    SerializedProperty position, pivot, rotation, scale;
    SerializedProperty proj_Pos, proj_Rot, proj_Perspective, proj_orthoSize, proj_perspAngle;
    
    void OnEnable () {
        // renderers = serializedObject.FindProperty("renderers");
        position = serializedObject.FindProperty("position");
        pivot = serializedObject.FindProperty("pivot");
        rotation = serializedObject.FindProperty("rotation");
        scale = serializedObject.FindProperty("scale");
        proj_Pos = serializedObject.FindProperty("proj_Pos");
        proj_Rot = serializedObject.FindProperty("proj_Rot");
        proj_Perspective = serializedObject.FindProperty("proj_Perspective");
        proj_orthoSize = serializedObject.FindProperty("proj_orthoSize");
        proj_perspAngle = serializedObject.FindProperty("proj_perspAngle");
    }
    
    public override void OnInspectorGUI () {
        serializedObject.Update();
        EditorGUILayout.LabelField("4D Transformation", Styles.Header, GUILayout.Height(20));
        EditorGUILayout.PropertyField(position);
        EditorGUILayout.PropertyField(pivot);
        EditorGUILayout.PropertyField(rotation);
        EditorGUILayout.PropertyField(scale);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Projection", Styles.Header, GUILayout.Height(20));
        EditorGUILayout.PropertyField(proj_Pos, new GUIContent("Position"));
        EditorGUILayout.PropertyField(proj_Rot, new GUIContent("Rotation"));
        EditorGUILayout.PropertyField(proj_Perspective, new GUIContent("Perspective"));
        if (proj_Perspective.hasMultipleDifferentValues || proj_Perspective.boolValue)
            EditorGUILayout.PropertyField(proj_perspAngle, new GUIContent("Perspective Angle"));
        if (proj_Perspective.hasMultipleDifferentValues || !proj_Perspective.boolValue)
            EditorGUILayout.PropertyField(proj_orthoSize, new GUIContent("Orthographic Size"));

        EditorGUILayout.Space();
        
        if (!serializedObject.isEditingMultipleObjects) {
            var t = (M4Viewer)target;
            // Buttons 
            EditorGUILayout.BeginHorizontal();
            {
                var r = EditorGUILayout.GetControlRect();
                r.width = r.width / 4f - 1;
                if (GUI.Button(r, "Manipulated", Styles.OptLeft))
                    (t).SetAllRenderersMode(M4RenderingMode.Manipulated);
                r.x += r.width;
                if (GUI.Button(r, "Original", Styles.OptMid))
                    (t).SetAllRenderersMode(M4RenderingMode.Original);
                r.x += r.width;
                if (GUI.Button(r, "Invisible", Styles.OptRight))
                    (t).SetAllRenderersMode(M4RenderingMode.Invisible);
                r.x += r.width + 3;
                if (GUI.Button(r, "Flip Face", Styles.Opt)) {
                    foreach (var m in t.renderers)
                    {
                        m.FlipFace();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                var r = EditorGUILayout.GetControlRect();
                r.width = r.width / 3f;
                if (GUI.Button(r, "Normalize", Styles.OptLeft))
                    (t).NormalizeProjection();
                r.x += r.width;
                if (GUI.Button(r, "Rebuild", Styles.OptMid))
                    (t).SetChildrenDirty(true);
                r.x += r.width;
                if (GUI.Button(r, "Reset", Styles.OptRight))
                    (t).ResetTransformation();
            }
            EditorGUILayout.EndHorizontal();
            
            
            if (Vector4.Scale(t.bound.extent, t.scale).sqrMagnitude >= t.proj_Pos.sqrMagnitude) {
                GUILayout.Label("There is a possibility that the projection camera will be inside of mesh.\nPlease make projection distance futher.", EditorStyles.helpBox);
//                EditorGUILayout.HelpBox(, MessageType.Warning);
            } //else
  //              EditorGUIUtility.GetControlID(FocusType.Passive);
        }
        serializedObject.ApplyModifiedProperties();
    }
    
}

*/