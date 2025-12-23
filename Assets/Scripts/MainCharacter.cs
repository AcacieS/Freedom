using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class MainCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    private Vector3 startScale;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject scoreUIPrefab;
    
    private Dictionary<GameObject, EnemyInfo> enemiesInRange = new Dictionary<GameObject, EnemyInfo>();
    private Dictionary<GameObject, EnemyInfo> enemiesDie = new Dictionary<GameObject, EnemyInfo>();
    // private List<EnemyGO> currentEnemies = new List<EnemyGO>();
    private float showDuration = 1.2f;
    private Vector3 moveUp = new Vector3(0, 5f, 0);
    [SerializeField] private Color endColor = new Color(1, 1, 1, 0);
    [SerializeField] private AnimatorOverrideController[] mcAnimOverride;
    private int currentState = -1;
    [SerializeField] private ParticleSystem bloodParticles;
    
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
        if(enemiesInRange.Count == 0) return;
        enemiesDie = new Dictionary<GameObject, EnemyInfo>();
        foreach(KeyValuePair<GameObject, EnemyInfo> enemy in enemiesInRange)
        {
            GameManager.Instance.AddPoint(enemy.Value.point);
            enemy.Key.GetComponent<Animator>().SetTrigger("die");
            InitScore(enemy.Value.point, enemy.Key);
            SpawnParticle(enemy.Key);
            enemiesDie.Add(enemy.Key, enemy.Value);
        }
        foreach(KeyValuePair<GameObject, EnemyInfo> enemy in enemiesDie)
        {
            enemiesInRange.Remove(enemy.Key);
        }

        Debug.Log("Enemy Defeated");
    }
    private void SpawnParticle(GameObject enemy)
    {
        ParticleSystem ps = Instantiate(bloodParticles, enemy.transform.position, Quaternion.identity);
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
    private void InitScore(float point, GameObject enemy)
    {
        GameObject scoreUI = Instantiate(scoreUIPrefab, transform.position, Quaternion.identity);
        TextMeshPro scoreText = scoreUI.GetComponent<TextMeshPro>();
        scoreText.text = "+"+point.ToString();
        StartCoroutine(FadeAndMove(enemy, scoreUI, scoreText));
    }

    private IEnumerator FadeAndMove(GameObject enemy, GameObject scoreUI, TextMeshPro scoreText)
    {
        Vector3 startPos = enemy.transform.position;
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
        Destroy(scoreUI);
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
            if (!enemiesInRange.ContainsKey(other.gameObject) && !enemiesDie.ContainsKey(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject, other.gameObject.GetComponent<Enemy>().GetEnemyInfo());
            }
            
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!enemiesInRange.ContainsKey(other.gameObject) && !enemiesDie.ContainsKey(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject, other.gameObject.GetComponent<Enemy>().GetEnemyInfo());
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (enemiesInRange.ContainsKey(other.gameObject) && !enemiesDie.ContainsKey(other.gameObject))
            {
                enemiesInRange.Remove(other.gameObject);
            }
        }
    }
}
