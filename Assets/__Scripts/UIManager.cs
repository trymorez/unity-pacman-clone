using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    PlayerInput PlayerInput;

    void Awake()
    {
        Bean.OnScoreUpdate += OnScoreUpdate;
    }

    void OnDestroy()
    {
        Bean.OnScoreUpdate -= OnScoreUpdate;
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        GameManager GM = GameManager.Instance;

        if (context.performed)
        {

            if (GM.State == GameManager.GameState.Playing)
            {
                Debug.Log("Esc pressed " + GameManager.Instance.State);
                GM.GameStateChange(GameManager.GameState.Paused);
                menuPanel.SetActive(true);
            }
            else if (GM.State == GameManager.GameState.Paused)
            {
                GM.GameStateChange(GameManager.GameState.Playing);
                menuPanel.SetActive(false);
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
