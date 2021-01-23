using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(CharacterController2D))]
public class CharacterBehaviour : MonoBehaviour
{
    public float jumpHeight = 3.0f;
    public float timeToJumpApex = 0.3f;
    public float moveSpeed = 8.0f;
    public GameObject deathEffect;

    public bool pauseCheck = true;

    public AudioClip jumpSfx;
    public AudioClip deathSfx;

    float m_Gravity;
    float m_JumpVelocity;
    Vector3 m_Velocity;
    bool m_JumpPressed;
    CharacterController2D m_Controller;

    [SerializeField] private IntVariable _totalDeaths;

    private void Start()
    {

        m_Controller = GetComponent<CharacterController2D>();

        m_Gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        m_JumpVelocity = Mathf.Abs(m_Gravity) * timeToJumpApex;
        m_JumpPressed = false;
    }

    private void FixedUpdate()
    {
        if (GameTime.isPaused)
        {
            return;
        }
        if (m_Controller.isGrounded && !m_JumpPressed)
        {
            m_Velocity.y = 0;
        }

        m_Velocity.x = moveSpeed;
        m_Velocity.y += m_Gravity * GameTime.deltaTime;
        m_Controller.Move(m_Velocity * GameTime.deltaTime);
        m_JumpPressed = false;
    }

    private void Update()
    {
        if (GameTime.isPaused)
        {
            return;
        }
        if (pauseCheck && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)|| Input.GetKeyDown(KeyCode.Space)) && m_Controller.isGrounded)
        {
            m_JumpPressed = true;
            m_Velocity.y = m_JumpVelocity;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(jumpSfx);
            }
        }
    }

    public void Kill()
    {
        if (deathEffect != null)
        {
            Destroy(Instantiate(deathEffect, transform.position, Quaternion.FromToRotation(Vector2.left, transform.position)) as GameObject, 2);
        }  
        if (deathSfx != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(deathSfx);
        }
        m_Controller.ResetToStartPosition();
        m_Velocity.y = 0;
        _totalDeaths.Value++;
    }
}