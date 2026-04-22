using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponInHand;
    [SerializeField] private UnityEvent onPickup;
    private GameObject nearbyPickup;
    private PickupPrompt nearbyPrompt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            nearbyPickup = other.gameObject;
            nearbyPrompt = other.GetComponent<PickupPrompt>();
            nearbyPrompt?.ShowPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            nearbyPrompt?.HidePrompt();
            nearbyPickup = null;
            nearbyPrompt = null;
        }
    }

    private void Update()
    {
        if (nearbyPickup != null && Input.GetKeyDown(KeyCode.E))
        {
            weaponInHand.SetActive(true);
            ThirdPersonController.Instance.EquipWeapon();
            nearbyPrompt?.HidePrompt();
            onPickup?.Invoke();
            Destroy(nearbyPickup);
            nearbyPickup = null;
            nearbyPrompt = null;
        }
    }
}