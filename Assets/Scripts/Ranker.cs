using UnityEngine;
using TMPro;

public class Ranker : MonoBehaviour
{
    public static Ranker Instance { get; private set; }
    public TextMeshProUGUI[] nameTexts = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] timeTexts = new TextMeshProUGUI[5];


    void Awake()
    {
        if (Ranker.Instance == null)
        {
            Ranker.Instance = this;
        }
    }
    void Start()
    {
        FakeRanking();
        renderRanking();
    }

    public void addPlayer(string _name, int _time)
    {
        string[] names = new string[5];
        int[] times = new int[5];

        for (int i = 0; i < names.Length; i++)
        {
            names[i] = PlayerPrefs.GetString("Name_Top-" + i, "");
            times[i] = PlayerPrefs.GetInt("Time_Top-" + i, 0);
        }

        for (int i = 0; i < 5; i++)
        {
            if (_time < times[i])
            {
                for (int j = 4; j > i; j--)
                {
                    names[j] = names[j - 1];
                    times[j] = times[j - 1];
                }
                names[i] = _name;
                times[i] = _time;
                break;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetString("Name_Top-" + i, names[i]);
            PlayerPrefs.SetInt("Time_Top-" + i, times[i]);
        }
    }

    public void renderRanking()
    {
        string name;
        int time;

        for (int i = 0; i < 5; i++)
        {
            name = PlayerPrefs.GetString("Name_Top-" + i, "");
            time = PlayerPrefs.GetInt("Time_Top-" + i, 0);

            if (name == "") break;
            nameTexts[i].text = name;
            timeTexts[i].text = time.ToString();
        }

    }

    public void FakeRanking()
    {
        if (PlayerPrefs.GetString("Name_Top-0", "") == "")
        {
            PlayerPrefs.SetString("Name_Top-0", "noone");
            PlayerPrefs.SetInt("Time_Top-0", 100);
        }
        if (PlayerPrefs.GetString("Name_Top-1", "") == "")
        {
            PlayerPrefs.SetString("Name_Top-1", "nhu");
            PlayerPrefs.SetInt("Time_Top-1", 120);
        }
        if (PlayerPrefs.GetString("Name_Top-2", "") == "")
        {
            PlayerPrefs.SetString("Name_Top-2", "nguyen");
            PlayerPrefs.SetInt("Time_Top-2", 140);
        }
        if (PlayerPrefs.GetString("Name_Top-3", "") == "")
        {
            PlayerPrefs.SetString("Name_Top-3", "nhansensi");
            PlayerPrefs.SetInt("Time_Top-3", 150);
        }
        if (PlayerPrefs.GetString("Name_Top-4", "") == "")
        {
            PlayerPrefs.SetString("Name_Top-4", "loitaitoi");
            PlayerPrefs.SetInt("Time_Top-4", 155);
        }
    }
}