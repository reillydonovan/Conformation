using UnityEngine;
using System.Collections;
using UnityEditor;

namespace M4DLib.Editors {

public abstract class NoScriptEditor : Editor
{

    static string[] _excludes = { "m_Script" };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, _excludes);
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(S4Viewer)), CanEditMultipleObjects]
public class S4ViewerEditor : NoScriptEditor {}

[CustomEditor(typeof(S4UploaderBase), true), CanEditMultipleObjects]
public class S4UploaderEditor : NoScriptEditor {}

[CustomEditor(typeof(M4Viewer)), CanEditMultipleObjects]
public class M4ViewerEditor : NoScriptEditor {}

[CustomEditor(typeof(M4Renderer)), CanEditMultipleObjects]
public class M4RendererEditor : NoScriptEditor {}

[CustomEditor(typeof(M4UploaderBase), true), CanEditMultipleObjects]
public class M4UploaderEditor : NoScriptEditor {}

 
}