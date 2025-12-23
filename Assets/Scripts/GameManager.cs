using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static float point = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private GameObject[] powerUis;
    [SerializeField] private Slider slider;

    [Header("Ground")]
    [SerializeField] private GameObject ground;

    [Header("Multiplier Settings")]
    [SerializeField] private int activatePowerNb = 10;
    [SerializeField] private int multiplierUpdateRate = 30;
    [SerializeField] private float multiplierUpdateAdd = 1f;
    private float multiplier = 0.5f;
    private float timeSurvived = 0f;
    private int multiplierUpdateTime = 1;
    private bool activatePowerDecrease = false;
    private float respawnRate = 5f;
    private float maxHealth = 100f;

    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private int maxAnimal = 10;
    private int nbAnimal = 0;

    private Animator player_anim;
    private MainCharacter player;
    [SerializeField] private float hungryAmount = 0;
    
    private bool isDead = false;
    // -------------------------------------------------- POINT ------------------------------------------------
    public bool getDeath()
    {
        return isDead;
    }
    public void AddPoint(float newPoint)
    {
        if(isDead) return;
        Point+=newPoint;
        hungryAmount+=newPoint;
        slider.value = hungryAmount;
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
        int pointInt = (int)point;
        pointText.text = pointInt.ToString();
    }
    

    void Start()
    {
        player_anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>();
        slider.maxValue = maxHealth;
        StartCoroutine("SpawnEnemyRate");
    }
    
    void Update()
    {
        if (activatePowerDecrease && !isDead)
        {
            hungryAmount -= multiplier * Time.deltaTime;
            slider.value = hungryAmount;
            if (hungryAmount <= 0)
            {
                isDead = true;
                player_anim.SetTrigger("die");
            }
            timeSurvived += Time.deltaTime;
            MultiplierUpdate();
        }
        
    }
    public void RemoveAnimal()
    {
        nbAnimal--;
    }
    public void Die()
    {
        
    }
    
    private void MultiplierUpdate()
    {
        if(Point > multiplierUpdateRate*multiplierUpdateTime)
        {
            multiplierUpdateTime++;
            player.mc_changeBody(); 
            if(multiplierUpdateTime <= powerUis.Length) powerUis[multiplierUpdateTime-1].SetActive(true);
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
        if(nbAnimal >= maxAnimal*multiplierUpdateTime) return;
        nbAnimal++;
        int randomEnemy = Random.Range(0, enemyPrefabs.Count);
        float size = ground.transform.position.x-ground.GetComponent<SpriteRenderer>().bounds.size.x/2;
        float randomX = Random.Range(ground.transform.position.x-size+5f, ground.transform.position.x+size-5f);
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
    public void SetHungry(int hungryAmount)
    {
        slider.value = hungryAmount;
    }
    public float GetTimeSurvived()
    {
        return timeSurvived;
    }

}
