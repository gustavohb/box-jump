using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{

    [SerializeField] private BoxCollider2D _referenceCollider;

    private Camera _camera;                        // Used for referencing the camera.


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        float orthoSize = _referenceCollider.bounds.size.x * Screen.height / Screen.width * 0.5f;

        _camera.orthographicSize = orthoSize;
    }

}