using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_TargetLocation = Vector3.zero;

    [SerializeField]
    private Ease m_MoveEase = Ease.Linear;

    
    public AudioClip m_SlideOutSound;

    [Range(0.01f, 10.0f), SerializeField]
    private float m_MoveDuration = 1.0f;

    private bool m_Enable;

    private Vector3 m_StartPos;

    
    private BoxCollider2D m_BoxCollider2D;

    void Start()
    {
        m_Enable = true;
        m_StartPos = transform.localPosition;
        if (m_TargetLocation == Vector3.zero)
        {
            m_TargetLocation = transform.localPosition;
        }
        transform.DOLocalMove(m_TargetLocation, m_MoveDuration).SetEase(m_MoveEase);
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        if (GameManager.Instance.Character == null)
        {
            return;
        }
        
        if (m_Enable && GameManager.Instance.Character.transform.position.x > m_BoxCollider2D.bounds.max.x + 2.0f)

        {
            transform.DOLocalMove(m_StartPos, m_MoveDuration);

            
            if (AudioManager.Instance != null && m_SlideOutSound != null)
            {                
                AudioManager.Instance.PlaySound(m_SlideOutSound, Vector3.zero, 0.1f);
            }
            m_Enable = false;
        }
        else if (!m_Enable && GameManager.Instance.Character.transform.position.x < transform.position.x)
        {
            transform.DOLocalMove(m_TargetLocation, m_MoveDuration).SetEase(m_MoveEase);
                       
            m_Enable = true;
        }
    }
}
