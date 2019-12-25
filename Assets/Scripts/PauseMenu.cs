using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_QuitButton;

    private void Start()
    {
#if UNITY_WEBGL
     m_QuitButton.SetActive(false);
#endif
    }
}
