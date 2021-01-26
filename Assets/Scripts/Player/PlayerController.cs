using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    private float movementSpeed;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;


    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private CharacterController controller;

    [SerializeField] private AnimationCurve jumpFalloff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    bool isJumping;
    bool isCarrying;

    Transform item;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    

    void PlayerMovement()
    {
        float verticalInput = Input.GetAxis(verticalInputName);
        float horizontalInput = Input.GetAxis(horizontalInputName);

        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 rightMovement = transform.right * horizontalInput;

        controller.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if ((verticalInput != 0 || horizontalInput != 0) && OnSlope())
        {
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }

        SetMovementSpeed();
        JumpInput();
    }

    void SetMovementSpeed()
    {
        if (Input.GetKey(runKey))
        {
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }
    }

    bool OnSlope()
    {
        if (isJumping)
        {
            return false;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
            
        }

        return false;
    }

    void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        // stop jittering
        controller.slopeLimit = 90.0f;
        float inAir = 0.0f;
        do
        {
            float jumpForce = jumpFalloff.Evaluate(inAir);
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            inAir += Time.deltaTime;
            yield return null;
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = 45.0f;
        isJumping = false;
    }


    public bool CheckIfCarrying()
    {
        return !isCarrying;
    }

    public void Carrying()
    {
        isCarrying = true;
    }

    public void AddCarryItem(Transform t)
    {
        t.tag = "pickup";
        t.SetParent(transform.GetChild(0).GetChild(0));
        item = t;
    }

    public Transform returnItem()
    {
        return item;
    }

    public void RemoveCarryItem()
    {
        item = null;
    }


}
