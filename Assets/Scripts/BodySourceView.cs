using UnityEngine;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour
{
    public static Transform[] jointObjs = new Transform[25];
    public static bool bodyTracked = false;

    public GameObject BodySourceManager;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    // Bone connections
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap =
        new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    void Update()
    {
        if (BodySourceManager == null) return;

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null) return;

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null) return;

        List<ulong> trackedIds = new List<ulong>();

        foreach (var body in data)
        {
            if (body != null && body.IsTracked)
            {
                bodyTracked = true;
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // Remove untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
                bodyTracked = false;
            }
        }

        foreach (var body in data)
        {
            if (body == null) continue;

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        // Create Joints (Green spheres)
        for (Kinect.JointType jt = Kinect.JointType.SpineBase;
             jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
            jointObj.transform.localScale = Vector3.one * 0.2f;

            Renderer r = jointObj.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Standard"));
            r.material.color = Color.green;
        }

        // Create Bone Lines (Yellow)
        foreach (var bone in _BoneMap)
        {
            GameObject lineObj = new GameObject(bone.Key + "_Bone");
            lineObj.transform.parent = body.transform;

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.yellow;
            lr.endColor = Color.yellow;
            lr.positionCount = 2;
        }

        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        // Update Joint Positions
        for (Kinect.JointType jt = Kinect.JointType.SpineBase;
             jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];

            Transform jointTransform = bodyObject.transform.Find(jt.ToString());
            jointObjs[(int)jt] = jointTransform;

            jointTransform.localPosition = GetVector3FromJoint(sourceJoint);
        }

        // Update Bone Lines
        int boneIndex = 0;
        foreach (var bone in _BoneMap)
        {
            Transform jointA = bodyObject.transform.Find(bone.Key.ToString());
            Transform jointB = bodyObject.transform.Find(bone.Value.ToString());

            LineRenderer lr =
                bodyObject.transform.GetChild(25 + boneIndex)
                .GetComponent<LineRenderer>();

            lr.SetPosition(0, jointA.position);
            lr.SetPosition(1, jointB.position);

            boneIndex++;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        // Adjust scale if skeleton is too large
        float scale = 3f;
        return new Vector3(
            joint.Position.X * scale,
            joint.Position.Y * scale,
            -joint.Position.Z * scale
        );
    }
}