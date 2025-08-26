using UnityEngine;

public class BasicModifier : StatModifier
{
    public enum OperatorType { Add, Subtract, Multiply, Divide }
    public StatType type;
    public OperatorType operatorType;
    public int value;

    public BasicModifier(StatType type, OperatorType operatorType, int value, int duration = -999) : base(duration)
    {
        this.type = type;
        this.operatorType = operatorType;
        this.value = value;
        switch (operatorType)
        {
            case OperatorType.Add:
            case OperatorType.Subtract:
                this.priority = 1;
                break;
            case OperatorType.Multiply:
            case OperatorType.Divide:
                this.priority = 5;
                break;
        }
    }

    public override int Handle(StatType type, int value)
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