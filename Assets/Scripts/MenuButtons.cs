using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Continue()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
    }

    public void Settings()
    {
        GameStateManager.Instance.SetState(GameState.Settings);
    }

    public void SettingsBack()
    {
        GameState prevState = GameStateManager.Instance.PrevState;
        if (prevState == GameState.MainMenu)
        {
            GameStateManager.Instance.SetState(GameState.MainMenu);
        }
        else
        {
            GameStateManager.Instance.SetState(GameState.Paused);
        }
    }

    public void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
