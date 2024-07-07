using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 600;
    
    [SerializeField] private float detectionRange = 1;
    [SerializeField] private LayerMask playerLayer;

    [HideInInspector] public bool isAttacking;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    
    bool CanSeePlayer(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(transform.position, Vector3.right, detectionRange, playerLayer);
        return hit;
    }
    
    public bool IsHitting(out RaycastHit2D hit)
    {
        return CanSeePlayer(out hit) && isAttacking;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (CanSeePlayer(out _))
        {
            _animator.SetTrigger("Attack");
        }

        if (IsHitting(out RaycastHit2D hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                hit.transform.gameObject.GetComponent<PlayerController>().Kill();
            }
        }
        
        Move();
    }

    private void Move()
    {
        _rb.velocity = new Vector2(speed * Time.deltaTime, _rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            _animator.SetTrigger("Death");
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
