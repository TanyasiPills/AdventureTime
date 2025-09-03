using System;
using UnityEngine;

public enum OperatorType { Add, Subtract, Multiply, Divide }

[Serializable]
public class BasicModifier : StatModifier
{
    [SerializeField]
    public StatType type;
    [SerializeField]
    public OperatorType operatorType;
    [SerializeField]
    public float value;
    [SerializeField]


    public BasicModifier() : base() { }

    public BasicModifier(StatType type, OperatorType operatorType, float value, Source source, int duration = 0) : base(source, duration)
    {
        this.type = type;
        this.operatorType = operatorType;
        this.value = value;
        this.source = source;
        switch (operatorType)
        {
            case OperatorType.Add:
            case OperatorType.Subtract:
                this.priority = 5;
                break;
            case OperatorType.Multiply:
            case OperatorType.Divide:
                this.priority = 10;
                break;
        }
        if (source == Source.Equipment) priority -= 100;
    }

    public override void applied()
    {
        switch (operatorType)
        {
            case OperatorType.Add:
            case OperatorType.Subtract:
                this.priority = 5;
                break;
            case OperatorType.Multiply:
            case OperatorType.Divide:
                this.priority = 10;
                break;
        }
        if (source == Source.Equipment) priority -= 100;
        base.applied();
    }

    public override float Handle(StatType type, float value)
    {
        if (this.type == type)
        {
            switch (operatorType)
            {
                case OperatorType.Add:
                    return value + this.value;
                case OperatorType.Subtract:
                    return value - this.value;
                case OperatorType.Multiply:
                    return value * this.value;
                case OperatorType.Divide:
                    return value / this.value;
                default:
                    return value;
            }
        }
        else return value;
    }
}