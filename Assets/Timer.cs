using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public string prefix = "Timer to next level: ";
    public bool BeginTimerAtStart = true;
    public float timer = 5.0f;
    public Text Counter;
    public UnityEvent OnTimer;

    private bool enabledTimer = false;

    private void Start()
    {
        if (BeginTimerAtStart)
            StartTimer();
    }

    public void StartTimer()
    {
        enabledTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabledTimer) return;

        timer -= Time.deltaTime;
        if (Counter != null)
            Counter.text = $"{prefix} {(int)timer / 60}:{(int)timer % 60}";

        if( timer < 0 )
        {
            OnTimer.Invoke();
            enabledTimer = false;
        }
    }
}
