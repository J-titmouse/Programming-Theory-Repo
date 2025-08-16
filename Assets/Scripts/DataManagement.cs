using System.IO;
using UnityEngine;

public class DataManagement : MonoBehaviour
{
    public static DataManagement Instance;

    private int[] topScores = new int[5];
    private string[] topScoresName = {"Player", "Player", "Player", "Player", "Player",};
    private string playerName = "";
    private int Scores;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadFromFile();
    }

    public string player 
    {
        get { return playerName; }
        set
        {
            playerName = value;
        }
    }

    public string scoresboardNames
    {
        get
        {
            string tempText = "";
            for (int i = 0; i < topScoresName.Length; i++)
            {
                tempText = tempText + topScoresName[i] + "\n";
            }
            return tempText;
        }
    }
    public string scoresboardScores
    {
        get
        {
            string tempText = "";
            for (int i = 0; i < topScores.Length; i++)
            {
                tempText = tempText + topScores[i] + "\n";
            }
            return tempText;
        }
    }

    public void AddToScore()
    {
        Scores++;
    }

    public void newentry()
    {
        NewEntry(Scores, playerName);
    }

    private void NewEntry(int playerScore, string name)
    {
        bool once = false;
        int tempScore;
        string tempName;
        for (int i = 0; i < topScores.Length; i++)
        {
            if (playerScore > topScores[i])
            {
                tempScore = topScores[i];
                topScores[i] = playerScore;
                playerScore = tempScore;

                tempName = topScoresName[i];
                topScoresName[i] = name;
                name = tempName;
                once = true;
            }
            else if (playerScore == topScores[i] && once)
            {
                tempScore = topScores[i];
                topScores[i] = playerScore;
                playerScore = tempScore;

                tempName = topScoresName[i];
                topScoresName[i] = name;
                name = tempName;
                once = true;
            }
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int[] topScores = new int[5];
        public string[] topScoresName = new string[5];
    }
    public void SaveToFile()
    {
        SaveData data = new SaveData();
        data.topScores = topScores;
        data.topScoresName = topScoresName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    private void LoadFromFile()
    { 
         string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            topScores = data.topScores;
            topScoresName = data.topScoresName;
        }
    }
}
