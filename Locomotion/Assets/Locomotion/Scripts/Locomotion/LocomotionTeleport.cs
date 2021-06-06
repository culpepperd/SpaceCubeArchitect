using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]  // adds a line renderer component to what it's attached to if it doesn't have one
public class LocomotionTeleport : MonoBehaviour
{
    private enum Hand { Left, Right };

    [SerializeField]
    private Hand hand;

    [SerializeField]
    private Transform XR_Rig;

    [SerializeField]
    private Transform teleportReticle;

    private string buttonName;

    private Vector3 hitPoint;

    private bool canTeleport;

    [SerializeField]
    private int lineResolution = 20;
    [SerializeField]
    private float lineCurvature = 1f;
    private LineRenderer line;

    private bool teleportLock;

    [SerializeField]
    private MeshRenderer blackScreen;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        line.positionCount = lineResolution;

        buttonName = "XRI_" + hand + "_PrimaryButton";
        
    }

    void Update()
    {

        if(Input.GetButton(buttonName) && teleportLock == false)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit))
            {
                hitPoint = hit.point;

                //line.SetPosition(1, hitPoint);
                SetLinePositions(transform.position, hitPoint, lineCurvature);

                canTeleport = true;
                teleportReticle.gameObject.SetActive(true);
                teleportReticle.position = hitPoint;
            }
            else
            {
                SetLinePositions(transform.position, transform.position + transform.forward * 30f, 0f);
                // line.enabled = false;

                canTeleport = false;
                teleportReticle.gameObject.SetActive(false);
            }
        }
        else if(Input.GetButtonUp(buttonName))
        {
            line.enabled = false;
            teleportReticle.gameObject.SetActive(false);
            if (canTeleport == true)
            {
                StartCoroutine(FadedTeleport());
            }
        }

    }

    void SetLinePositions(Vector3 start, Vector3 end, float curve)
    {
        Vector3 startToEnd = end - start;
        Vector3 midPoint = start + startToEnd / 2 + Vector3.up * lineCurvature;

        for(int i = 0; i < lineResolution; i++)
        {
            float t = i / (float)lineResolution;
            Vector3 startToMid = Vector3.Lerp(start, midPoint, t);
            Vector3 midToEnd = Vector3.Lerp(midPoint, end, t);
            Vector3 curvePoint = Vector3.Lerp(startToMid, midToEnd, t);

            line.SetPosition(i, curvePoint);
        }
    }

    IEnumerator FadedTeleport()
    {
        teleportLock = true;

        float currentTime = 0;

        while(currentTime < 1)
        {
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
            // fade out
            blackScreen.material.color = Color.Lerp(Color.clear, Color.black, currentTime);
        }

        XR_Rig.position = hitPoint;

        yield return new WaitForSeconds(0.5f);

        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
            // fade in
            blackScreen.material.color = Color.Lerp(Color.clear, Color.black, currentTime);
        }

        teleportLock = false;
    }
}
