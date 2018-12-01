using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    Vector3 initialOffset = new Vector3(0.0f, 13.0f, -7.5f);

    void Start()
    {
        if (!target)
            throw new System.Exception("ERROR : Camera needs a target");

        transform.position = target.transform.position + initialOffset;
    }

    void Update()
    {
        FollowPlayer();

        // TODO : Zoom In, Zoom Out with Mouse Wheel, Bounds on edges of arena, lerped camera movement?
    }

    void FollowPlayer()
    {
        transform.position = target.transform.position + initialOffset;
    }
}
