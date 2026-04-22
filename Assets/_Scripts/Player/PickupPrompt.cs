using UnityEngine;

public class PickupPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptCanvas;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        promptCanvas.SetActive(false);
    }

    private void LateUpdate()
    {
        if (promptCanvas.activeSelf)
            promptCanvas.transform.rotation = Quaternion.LookRotation(promptCanvas.transform.position - cameraTransform.position);
    }

    public void ShowPrompt() => promptCanvas.SetActive(true);
    public void HidePrompt() => promptCanvas.SetActive(false);
}