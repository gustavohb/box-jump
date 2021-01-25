using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CharacterBehaviour _characterPrefab;

    [SerializeField] private Transform _characterStartPoint;

    [SerializeField] private float _startDelay = 3;

    [SerializeField] private float _characterSpawnDelay = 3.5f;

    private CharacterBehaviour _character;

    [SerializeField] private GameObject[] _levels;

    [SerializeField] private Transform _levelSpawnPoint;

    [SerializeField] private StringGameEvent _onNewLevelEvent = default;

    [SerializeField] private GameEvent _onLevelFinishedEvent = default;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private GameEvent _onGameFinishedEvent = default;

    private GameObject _currentLevelGO;

   
    private void Start()
    {
        Invoke("StartNextLevel", _startDelay);
        Invoke("InstantiateCharacter", _characterSpawnDelay);
        _onLevelFinishedEvent.AddListener(StartNextLevel);
    }


    private void StartNextLevel()
    {
        if (_currentLevelGO != null)
        {
            Destroy(_currentLevelGO);
        }
        if (_currentLevelIndex >= _levels.Length)
        {
            _currentLevelIndex.Value = 0;
            _onGameFinishedEvent.Raise();
        }
        else
        {
            _currentLevelGO = Instantiate(_levels[_currentLevelIndex.Value]);
            if (_levelSpawnPoint != null)
            {
                _currentLevelGO.transform.position = _levelSpawnPoint.position;
            }
            _onNewLevelEvent.Raise("LEVEL " + (_currentLevelIndex + 1));

            ResetCharacterPosition();
        }  
    }


    
    private void InstantiateCharacter()
    {
        if (_characterPrefab != null)
        {
            _character = Instantiate<CharacterBehaviour>(_characterPrefab, _characterStartPoint.position, Quaternion.identity);
            GameManager.Instance.Character = _character;

        }
    }

    private void ResetCharacterPosition()
    {
        if (_character != null && _characterStartPoint != null)
        {
            _character.transform.position = _characterStartPoint.position;
        }
    }

    private void OnDestroy()
    {
        _onLevelFinishedEvent.RemoveListener(StartNextLevel);
    }

}
