using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayTimeMenu : BaseMenu
{
    public Button PauseButton,ReplayButton;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI GoldText;
    public Image GoldImage;
    public GameObject JoyStick;

    private void Start()
    {
        PauseButton.onClick.AddListener(PauseButtonFunc);
        ReplayButton.onClick.AddListener(ReplayButtonFunc);
        JoyStick.gameObject.SetActive(false);
    }

    private void PauseButtonFunc()
    {
        EventManager.OnPause?.Invoke(true);
    }

    private void OnEnable()
    {
        DataManager.OnSetData += SetDatas;
    }

    private void OnDisable()
    {
        DataManager.OnSetData -= SetDatas;
    }

    private void SetDatas(int level, int gold)
    {
        LevelText.text = "LEVEL " + level;
        GoldText.text = gold.ToString();
    }

    public void JoyStickOption(bool o)
    {
        JoyStick.SetActive(o);
    }

    public void ReplayButtonFunc()
    {
        EventManager.RestartLevel?.Invoke();
    }
}
