using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;
public class GameBase : Singleton<GameBase>
{
    [HideInInspector]
    public DataManager DataManager;
    [HideInInspector]
    public LevelManager LevelManager;
    [HideInInspector]
    public MenuManager MenuManager;
    [HideInInspector]
    public PoolManager PoolManager;
    public GameStat _GameStat => gameStat;
    [SerializeField]
    private GameStat gameStat;
    private int timer;
    public int Timer=> timer;

    protected override async void Awake()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
#endif

        base.Awake();

        DataManager = GetComponent<DataManager>();
        LevelManager = GetComponent<LevelManager>();
        PoolManager = GetComponent<PoolManager>();

        gameStat = GameStat.Start;
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        await Task.Delay(100);
        MenuManager = GetComponent<MenuManager>();
        MenuManager.Setup();

        //First DataManager
        await DataManager.CheckSave();
        LevelManager.LoadLevel();
        PoolManager.StartPool();
    }

    public void ChangeStat(GameStat stat)
    {
        gameStat = stat;
    }

    private void StartGame()
    {
        gameStat = GameStat.Playing;
        StartCoroutine(Counter());
    }

    private void OnEnable()
    {
        EventManager.OnBeforeLoadedLevel += ResetStat;
        EventManager.FirstTouch += StartGame;
    }

    protected override void OnDisable()
    {
        EventManager.OnBeforeLoadedLevel -= ResetStat;
        EventManager.FirstTouch -= StartGame;
    }

    private void ResetStat()
    {
        gameStat = GameStat.Start;
        timer = 0;
    }
    IEnumerator Counter()
    {
        while (Base.IsPlaying())
        {
            timer++;
            yield return new WaitForSeconds(1);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else Time.timeScale = 1;
        }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    float deltaTime = 0.0f;

    void OnGUI()
    {
        ShowFPS();
    }

    private void ShowFPS()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
#endif
}

public static class EventManager
{
    public static Action<GameStat> BeforeFinishGame;
    public static Action<GameStat> FinishGame;
    public static Action NextLevel;
    public static Action RestartLevel;
    public static Action FirstTouch;
    public static Action<bool> OnPause;
    public static Action OnBeforeLoadedLevel;
    public static Action OnAfterLoadedLevel;
    public static Action<Transform> FinishLine;
}

public static class Base
{
    public static void ChangeStat(GameStat stat)
    {
        GameBase.Instance.ChangeStat(stat);
    }

    public static Transform GetLevelHolder()
    {
        return GameBase.Instance.LevelManager.LevelHolder;
    }
    public static bool IsPlaying()
    {
        return GameBase.Instance._GameStat == GameStat.Playing;
    }

    public static int GetTimer()
    {
        return GameBase.Instance.Timer;
    }

    public async static void FinisGame(GameStat gameStat, float time = 0f)
    {
        if (GameBase.Instance._GameStat == GameStat.Playing |
        GameBase.Instance._GameStat == GameStat.FinishLine)
            GameBase.Instance.ChangeStat(gameStat);

        EventManager.BeforeFinishGame?.Invoke(gameStat);
        await Task.Delay((int)time * 1000);
        if (!Application.isPlaying) return;
        EventManager.FinishGame?.Invoke(gameStat);
    }

    public static void StartGameAddFunc(Action func)
    {
        EventManager.FirstTouch += func;
    }

    public static void NextLevelAddFunc(Action func)
    {
        EventManager.NextLevel += func;
    }

    public static void RestartLevelAddFunc(Action func)
    {
        EventManager.RestartLevel += func;
    }

    public static void FinishGameAddFunc(Action<GameStat> func)
    {
        EventManager.FinishGame += func;
    }

}
