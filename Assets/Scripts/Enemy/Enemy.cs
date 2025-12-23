using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfo enemyInfo;
    private int direction = 1;
    private bool isResting = false;
    private Vector3 startScale;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    CinemachineImpulseSource impulseSource ;
    public virtual void Start()
    {
        int randomDir = Random.Range(0,2);
        direction = randomDir == 0 ? -1 : 1;
        StartCoroutine(RestLoop());
        startScale = transform.localScale;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = Random.Range(0, 2) == 0? 1: -1;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public EnemyInfo GetEnemyInfo()
    {
        return enemyInfo;
    }
    public void Update()
    {
        if(isDead) return;
        Movement();
    }
    public void ChangeDirection()
    {
        direction *= -1;
    }
    public virtual void Movement()
    {
        if (!isResting)
        {
            if(direction == -1)
            {
                transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
            }
            else
            {
                transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
            }
            transform.GetComponent<Rigidbody2D>().linearVelocityX = direction * enemyInfo.speed;
            
        }
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocityX));
    }
    
    public void Attacked()
    {
        GetComponent<Animator>().SetTrigger("die");
        isDead = true;
        transform.GetComponent<Rigidbody2D>().linearVelocityX = 0;
        impulseSource.GenerateImpulse();
        
    }
    
    private IEnumerator RestLoop()
    {
        while (true && !isDead)
        {
            // Wait before resting
            float randomRate = Random.Range(enemyInfo.minRestRate, enemyInfo.maxRestRate);
            yield return new WaitForSeconds(randomRate);

            // Rest
            isResting = true;
            float randomRest = Random.Range(enemyInfo.minRest, enemyInfo.maxRest);
            yield return new WaitForSeconds(randomRest);
            int randomDir = Random.Range(0,2);
            direction = randomDir == 0 ? -1 : 1;
            isResting = false;
        }
    }
    
    
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}