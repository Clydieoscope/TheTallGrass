using UnityEngine;
using StarterAssets;

public class IntroAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private string animationName = "Smoking";

    private bool _isPlayingIntro = true;

    private void Start()
    {
        animator.Play(animationName);
    }

    private void Update()
    {
        if (!_isPlayingIntro) return;

        // Cancel if player moves
        if (input.move != Vector2.zero)
        {
            CancelIntro();
        }
    }

    private void CancelIntro()
    {
        _isPlayingIntro = false;
        animator.CrossFade("Idle Walk Run Blend", 0.1f);
    }
}