using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class GoalTrigger : MonoBehaviour
{
    public UnityEvent OnGoalReached;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGoalReached?.Invoke();
        }
    }

}