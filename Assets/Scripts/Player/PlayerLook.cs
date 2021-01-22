using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;

    [SerializeField] private float mouseSensitivity;

    [SerializeField] private float maxDistance;

    [SerializeField] private Transform playerBody;


    ScrollThrough lastHit;
    float xAxisClamp = 0.0f;

    PlayerController playerController;

    void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        LockCursor();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        DrawRayCast();
    }

    void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        // look up
        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void DrawRayCast()
    {
        RaycastHit hit;

        bool move = false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {

            if (hit.collider.CompareTag("object"))
            {
                lastHit = hit.collider.gameObject.GetComponent<ScrollThrough>();
                lastHit.LookAt(true);
                move = true;
            }

        }


        if (!move && lastHit != null)
        {
            lastHit.LookAt(false);
            lastHit = null;
        }
    }
}
    

