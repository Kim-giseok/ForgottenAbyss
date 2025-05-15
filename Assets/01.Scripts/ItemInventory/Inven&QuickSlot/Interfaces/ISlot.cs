

public interface ISlot
{
    Item Item { get; }
    int Quantity { get; }

    bool IsEmpty { get; }
    bool CanStack(Item item);
    bool Use();
}
