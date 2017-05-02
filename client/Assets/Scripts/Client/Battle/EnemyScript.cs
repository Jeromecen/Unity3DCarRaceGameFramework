using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private bool hasSpawn;
    private MoveScript moveScript;

    void Awake()
    {
        // Retrieve scripts to disable when not spawn
        moveScript = GetComponent<MoveScript>();
    }

    void Start()
    {
        hasSpawn = false;

        // Disable everything
        // -- collider
        GetComponent<Collider2D>().enabled = false;
        // -- Moving
        moveScript.enabled = false;
    }

    void Update()
    {
        // Check if the enemy has spawned
        if (hasSpawn == false)
        {
            if (GetComponent<Renderer>().IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            // Out of camera?
            if (GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false)
            {
                var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
                transform.position = new Vector3(
                  transform.position.x,
                  transform.position.y + bottomBorder.y * 2,
                  transform.position.z
                  );
                //Destroy(gameObject);
            }
        }
    }

    private void Spawn()
    {
        hasSpawn = true;

        // Enable everything
        // -- Collider
        GetComponent<Collider2D>().enabled = true;
        // -- Moving
        moveScript.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with enemy
        PlayerMove enemy = collision.gameObject.GetComponent<PlayerMove>();
        if (enemy != null)
        {
//            var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
            transform.position = new Vector3(
              transform.position.x,
              transform.position.y ,
              transform.position.z
              );
            
        }  
    }
}