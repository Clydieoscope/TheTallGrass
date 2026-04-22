using UnityEngine;

public class GhoulAttack : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider rightHandCollider;
    [SerializeField] private Collider headCollider;
    [SerializeField] private ParticleParentController leftParticleController;
    [SerializeField] private ParticleParentController rightParticleController;

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
        leftParticleController.PlayAll();
    }
    
    public void DisableLeftHandCollider()
    {
        leftHandCollider.enabled = false;
        leftParticleController.StopAll();
    }

    public void EnableRightHandCollider()
    {
        rightHandCollider.enabled = true;
        rightParticleController.PlayAll();
    }
    
    public void DisableRightHandCollider()
    {
        rightHandCollider.enabled = false;
        rightParticleController.StopAll();
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
