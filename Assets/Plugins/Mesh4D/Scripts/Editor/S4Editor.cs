using UnityEngine;
using System;
using UnityEditor;

namespace M4DLib.Editors {

[CustomEditor(typeof(S4Renderer)), CanEditMultipleObjects]
public class S4RendererEditor : NoScriptEditor {

    static class Styles {
       public static GUIContent[] viewsGUI = new GUIContent[] {
        new GUIContent("XYZ"), new GUIContent("WYZ"), new GUIContent("XWZ"), new GUIContent("XYW"),
        };

       public static GUIStyle[] viewsStyle = new GUIStyle[] {
            new GUIStyle(EditorStyles.miniButtonLeft), 
            new GUIStyle(EditorStyles.miniButtonMid), 
            new GUIStyle(EditorStyles.miniButtonMid), 
            new GUIStyle(EditorStyles.miniButtonRight), 
        };

        public static Quaternion4[] viewModes = new Quaternion4[] {
                Quaternion4.Euler(0, 0, 0, 0, 0, 0),
                Quaternion4.Euler(0, 0, 0, 90, 0, 0),
                Quaternion4.Euler(0, 0, 0, 0, 90, 0),
                Quaternion4.Euler(0, 0, 0, 0, 0, 90),
        };

        public static int overlayHash = "S4Renderer".GetHashCode();
    }

    int activeView {
        get {
            var r = GetViewer().rotation;
            return Array.IndexOf(Styles.viewModes, r);
        }
        set {
            if (value >= 0) {
                GetViewer().rotation = Styles.viewModes[value];
            }
        }
    }

    protected virtual S4Viewer GetViewer ()
    {
        return ((S4Renderer)target).manager;
    }

    void OnSceneGUI ()
    {
        var m = GetViewer();

        if (!m)
            return;

        Handles.BeginGUI();
        var r = new Rect(Screen.width - 190, Screen.height - 68, 180, 42);
        GUI.Window(Styles.overlayHash, r, new GUI.WindowFunction(DrawOverlay), "S4 View Shortcut");
        Handles.EndGUI();
    }

    void DrawOverlay (int id)
    {
        EditorGUILayout.BeginHorizontal();
        {
            var act = activeView;
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < 4; i++)
            {
                if(GUILayout.Toggle(act == i, Styles.viewsGUI[i], Styles.viewsStyle[i]))
                    act = i;
            }
            if (EditorGUI.EndChangeCheck())
                activeView = act;
        }
        EditorGUILayout.EndHorizontal();
    }
}

}