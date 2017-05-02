using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public Vector3 speed = new Vector3(1, 1, 1);
    public Vector3 direction = new Vector3(0, 1, 0);
    private Vector2 movement;
    private Rigidbody2D rigidBodyComponent;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // 2 - Retrieve axis information
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 3 - Movement per direction
        movement = new Vector3(
          speed.x * inputX,
          speed.y * inputY);


        // 6 - Make sure we are not outside the camera bounds
        var dist = (transform.position - Camera.main.transform.position);
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, dist.y, dist.z)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, dist.y, dist.z)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(dist.x, 0, dist.z)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(dist.x, 1, dist.z)).y;

        transform.position = new Vector3(
                  Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                  Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
                  transform.position.z
                  );
    }

    void FixedUpdate()
    {
        // 4 - Move the game object
        if (rigidBodyComponent == null) rigidBodyComponent = GetComponent<Rigidbody2D>();
        rigidBodyComponent.velocity = movement;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        // Collision with enemy
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Kill the enemy
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);

            damagePlayer = true;
        }

        // Collision with the boss
        /**
        BossScript boss = collision.gameObject.GetComponent<BossScript>();
        if (boss != null)
        {
            // Boss lose some hp too
            HealthScript bossHealth = boss.GetComponent<HealthScript>();
            if (bossHealth != null) bossHealth.Damage(5);

            damagePlayer = true;
        }
        */

        // Damage the player
        if (damagePlayer)
        {
            HealthScript playerHealth = this.GetComponent<HealthScript>();
            if (playerHealth != null) playerHealth.Damage(1);
        }
    }
}
