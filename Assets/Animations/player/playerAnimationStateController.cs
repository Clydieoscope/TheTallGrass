using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.5f;
    public float deceleration = 0.5f;
    int VelocityHash;

    // Start is called before the first frame update
    void Start()
    {
        // set reference to animator
        animator = GetComponent<Animator>();

        // increases performance (what?)
        VelocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {

        // get key input from player
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");

        if (forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocity > 0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (velocity < 0f)
        {
            velocity = 0f;
        }

        animator.SetFloat(VelocityHash, velocity); // references the velocity
    }
}
