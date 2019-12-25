using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    public CharacterBehaviour characterPrefab;

    public Vector3 characterStartPoint;

    public int characterInstantiateDelay = 3;

    private CharacterBehaviour m_Character;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke("InstantiateCharacter", characterInstantiateDelay);
    }

    private void InstantiateCharacter()
    {
        if (characterPrefab != null)
        {
            m_Character = Instantiate<CharacterBehaviour>(characterPrefab, characterStartPoint, Quaternion.identity);
            GameManager.Instance.Character = m_Character;

        }
    }

}
