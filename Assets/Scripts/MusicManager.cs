using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public AudioClip mainTheme;
    public AudioClip menuTheme;

    private static MusicManager s_Instance;

    public static MusicManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<MusicManager>();
                if (s_Instance == null)
                {
                    GameObject obj = new GameObject();
                    s_Instance = obj.AddComponent<MusicManager>();
                }
            }

            return s_Instance;
        }
    }



    [SerializeField]
    private float m_MusicFadeDuration = 0.4f;

    private float m_InvokeDelay = 0.2f;
    private string m_SceneName;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;

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

    void Start()
    {
        OnLevelWasLoaded();
    }

    public void OnLevelWasLoaded()
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != m_SceneName)
        {
            m_SceneName = newSceneName;
            Invoke("PlayMusic", m_InvokeDelay);
        }
    }

    public void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if (m_SceneName == "Menu")
        {
            clipToPlay = menuTheme;
        }
        else if (m_SceneName == "Gameplay")
        {
            clipToPlay = mainTheme;
        }

        if (clipToPlay != null)
        {
            AudioManager.Instance.PlayMusic(clipToPlay, m_MusicFadeDuration);
            Invoke("PlayMusic", clipToPlay.length);
        }

    }

}