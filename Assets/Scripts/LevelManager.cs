using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float lerpTimer, timer, aliveTimer, timerValue;
    [SerializeField] private bool timerDone, isLerping;
    [SerializeField] public float gameSpeed;
    [SerializeField] public float currentGameSpeed;
    [SerializeField] public float nextGameSpeed;
    public float kills;

    PlayerMovement playerMovement;

    [Header("User Interface")]
    [SerializeField] TextMeshProUGUI aliveTimerUi, killsUi, ammoCountUi;
    [SerializeField] private GameObject gameOverScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        GameSpeedTransition();
        InterfaceFunction();
        AliveTimerFunction();
        GameOver();
    }

    void GameSpeedTransition()
    {
        timer -= Time.deltaTime;
        if (!timerDone && timer <= 0)
        {
            timerDone = true;
            currentGameSpeed = gameSpeed;
            nextGameSpeed = currentGameSpeed + 3;
            isLerping = true;
        }

        if (isLerping)
        {
            lerpTimer += Time.deltaTime;
            gameSpeed = Mathf.Lerp(currentGameSpeed, nextGameSpeed, lerpTimer / 2);
            if (gameSpeed == nextGameSpeed)
            {
                isLerping = false;
                timerDone = false;
                lerpTimer = 0;
                timer = timerValue;
            }
        }
    }

    void InterfaceFunction()
    {
        killsUi.SetText("Kills: " + kills.ToString("F0"));
        aliveTimerUi.SetText("Time Alive: " + aliveTimer.ToString("F0") + " Seconds");
        ammoCountUi.SetText("X " + playerMovement.ammoCount.ToString("F0"));
    }

    void GameOver()
    {
        if (playerMovement.health <= 0)
        {
            gameSpeed = 0;
            gameOverScreen.SetActive(true);
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void Home()
    {
        SceneManager.LoadScene("Home");
    }

    void AliveTimerFunction()
    {
        if (playerMovement.health > 0)
        {
            aliveTimer += Time.deltaTime;

        }
    }
}
