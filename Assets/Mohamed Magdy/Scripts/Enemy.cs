using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int currentState = 0;

    [SerializeField] private Transform player;
    [SerializeField] private float range = 50;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem laser;
    [SerializeField] private Slider healthBar;

    private float totalHealth = 100;
    private float health;
    private float level = 0;
    private float damage = 9;
    private float time = 0;
    private float shootingInterval = 0.7f;
    private Rigidbody rb;

    void Start()
    {
        totalHealth = GameManager.Instance.Save.data[0].e_totalHealth;
        damage = GameManager.Instance.Save.data[0].e_damage;
        level = GameManager.Instance.Save.data[0].e_level;
        health = totalHealth;
        healthBar.value = health / totalHealth;
        rb = GetComponent<Rigidbody>();
        currentState = 0;
    }

    void Update()
    {
        if (GameManager.Instance.paused) return;

        switch (currentState)
        {
            case 0:
                CheckForPlayer();
                break;
            case 1:
                ChasePlayer();
                break;
            case 2:
                AttackPlayer();
                break;
            case 3:
                // Enemy is dying, no actions required.
                break;
        }
    }

    private void CheckForPlayer()
    {
        if ((player.position - transform.position).sqrMagnitude < range * range)
        {
            currentState = 1;
        }
    }

    private void ChasePlayer()
    {
        transform.LookAt(player);
        // Movement towards the player can be added here
        if ((player.position - transform.position).sqrMagnitude < range * range * 0.5f)
        {
            currentState = 2;
        }
    }

    private void AttackPlayer()
    {
        time += Time.deltaTime;
        if (time > shootingInterval)
        {
            time = 0;
            SoundManager.Instance.PlayenemyShootingSound();
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hitInfo, range, layerMask))
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    hitInfo.collider.GetComponent<PlayerStats>().TakeDamege(damage);
                    laser.Play();
                }
            }
        }

        if ((player.position - transform.position).sqrMagnitude > range * range * 0.5f)
        {
            currentState = 1;
        }
    }

    public void TakeDamege(float _damage)
    {
        health -= _damage;
        healthBar.value = health / totalHealth;

        if (health <= 0)
        {
            player.GetComponent<PlayerStats>().AddScore((level + 5) * 3);
            currentState = 3;
            Die();
        }
    }

    private void Die()
    {
        GetComponent<suicide>().enabled = true;
        gameObject.tag = "Untagged";
        rb.isKinematic = false;
        rb.AddForce(5 * transform.forward);
        laser.Clear();
        Destroy(this);
    }
}
