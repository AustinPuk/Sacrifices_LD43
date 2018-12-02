using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    GameObject camera;

    [SerializeField]
    Vector3 initialOffset = new Vector3(0.0f, 8.13f, -7.77f);

    bool goOrig = false;

    void Start()
    {
        if (!target)
            throw new System.Exception("ERROR : Camera needs a target");
    }

    void Update()
    {

        if (!Game.game.isPaused)
        {
            if (!goOrig)
            {
                goOrig = true;
                camera.transform.localPosition = Vector3.zero; // TODO : Clean
                camera.transform.localEulerAngles = Vector3.zero;
            }

            FollowPlayer();
        }

        // TODO : Zoom In, Zoom Out with Mouse Wheel, Bounds on edges of arena, lerped camera movement?
    }

    void FollowPlayer()
    {
        transform.position = target.transform.position + initialOffset;
    }
}
