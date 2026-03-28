using UnityEngine;

public class GhoulAttack : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider rightHandCollider;
    [SerializeField] private Collider headCollider;

    private Animator animator;

    void Start()
    {
        // Get a reference to the Animator component
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }


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

    public void EnableRootMotion()
    {
        if (animator != null)
        {
            animator.applyRootMotion = true;
        }
    }
    
    public void DisableRootMotion()
    {
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }
}
