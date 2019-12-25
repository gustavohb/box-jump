using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelCollider : MonoBehaviour
{
    public int nextLevelNumber = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            if (nextLevelNumber != 0)
            {
                GameManager.Instance.CurrentLevel = nextLevelNumber;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + nextLevelNumber.ToString());
            }
            else
            {
                GameManager.Instance.CurrentLevel = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }   
        }
    }
}
