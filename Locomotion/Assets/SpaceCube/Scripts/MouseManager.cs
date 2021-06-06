using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject cubeToSpawn;

    void Update()
    {


        // Was the mouse presed down this frame?
        if (Input.GetMouseButtonDown(0))
        {
            // Yes, the left mouse button was pressed this frame
            // Is the mouse over a cube?

            Camera theCamera = Camera.main;

            Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                //Debug.Log("Yes, we hit " + hitInfo.collider.gameObject.name);

                // spawn a new object
                Vector3 spawnLocation = hitInfo.collider.transform.position + hitInfo.normal;

                Instantiate(cubeToSpawn, spawnLocation, Quaternion.identity);
            }
        }
    }
}
