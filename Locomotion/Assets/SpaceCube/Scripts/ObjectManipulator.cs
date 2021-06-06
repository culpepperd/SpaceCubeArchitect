using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ObjectManipulator : MonoBehaviour
{
    public GameObject cubeToSpawn;

    private string primaryButton;
    private string secondaryButton;
    private string triggerButton;
    public bool lineRendererOn;

    // private Vector3 hitPoint;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        lineRendererOn = false;

        primaryButton = "XRI_Right_PrimaryButton"; // A-button on Oculus Quest 2 Right Controller
        secondaryButton = "XRI_Right_SecondaryButton"; // B-button on Oculus Quest 2 Right Controller
        triggerButton = "XRI_Right_TriggerButton"; // Trigger on Quest 2 Right Controller
    }

    void Update()
    {

        if(Input.GetButtonDown(primaryButton))
        {
            if(lineRendererOn == false) // If line is off, and button is pressed, turn line on
            {
                TurnOnLineRenderer();
                lineRendererOn = true;
            }
            else if(lineRendererOn == true) // If line is on, and button is pressed, turn line off
            {
                TurnOffLineRenderer();
                lineRendererOn = false;
            }
        }

        if(lineRendererOn == true)
        {
            TurnOnLineRenderer();
        }


    }

    // Turns ON the Line Renderer component
    public void TurnOnLineRenderer()
    {
        // turns on line renderer
        line.enabled = true;
        // declares hit variable
        RaycastHit hit;
        // sets line starting position
        line.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            line.SetPosition(1, hit.point);

            Vector3 spawnLocation = hit.collider.transform.position + hit.normal;

            if (Input.GetButtonDown(triggerButton) && lineRendererOn == true)
            {
                SpawnObject(spawnLocation);
            }
            else if(Input.GetButtonDown(secondaryButton) && lineRendererOn == true)
            {
                if(hit.transform.gameObject.tag == "Cube")
                {
                    Destroy(hit.transform.gameObject);
                    // Play sound
                }
            }

        }
        else
        {
            line.SetPosition(1, transform.position + transform.forward * 10f);
        }
    }

    // Turns OFF the Line Renderer component
    public void TurnOffLineRenderer()
    {
        line.enabled = false;
    }

    // Spawns an object
    public void SpawnObject(Vector3 location)
    {
        Vector3 spawnLocation = location;
        Instantiate(cubeToSpawn, spawnLocation, Quaternion.identity);
        GetComponent<AudioSource>().Play();
    }
}
