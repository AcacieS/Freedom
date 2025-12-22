using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    private Vector3 startScale;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject scoreUIPrefab;
    private TextMeshPro scoreText;
    private GameObject scoreUI;
    private EnemyInfo currentEnemyInfo;
    private GameObject currentEnemy;
    private float showDuration = 1.2f;
    private Vector3 moveUp = new Vector3(0, 5f, 0);
    [SerializeField] private Color endColor = new Color(1, 1, 1, 0);
    void Start()
    {
        moveUp = new Vector3(0, transform.position.y + 5f, 0);
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
            
            anim.SetTrigger("attack");
        }
    }
    public void Attack()
    {
        if(currentEnemyInfo == null) return;
        GameManager.Instance.AddPoint(currentEnemyInfo.point);
        Debug.Log("Enemy Defeated");
        currentEnemy.GetComponent<Animator>().SetTrigger("die");
        initScore(currentEnemyInfo.point);
        currentEnemy = null;
        currentEnemyInfo = null;
    }
    public void initScore(float point)
    {
        scoreUI = Instantiate(scoreUIPrefab, transform.position, Quaternion.identity);
        scoreText = scoreUI.GetComponent<TextMeshPro>();
        scoreText.text = "+"+point.ToString();
        StartCoroutine(FadeAndMove());
    }
    
    
    private IEnumerator FadeAndMove()
{
    Vector3 startPos = currentEnemy.transform.position;
    Vector3 targetPos = startPos + moveUp;
    Color startColor = scoreText.color;

    float timePassed = 0f;

    while (timePassed < showDuration)
    {
        float t = timePassed / showDuration;

        scoreUI.transform.position = Vector3.Lerp(startPos, targetPos, t);
        scoreText.color = Color.Lerp(startColor, endColor, t);

        timePassed += Time.deltaTime;
        yield return null;
    }
}
    private void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
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
            Debug.Log("Enemy in range");
            currentEnemy = other.gameObject;
            currentEnemyInfo = other.gameObject.GetComponent<Enemy>().GetEnemyInfo();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(currentEnemyInfo == other.gameObject.GetComponent<Enemy>().GetEnemyInfo())
            {
                currentEnemy = null;
                currentEnemyInfo = null;
            }
        }
    }
}
