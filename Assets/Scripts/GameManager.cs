using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public int TotalDeaths {
        get
        {
            return totalDeaths;
        }
        set
        {
            totalDeaths = value;
            PlayerPrefs.SetInt("totalDeaths", totalDeaths);
        }
    }

    private float m_SavedTimeScale = 1.0f;

    public CharacterBehaviour Character { get; set; }

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
    }

    private int currentLevel;
    private int totalDeaths;

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
        totalDeaths = 0;
    }

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            currentLevel = PlayerPrefs.GetInt("currentLevel");
            if (currentLevel == 0)
            {
                currentLevel = 1;
            }
            totalDeaths = PlayerPrefs.GetInt("totalDeaths");

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

    public void IncrementTotalDeaths()
    {
        totalDeaths++;
        PlayerPrefs.SetInt("totalDeaths", totalDeaths);
    }

}
