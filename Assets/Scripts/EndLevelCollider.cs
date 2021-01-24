using UnityEngine;
using ScriptableObjectArchitecture;

public class EndLevelCollider : MonoBehaviour
{
    [SerializeField] private IntVariable _currentLevelIndex = default;

    [SerializeField] private GameEvent _onLevelFinished = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            _currentLevelIndex.Value++;
            _onLevelFinished.Raise();
        }
    }
}
