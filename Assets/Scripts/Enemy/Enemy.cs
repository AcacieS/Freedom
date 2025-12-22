using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfo enemyInfo;
    private int direction = 1;
    private bool isResting = false;
    public virtual void Start()
    {
        int randomDir = Random.Range(0,2);
        direction = randomDir == 0 ? -1 : 1;
        StartCoroutine(RestLoop());
    }
    public EnemyInfo GetEnemyInfo()
    {
        return enemyInfo;
    }
    public void Update()
    {
        Movement();
    }
    public virtual void Movement()
    {
        if (!isResting)
        {
            transform.GetComponent<Rigidbody2D>().linearVelocityX = direction * enemyInfo.speed;
        }
        
    }
    private IEnumerator RestLoop()
    {
        while (true)
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
}