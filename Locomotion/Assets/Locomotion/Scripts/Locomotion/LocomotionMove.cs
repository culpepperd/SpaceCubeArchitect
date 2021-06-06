using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionMove : MonoBehaviour
{
    private enum Hand { Left, Right };

    [SerializeField]
    private Hand hand;

    private string horizontalAxisName;
    private string verticalAxisName;


    [SerializeField]
    private Transform playerHeadset; // this will be the pivot point for the XR_Rig
    [SerializeField]
    private Transform XR_Rig;

    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private float moveSpeed;



    private void Awake()
    {
        horizontalAxisName = "XRI_" + hand + "_Primary2DAxis_Horizontal";
        verticalAxisName = "XRI_" + hand + "_Primary2DAxis_Vertical";
    }

    void Update()
    {
        float x = Input.GetAxis(horizontalAxisName);

        XR_Rig.RotateAround(playerHeadset.position, Vector3.up, turnSpeed * Time.deltaTime * x);

        float y = Input.GetAxis(verticalAxisName);
        Vector3 direction = playerHeadset.forward;
        direction.y = 0;
        direction.Normalize();

        XR_Rig.position += direction * Time.deltaTime * moveSpeed * y;
    }
}


/*
 * Snap rotation by 15 or 30 degrees
 * Set boolean so if user has "snapped" they must
 * release the thumbstick before the boolean can return true
 * so that the user may snap turn again.
 */