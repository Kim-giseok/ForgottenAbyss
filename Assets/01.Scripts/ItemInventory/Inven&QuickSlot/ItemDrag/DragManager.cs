using UnityEngine;

public class DragManager
{
    public Item Item { get; private set; }
    public IItemContainer OriginContainer { get; private set; }
    public int OriginIndex { get; private set; }

    public void Set(Item item, IItemContainer container, int index)
    {
        Item = item;
        OriginContainer = container;
        OriginIndex = index;
    }

    public void Clear()
    {
        Item = null;
        OriginContainer = null;
        OriginIndex = -1;
    }

    public bool HasItem => Item != null;
}
