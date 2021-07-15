using GOAP_Resources;
using UnityEditor;
using UnityEngine;

namespace GOAP_Editor
{
    [CustomPropertyDrawer(typeof(FixedEnumNames))]
    public class EnumNameFixDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect contentRect = EditorGUI.PrefixLabel(position, label);
            property.enumValueIndex = EditorGUI.Popup(contentRect, property.enumValueIndex, property.enumNames, EditorStyles.popup);
        }
    }
}