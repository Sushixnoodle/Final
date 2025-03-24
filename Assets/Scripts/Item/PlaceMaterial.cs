using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMaterial : MonoBehaviour
{
    [Tooltip("The GameObject to enable when this object is clicked")]
    public GameObject objectToEnable;

    private void OnMouseDown()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}
