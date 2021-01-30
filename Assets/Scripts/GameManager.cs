using UnityEngine;
using ScriptableObjectArchitecture;

public static class GameTime
{
    public static bool isPaused = false;
    public static float deltaTime { get { return isPaused ? 0 : Time.deltaTime; } }
} 

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public int targetFrameRate = 60;

    public float timeScale { get; private set; }

    public bool paused { get; set; }

    public bool continueWasEnabled { get; set; }

    [SerializeField] private IntVariable _totalDeaths = default;
    
    private float m_SavedTimeScale = 1.0f;

    public CharacterBehaviour Character { get; set; }

    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private GameEvent _levelFinishedEvent = default;


    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _currentLevelIndex.Value = PlayerPrefs.GetInt("currentLevel");

            _totalDeaths.Value = PlayerPrefs.GetInt("totalDeaths");

            Application.targetFrameRate = targetFrameRate;

            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        _levelFinishedEvent.AddListener(SaveGame);
    }

    public void SetTimeScale(float newTimeScale)
    {
        m_SavedTimeScale = Time.timeScale;
        Time.timeScale = newTimeScale;
    }

    public virtual void ResetTimeScale()
    {
        Time.timeScale = m_SavedTimeScale;
    }

    public void Pause()
    {
        if (Time.timeScale > 0.0f)
        {
            Instance.SetTimeScale(0.0f);
            Instance.paused = true;
        }
        else
        {
            UnPause();
        }
    }

    public void UnPause()
    {
        Instance.ResetTimeScale();
        Instance.paused = false;

    }

    public void Reset()
    {
        _totalDeaths.Value = 0;
    }


    private void SaveGame()
    {
        PlayerPrefs.SetInt("totalDeaths", _totalDeaths.Value);
        PlayerPrefs.SetInt("currentLevel", _currentLevelIndex.Value);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
