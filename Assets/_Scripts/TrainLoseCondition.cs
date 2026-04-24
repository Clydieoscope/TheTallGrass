using UnityEngine;
using UnityEngine.Splines;
using TMPro;

public class TrainLoseCondition : MonoBehaviour
{
    private SplineAnimate _splineAnimate;
    [SerializeField] private TextMeshProUGUI myText;

    private void Start()
    {
        _splineAnimate = GetComponent<SplineAnimate>();

        if (_splineAnimate != null)
            _splineAnimate.Completed += OnTrainReachedEnd;
    }

    private void OnDestroy()
    {
        if (_splineAnimate != null)
            _splineAnimate.Completed -= OnTrainReachedEnd;
    }

    private void OnTrainReachedEnd()
    {
        myText.text = "The Train Didn't Wait.";
        GameEndHandler.Instance.TriggerLose();
    }
}