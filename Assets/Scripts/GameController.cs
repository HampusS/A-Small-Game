using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text Score;
    public Text Timer;
    public Text FinalScore;

    public float Hits { get; set; }
    public float SurfaceTime;
    public bool PauseTimer { get; set; }
    public bool isGameOver { get; set; }

    [SerializeField]
    GameObject pause_canvas;
    [SerializeField]
    GameObject gameover_canvas;
    [SerializeField]
    GameObject ingame_canvas;

    bool paused;
    float hits_timer, time_limit = 6, count_down;

    void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Score != null && !PauseTimer && hits_timer >= 0)
        {
            hits_timer += Time.deltaTime;
            if (hits_timer > time_limit)
            {
                //GameOver();
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
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                pause_canvas.SetActive(true);
                ShowMouse();
                SmartCam.Instance.Enabled = false;
                PlayerController.Instance.Enabled = false;
            }
            else
            {
                paused = false;
                Time.timeScale = 1;
                pause_canvas.SetActive(false);
                HideMouse();
                SmartCam.Instance.Enabled = true;
                PlayerController.Instance.Enabled = true;
            }
        }
    }

    public void UnpauseGame()
    {
        paused = false;
        Time.timeScale = 1;
        pause_canvas.SetActive(false);
        HideMouse();
        SmartCam.Instance.Enabled = true;
        PlayerController.Instance.Enabled = true;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameover_canvas.SetActive(true);
        ingame_canvas.SetActive(false);
        AudioManager am = FindObjectOfType<AudioManager>();
        hits_timer = -1;
        isGameOver = true;
        ShowMouse();
        for (int i = 0; i < am.sounds.Length; i++)
        {
            am.StopSound(i);
        }
        FinalScore.text = Hits.ToString();
        am.PlaySound(4);
    }
}
