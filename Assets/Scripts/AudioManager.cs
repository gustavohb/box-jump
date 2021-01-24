using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioListener))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager s_Instance;

    public enum AudioChannel { Master, Sfx, Music };

    public float masterVolumePercent{ get; private set; }
    public float sfxVolumePercent{ get; private set; }
    public float musicVolumePercent{ get; private set; }

    public bool MusicOn { get; private set; }
    public bool SfxOn { get; private set; }

    [SerializeField, Range(0, 1)]
    private float defaultMusicVolumePercent = 0.3f;

    private AudioSource m_Sfx2DSource;
    private AudioSource[] m_MusicSources;
    private int m_ActiveMusicSourceIndex;

    Transform m_AudioListenerTransform;


    public static AudioManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<AudioManager>();
                if (s_Instance == null)
                {
                    GameObject obj = new GameObject();
                    s_Instance = obj.AddComponent<AudioManager>();
                }
            }

            return s_Instance;
        }
    }

    void Awake()
    {

        if (s_Instance == null)
        {
            s_Instance = this;

            m_MusicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("MusicSource " + (i + 1));
                m_MusicSources[i] = newMusicSource.AddComponent<AudioSource>();
                m_MusicSources[i].loop = true;
                newMusicSource.transform.parent = transform;

            }
            GameObject newSfx2Dsource = new GameObject("2DSfxSource");
            m_Sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
            newSfx2Dsource.transform.parent = transform;

            //m_AudioListenerTransform = FindObjectOfType<AudioListener>().transform;

            m_AudioListenerTransform = transform;

            masterVolumePercent = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            sfxVolumePercent = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
            musicVolumePercent = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolumePercent);


            MusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

            SfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;

            
            if (MusicOn)
            {
                SetVolume(musicVolumePercent, AudioChannel.Music);
            }
            else
            {
                SetVolume(0.0f, AudioChannel.Music);
            }
            

            if (SfxOn)
            {
                SetVolume(sfxVolumePercent, AudioChannel.Sfx);
            }
            else
            {
                SetVolume(0.0f, AudioChannel.Sfx);
            }

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

    public void MuteMusic()
    {
        if (MusicOn)
        {

            MusicOn = false;
            m_MusicSources[m_ActiveMusicSourceIndex].Stop();
            SetVolume(0.0f, AudioChannel.Music);

            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            MusicOn = true;


            SetVolume(defaultMusicVolumePercent, AudioChannel.Music);
            m_MusicSources[m_ActiveMusicSourceIndex].Play();

            PlayerPrefs.SetInt("MusicOn", 1);

        }

        PlayerPrefs.Save();
    }

    public void MuteSfx()
    {
        if (SfxOn)
        {
            SfxOn = false;
            SetVolume(0.0f, AudioChannel.Sfx);
            PlayerPrefs.SetInt("SfxOn", 0);
        }
        else
        {
            SfxOn = true;
            SetVolume(1.0f, AudioChannel.Sfx);
            PlayerPrefs.SetInt("SfxOn", 1);  
        }

        PlayerPrefs.Save();
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        m_MusicSources[0].volume = musicVolumePercent * masterVolumePercent;
        m_MusicSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("MasterVolume", masterVolumePercent);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolumePercent);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1.0f)
    {
        m_ActiveMusicSourceIndex = 1 - m_ActiveMusicSourceIndex;
        m_MusicSources[m_ActiveMusicSourceIndex].clip = clip;
        m_MusicSources[m_ActiveMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, m_AudioListenerTransform.position, sfxVolumePercent * masterVolumePercent);
        }
    }


    public void PlaySound(AudioClip clip, Vector3 pos, float volume)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, volume * sfxVolumePercent * masterVolumePercent);
        }
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            m_MusicSources[m_ActiveMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            m_MusicSources[1 - m_ActiveMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }

        m_MusicSources[1 - m_ActiveMusicSourceIndex].Stop();
    }
}
