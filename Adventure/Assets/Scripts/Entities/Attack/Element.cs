using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "ScriptableObjects/Element")]
[Serializable]
public class Element : ScriptableObject
{
    public string elementName;
    [SerializeField]
    public List<ElementModifier> modifiersList;
    public Dictionary<Element, float> modifiers;

    public Element(string name, Dictionary<Element, float> modifiers)
    {
        this.elementName = name;
        this.modifiers = modifiers;
    }

    public Element(string name)
    {
        this.elementName = name;
        this.modifiers = new Dictionary<Element, float>();
    }

    private void OnValidate()
    {
        if (modifiersList == null) return;
        if (modifiers == null) modifiers = new Dictionary<Element, float>();
        else modifiers.Clear();
        modifiersList.ForEach(e => {
            if (e != null && e.element != null && !modifiers.ContainsKey(e.element)) modifiers.Add(e.element, e.value);
        });
    }

    public float GetModifier(Element other)
    {
        float value;
        if (!modifiers.TryGetValue(other, out value)) value = 1;
        return value;
    }
}

[Serializable]
public class ElementModifier
{
    public Element element;
    public float value;
}