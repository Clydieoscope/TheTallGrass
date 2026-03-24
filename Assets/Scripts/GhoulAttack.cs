using UnityEngine;

public class GhoulAttack : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider rightHandCollider;
    [SerializeField] private Collider headCollider;

    public void EnableLeftHandCollider()
    {
        leftHandCollider.enabled = true;
    }
    
    public void DisableLeftHandCollider()
    {
        leftHandCollider.enabled = false;
    }

    public void EnableRightHandCollider()
    {
        rightHandCollider.enabled = true;
    }
    
    public void DisableRightHandCollider()
    {
        rightHandCollider.enabled = false;
    }

    public void EnableHeadCollider()
    {
        headCollider.enabled = true;
    }
    
    public void DisableHeadCollider()
    {
        headCollider.enabled = false;
    }
}
