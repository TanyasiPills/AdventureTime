using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : Editor
{
    private SerializedProperty modifiersProp;

    private void OnEnable()
    {
        modifiersProp = serializedObject.FindProperty("modifiers");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw everything except "modifiers" using default inspector
        DrawPropertiesExcluding(serializedObject, "modifiers");

        EditorGUILayout.LabelField("Modifiers", EditorStyles.boldLabel);

        for (int i = 0; i < modifiersProp.arraySize; i++)
        {
            SerializedProperty element = modifiersProp.GetArrayElementAtIndex(i);

            if (element.managedReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Null Modifier", MessageType.Warning);
                continue;
            }

            // Header row with class name + remove button
            GUILayout.BeginHorizontal();
            element.isExpanded = EditorGUILayout.Foldout(
                element.isExpanded,
                element.managedReferenceValue.GetType().Name,
                true
            );

            if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
            {
                modifiersProp.DeleteArrayElementAtIndex(i);
                GUILayout.EndHorizontal();
                break; // avoid errors after deleting
            }

            GUILayout.EndHorizontal();

            // Draw all child properties
            if (element.isExpanded)
            {
                EditorGUI.indentLevel++;
                SerializedProperty iterator = element.Copy();
                SerializedProperty end = iterator.GetEndProperty();

                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
                {
                    // Skip internal Unity metadata
                    if (iterator.name == "managedReferenceFullTypename")
                        continue;

                    EditorGUILayout.PropertyField(iterator, true);
                    enterChildren = false;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Modifier"))
        {
            ShowAddModifierMenu();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowAddModifierMenu()
    {
        var modifierTypes = typeof(StatModifier).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(StatModifier)) && !t.IsAbstract)
            .ToList();

        GenericMenu menu = new GenericMenu();

        foreach (var type in modifierTypes)
        {
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                int newIndex = modifiersProp.arraySize;
                modifiersProp.InsertArrayElementAtIndex(newIndex);
                var element = modifiersProp.GetArrayElementAtIndex(newIndex);
                element.managedReferenceValue = Activator.CreateInstance(type);
                serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }
}
