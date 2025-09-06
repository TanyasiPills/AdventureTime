using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttackScaling), true)]
public class AttackScalingDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
        {
            // Show dropdown when null
            Rect buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            if (GUI.Button(buttonRect, "Add Scaling..."))
            {
                GenericMenu menu = new GenericMenu();

                // Find all non-abstract subclasses of AttackScaling
                var type = typeof(AttackScaling);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => type.IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var t in types)
                {
                    menu.AddItem(new GUIContent(t.Name), false, () =>
                    {
                        property.serializedObject.Update();

                        // Create the instance
                        property.managedReferenceValue = Activator.CreateInstance(t);

                        // Expand it automatically
                        property.isExpanded = true;

                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }
        }
        else
        {
            // Draw the fields of the concrete class
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
