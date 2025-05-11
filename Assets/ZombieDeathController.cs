using System.Collections;
using UnityEngine;
using Invector;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(vHealthController))]
public class ZombieDeathController : MonoBehaviour
{
    [Header("Death Settings")]
    [Tooltip("Time to destroy the zombie after death animation starts")]
    public float destroyAfterDeathTime = 3f;

    [Tooltip("Freeze position when dying to prevent being pushed out of terrain")]
    public bool freezePositionOnDeath = true;

    [Header("References")]
    private Animator animator;
    private vHealthController healthController;
    private Rigidbody rb;
    private Collider[] colliders;

    // Animation hash parameters
    private int deathAnimHash;
    private bool isDying = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthController = GetComponent<vHealthController>();
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        // Cache animation parameter hash for better performance
        deathAnimHash = Animator.StringToHash("Death");
    }

    private void OnEnable()
    {
        // Subscribe to death event from health controller
        healthController.onDead.AddListener(HandleDeath);
    }

    private void OnDisable()
    {
        // Clean up the listener when disabled
        healthController.onDead.RemoveListener(HandleDeath);
    }

    public void HandleDeath(GameObject victim)
    {
        if (isDying || victim != gameObject) return;

        isDying = true;

        // Trigger death animation
        if (animator)
        {
            animator.SetTrigger(deathAnimHash);
        }

        if (freezePositionOnDeath && rb)
        {
            // Freeze position but allow rotation to fall naturally
            rb.constraints = RigidbodyConstraints.FreezePosition;

            // Or fully kinematic to prevent any physics interactions
            // rb.isKinematic = true;
        }

        // Disable colliders to prevent further interactions
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        // Start the destroy coroutine
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the animation to complete
        yield return new WaitForSeconds(destroyAfterDeathTime);

        // Destroy the game object
        Destroy(gameObject);
    }
}