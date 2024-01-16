using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }
    private float timer;
    void Awake()
    {
        if (Timer.Instance == null)
        {
            Timer.Instance = this;
        }
    }
    void Start()
    {
        ResetTimer(0);
    }
    void Update()
    {
        timer += Time.deltaTime;
        UpdateTimerDisplay(timer);
    }
    public void ResetTimer(int timeStart)
    {
        timer = timeStart;
    }
    private void UpdateTimerDisplay(float time)
    {
        int _time = (int)time;
        HandleGame.Instance.timeText.text = "Time " + _time.ToString();
    }
}
