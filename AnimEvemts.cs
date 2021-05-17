using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvemts : MonoBehaviour
{
    [SerializeField]
    Movement movement;
    ProjectileGun Gun;
    [SerializeField]
    GameObject Mag;
    [SerializeField]
    GameObject SpawnPos;
    [SerializeField]
    float MagTimeToLive = 10f;

    [Header("AK")]
    [SerializeField]
    AudioClip[] reloads;
    [Header("Glock")]
    [SerializeField]
    AudioClip[] reloadsGlck;

    [SerializeField]
    Grounder grounder;
    [SerializeField]
    AudioSource audioSource;
    [Header("Footsteps SFX")]
    [Space]
    [Header("Concrete")]
    [SerializeField]
    AudioClip[] footSteps;
    [Header("Wood")]
    [SerializeField]
    AudioClip[] footSteps2;
    [Header("Metal")]
    [SerializeField]
    AudioClip[] footSteps3;

    

    void ReloadEmptyAK() 
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloads[0]);
    }

    void ReloadEmptyGlck() 
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[0]);
    }

    void UngearAK() 
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloads[1]);
    }

    void UngearGlck()
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[1]);
    }

    void HoldGlockStart() 
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[4]);
    }

    void HoldGlockMid()
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[5]);
    }

    void HoldGlockEnd()
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[5]);
    }

    void PullInMagAK() 
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloads[2]);
    }

    void PullingInMagGlck()
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[2]);
    }

    void LoadBulletAK() 
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloads[3]);
    }

    void LoadBulletGlck()
    {
        Gun = movement.Guns[0];
        Gun.audioSource.PlayOneShot(reloadsGlck[3]);
    }


    void StopReload()
    {
        Gun = movement.Guns[0];
        Gun.currentMagCapacity = Gun.magCapacity;
    }

   
    void DropMag()
    {
        if (Gun.currentMagCapacity <= (Gun.magCapacity / 2))
        {
            GameObject newMag = Instantiate(Mag);
            newMag.transform.position = SpawnPos.transform.position;
            Destroy(newMag, MagTimeToLive);
        }
    }

    public void Footsteps() 
    {
        if (!audioSource.isPlaying)
        {
            if(grounder.layer == "Wood") 
            {
                AudioClip clip = footSteps2[Random.Range(0, footSteps2.Length - 1)];
                audioSource.PlayOneShot(clip);
            }
            else if (grounder.layer == "Metal")
            {
                AudioClip clip = footSteps3[Random.Range(0, footSteps3.Length - 1)];
                audioSource.PlayOneShot(clip);
            }
            else 
            {
                AudioClip clip = footSteps[Random.Range(0, footSteps.Length - 1)];
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
