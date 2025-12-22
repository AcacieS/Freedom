using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static float point = 0;
    [Header("UI")]
    [SerializeField]private TextMeshProUGUI pointText;
    

    [Header("Multiplier Settings")]
    [SerializeField]private int activatePowerNb = 10;
    [SerializeField] private float timeSurvived = 0;
    
    [SerializeField] private int multiplierUpdateRate = 30;
    [SerializeField] private float multiplierUpdateAdd = 0.2f;
    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private int maxAnimal = 10;
    private Animator player_anim;
    private int nbAnimal = 0;
    private float multiplier = 0.1f;
    private int multiplierUpdateTime = 1;
    private bool activatePowerDecrease = false;
    private float respawnRate = 5f;
    public void AddPoint(float newPoint)
    {
        Point+=newPoint;
        //SPAWN NEW TEXT.
        //pointText.text = "+ " + point.ToString();
    }
    public float Point {
        get => point;
        set {
            if (point != value) {
                point = value;
                OnPointChanged();
            }
        }
    }
    void OnPointChanged() {
        if(!activatePowerDecrease && point >= activatePowerNb)
        {
            activatePowerDecrease = true;
        }
        pointText.text = "Point: " + point.ToString();
    }

    void Start()
    {
        player_anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        StartCoroutine("SpawnEnemyRate");
    }
    
    void Update()
    {
        if (activatePowerDecrease)
        {
            Point -= multiplier * Time.deltaTime;
            if (Point <= 0)
            {
                player_anim.SetTrigger("die");
            }
            timeSurvived += Time.deltaTime;
            MultiplierUpdate();
        }
        
    }
    
    private void MultiplierUpdate()
    {
        if(timeSurvived > multiplierUpdateRate*multiplierUpdateTime)
        {
            multiplierUpdateTime++;
            multiplier = multiplier + multiplierUpdateAdd;
        }
        
    }
    
    private IEnumerator SpawnEnemyRate()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(respawnRate);
        }
    }

    private void SpawnEnemy()
    {
        if(nbAnimal >= maxAnimal) return;
        nbAnimal++;
        int randomEnemy = Random.Range(0, enemyPrefabs.Count);
        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPos = new Vector3(randomX, 10f, 0f);
        Instantiate(enemyPrefabs[randomEnemy], spawnPos, Quaternion.identity);
    }
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            
        }
        DontDestroyOnLoad(gameObject);
        
    }
}
