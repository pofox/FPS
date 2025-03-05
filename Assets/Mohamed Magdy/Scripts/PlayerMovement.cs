using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float cameraSpeed = 1f;
    [SerializeField] GameObject playerCamera;

    private string groundtag = "ground";
    private Vector3 dir;
    private Rigidbody rb;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private Collider onGround;
    private float cameraPitch = 0f;
    [HideInInspector] public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        onGround = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpBufferCounter -= Time.deltaTime;
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpBufferCounter = 0;
        }
    }
    private void FixedUpdate()
    {
        // Convert the input direction to the player's local space
        Vector3 localDirection = transform.TransformDirection(dir);

        // Apply local movement to the Rigidbody
        rb.linearVelocity = new Vector3(localDirection.x * speed, rb.linearVelocity.y, localDirection.z * speed);

        // Apply gravity multiplier when falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private void OnMove(InputValue value)
    {
        Vector2 val = value.Get<Vector2>();
        dir = new Vector3(val.x, 0, val.y);
    }
    private void OnJump(InputValue value)
    {
        jumpBufferCounter = jumpBufferTime;
    }
    

    private void OnMouse(InputValue value)
    {
        if (!GameManager.Instance.paused)
        {
            Vector2 delta = value.Get<Vector2>();
            rb.rotation = Quaternion.Euler(new Vector3(0, delta.x * cameraSpeed, 0) + rb.rotation.eulerAngles);
            cameraPitch -= delta.y * cameraSpeed;
            cameraPitch = Mathf.Clamp(cameraPitch, -45f, 45f);
            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == groundtag)
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == groundtag)
        {
            isGrounded = false;
        }
    }
}
