using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Vector2 speed = new Vector2(2, 2);

    public Vector2 direction = new Vector2(0, -1);

    private Vector2 movement;

    void Update()
    {
        // 1 - Movement
        movement = new Vector2(
          speed.x * direction.x,
          speed.y * direction.y);
    }

    void FixedUpdate()
    {
        // Apply movement to the rigidbody
        GetComponent<Rigidbody2D>().velocity = movement;
    }
}