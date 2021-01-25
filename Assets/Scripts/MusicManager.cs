using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    [SerializeField] private AudioClip _mainMenuTheme;
    [SerializeField] private AudioClip _gameplayTheme;

    private static MusicManager _instance;

    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<MusicManager>();
                }
            }

            return _instance;
        }
    }


    [SerializeField] private float _musicFadeDuration = 0.4f;

    private float _invokeDelay = 0.2f;

    private string _sceneName;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != _sceneName)
        {
            _sceneName = newSceneName;
            Invoke("PlayMusic", _invokeDelay);
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

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

    public void PlayMusic()
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        AudioClip clipToPlay = null;

        if (newSceneName == "Menu")
        {
            clipToPlay = _mainMenuTheme;
        }
        else if (newSceneName == "Gameplay")
        {
            clipToPlay = _gameplayTheme;
        }

        if (clipToPlay != null)
        {
            AudioManager.Instance.PlayMusic(clipToPlay, _musicFadeDuration);
        }

    }

}