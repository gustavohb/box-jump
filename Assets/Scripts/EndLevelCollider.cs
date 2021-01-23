using UnityEngine;
using ScriptableObjectArchitecture;

public class EndLevelCollider : MonoBehaviour
{
    [SerializeField] private bool _isLastLevel = false;

    [SerializeField] private IntVariable _currentLevelIndex = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            if (!_isLastLevel)
            {
                _currentLevelIndex.Value++;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + _currentLevelIndex.ToString());
            }
            else
            {
                _currentLevelIndex.Value = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }   
        }
    }
}
