using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float boundsX;
    public float speed;
    public float tilt;

    private Rigidbody rb;
    private Animator anim;

    private float inputHorizontal;

    bool grabUsed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerInput();
        CheckGrabAnimation();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void PlayerInput()
    {
        if (BodySourceView.jointObjs[11] != null && BodySourceView.jointObjs[7] != null)
        {
            Vector3 handLeft = BodySourceView.jointObjs[7].position;
            Vector3 handRight = BodySourceView.jointObjs[11].position;

            float angle = Mathf.Atan2(handRight.y - handLeft.y, handRight.x - handLeft.x) * Mathf.Rad2Deg;

            inputHorizontal = Mathf.Lerp(1f, -1f, Mathf.InverseLerp(-45f, 45f, angle));
        }
        else
        {
            inputHorizontal = Input.GetAxis("Horizontal");
        }
    }

    void Movement()
    {
        Vector3 input = new Vector3(inputHorizontal, 0, 0);

        rb.linearVelocity = input * speed;

        Vector3 pos = rb.position;
        rb.position = new Vector3(Mathf.Clamp(pos.x, -boundsX, boundsX), pos.y, pos.z);

        float tiltZ = rb.linearVelocity.x * -tilt;
        rb.rotation = Quaternion.Euler(0, 0, tiltZ);
    }

    void CheckGrabAnimation()
    {
        if (anim == null) return;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Grab"))
        {
            if (!grabUsed)
            {
                grabUsed = true;
                CollectLetters();
            }
        }
        else
        {
            grabUsed = false;
        }
    }

    void CollectLetters()
    {
        Debug.Log("GRAB DETECTED");

        Collider[] hits = Physics.OverlapSphere(transform.position, 4f);

        foreach (Collider hit in hits)
        {
            LetterCollect letter = hit.GetComponent<LetterCollect>();

            if (letter != null)
            {
                Debug.Log("FOUND LETTER: " + hit.name);
                letter.TryCollect();
            }
        }
    }
}