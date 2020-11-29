using System;
[Serializable]
public class GameOrder{
    public bool isPefrormed { get; set; } = false;

    public virtual void StartOrder(object OrderableObject)
    {
        isPefrormed = true;
    }

    public virtual void UpdateOrder(object OrderableObject)
    {

    }
}