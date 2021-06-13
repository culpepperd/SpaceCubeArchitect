using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Material material;
    private Color originalColor;
    private Color sphereColor;

    public Color hoveredColor;
    public bool isGrabbed;
    public GameObject sphere;
    private Renderer sphereRenderer;
    [SerializeField]
    private List<Vector3> positions = new List<Vector3>();
    private int positionMax = 20;
    private int listStart = 0;
    private float throwForce = 20;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        sphereRenderer = sphere.GetComponent<Renderer>();
        isGrabbed = false;
    }

    private void Update()
    {
        if (isGrabbed == true)
        {
            // set sphere's color
            sphereRenderer.material.color = hoveredColor;

            // Update the vector3 positions in the list
            if (positions.Count > positionMax)
            {
                positions.RemoveAt(listStart);
                positions.Add(transform.position);
            }
            else
            {
                positions.Add(transform.position);
            }
        }

    }

    public void OnHoverStart()
    {
        // change color of object while hovering 
        material.color = hoveredColor;
    }

    public void OnHoverEnd()
    {
        // return object's color to it's original color
        material.color = originalColor;
    }

    public void OnGrabStart(Grabber hand)
    {
        // Two different options for grabbing:
        // 1. Kinematic Grab
        // 2. Fixed Joint

        isGrabbed = true;

        #region Kinematic Grab
        //transform.SetParent(hand.transform);
        //GetComponent<Rigidbody>().useGravity = false;
        //GetComponent<Rigidbody>().isKinematic = true;
        #endregion

        #region Fixed Joint
        // if objects are jittering in the hand, uncomment the next line
        // transform.SetParent(hand.transform);
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.connectedBody = hand.GetComponent<Rigidbody>();
        fx.breakForce = 5000; // pushing an object into another object with too much force will cause the object to snap out of grip
        fx.breakTorque = 5000;
        #endregion

    }

    public void OnGrabEnd()
    {
        isGrabbed = false;

        #region Kinematic Grab Release
        //transform.SetParent(null);
        //GetComponent<Rigidbody>().useGravity = true;
        //GetComponent<Rigidbody>().isKinematic = false;
        #endregion

        #region Fixed Joint Release
        FixedJoint fx = GetComponent<FixedJoint>();

        // if objects are jittering in the hand, uncomment the next line
        // transform.SetParent(null);
        if (fx != null)
        {
            Destroy(fx);
        }
        #endregion

        // Release the object and throw it
        Rigidbody rb = GetComponent<Rigidbody>();

        // assign a velocity to the thrown object
        rb.velocity = (positions[positions.Count - 1] - positions[0]) * throwForce;

        // clear position of previously grabbed object
        positions.Clear();
    }



}
// Today I learned about saving in a Json and YAML 
