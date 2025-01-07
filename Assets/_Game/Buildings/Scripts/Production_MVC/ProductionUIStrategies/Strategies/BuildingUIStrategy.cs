using UnityEngine.UI;

public class BuildingUIStrategy : UIStrategyBase
{
    public BuildingData[] buildingDatas;

    public BuildingUIStrategy(ProductionController controller, BuildingData[] datas) : base(controller)
    {
        buildingDatas = datas;
    }

    public override int DataCount => buildingDatas.Length;
    public override Data[] Data => buildingDatas;
    public override void AddListener(Button button, int dataIndex)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _controller.SelectBuilding(buildingDatas[dataIndex]));
    }

}