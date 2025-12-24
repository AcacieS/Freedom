using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;

public class SimpleGameOver : MonoBehaviour
{
    public static SimpleGameOver Instance {get; private set;}
    [SerializeField] private GameObject[] Covers;
    [SerializeField] private GameObject gameOverPanel;
    public Animator transition;
    public float transitionTime = 1f;
    
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeSurvivedText;
    public void Start()
    {
        
    }
    private int index = 0;
    public void NextCover()
    {
        
        if(index >= Covers.Length-1)
        {
            StartCoroutine(LoadLevel("Scenes/Game"));
        }
        else
        {
            Covers[index].SetActive(false);
            index++;
            Covers[index].SetActive(true);
        }
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = GameManager.Instance.Point.ToString();
        finalTimeSurvivedText.text = GameManager.Instance.GetTimeSurvived().ToString("F2")+"s";
    }

    public void Restart()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetGame();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetGame();
        StartCoroutine(LoadLevel("Scenes/Menu"));
    }
    IEnumerator LoadLevel(string levelName)
    {
        Debug.LogWarning("Load next level");
        transition.SetTrigger("Start");
        Debug.LogWarning("Doing Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);

    }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            
        }
        
    }
}