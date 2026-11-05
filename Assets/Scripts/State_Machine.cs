using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class State_Machine : MonoBehaviour
{
    private enum GliderState
    {
        Locked,
        Ready,
        Gliding
    }

    private enum SwordState
    {
        Locked,
        Ready,
        Attacking
    }

    private static readonly int AttackStateHash = Animator.StringToHash("Attack");
    private static readonly int IdleStateHash = Animator.StringToHash("Idle");

    [SerializeField, Min(0f)] private float gliderGravityMultiplier = 0.5f;
    [SerializeField, Min(0.01f)] private float attackAnimationDuration = 0.6f;

    private Rigidbody2D body;
    private Animator animator;
    private float normalGravityScale;
    private float attackStartedAt;
    private GliderState gliderState;
    private SwordState swordState;
    private bool canPlayAttack;

    public bool HasGlider => gliderState != GliderState.Locked;
    public bool IsGliding => gliderState == GliderState.Gliding;
    public bool HasSword => swordState != SwordState.Locked;
    public bool IsAttacking => swordState == SwordState.Attacking;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        normalGravityScale = body.gravityScale;

        animator = GetComponent<Animator>();
        canPlayAttack = animator != null
            && animator.HasState(0, AttackStateHash)
            && animator.HasState(0, IdleStateHash);

        if (!canPlayAttack)
        {
            Debug.LogWarning("State_Machine needs an Animator with 'Attack' and 'Idle' states to play the sword attack.", this);
        }
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        UpdateGlider(keyboard != null && keyboard.spaceKey.isPressed);
        UpdateSword(Mouse.current);
    }

    private void UpdateGlider(bool spaceHeld)
    {
        switch (gliderState)
        {
            case GliderState.Ready when spaceHeld:
                SetGliderState(GliderState.Gliding);
                break;

            case GliderState.Gliding when !spaceHeld:
                SetGliderState(GliderState.Ready);
                break;
        }
    }

    private void UpdateSword(Mouse mouse)
    {
        switch (swordState)
        {
            case SwordState.Ready when mouse != null && mouse.leftButton.wasPressedThisFrame:
                StartAttack();
                break;

            case SwordState.Attacking when Time.time - attackStartedAt >= attackAnimationDuration:
                animator.Play(IdleStateHash, 0, 0f);
                swordState = SwordState.Ready;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AbilityPickup pickup = other.GetComponent<AbilityPickup>();
        if (pickup == null)
        {
            pickup = other.GetComponentInParent<AbilityPickup>();
        }

        if (pickup != null && Unlock(pickup.Ability))
        {
            Destroy(pickup.gameObject);
        }
    }

    private bool Unlock(AbilityPickupType ability)
    {
        switch (ability)
        {
            case AbilityPickupType.Glider:
                if (HasGlider)
                {
                    return false;
                }

                SetGliderState(GliderState.Ready);
                return true;

            case AbilityPickupType.Sword:
                if (HasSword)
                {
                    return false;
                }

                swordState = SwordState.Ready;
                return true;

            default:
                return false;
        }
    }

    private void SetGliderState(GliderState newState)
    {
        if (gliderState == newState)
        {
            return;
        }

        gliderState = newState;
        body.gravityScale = gliderState == GliderState.Gliding
            ? normalGravityScale * gliderGravityMultiplier
            : normalGravityScale;
    }

    private void StartAttack()
    {
        if (!canPlayAttack)
        {
            return;
        }

        animator.Play(AttackStateHash, 0, 0f);
        attackStartedAt = Time.time;
        swordState = SwordState.Attacking;
    }
}
