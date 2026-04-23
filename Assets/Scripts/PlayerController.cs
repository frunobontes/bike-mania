using UnityEngine;

// Minimal 2D player controller to approximate Bike Mania bike physics.
// This is a starting point — tweak physics settings inside Unity for feel.
public class PlayerController : MonoBehaviour
{
    public float motorForce = 200f; // force applied to move forward/back
    public float torque = 200f; // torque applied to rotate the bike
    public float maxSpeed = 20f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.angularDrag = 2f;
            rb.drag = 0.5f;
        }
    }

    void Update()
    {
        // Input
        float move = Input.GetAxis("Vertical"); // accelerate/brake
        float turn = Input.GetAxis("Horizontal"); // tilt bike

        // Apply motor force in the forward (local right) direction
        Vector2 forward = transform.right; // bike faces right by default in 2D
        if (Mathf.Abs(rb.velocity.magnitude) < maxSpeed)
        {
            rb.AddForce(forward * move * motorForce * Time.deltaTime);
        }

        // Apply torque for rotation
        rb.AddTorque(-turn * torque * Time.deltaTime);

        // Simple wheelie control: holding space gives a backward torque
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddTorque(50f * Time.deltaTime);
        }
    }
}
