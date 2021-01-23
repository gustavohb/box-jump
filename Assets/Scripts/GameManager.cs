using UnityEngine;
using ScriptableObjectArchitecture;

public static class GameTime
{
    public static bool isPaused = false;
    public static float deltaTime { get { return isPaused ? 0 : Time.deltaTime; } }
} 

public class GameManager : MonoBehaviour
{
    private static GameManager s_Instance;

    public int targetFrameRate = 60;

    public float timeScale { get; private set; }

    public bool paused { get; set; }

    public bool continueWasEnabled { get; set; }

    [SerializeField] private IntVariable _totalDeaths = default;
    
    private float m_SavedTimeScale = 1.0f;

    public CharacterBehaviour Character { get; set; }

    [SerializeField] private IntVariable _currentLevelIndex = default;


    public static GameManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<GameManager>();
                if (s_Instance == null)
                {
                    GameObject obj = new GameObject();
                    s_Instance = obj.AddComponent<GameManager>();
                }
            }

            return s_Instance;
        }
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

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            _currentLevelIndex.Value = PlayerPrefs.GetInt("currentLevel");
            if (_currentLevelIndex.Value == 0)
            {
                _currentLevelIndex.Value = 1;
            }
            _totalDeaths.Value = PlayerPrefs.GetInt("totalDeaths");

            Application.targetFrameRate = targetFrameRate;

            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            if (this != s_Instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("totalDeaths", _totalDeaths.Value);
        PlayerPrefs.SetInt("currentLevel", _currentLevelIndex.Value);
        PlayerPrefs.Save();
    }
}
