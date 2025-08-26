using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

[CanEditMultipleObjects]
[CustomEditor(typeof(BuffConsumable), true)]
public class BuffConsumableEditor : Editor
{
    private SerializedProperty prop;
    private List<Type> derivedTypes;
    private string[] typeNames;
    private int selectedIndex = -1;

    private void OnEnable()
    {
        prop = serializedObject.FindProperty("modifier");

        // Reflectively find all non-abstract subclasses of MyBaseClass
        derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(StatModifier)) && !t.IsAbstract)
            .ToList();

        typeNames = derivedTypes.Select(t => t.Name).ToArray();

        // Set selectedIndex based on current type
        if (prop != null && prop.managedReferenceValue != null)
        {
            Type currentType = prop.managedReferenceValue.GetType();
            selectedIndex = derivedTypes.IndexOf(currentType);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var component = (BuffConsumable)target;

        // Draw all regular fields except the polymorphic one
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        while (iterator.NextVisible(enterChildren))
        {
            if (iterator.propertyPath == "modifier")
            {
                int newIndex = EditorGUILayout.Popup("Stat Modifier Type", selectedIndex, typeNames);

                if (newIndex != selectedIndex)
                {
                    selectedIndex = newIndex;
                    Type selectedType = derivedTypes[selectedIndex];
                    component.modifier = Activator.CreateInstance(selectedType) as StatModifier;
                    EditorUtility.SetDirty(component);
                }
            }

            EditorGUILayout.PropertyField(iterator, true);
            enterChildren = false;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
