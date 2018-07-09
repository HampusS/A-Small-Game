using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text Score;
    public Text Timer;
    public float Hits { get; set; }
    public float SurfaceTime;
    public bool PauseTimer { get; set; }
    [SerializeField]
    GameObject pause_canvas;
    bool paused;
    float hits_timer, time_limit = 6, count_down;

    private void Update()
    {
        if (Score != null && !PauseTimer)
        {
            hits_timer += Time.deltaTime;
            if (hits_timer > time_limit)
            {
                //Debug.Log("LOST");
            }
        }
        if (Timer != null)
            UpdateTimerText();
        PauseGame();
    }

    public void AddHit()
    {
        Hits++;
        hits_timer = 0;
        Score.text = Hits.ToString();
        PauseTimer = true;
    }

    void UpdateTimerText()
    {
        Debug.Log(count_down);
        count_down = (int)(time_limit - hits_timer);
        if (count_down <= 3)
        {
            Timer.enabled = true;
            if (count_down >= 0)
                Timer.text = count_down.ToString();
        }
        else
            Timer.enabled = false;
    }

    public void IncreaseDifficulty()
    {
        if (SurfaceTime > 0)
            SurfaceTime -= Time.deltaTime * 4;
        else if (SurfaceTime < 0)
            SurfaceTime = 0;

        if (time_limit > 3)
            time_limit -= Time.deltaTime * 2;
        else if (time_limit < 3)
            time_limit = 3;
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                pause_canvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                paused = false;
                Time.timeScale = 1;
                pause_canvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void UnpauseGame()
    {
        paused = false;
        Time.timeScale = 1;
        pause_canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
