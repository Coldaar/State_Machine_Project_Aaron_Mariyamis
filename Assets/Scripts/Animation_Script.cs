using UnityEngine;

public class Animation_Script : MonoBehaviour
{
    private static readonly int IsRunningParameter = UnityEngine.Animator.StringToHash("IsRunning");
    private static readonly int IsJumpingParameter = UnityEngine.Animator.StringToHash("IsJumping");
    private static readonly int IsFallingParameter = UnityEngine.Animator.StringToHash("IsFalling");
    private static readonly int IsGroundedParameter = UnityEngine.Animator.StringToHash("IsGrounded");

    [SerializeField] private UnityEngine.Animator animator;
    [SerializeField] private float verticalVelocityThreshold = 0.1f;

    private bool hasRunningParameter;
    private bool hasJumpingParameter;
    private bool hasFallingParameter;
    private bool hasGroundedParameter;
    private bool hasStartedFalling;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<UnityEngine.Animator>();
        }

        if (animator == null)
        {
            Debug.LogWarning("Animation_Script requires an Animator component on the same GameObject.", this);
            return;
        }

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type != AnimatorControllerParameterType.Bool)
            {
                continue;
            }

            if (parameter.nameHash == IsRunningParameter) hasRunningParameter = true;
            else if (parameter.nameHash == IsJumpingParameter) hasJumpingParameter = true;
            else if (parameter.nameHash == IsFallingParameter) hasFallingParameter = true;
            else if (parameter.nameHash == IsGroundedParameter) hasGroundedParameter = true;
        }

        if (!hasRunningParameter || !hasJumpingParameter || !hasFallingParameter || !hasGroundedParameter)
        {
            Debug.LogWarning("The assigned Animator Controller needs bool parameters named 'IsRunning', 'IsJumping', 'IsFalling', and 'IsGrounded'.", this);
        }
    }

    public void UpdateAnimation(float horizontalInput, float verticalVelocity, bool isGrounded)
    {
        if (isGrounded)
        {
            hasStartedFalling = false;
        }
        else if (verticalVelocity < -verticalVelocityThreshold)
        {
            hasStartedFalling = true;
        }

        if (hasRunningParameter)
        {
            animator.SetBool(IsRunningParameter, Mathf.Abs(horizontalInput) > 0.01f);
        }

        if (hasJumpingParameter)
        {
            animator.SetBool(IsJumpingParameter, !isGrounded && !hasStartedFalling);
        }

        if (hasFallingParameter)
        {
            animator.SetBool(IsFallingParameter, !isGrounded && hasStartedFalling);
        }

        if (hasGroundedParameter)
        {
            animator.SetBool(IsGroundedParameter, isGrounded);
        }
    }
}
