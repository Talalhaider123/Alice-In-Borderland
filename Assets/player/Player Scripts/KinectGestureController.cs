using UnityEngine;
using Windows.Kinect;

public class KinectGestureController : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private BodySourceManager bodyManager;
    private Body[] bodies;

    void Start()
    {
        bodyManager = GameObject.Find("KinectBody").GetComponent<BodySourceManager>();
    }

    void Update()
    {
        if (bodyManager == null) return;

        bodies = bodyManager.GetData();

        if (bodies == null) return;

        foreach (var body in bodies)
        {
            if (body == null) continue;

            if (body.IsTracked)
            {
                DetectRun(body);
                DetectTurn(body);
                DetectJump(body);
                DetectGrab(body);

                break;
            }
        }
    }

    void DetectRun(Body body)
    {
        float leftKnee = body.Joints[JointType.KneeLeft].Position.Y;
        float rightKnee = body.Joints[JointType.KneeRight].Position.Y;

        float diff = Mathf.Abs(leftKnee - rightKnee);

        if (diff > 0.12f)
        {
            playerMovement.isRunning = true;
        }
        else
        {
            playerMovement.isRunning = false;
        }
    }

    void DetectTurn(Body body)
    {
        float spineX = body.Joints[JointType.SpineBase].Position.X;

        if (spineX > 0.15f)
        {
            playerMovement.turnDirection = 1f;
        }
        else if (spineX < -0.15f)
        {
            playerMovement.turnDirection = -1f;
        }
        else
        {
            playerMovement.turnDirection = 0f;
        }
    }

    void DetectJump(Body body)
    {
        float footLeft = body.Joints[JointType.FootLeft].Position.Y;
        float footRight = body.Joints[JointType.FootRight].Position.Y;

        if (footLeft > 0.25f && footRight > 0.25f)
        {
            playerMovement.SendMessage("Jump");
        }
    }

    void DetectGrab(Body body)
    {
        float handZ = body.Joints[JointType.HandRight].Position.Z;
        float spineZ = body.Joints[JointType.SpineMid].Position.Z;

        if (handZ < spineZ - 0.25f)
        {
            playerMovement.SendMessage("Grab");
        }
    }
}