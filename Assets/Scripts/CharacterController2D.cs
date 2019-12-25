using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{

    public LayerMask collisionMask;
    private Transform characterMeshTransform;
    public float skinWidth = .015f;
    public float rotationSpeed = 0.1f;

    [HideInInspector]
    public bool isGrounded;

    private BoxCollider2D m_Collider;
    private Vector3 m_StartPosition;

    private void Awake()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        m_StartPosition = transform.position;
        characterMeshTransform = GetComponentInChildren<MeshRenderer>().transform;
    }


    public void Move(Vector3 velocity)
    {
        isGrounded = false;

        if (velocity.y != 0)
        {
            VerticalCollision(ref velocity);
        }

        transform.Translate(velocity);

        if (!isGrounded)
        {    
            characterMeshTransform.RotateAroundLocal(Vector3.forward, -Mathf.Sign(velocity.x) * rotationSpeed);
        }
        else
        {
            characterMeshTransform.SetPositionAndRotation(characterMeshTransform.position, Quaternion.identity);
        }

    }

    void VerticalCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        Bounds bounds = m_Collider.bounds;
        Vector2 rayOrigin = new Vector2(bounds.min.x, bounds.min.y);
        rayOrigin += Vector2.right * velocity.x;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);


        if (hit)
        {
            velocity.y = (hit.distance - skinWidth) * directionY;
            rayLength = hit.distance;

            isGrounded = directionY == -1;
        }

        Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.magenta);
    }
  
    public void ResetToStartPosition()
    {
        transform.position = m_StartPosition;
    }

}
