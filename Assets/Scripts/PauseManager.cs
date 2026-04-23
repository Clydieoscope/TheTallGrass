using UnityEngine;
using static GameStateManager;
namespace StarterAssets
{
    public class PauseManager : MonoBehaviour
    {
        private StarterAssetsInputs _input;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _input = GetComponent<StarterAssetsInputs>();
            Debug.Log(_input);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Alpha1)) // Check for a specific key press
            {
                OnPause();
            }
        }

        void OnEnable()
        {
        }

        void OnPause()
        {
            if (GameStateManager.Instance.CurrentState == GameState.Playing)
            {
                GameStateManager.Instance.SetState(GameState.Paused);
            }
            else if (GameStateManager.Instance.CurrentState == GameState.Paused)
            {
                GameStateManager.Instance.SetState(GameState.Playing);
            }
            Debug.Log("Pause Toggled. Current State: " + GameStateManager.Instance.CurrentState);
        }
    }
}