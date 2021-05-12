using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPawn : MonoBehaviour
{
    [SerializeField]
    AudioClip[] grassCollision;
    [SerializeField]
    AudioSource AudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Collider>() != null) 
        {
            AudioSource.PlayOneShot(grassCollision[Random.Range(0, grassCollision.Length - 1)]);
        }
    }
}
