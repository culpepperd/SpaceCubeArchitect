using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverInteractions : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Lever")
        {
            onTriggerEntered.Invoke();
        }
    }
}
