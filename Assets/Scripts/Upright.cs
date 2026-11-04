using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Upright : MonoBehaviour
{
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints |= RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        body.angularVelocity = 0f;
    }
}
