using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreenController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainMenu;


    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			m_MainMenu.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
