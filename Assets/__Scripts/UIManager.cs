using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] RectTransform readyText;
    PlayerInput PlayerInput;

    void Awake()
    {
        Bean.OnScoreUpdate += OnScoreUpdate;
        GameManager.OnScoreUpdate += OnScoreUpdate;
        GameManager.OnBeforeGameStateChange += OnBeforeGameStateChange;
    }

    void OnDestroy()
    {
        Bean.OnScoreUpdate -= OnScoreUpdate;
        GameManager.OnScoreUpdate -= OnScoreUpdate;
        GameManager.OnBeforeGameStateChange -= OnBeforeGameStateChange;
    }

    void OnBeforeGameStateChange(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Starting:
                break;
            case GameManager.GameState.Playing:
                DisplayLevel(GameManager.Level);
                DisplayReady();
                break;
            case GameManager.GameState.Paused:
                break;
            case GameManager.GameState.PacmanPowerUp:
                break;
            case GameManager.GameState.PacmanDying:
                break;
            case GameManager.GameState.LevelCompleted:
                break;
            case GameManager.GameState.GameOver:
                break;
        }
    }

    void DisplayLevel(int level)
    {
        levelText.text = "LEVEL " + GameManager.Level + 1.ToString();
    }

    void DisplayReady()
    {
        readyText.DOScale(Vector3.zero, 1f).SetDelay(0.1f);
    }


    public void OnMenu(InputAction.CallbackContext context)
    {
        GameManager GM = GameManager.Instance;

        if (context.performed)
        {

            if (GameManager.State == GameManager.GameState.Playing)
            {
                GM.GameStateChange(GameManager.GameState.Paused);
                menuPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else if (GameManager.State == GameManager.GameState.Paused)
            {
                GM.GameStateChange(GameManager.GameState.Playing);
                menuPanel.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    void OnScoreUpdate(int score)
    {
        scoreText.text = score.ToString();
    }

    void OnHighScoreUpdate()
    {

    }
}
