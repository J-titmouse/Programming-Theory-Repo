using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_Text highname;
    [SerializeField] private TMP_Text highscore;
    [SerializeField] private TMP_Text Placeholder;
    void Start()
    {
        if (DataManagement.Instance.player != "")
        {
            Placeholder.color = Color.black;
            Placeholder.fontStyle = FontStyles.Bold; 
            Placeholder.text = DataManagement.Instance.player;
        }
        highname.text = DataManagement.Instance.scoresboardNames;
        highscore.text = DataManagement.Instance.scoresboardScores;
    }
    public void StartNew()
    {
        if (playerName.text != "")
        {
            DataManagement.Instance.player = playerName.text;
        }
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        DataManagement.Instance.SaveToFile();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
