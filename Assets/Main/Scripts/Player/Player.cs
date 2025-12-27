using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public UnityEvent OnDeath;

    [Header("Movement Settings")]

    [Range(1f, 30f)]
    [SerializeField] 
    private float _movementSpeed = 5f;

    [Range(1f, 100f)]
    [SerializeField] 
    private float _jumpForce = 5f;

    [Range(1f, 10f)]
    [SerializeField] 
    private float _gravityModifier = 1f;

    [Header("CameraSettings")]

    [SerializeField]
    private Transform _cameraRoot;

    [Range(0.1f, 10f)]
    [SerializeField]
    private float _cameraSensitivity;

    [Range(-70f, -20f)]
    [SerializeField] 
    private float _minVerticalAngle = -40f;

    [Range(20f, 70f)]
    [SerializeField] 
    private float _maxVerticalAngle = 40f;

    [Header("GameplaySettings")]

    [Range(1, 3)]
    [SerializeField]
    private int _hitpoints = 1;

    [Header("Animation Settings")]

    [SerializeField] private Animator _animator;

    private CharacterController _characterController;

    private Vector2 _movementInput;
    private Vector2 _lookInput;

    private float _cameraRotX = 0f;
    private float _cameraRotY = 0f;

    private bool _isJumping = false;

    private Vector3 _velocity;
    private float _currentJumpForce;

    public int Hitpoints
    {
        get
        {
            return _hitpoints;
        }
        private set
        {
            if (value <= 0)
            {
                _hitpoints = 0;
                OnDeath.Invoke();
            }
            else
            {
                _hitpoints = value;
            }
        }
    }
    private void Start()
    {
        if(TryGetComponent<CharacterController>(out var characterController))
        {
            _characterController = characterController;
        }

        Cursor.lockState = CursorLockMode.Locked;

        Vector3 angles = transform.rotation.eulerAngles;

        _cameraRotX = angles.x;
        _cameraRotY = angles.y;
    }

    public void OnMove(InputValue _value)
    {
        _movementInput = _value.Get<Vector2>();
    }

    public void OnLook(InputValue _value)
    {
        _lookInput = _value.Get<Vector2>();
    }

    public void OnJump()
    {
        Jump(_characterController.isGrounded);
    }

    private void Update()
    {
        ManageMovement(_movementInput);
        ManageLookDirection(_lookInput);
        ManageAnimations(_animator);
    }

    private void ManageMovement(Vector2 input)
    {
        _velocity = (transform.right * input.x * _movementSpeed) + (transform.forward * input.y * _movementSpeed) - (transform.up * _gravityModifier);

        if (_isJumping)
        {
            _currentJumpForce = Mathf.Lerp(_currentJumpForce, 0f, Time.deltaTime);
            _velocity.y += _currentJumpForce;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void ManageLookDirection(Vector2 input)
    {
        _cameraRotY += _lookInput.x * _cameraSensitivity;
        _cameraRotX += _lookInput.y * _cameraSensitivity;

        _cameraRotX = Mathf.Clamp(_cameraRotX, _minVerticalAngle, _maxVerticalAngle);

        Quaternion _cameraRootRotation = Quaternion.Euler(_cameraRotX, _cameraRotY, 0);
        Quaternion _playerRotation = Quaternion.Euler(0f, _cameraRotY, 0f);

        _characterController.transform.rotation = _playerRotation;
        _cameraRoot.rotation = _cameraRootRotation;
    }

    private void Jump(bool isGrounded)
    {
        if (!isGrounded) return;

        _isJumping = true;
        _currentJumpForce = _jumpForce;
    }

    private void ManageAnimations(Animator animator)
    {
        animator.SetFloat("Speed", _movementInput.magnitude * _movementSpeed);
        animator.SetBool("Jump", _isJumping && !_characterController.isGrounded);
        animator.SetBool("Grounded", _characterController.isGrounded);
        animator.SetBool("FreeFall", !_characterController.isGrounded && _characterController.velocity.y < -0.1f);
    }

    public void StopJumpPhase()
    {
        _isJumping = false;
    }

    public void TakeDamage(int damage)
    {
        Hitpoints -= damage;
    }
}
