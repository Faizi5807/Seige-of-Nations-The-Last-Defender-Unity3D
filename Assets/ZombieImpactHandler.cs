using UnityEngine;
using Invector;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class ZombieImpactHandler : MonoBehaviour
{
    [Header("Impact Settings")]
    [Tooltip("Maximum force that can push the zombie")]
    public float maxImpactForce = 0.5f; // Reduced from 2.0f

    [Tooltip("Maximum distance the zombie can be pushed")]
    public float maxPushDistance = 1.5f; // Limit how far zombie can be pushed

    [Tooltip("How quickly the zombie recovers position after being hit")]
    public float recoverySpeed = 2.0f;

    [Tooltip("Enable to clamp position to prevent falling off terrain")]
    public bool preventFallingOffTerrain = true;

    [Tooltip("Temporarily disable NavMeshAgent when hit")]
    public bool disableNavMeshWhenHit = true;

    [Header("Terrain Boundaries")]
    [Tooltip("Set to match your 2km x 2km terrain")]
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minZ = -1000f;
    public float maxZ = 1000f;

    [Tooltip("Auto-detect terrain boundaries on start")]
    public bool autoDetectTerrainBoundaries = true;

    // References
    private Rigidbody rb;
    private vHealthController healthController;
    private NavMeshAgent navAgent;
    private Vector3 originalPosition;
    private Vector3 startPosition;
    private bool isRecovering = false;
    private float recoveryTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthController = GetComponent<vHealthController>();
        navAgent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        startPosition = transform.position;

        if (autoDetectTerrainBoundaries)
        {
            DetectTerrainBoundaries();
        }

        // Make sure rigidbody is properly configured
        if (rb)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.linearDamping = 2f; // Add drag to slow down push effects
        }
    }

    private void DetectTerrainBoundaries()
    {
        // Try to find the terrain in the scene
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            // Get terrain size
            Vector3 terrainSize = terrain.terrainData.size;
            Vector3 terrainPosition = terrain.transform.position;

            // Calculate terrain boundaries
            minX = terrainPosition.x;
            maxX = terrainPosition.x + terrainSize.x;
            minZ = terrainPosition.z;
            maxZ = terrainPosition.z + terrainSize.z;

            // Add small buffer inside the terrain (5 meters)
            minX += 5f;
            maxX -= 5f;
            minZ += 5f;
            maxZ -= 5f;

            Debug.Log($"Terrain boundaries detected: X({minX} to {maxX}), Z({minZ} to {maxZ})");
        }
        else
        {
            Debug.LogWarning("No terrain found. Using default boundary values.");
        }
    }

    private void OnEnable()
    {
        if (healthController)
        {
            healthController.onReceiveDamage.AddListener(HandleImpact);
        }
    }

    private void OnDisable()
    {
        if (healthController)
        {
            healthController.onReceiveDamage.RemoveListener(HandleImpact);
        }
    }

    private void Update()
    {
        // Ensure zombie stays within terrain boundaries
        if (preventFallingOffTerrain && !healthController.isDead)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

            if (clampedPosition != transform.position)
            {
                transform.position = clampedPosition;
            }
        }

        // Handle recovery after getting hit
        if (isRecovering && !healthController.isDead)
        {
            recoveryTimer -= Time.deltaTime;

            if (recoveryTimer <= 0)
            {
                // Re-enable NavMeshAgent if it exists and was disabled
                if (navAgent && !navAgent.enabled && disableNavMeshWhenHit)
                {
                    navAgent.enabled = true;
                }

                isRecovering = false;
            }
        }
    }

    public void HandleImpact(vDamage damage)
    {
        if (healthController.isDead || damage == null) return;

        // Store position before impact for limiting movement
        startPosition = transform.position;

        // Apply limited impact force in the direction of the hit
        if (damage.hitPosition != Vector3.zero && rb)
        {
            // Calculate direction from hit position to zombie
            Vector3 pushDirection = (transform.position - damage.hitPosition).normalized;
            pushDirection.y = 0; // Keep force horizontal

            // Apply very reduced force - much lower than before
            float forceMagnitude = Mathf.Min(damage.damageValue * 0.05f, maxImpactForce);
            rb.linearVelocity = Vector3.zero; // Clear any existing velocity first
            rb.AddForce(pushDirection * forceMagnitude, ForceMode.Impulse);

            // Temporarily disable NavMeshAgent if it exists
            if (navAgent && navAgent.enabled && disableNavMeshWhenHit)
            {
                navAgent.enabled = false;
            }

            // Start recovery timer
            isRecovering = true;
            recoveryTimer = 0.5f; // Short recovery time

            // Start a coroutine to limit how far the zombie can move
            StartCoroutine(LimitMovement());
        }
    }

    private System.Collections.IEnumerator LimitMovement()
    {
        // Wait for physics to apply
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Check if zombie has moved too far
        float distanceMoved = Vector3.Distance(startPosition, transform.position);

        if (distanceMoved > maxPushDistance)
        {
            // Calculate the limited position
            Vector3 pushDirection = (transform.position - startPosition).normalized;
            Vector3 limitedPosition = startPosition + (pushDirection * maxPushDistance);

            // Keep Y position unchanged
            limitedPosition.y = transform.position.y;

            // Apply the limited position
            transform.position = limitedPosition;

            // Stop any further movement
            if (rb)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }

        // Re-enable NavMeshAgent after a short delay if it exists
        if (navAgent && !navAgent.enabled && disableNavMeshWhenHit)
        {
            yield return new WaitForSeconds(0.3f);

            // Make sure we're on NavMesh before enabling
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                navAgent.enabled = true;
            }
        }
    }
}