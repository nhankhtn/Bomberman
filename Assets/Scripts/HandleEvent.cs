
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class HandleMenu : MonoBehaviour
{
    public TMP_InputField inputName; // in the Enter Infor Scene
    public TextMeshProUGUI errorText;
    public void EnterName()
    {
        SceneManager.LoadScene(1);
    }
    public void Ranking()
    {
        SceneManager.LoadScene(2);
    }
    public void About()
    {
        SceneManager.LoadScene(3);

    }
    public void QuitMenu()
    {
        //Write logic when exporting game 
    }
    // Script handle in the Enter Infor Scene 
    public void Play()
    {
        string namePlayer = inputName.text;
        if (string.IsNullOrEmpty(namePlayer))
        {
            errorText.text = "  Please enter your name!";
        }
        else if (namePlayer.Length > 14)
        {
            errorText.text = "  Too long!";
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", namePlayer);
            SceneManager.LoadScene(4);
            if (HandleGame.Instance != null && HandleGame.Instance.canvas != null)
            {
                HandleGame.Instance.resetHeader();
                if (GameManager.Instance.namePlayer != null)
                    GameManager.Instance.namePlayer.text = PlayerPrefs.GetString("PlayerName", "No Name");
                Timer.Instance.ResetTimer(0);
                HandleGame.Instance.canvas.enabled = true;
            }
        }
    }
    public void playAgain()
    {
        Time.timeScale = 1;
        HandleGame.Instance.resetHeader();
        Timer.Instance.ResetTimer(0);
        HandleGame.Instance.Panel.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.totalMonsters = 7;

    }
    public void nextRound()
    {
        Time.timeScale = 1f;
        int index = SceneManager.GetActiveScene().buildIndex;

        HandleGame.Instance.Panel.SetActive(false);
        if (index != 6)
        {
            HandleGame.Instance.resetHeader();
            SceneManager.LoadScene(index + 1);

            GameManager.Instance.totalMonsters = 7;
        }
        else
        {
            string namePlayer = PlayerPrefs.GetString("PlayerName", "No name");
            int time = int.Parse(HandleGame.Instance.timeText.text.Substring(5));
            Ranker ranker = new Ranker();
            ranker.addPlayer(namePlayer, time);
            HandleGame.Instance.canvas.enabled = false;
            SceneManager.LoadScene(0);
        }
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        if (HandleGame.Instance.canvas != null)
            HandleGame.Instance.canvas.enabled = false;
        SceneManager.LoadScene(0);
    }
}
