using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float turnSpeed = 120f;
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;

    // Movement values (controlled by Kinect script)
    public bool isRunning = false;
    public float turnDirection = 0f;

    // Animation triggers
    public bool jumpTrigger = false;
    public bool grabTrigger = false;

    // 🔥 NEW (only addition)
    private bool isGrabbing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (grabTrigger)
        {
            Debug.Log("GRAB TRIGGER RECEIVED");
        }

        MovePlayer();
        HandleAnimations();
    }

    void MovePlayer()
    {
        // 🔥 STOP movement while grabbing
        if (isGrabbing)
            return;

        // Forward movement
        if (isRunning)
        {
            Vector3 move = transform.forward * moveSpeed;
            controller.Move(move * Time.deltaTime);
        }

        // Turning
        transform.Rotate(Vector3.up * turnDirection * turnSpeed * Time.deltaTime);

        // Jump
        if (jumpTrigger && controller.isGrounded)
        {
            velocity.y = jumpForce;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void HandleAnimations()
    {
        animator.SetBool("isWalking", isRunning);

        if (jumpTrigger)
        {
            animator.SetTrigger("Jump");
            jumpTrigger = false;
        }

        if (grabTrigger && !isGrabbing)
        {
            Debug.Log("PLAYING GRAB ANIMATION");

            isGrabbing = true; // 🔥 LOCK movement

            animator.SetTrigger("Grab");

            // 🔥 Collect letters when grabbing
            CollectLetters();

            Invoke("EndGrab", 1f); // adjust if needed

            grabTrigger = false;
        }
    }

    void EndGrab()
    {
        isGrabbing = false; // 🔓 UNLOCK movement
    }

    void CollectLetters()
    {
        Debug.Log("COLLECT CHECK");

        Collider[] hits = Physics.OverlapSphere(transform.position, 3f);

        Debug.Log("Colliders found: " + hits.Length);

        foreach (Collider hit in hits)
        {
            Debug.Log("Hit object: " + hit.name);

            LetterCollect letter = hit.GetComponent<LetterCollect>();

            if (letter != null)
            {
                Debug.Log("LETTER FOUND: " + letter.letter);
                letter.TryCollect();
            }
        }
    }
}