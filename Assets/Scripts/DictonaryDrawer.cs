using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Drawer
{
    [CustomPropertyDrawer(typeof(Dictionary<string, List<AudioClip>>))] // Dictionary türüne özgü bir PropertyDrawer

    public class DictonaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Dictionary'in içeriğine erişim ve düzenleme işlemleri burada yapılabilir
            // Örneğin, "property" üzerinden dictionary'e erişebilirsiniz.

            EditorGUI.EndProperty();
        }
    }
}
