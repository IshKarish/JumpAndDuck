using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _canMove = true;

    [Header("Speeds")]
    [SerializeField] private float speed = 600;
    [SerializeField] private float jumpForce = 5000;

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
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) Jump();
        if (Input.GetKeyDown(KeyCode.C) && !IsGrounded()) Roll();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
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
        _rb.AddForce(new Vector2(0, -jumpForce));
        
        _canMove = false;
        _animator.SetTrigger(rollTrigger);
    }

    public void UnlockMovement()
    {
        _canMove = true;
    }
}
