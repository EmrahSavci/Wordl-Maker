using UnityEngine.UI;

public class MarketPlaceMenu : BaseMenu
{
    public Button DoneButton;

    private void Start()
    {
        DoneButton.onClick.AddListener(DoneButtonFunc);
        Hide();
    }

    private void DoneButtonFunc()
    {
        MarketManager.Instance.SendData();
        Menus.MarketMenu.Hide();
        Base.FinisGame(GameStat.Win, 1f);
    }
}
