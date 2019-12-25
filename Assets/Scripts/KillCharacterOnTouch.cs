using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillCharacterOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterBehaviour character = collision.GetComponent<CharacterBehaviour>();

        if (character != null)
        {
            character.Kill();
        }
    }

}
