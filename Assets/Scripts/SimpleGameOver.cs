using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class SimpleGameOver : MonoBehaviour
{
    public static SimpleGameOver Instance {get; private set;}
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeSurvivedText;
    public void Start(){
        finalScoreText.text = GameManager.Instance.Point.ToString();
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        finalScoreText.text = GameManager.Instance.Point.ToString();
        finalTimeSurvivedText.text = GameManager.Instance.GetTimeSurvived().ToString("F2")+"s";
    }

    public void Restart()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            
        }
        
    }
}