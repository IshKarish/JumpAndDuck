using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _canMove = true;
    private float _playerScale;
    private Sprite _loadedSkin;
    private float _horizontal;

    [SerializeField] private Sprite godSkin;
    
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

    [Header("Controls")] 
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject buttons;
    
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

        SpriteRenderer sp = GetComponent<SpriteRenderer>();

        if (!MainMenu.InGodMod)
        {
            _loadedSkin = SkinsManager.LoadSkinSprite();
            if (_loadedSkin)
            {
                sp.sprite = _loadedSkin;
                GetComponent<Animator>().enabled = false;
            }
            else sp.drawMode = SpriteDrawMode.Simple;
        }
        else
        {
            sp.sprite = godSkin;
            GetComponent<Animator>().enabled = false;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            string controls = PlayerPrefs.GetString("Controls");
            if (controls == "JOYSTICK")
            {
                joystick.SetActive(true);
                buttons.SetActive(false);
            }
            else if (controls == "BUTTONS")
            {
                joystick.SetActive(false);
                buttons.SetActive(true);
            }
        }
        else
        {
            joystick.SetActive(false);
            buttons.SetActive(false);
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            float vertical = joystick.GetComponent<Joystick>().Vertical;
        
            if (vertical >= .5f && IsGrounded()) Jump();
            if (vertical <= -.5f && !IsGrounded()) Roll();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsGrounded()) Jump();
                else Roll();
            }
        }

        if (transform.position.y <= -2) Restart();

        if (GameObject.FindGameObjectsWithTag("Collectable").Length == 0)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void JumpButtons()
    {
        if (IsGrounded()) Jump();
        else Roll();
    }

    private void FixedUpdate()
    {
        if (_canMove && joystick.activeInHierarchy) _horizontal = joystick.GetComponent<Joystick>().Horizontal;
        if (Application.platform != RuntimePlatform.Android) _horizontal = Input.GetAxisRaw("Horizontal");

        if (_horizontal > 0) transform.localScale = new Vector2(_playerScale, _playerScale);
        else if (_horizontal < 0) transform.localScale = new Vector2(-_playerScale, _playerScale);
        
        if (_canMove) MoveHorizontal(_horizontal);
    }

    public void MoveHorizontal(int axis)
    {
        _horizontal = axis;
        MoveHorizontal((float)axis);
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

        if (!_loadedSkin)
        {
            _canMove = false;
            _animator.SetTrigger(rollTrigger);
        }
    }

    public void UnlockMovement()
    {
        _canMove = true;
    }

    public void Kill()
    {
        if (GetComponent<SpriteRenderer>().drawMode == SpriteDrawMode.Simple) _animator.SetTrigger("Death");
        
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<BoxCollider2D>().enabled = false;
        
        this.enabled = false;

        Invoke(nameof(Restart), 3);
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
