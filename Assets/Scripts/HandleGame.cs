using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HandleGame : MonoBehaviour
{
    public static HandleGame Instance { get; private set; }
    public GameObject Panel;// Display the result when the player passes the level
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI score;
    public Canvas canvas;

    void Awake()
    {
        if (HandleGame.Instance == null)
        {
            HandleGame.Instance = this;
        }
    }

    public void setPanel(string panel)
    {
        Panel.SetActive(true);
        // Pause the game
        Time.timeScale = 0f;

        Transform resultPanel = transform.Find("Result");
        if (resultPanel != null)
        {
            TextMeshProUGUI title = resultPanel.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI result = resultPanel.Find("Result").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI time = resultPanel.Find("Time").GetComponent<TextMeshProUGUI>();
            Button[] buttons = resultPanel.GetComponentsInChildren<Button>(true);

            if (panel == "win")
            {
                title.text = "Completed !";
                result.text = "Score " + this.score.text;
                time.text = this.timeText.text;

                buttons[1].gameObject.SetActive(false);
                buttons[0].gameObject.SetActive(true);
            }
            else if (panel == "lose")
            {
                title.text = "Game Over !";
                result.text = "Score " + this.score.text;
                time.text = this.timeText.text;

                buttons[0].gameObject.SetActive(false);
                buttons[1].gameObject.SetActive(true);
            }
        }
    }
    public void IncreaseScore(int _score)
    {
        int score = int.Parse(this.score.text);
        score += _score;
        this.score.text = score.ToString();
    }
    public void resetHeader()
    {
        bombText.text = "Bom 1";
        speedText.text = "Speed 5";
        heartText.text = "1";
        score.text = "0";
    }
}