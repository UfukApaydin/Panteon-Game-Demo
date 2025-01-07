
using Game.Unit;
using UnityEngine.UI;

public class UnitUIStrategy : UIStrategyBase
{
    public UnitData[] unitDatas;

    public UnitUIStrategy(ProductionController controller, UnitData[] datas) : base(controller)
    {
        unitDatas = datas;
    }

    public override int DataCount => unitDatas.Length;
    public override Data[] Data => unitDatas;

    public override void AddListener(Button button, int dataIndex)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _controller.ProduceSoldier(unitDatas[dataIndex]));
    }
}
