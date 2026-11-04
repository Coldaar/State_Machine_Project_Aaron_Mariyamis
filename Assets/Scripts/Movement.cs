using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animation_Script))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayers = ~0;

    private Rigidbody2D body;
    private Collider2D bodyCollider;
    private Animation_Script animationScript;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[1];
    private ContactFilter2D groundFilter;
    private float horizontalInput;
    private bool jumpRequested;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        animationScript = GetComponent<Animation_Script>();
        if (animationScript == null)
        {
            animationScript = gameObject.AddComponent<Animation_Script>();
        }

        if (bodyCollider == null)
        {
            Debug.LogError("Movement requires a 2D collider on the same GameObject.", this);
            enabled = false;
            return;
        }

        groundFilter.SetLayerMask(groundLayers);
        groundFilter.useTriggers = false;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            horizontalInput = 0f;
            return;
        }

        horizontalInput = 0f;
        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
        {
            horizontalInput -= 1f;
        }

        if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
        {
            horizontalInput += 1f;
        }

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }

    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontalInput * moveSpeed, body.linearVelocity.y);

        bool isGrounded = bodyCollider.Cast(Vector2.down, groundFilter, groundHits, 0.05f) > 0;
        if (jumpRequested && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        animationScript.UpdateAnimation(horizontalInput, body.linearVelocity.y, isGrounded);
        jumpRequested = false;
    }
}
