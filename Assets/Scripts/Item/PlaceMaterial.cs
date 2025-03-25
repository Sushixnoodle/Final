using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMaterial : MonoBehaviour
{
    [Tooltip("List of GameObjects to enable when this object is clicked")]
    public List<GameObject> objectsToEnable;

    private void OnMouseDown()
    {
        if (objectsToEnable != null && objectsToEnable.Count > 0)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}