using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public Button ShakeButton;
    public Button SoundButton;
    public Button ExitButton;
    public Button ReplayButton;
    private void Start()
    {
        ShakeButton.onClick.AddListener(ShakeButtonFunc);
        SoundButton.onClick.AddListener(SoundButtonFunc);
        ExitButton.onClick.AddListener(ExitButtonFunc);
        ReplayButton.onClick.AddListener(ReplayButtonFunc);
    }

    private void ShakeButtonFunc()
    {
        Haptics.hapticOff = !Haptics.hapticOff;
        if (Haptics.hapticOff)
            ShakeButton.GetComponent<Image>().color = Color.gray;
        else
            ShakeButton.GetComponent<Image>().color = Color.white;
    }

    private void SoundButtonFunc()
    {
        AudioManager.Instance.stopSound = !AudioManager.Instance.stopSound;

        if (AudioManager.Instance.stopSound)
            SoundButton.GetComponent<Image>().color = Color.gray;
        else
            SoundButton.GetComponent<Image>().color = Color.white;
    }

    private void ExitButtonFunc()
    {
        EventManager.OnPause.Invoke(false);
    }

    public void ReplayButtonFunc()
    {
        Time.timeScale = 1;
        Hide();
        EventManager.RestartLevel?.Invoke();
    }
}
