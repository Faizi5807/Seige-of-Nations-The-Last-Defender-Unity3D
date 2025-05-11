using UnityEngine;
using Invector;

using Invector.vCharacterController; // Make sure you include this namespace

public class EnemyPunch : MonoBehaviour
{
    public int damage = 10;
    public bool isDead = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the Player!");

            // Try to get the vHealthController from the player
            vHealthController health = collision.gameObject.GetComponent<vHealthController>();

            if (health != null)
            {
                vDamage vDmg = new vDamage(damage); // You can customize this with more parameters
                health.TakeDamage(vDmg);
            }
        }
    }
}
