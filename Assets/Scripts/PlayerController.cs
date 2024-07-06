using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _canMove = true;
    private float _playerScale;

    [SerializeField] private Joystick joystick;
    
    [Header("Speeds")]
    [SerializeField] private float speed = 100;
    [SerializeField] private float jumpForce = 300;
    [SerializeField][Tooltip("That's hard to understand from the name so i meant how much force to apply when going from air to roll")] private float downRollForce = 600;

    [Header("Ground Check")] 
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Animator stuff")]
    [SerializeField] private string runBool = "Run";
    [SerializeField] private string rollTrigger = "Roll";
    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void Awake()
    {
        _playerScale = transform.localScale.x;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        float vertical = joystick.Vertical;
        
        if (vertical >= .5f && IsGrounded()) Jump();
        if (vertical <= -.5f && !IsGrounded()) Roll();
    }

    private void FixedUpdate()
    {
        float horizontal = joystick.Horizontal;

        if (horizontal > 0) transform.localScale = new Vector2(_playerScale, _playerScale);
        else if (horizontal < 0) transform.localScale = new Vector2(-_playerScale, _playerScale);
        
        if (_canMove) MoveHorizontal(horizontal);
    }

    private void MoveHorizontal(float axis)
    {
        float xVelocity = (axis * speed) * Time.deltaTime; 
        _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);

        if (xVelocity != 0) _animator.SetBool(runBool, true);
        else _animator.SetBool(runBool, false);
    }

    private void Jump()
    {
        _rb.AddForce(new Vector2(0, jumpForce));
    }

    private void Roll()
    {
        _rb.AddForce(new Vector2(0, -downRollForce));
        
        _canMove = false;
        _animator.SetTrigger(rollTrigger);
    }

    public void UnlockMovement()
    {
        _canMove = true;
    }
}
