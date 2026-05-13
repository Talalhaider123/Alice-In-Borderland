using UnityEngine;
using Windows.Kinect;

public class KinectPlayerController : MonoBehaviour
{
    public PlayerMovement playerMovement;

    Transform head;
    Transform spine;
    Transform kneeLeft;
    Transform kneeRight;
    Transform handRight;

    float previousSpineX;
    float previousHeadY;
    float previousHandZ;

    float jumpCooldown = 0f;
    float grabCooldown = 0f;

    void Update()
    {
        if (!BodySourceView.bodyTracked)
            return;

        head = BodySourceView.jointObjs[(int)JointType.Head];
        spine = BodySourceView.jointObjs[(int)JointType.SpineBase];
        kneeLeft = BodySourceView.jointObjs[(int)JointType.KneeLeft];
        kneeRight = BodySourceView.jointObjs[(int)JointType.KneeRight];
        handRight = BodySourceView.jointObjs[(int)JointType.HandRight];

        if (head == null || spine == null || kneeLeft == null || kneeRight == null || handRight == null)
            return;

        jumpCooldown -= Time.deltaTime;
        grabCooldown -= Time.deltaTime;

        DetectRun();
        DetectTurn();
        DetectJump();
        DetectGrab();

        previousSpineX = spine.position.x;
        previousHeadY = head.position.y;
        previousHandZ = handRight.position.z;
    }

    void DetectRun()
    {
        float diff = Mathf.Abs(kneeLeft.position.y - kneeRight.position.y);

        if (diff > 0.2f)
            playerMovement.isRunning = true;
        else
            playerMovement.isRunning = false;
    }

    void DetectTurn()
    {
        Transform shoulderLeft = BodySourceView.jointObjs[(int)JointType.ShoulderLeft];
        Transform shoulderRight = BodySourceView.jointObjs[(int)JointType.ShoulderRight];

        if (shoulderLeft == null || shoulderRight == null)
            return;

        float shoulderDiff = shoulderLeft.position.z - shoulderRight.position.z;

        float threshold = 0.15f;

        if (shoulderDiff > threshold)
        {
            // body turned right
            playerMovement.turnDirection = 1f;
        }
        else if (shoulderDiff < -threshold)
        {
            // body turned left
            playerMovement.turnDirection = -1f;
        }
        else
        {
            playerMovement.turnDirection = 0f;
        }
    }

    void DetectJump()
    {
        float jumpVelocity = head.position.y - previousHeadY;

        if (jumpVelocity > 0.15f && jumpCooldown <= 0f)
        {
            playerMovement.jumpTrigger = true;
            jumpCooldown = 1.2f;
        }
    }

    void DetectGrab()
    {
        float handVelocity = previousHandZ - handRight.position.z;

        if (handVelocity > 0.15f && grabCooldown <= 0f)
        {
            playerMovement.grabTrigger = true;
            grabCooldown = 1.0f;
        }
    }
}