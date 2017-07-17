using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

namespace M4DLib.Editors
{
    [CustomEditor(typeof(Transform4)), CanEditMultipleObjects]
    public class Transform4Editor : NoScriptEditor
    {
        public enum Tool4
        {
            None = -1,
            Move = 0,
            Rotate = 1,
            Scale = 2,
        }

        static Color _themeSolid = new Color(0.4f, 0.1f, 0.6f, 0.2f);
        static Color _themeWire = new Color(0.4f, 0.1f, 0.6f, 1f);
        static Tool4 _tool = Tool4.None;
        static GUIStyle _toolStyle;
        static GUIContent[] _toolIcons;
        static GUIContent[] _toolIcons_ = new GUIContent[3];
        static bool _initStyle = false;

        void InitStyle()
        {
            _toolIcons = _toolIcons ?? new GUIContent[]
            {
                EditorGUIUtility.IconContent("MoveTool", "|Move the selected objects."),
                EditorGUIUtility.IconContent("RotateTool", "|Rotate the selected objects."),
                EditorGUIUtility.IconContent("ScaleTool", "|Scale the selected objects."),
                EditorGUIUtility.IconContent("MoveTool On"),
                EditorGUIUtility.IconContent("RotateTool On"),
                EditorGUIUtility.IconContent("ScaleTool On"),
            };
            _toolStyle = new GUIStyle("Commandmid");
            _toolStyle.margin = new RectOffset();
            _toolStyle.padding = new RectOffset();
            _toolStyle.stretchHeight = _toolStyle.stretchWidth = true;
            _toolStyle.fixedWidth = _toolStyle.fixedHeight = 0;
            _toolStyle.overflow = new RectOffset();
            _initStyle = true;
        }

        void GUITools(Rect r)
        {
            var n = (int)(Tools.current == Tool.None ? _tool : Tool4.None);
            for (int i = 0; i < 3; i++)
            {
                _toolIcons_[i] = _toolIcons[n == i ? i + 3 : i];
            }
            n = GUI.SelectionGrid(r, n, _toolIcons_, 1, _toolStyle);
            if (n >= 0)
            {
                Tools.current = Tool.None;
                _tool = (Tool4)n;
            }
        }

        void GUIKeys(Event ev)
        {
            if (!ev.isKey || !(ev.shift))
                return;
            if (ev.keyCode == KeyCode.W)
            {
                Tools.current = Tool.None;
                _tool = Tool4.Move;
            }
            else if (ev.keyCode == KeyCode.E)
            {
                Tools.current = Tool.None;
                _tool = Tool4.Rotate;
            }
            else if (ev.keyCode == KeyCode.R)
            {
                Tools.current = Tool.None;
                _tool = Tool4.Scale;
            }
        }

        public override void OnInspectorGUI()
        {
            if (!_initStyle)
                InitStyle();

            var r = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            base.OnInspectorGUI();
            EditorGUILayout.GetControlRect(GUILayout.Height(2));
            EditorGUILayout.EndVertical();
            var r2 = EditorGUILayout.BeginVertical(GUILayout.MinWidth(40));
            {
                //r2.width = r.height;
                // r2.height = 40;
                r2.height = r.height;
                GUITools(r2);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        void OnSceneGUI()
        {
            GUIKeys(Event.current);
            if (Tools.current != Tool.None)
                return;
            var t = (Transform4)target;
            var v = t.viewer;
            if (v == null)
                return;
            Vector3 p;
            if (!v.WorldToProjectionPoint(t.position, out p))
                return;
            Vector3 n = Vector3.Normalize(Camera.current.transform.position - p);
            Quaternion orient = v.WorldToProjectionOrientation(t.rotation);
            float s = HandleUtility.GetHandleSize(p) * 0.2f;
            Handles.matrix = t.transform.localToWorldMatrix;

            Handles.color = _themeSolid;
            Handles.DrawSolidDisc(p, n, s);

            Handles.color = _themeWire;
            Handles.DrawWireDisc(p, n, s);

            switch (_tool)
            {
                case Tool4.Move:
                    HandlePosition(t, v, p, n, orient);
                    break;
                case Tool4.Rotate:
                    HandleRotation(t, v, p, n, orient);
                    break;
                case Tool4.Scale:
                    HandleScale(t, v, p, n, orient);
                    break;
            }
        }

        void HandlePosition(Transform4 t, IViewer v, Vector3 p, Vector3 n, Quaternion orient)
        {
            var v_ = Handles.PositionHandle(p, Quaternion.identity);
            if (p != v_)
            {
                Undo.RecordObject(t, "Transform4 Position");
                t.position = v.ProjectionToWorldPoint(v_);
            }
        }

        void HandleRotation(Transform4 t, IViewer v, Vector3 p, Vector3 n, Quaternion orient)
        {
            var v_ = Handles.RotationHandle(orient, p);
            if (orient != v_)
            {
                Undo.RecordObject(t, "Transform4 Rotation");
                t.rotation = v.ProjectionToWorldOrientation(v_);
            }
        }

        void HandleScale(Transform4 t, IViewer v, Vector3 p, Vector3 n, Quaternion orient)
        {
            var r = v.WorldToProjectionVector(t.scale);
            var r_ = Handles.ScaleHandle(r, p, orient, HandleUtility.GetHandleSize(p));
            if (r != r_)
            {
                Undo.RecordObject(t, "Transform4 Scale");
                t.scale = v.ProjectionToWorldVector(r_);
            }
        }

    }

}