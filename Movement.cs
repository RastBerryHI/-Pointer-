using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Guns : byte
{
    AssaultRifle,
    Pistol,
    Special
}


public class Movement : MonoBehaviour
{
    [Header("Character parameters")]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float baseFov = 35f;

    [Header("Animation types")]
    [Space]
    public Animator anim;
    [Space]

    [SerializeField]
    RuntimeAnimatorController currentController;

    [SerializeField]
    RuntimeAnimatorController mainController;
    [Space]
    [SerializeField]
    RuntimeAnimatorController secondaryController;

    public Camera playerCamera;
    [Space]
    [Header("Flags")]
    public bool isAiming = false;
    public bool isMelee = false;
    [Header("Equipped")]
    public List<ProjectileGun> Guns = new List<ProjectileGun>();
    [SerializeField]
    bool isChanged = false;

    [Space]
    [Header("Main")]
    [SerializeField]
    ProjectileGun MainGun;
    [SerializeField]
    GameObject MainGFX;
    [SerializeField]
    GameObject AssaultRifleObject;
    [Space]
    [Header("Secondary")]
    [SerializeField]
    ProjectileGun SecondaryGun;
    [SerializeField]
    GameObject SecondaryGFX;
    [SerializeField]
    GameObject PistolObject;


    CharacterController controller;
    Vector3 velocity;
    [SerializeField]
    AnimEvemts AnimEvemts;

    [SerializeField]
    float akAimFov = 15f;
    [SerializeField]
    GameObject[] magBullets;
    [SerializeField]
    Transform AkCasePosition;
    [SerializeField]
    Transform PistolCasePosition;

    [Header("UI")]
    [SerializeField]
    Text AmmoStatus;

    bool isReloaded = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        SecondaryGFX.SetActive(false);
        PistolObject.SetActive(false);
    }

    // Update is called once per frame

    void AnimateMovement(Vector3 move) 
    {
        anim.SetFloat("velocity", move.magnitude);
    }

    void AnimateMelee() 
    {
        if (Input.GetKeyDown(KeyCode.V))
            isMelee = true;

        anim.SetBool("isMelee", isMelee);
        isMelee = false;
    }

    void AnimateShoot() 
    {
        if (Guns[0].isShoot && !isMelee)
        {
            anim.Play("Shoot");
        }
        
        Guns[0].isShoot = false;
    }
    
    IEnumerator HideRounds(float time) 
    {
        yield return new WaitForSeconds(time);
        isReloaded = true;
    }

    void AnimateReload() 
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Guns[0].currentMagCapacity > 0 && Guns[0].currentMagCapacity < Guns[0].magCapacity) 
            {
                anim.Play("Reload Not Empty");
            }
            else if(Guns[0].currentMagCapacity <= 0) 
            {
                anim.Play("Reload Empty");
            }

            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Not Empty") || !anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Empty")) 
            {
                isReloaded = false;
                StartCoroutine(HideRounds(anim.GetCurrentAnimatorStateInfo(0).length));
            }
        } 
    }

    void AnimateAim()
    {
        if (Input.GetMouseButton(1))
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, akAimFov, .1f);
            anim.SetBool("isAim", true);
            isAiming = true;
            if (isAiming == true) 
            {
                if (Guns[0].isShoot && !isMelee)
                {
                    anim.Play("Aim Fire");
                }
                Guns[0].isShoot = false;
            }
        }
        else
        {
            anim.SetBool("isAim", false);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, baseFov, .05f);
            isAiming = false;
        }
    }

    // Ammo UI space
    void AnimateCheckAmmo() 
    {
        anim.SetBool("isCheckAmmo", Input.GetKeyDown(KeyCode.T));
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Check Ammo 1") || (anim.GetCurrentAnimatorStateInfo(0).IsName("Check Ammo 2")))
        {
            if(Guns[0].currentMagCapacity == Guns[0].magCapacity) 
            {
                AmmoStatus.text = "Full";
            }
            else if(Guns[0].currentMagCapacity < Guns[0].magCapacity && Guns[0].currentMagCapacity > Guns[0].magCapacity / 2) 
            {
                AmmoStatus.text = "Nearly full";
            }
            else if(Guns[0].currentMagCapacity == Guns[0].magCapacity / 2) 
            {
                AmmoStatus.text = "Half-full";
            }
            else if(Guns[0].currentMagCapacity < Guns[0].magCapacity / 2 && Guns[0].currentMagCapacity > 0) 
            {
                AmmoStatus.text = "Nearly empty";
            }
            else if(Guns[0].currentMagCapacity == 0) 
            {
                AmmoStatus.text = "Empty";
            }
        }
        else 
        {
            AmmoStatus.text = "";
        }
    }
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    void CheckEmptyness() 
    {
        switch (Guns[0].name) 
        {
            case "AK":
                if (Guns[0].currentMagCapacity > Guns[0].magCapacity / 2 || isReloaded == false)
                {
                    magBullets[0].SetActive(true);
                }
                else if (Guns[0].currentMagCapacity == 0)
                {
                    magBullets[0].SetActive(false);
                }
                break;
            case "Pistol":
                if (Guns[0].currentMagCapacity > Guns[0].magCapacity / 2 || isReloaded == false)
                {
                    magBullets[1].SetActive(true);
                }
                else if (Guns[0].currentMagCapacity == 0)
                {
                    magBullets[1].SetActive(false);
                }
                break;
        }
    }
    
    IEnumerator SwitchToSecondary() 
    {
        yield return new WaitForSeconds(0.25f);

        MainGFX.SetActive(false);
        SecondaryGFX.SetActive(true);

        AssaultRifleObject.SetActive(false);
        PistolObject.SetActive(true);

        anim.runtimeAnimatorController = secondaryController;
        isChanged = true;
    }

    IEnumerator SwitchToMain()
    {
        yield return new WaitForSeconds(0.4f);

        MainGFX.SetActive(true);
        SecondaryGFX.SetActive(false);

        AssaultRifleObject.SetActive(true);
        PistolObject.SetActive(false);

        anim.runtimeAnimatorController = mainController as RuntimeAnimatorController;
        isChanged = false;
    }

    void AnimateChangeWeapon() 
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isChanged) 
        {
            anim.Play("Switch");
            StartCoroutine(SwitchToSecondary());
            Guns[0] = SecondaryGun;
            Guns[1] = MainGun;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isChanged)
        {
            anim.Play("Switch");
            Guns[1] = SecondaryGun;
            Guns[0] = MainGun;
            StartCoroutine(SwitchToMain());
        }
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        ApplyGravity();

        currentController = anim.runtimeAnimatorController;

        controller.Move(velocity * Time.deltaTime);

        if(move.magnitude > 0) 
        {
            AnimEvemts.Footsteps();
        }

        AnimateChangeWeapon();

        AnimateMovement(move);
        AnimateAim();
        CheckEmptyness();
        AnimateReload();

        AnimateCheckAmmo();
        AnimateMelee();
        AnimateShoot();
    }
}
