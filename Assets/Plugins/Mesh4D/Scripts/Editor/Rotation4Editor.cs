using UnityEngine;
using System.Collections;
using UnityEditor;

namespace M4DLib.Editors {

[CustomPropertyDrawer(typeof(Rotation4))]
public class Rotation4Editor : PropertyDrawer
{
    static GUIContent[] labels = new GUIContent[]
            {
                new GUIContent("YZ"), 
                new GUIContent("XZ"), 
                new GUIContent("XY"), 
                new GUIContent("WX"),
                new GUIContent("WY"),
                new GUIContent("WZ"),
            };

    static bool? m_inDeg = null;
    public bool inDegree {
        get {
            return m_inDeg ?? (bool)(m_inDeg = EditorPrefs.GetBool("Rotation4.InDegree", false));
        } set {
            EditorPrefs.SetBool("Rotation4.InDegree",(bool)(m_inDeg = value));
        }
    }
    
    public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
        return 32;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        label = EditorGUI.BeginProperty (position, label, property);
        if (!EditorGUIUtility.wideMode)
            EditorGUIUtility.labelWidth /= 2f;
        Rect r = EditorGUI.PrefixLabel (position, label); 
        {
            var rr = position;
            rr = new Rect(rr.x, rr.y + 16, 60, 16);
            
            inDegree = GUI.Toggle(rr, inDegree, "Degree", EditorStyles.miniButton);
        }
        r.height = 16;
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 24f;
        
        SerializedProperty yz = property.FindPropertyRelative("YZ");
        SerializedProperty xy = property.FindPropertyRelative("XY");
        SerializedProperty xz = property.FindPropertyRelative("XZ");
        SerializedProperty xw = property.FindPropertyRelative("XW");
        SerializedProperty yw = property.FindPropertyRelative("YW");
        SerializedProperty zw = property.FindPropertyRelative("ZW");

        Rect r2 = r;
        r2.width /= 3f;
        PropField(r2, yz, labels[0]);
        r2.x += r2.width;
        PropField(r2, xz, labels[1]);
        r2.x += r2.width;
        PropField(r2, xy, labels[2]);
        r2 = r;
        r2.width /= 3f;
        r2.y += 16;
        PropField(r2, xw, labels[3]);
        r2.x += r2.width;
        PropField(r2, yw, labels[4]);
        r2.x += r2.width;
        PropField(r2, zw, labels[5]);

        EditorGUI.EndProperty();
    }
    
    void PropField (Rect r, SerializedProperty prop, GUIContent label) {
        if (!inDegree) {
            EditorGUI.PropertyField(r, prop, label);
            return;
        }
        EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
        EditorGUI.BeginChangeCheck();
        var v = EditorGUI.FloatField(r, label, prop.floatValue * Mathf.Rad2Deg);
        if (EditorGUI.EndChangeCheck()) {
            prop.floatValue =  v * Mathf.Deg2Rad;
        }
        EditorGUI.showMixedValue = false;
        
    }
 
}

}