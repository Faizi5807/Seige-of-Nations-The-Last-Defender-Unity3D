using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    public float range = 100f;
    public GameObject hitEffect;

    private PlayerInput playerInput;
    private InputAction shootAction;

    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        shootAction = playerInput.actions["Fire"];
    }

    void Update()
    {
        if (shootAction != null && shootAction.triggered)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.transform.gameObject);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
