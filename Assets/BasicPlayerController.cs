using Mirror;
using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioListener _listener;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController _controller;
    private NetworkIdentity _identity;

    private float _cameraRotationX = 0f;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private bool _jumpRequested = false; // Flag to handle jump requests between frames

    private float _horizontalInput;
    private float _verticalInput;
    private float _mouseX;
    private float _mouseY;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _identity = GetComponent<NetworkIdentity>();

        _camera.enabled = _identity.isLocalPlayer;
        _listener.enabled = _identity.isLocalPlayer;
    }

    void Update()
    {
        if (!_identity.isLocalPlayer) return;

        // Gather input in Update for responsiveness
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        // Set jump request flag when Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpRequested = true;
        }

        _mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 100f;
        _mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 100f;

        // Camera rotation
        transform.Rotate(Vector3.up * _mouseX);
        _cameraRotationX -= _mouseY;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -90f, 90f);
        _camera.transform.localRotation = Quaternion.Euler(_cameraRotationX, 0f, 0f);
    }

    void FixedUpdate()
    {
        if (!_identity.isLocalPlayer) return;

        _groundedPlayer = _controller.isGrounded;

        // Reset velocity when grounded
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -0.5f; // Small negative value prevents floating
        }

        // Movement
        Vector3 move = transform.right * _horizontalInput + transform.forward * _verticalInput;

        // Normalize if moving diagonally to prevent faster diagonal movement
        if (move.magnitude > 1f)
            move.Normalize();

        _controller.Move(move * Time.fixedDeltaTime * moveSpeed);

        // Jumping - process the request flag
        if (_groundedPlayer && _jumpRequested)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            _jumpRequested = false; // Reset the jump request
        }

        // Apply gravity
        _playerVelocity.y += gravityValue * Time.fixedDeltaTime;
        _controller.Move(_playerVelocity * Time.fixedDeltaTime);
    }
}
