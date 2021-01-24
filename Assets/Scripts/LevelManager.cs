using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    public CharacterBehaviour characterPrefab;

    [SerializeField] private Transform _characterStartPoint;

    public int characterInstantiateDelay = 3;

    private CharacterBehaviour m_Character;

    [SerializeField] private GameObject[] _levels;

    [SerializeField] private Transform _levelSpawnPoint;

    [SerializeField] private StringGameEvent _onNewLevelEvent = default;

    [SerializeField] private GameEvent _onLevelFinishedEvent = default;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    private GameObject _currentLevelGO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Invoke("InstantiateCharacter", characterInstantiateDelay);
        InstantiateCharacter();
        StartNextLevel();
        
        _onLevelFinishedEvent.AddListener(StartNextLevel);

    }


    private void StartNextLevel()
    {
        if (_currentLevelGO != null)
        {
            Destroy(_currentLevelGO);
        }
        _currentLevelGO = Instantiate(_levels[_currentLevelIndex.Value]);
        if (_levelSpawnPoint != null)
        {
            _currentLevelGO.transform.position = _levelSpawnPoint.position;
        }
        _onNewLevelEvent.Raise("LEVEL " + (_currentLevelIndex+1));

        ResetCharacterPosition();
    }


    
    private void InstantiateCharacter()
    {
        if (characterPrefab != null)
        {
            m_Character = Instantiate<CharacterBehaviour>(characterPrefab, _characterStartPoint.position, Quaternion.identity);
            GameManager.Instance.Character = m_Character;

        }
    }

    private void ResetCharacterPosition()
    {
        m_Character.transform.position = _characterStartPoint.position;
    }

    private void OnDestroy()
    {
        _onLevelFinishedEvent.RemoveListener(StartNextLevel);
    }

}
