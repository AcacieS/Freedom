using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    private Vector3 startScale;
    [SerializeField] private float moveSpeed = 5f;
    private EnemyInfo currentEnemyInfo;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        AttackAnim();
    }
    void AttackAnim()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            anim.SetTrigger("Attack");
            Attack();
        }
    }
    void Attack()
    {
        if(currentEnemyInfo == null) return;
        GameManager.Instance.AddPoint(currentEnemyInfo.point);
    }
    private void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        anim.SetFloat("PlayerSpeed", Mathf.Abs(rb.linearVelocityX));
        anim.SetFloat("PlayerY", rb.linearVelocityY);
        if (moveInput > 0.01f)
        {
            transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (moveInput < -0.01f)
        {
            transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
        }
        
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            currentEnemyInfo = other.gameObject.GetComponent<Enemy>().GetEnemyInfo();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(currentEnemyInfo == other.gameObject.GetComponent<Enemy>().GetEnemyInfo())
            {
                currentEnemyInfo = null;
            }
        }
    }
}
