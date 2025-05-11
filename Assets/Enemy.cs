//using UnityEngine;

//public class Enemy : MonoBehaviour
//{
//    private Rigidbody enemyRb;
//    public float speed = 5f;
//    private GameObject player;

//    void Start()
//    {
//        enemyRb = GetComponent<Rigidbody>();
//        player = GameObject.Find("Player");
//    }

//    void FixedUpdate()
//    {
//        if (player != null)
//        {
//            Vector3 direction = (player.transform.position - transform.position).normalized;

//            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

//            enemyRb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
//        }
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            Debug.Log("Enemy hit the Player!");
//        }
//    }
//}

using UnityEngine;
using Invector;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private Animator animator;
    private GameObject player;
    private vHealthController healthController;

    public float speed = 5f;
    public float punchDistance = 1.5f;
    private bool isDead = false;
    private bool isRunning = false;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        healthController = GetComponent<vHealthController>();

        if (healthController != null)
        {
            healthController.onDead.AddListener(OnEnemyDead);
        }
    }

    void FixedUpdate()
    {
        if (player != null && !isDead)
        {
            Vector3 direction = player.transform.position - transform.position;
            float distance = direction.magnitude;

            if (distance <= punchDistance)
            {
                // Close enough to punch
                animator.SetTrigger("Punch");
                animator.SetFloat("Speed", 0f);
                isRunning = false;
            }
            else
            {
                // Need to run towards player
                isRunning = true;
                animator.SetFloat("Speed", speed);

                direction.Normalize();
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

                Vector3 nextPos = transform.position + direction * speed * Time.fixedDeltaTime;
                RaycastHit hit;
                if (Physics.Raycast(nextPos + Vector3.up * 1f, Vector3.down, out hit, 2f))
                {
                    nextPos.y = hit.point.y;
                }
                enemyRb.MovePosition(nextPos);
            }
        }
        else
        {
            // No player or enemy is dead
            animator.SetFloat("Speed", 0f);
            isRunning = false;
        }
    }

    // Called every frame after all physics calculations
    void LateUpdate()
    {
        // Ensure the Speed parameter is consistently set in the animator
        if (isRunning && !isDead)
        {
            animator.SetFloat("Speed", speed);
        }
    }

    private void OnEnemyDead(GameObject obj)
    {
        isDead = true;
        isRunning = false;
        animator.SetTrigger("IsDead");
        animator.SetFloat("Speed", 0f);
        enemyRb.linearVelocity = Vector3.zero;
        enemyRb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the Player!");
        }
    }
}


