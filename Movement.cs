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
    [Space]
    [Header("Secondary")]
    [SerializeField]
    GameObject SecondaryGFX;


    CharacterController controller;
    Vector3 velocity;
    [SerializeField]
    AnimEvemts AnimEvemts;

    [SerializeField]
    float akAimFov = 15f;
    [SerializeField]
    GameObject[] magBullets;

    float v, h, decV, decH;

    [Header("UI")]
    [SerializeField]
    Text AmmoStatus;

    bool isReloaded = true;

    void Start()
    {
        v = MainGun.vRecoil;
        h = MainGun.hRecoil;
        decV = v / 2;
        decH = h / 4;

        controller = GetComponent<CharacterController>();
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
        if (MainGun.isShoot && !isMelee)
        {
            anim.Play("Shoot");
        }
        MainGun.isShoot = false;
    }
    
    IEnumerator HideRounds(float time) 
    {
        yield return new WaitForSeconds(time);
        Debug.Log(true);
        isReloaded = true;
    }

    void AnimateReload() 
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (MainGun.currentMagCapacity > 0 && MainGun.currentMagCapacity < MainGun.magCapacity) 
            {
                anim.Play("Reload Not Empty");
            }
            else if(MainGun.currentMagCapacity <= 0) 
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
            MainGun.vRecoil = decV;
            MainGun.hRecoil = decH;

            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, akAimFov, .1f);
            anim.SetBool("isAim", true);
            isAiming = true;
            if (isAiming == true) 
            {
                if (MainGun.isShoot && !isMelee)
                {
                    anim.Play("Aim Fire");
                }
                MainGun.isShoot = false;
            }
        }
        else
        {
            MainGun.vRecoil = v;
            MainGun.hRecoil = h;

            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, baseFov, .1f);
            anim.SetBool("isAim", false);
            isAiming = false;
        }
    }

    void AnimateCheckAmmo() 
    {
        anim.SetBool("isCheckAmmo", Input.GetKeyDown(KeyCode.T));
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Check Ammo 1") || (anim.GetCurrentAnimatorStateInfo(0).IsName("Check Ammo 2")))
        {
            if(MainGun.currentMagCapacity == MainGun.magCapacity) 
            {
                AmmoStatus.text = "Full";
            }
            else if(MainGun.currentMagCapacity < MainGun.magCapacity && MainGun.currentMagCapacity > MainGun.magCapacity / 2) 
            {
                AmmoStatus.text = "Nearly full";
            }
            else if(MainGun.currentMagCapacity == MainGun.magCapacity / 2) 
            {
                AmmoStatus.text = "Half-full";
            }
            else if(MainGun.currentMagCapacity < MainGun.magCapacity / 2 && MainGun.currentMagCapacity > 0) 
            {
                AmmoStatus.text = "Nearly empty";
            }
            else if(MainGun.currentMagCapacity == 0) 
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
        if (MainGun.currentMagCapacity > MainGun.magCapacity / 2 || isReloaded == false) 
        {
            magBullets[0].SetActive(true);
            magBullets[1].SetActive(true);
        }
        else if(MainGun.currentMagCapacity <= MainGun.magCapacity / 2 && MainGun.currentMagCapacity > 0)
        {
            magBullets[0].SetActive(false);
            magBullets[1].SetActive(true);
        }
        else if(MainGun.currentMagCapacity == 0)
        {
            magBullets[0].SetActive(false);
            magBullets[1].SetActive(false);
        }
    }
    
    IEnumerator SwitchToSecondary() 
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 0.5f);
        MainGFX.SetActive(false);
        SecondaryGFX.SetActive(true);
        anim.runtimeAnimatorController = secondaryController;

        isChanged = true;
    }

    IEnumerator SwitchToMain()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 0.5f);
        MainGFX.SetActive(true);
        SecondaryGFX.SetActive(false);
        anim.runtimeAnimatorController = mainController;

        isChanged = false;
    }

    void AnimateChangeWeapon() 
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isChanged) 
        {
            anim.SetBool("isSwitch", true);
            StartCoroutine(SwitchToSecondary());
        }
        else if(Input.GetKeyDown(KeyCode.Q) && isChanged) 
        {
            anim.SetBool("isSwitchPistol", true);
            StartCoroutine(SwitchToMain());
        }
    }
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        ApplyGravity();

        currentController = anim.runtimeAnimatorController;

        AnimateMovement(move);
        controller.Move(velocity * Time.deltaTime);

        if(move.magnitude > 0) 
        {
            AnimEvemts.Footsteps();
        }

        AnimateChangeWeapon();

        AnimateAim();
        CheckEmptyness();
        AnimateReload();

        AnimateCheckAmmo();
        AnimateMelee();
        AnimateShoot();
    }
}
