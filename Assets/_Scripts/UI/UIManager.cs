using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Won)
        {
            Debug.Log("Show Win Screen");
        }
        else if (state == GameState.Lost)
        {
            Debug.Log("Show Lose Screen");
        }
    }
}
