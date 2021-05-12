using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    public float mouseSensitivity = 60f;
    public Transform playerBody;

    float xRotation = 0f;

    float vRecoil;
    float hRecoil;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime + hRecoil;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime + vRecoil;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void AddRecoil(float v, float h) 
    {
        vRecoil = v;
        hRecoil = h;
    }
}
