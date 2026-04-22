using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private ParticleParentController weaponParticleController;

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


    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
        weaponParticleController.PlayAll();
    }
    
    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
        weaponParticleController.StopAll();
    }

}
