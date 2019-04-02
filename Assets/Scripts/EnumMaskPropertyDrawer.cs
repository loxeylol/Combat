using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
public class EnumMaskPropertyDrawer : PropertyDrawer
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------

    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Methods ----------------------------------------------------------------------------------------------
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        if (property.propertyType != SerializedPropertyType.Enum || fieldInfo.FieldType.IsEnum == false)
        {
            Debug.LogErrorFormat("Property with attribute EnumMask ist not of type Enum but {0}", property.propertyType);
            return;
        }

        EnumMaskAttribute a = attribute as EnumMaskAttribute;
        
        if (a.DrawAsList == false)
        {
            Enum selection = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);
            selection = EditorGUI.EnumFlagsField(position, label, selection);
            property.intValue = (int)(object)selection;
        }
        else
        {
            Rect pos = new Rect(position);
            pos.height = EditorGUIUtility.singleLineHeight;

            property.isExpanded = EditorGUI.Foldout(pos, property.isExpanded, label, true);
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                pos.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                string[] options = property.enumDisplayNames;
                int current = property.intValue;
                int result = 0;

                for (int i = 1; i < options.Length; i++)
                {
                    int shifted = current >> (i - 1);
                    bool selected = EditorGUI.ToggleLeft(pos, options[i], shifted % 2 == 1);                    
                    if(selected)
                    {
                        result += 1 << (i - 1);
                    }

                    pos.y += EditorGUIUtility.singleLineHeight;
                }

                property.intValue = result;

                EditorGUI.indentLevel--;
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (attribute as EnumMaskAttribute).DrawAsList
            ? EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight * (property.isExpanded ? property.enumNames.Length : 1)
            : base.GetPropertyHeight(property, label);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------


    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
#endif