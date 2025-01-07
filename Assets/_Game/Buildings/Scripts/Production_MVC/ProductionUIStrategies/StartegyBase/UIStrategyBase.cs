using UnityEngine.UI;

public abstract class UIStrategyBase
{
    protected ProductionController _controller;
    public abstract Data[] Data { get; }
    public abstract int DataCount { get; }

    public abstract void AddListener(Button button, int dataIndex);

    public UIStrategyBase(ProductionController controller)
    {
        _controller = controller;
    }
}

