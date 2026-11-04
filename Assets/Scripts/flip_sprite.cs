using UnityEngine;
using UnityEngine.InputSystem;

public class flip_sprite : MonoBehaviour
{
    private float horizontalInput;
    private bool isFacingRight = true;

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        horizontalInput = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            horizontalInput -= 1f;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            horizontalInput += 1f;
        }

       
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
       
        isFacingRight = !isFacingRight;

      
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}