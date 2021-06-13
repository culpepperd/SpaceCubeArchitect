using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private Animator animator;

    private string triggerName;

    public string buttonName;

    private GrabbableObject hoveredObject;
    private GrabbableObject grabbedObject;


    private void OnTriggerEnter(Collider other)
    {
        GrabbableObject grabbedObj = other.GetComponent<GrabbableObject>();

        if (grabbedObj != null)
        {
            if (hoveredObject != null)
            {
                hoveredObject.OnHoverEnd();
            }

            hoveredObject = grabbedObj;
            hoveredObject.OnHoverStart();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GrabbableObject grabbedObj = other.GetComponent<GrabbableObject>();

        if (grabbedObj == hoveredObject)
        {
            hoveredObject.OnHoverEnd();
            hoveredObject = null;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check if the grip button was pressed
        if (Input.GetButtonDown(buttonName))
        {
            animator.SetBool("Gripped", true);

            if (hoveredObject != null)
            {
                // 'this' references the object that the script
                // is attached to, which is the one being referenced.
                // For example, if the Right Hand is hovering and triggers
                // this statement, then 'this' will reference the Right hand.
                hoveredObject.OnGrabStart(this);

                grabbedObject = hoveredObject;
                //hoveredObject = null;
            }
        }

        // check if the grip button was released
        if (Input.GetButtonUp(buttonName))
        {
            animator.SetBool("Gripped", false);

            if (grabbedObject != null)
            {
                grabbedObject.OnGrabEnd();
                grabbedObject = null;
            }


        }

        if(Input.GetButtonDown(triggerName))
        {
            if(grabbedObject != null)
            {
                grabbedObject.SendMessage("TriggerDown");
            }
        }
        else if(Input.GetButtonUp(triggerName))
        {
            if(grabbedObject != null)
            {
                grabbedObject.SendMessage("TriggerUp");
            }
        }
    }
}
