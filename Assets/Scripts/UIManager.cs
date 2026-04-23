using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject loseMenuUI;

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        GameStateManager.Instance.SetState(GameState.MainMenu);
    }

    private void HandleGameStateChanged(GameState state)
    {
        // handle old state
        GameState prevState = GameStateManager.Instance.PrevState;
        switch (prevState)
        {
            case GameState.MainMenu:
                mainMenuUI.SetActive(false);
                break;
            case GameState.Paused:
                pauseMenuUI.SetActive(false);
                break;
            case GameState.Playing:
                break;
            case GameState.Settings:
                settingsMenuUI.SetActive(false);
                break;
            case GameState.Won:
                winMenuUI.SetActive(false);
                break;
            case GameState.Lost:
                loseMenuUI.SetActive(false);
                break;
        }

        switch (state)
        {
            case GameState.MainMenu:
                mainMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Paused:
                pauseMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
                break;
            case GameState.Settings:
                settingsMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Won:
                winMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Lost:
                loseMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
        /*
        if (state == GameState.MainMenu)
        {
            mainMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (state == GameState.Paused)
        {
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (state == GameState.Playing)
        {
            pauseMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (state == GameState.Won)
        {
            Debug.Log("Show Win Screen");
        }
        else if (state == GameState.Lost)
        {
            Debug.Log("Show Lose Screen");
        }*/
    }
}
