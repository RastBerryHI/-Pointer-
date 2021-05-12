using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounder : MonoBehaviour
{
    Ray ray;
    [SerializeField]
    float length = 2f;
    [SerializeField]
    LayerMask layerMask;

    public string layer;
    void Update()
    {
        ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        Debug.DrawLine(transform.position, -transform.up, Color.red, length);
        if(Physics.Raycast(ray, out hit, length, layerMask, QueryTriggerInteraction.Collide)) 
        {
            layer = LayerMask.LayerToName(hit.transform.gameObject.layer);
        }
    }
}
