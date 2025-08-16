using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text UI_HighScroe;
    [SerializeField] private TMP_Text UI_PlayerName;
    [SerializeField] private TMP_Text UI_Score;
    [SerializeField] private TMP_Text UI_Round;
    [SerializeField] private GameObject gameOver;


    private string topScoresName;
    private int topScores;
    private int score;
    private string playerName;
    private int round = 0;


    void Start()
    {
        topScoresName = DataManagement.Instance.TopScoresName();
        topScores = DataManagement.Instance.TopScores();
        playerName = DataManagement.Instance.player;
        UpdateTopScore();
        
        UI_PlayerName.text = playerName;
        UI_Score.text = $"{score}";
        UI_Round.text = $"{round}";
    }
    void Update()
    {
        UpdateTopScore();
    }

    public void IncrementRound()
    {
        round++;
        UI_Round.text = $"Wave : {round}";
    }
    public void UpdateTopScore()
    {
        score = DataManagement.Instance.ScoreGettersetter;
        if (topScores < score)
        {
            topScoresName = playerName;
            topScores = score;
        }
        UI_HighScroe.text = $"High Score : {topScoresName} : {topScores}";
        UI_Score.text = $"{score}";
    }

    public void SceneSelecter(int Scene)
    {
        SceneManager.LoadScene(Scene);
        DataManagement.Instance.ScoreGettersetter = 0;
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
    }
}
