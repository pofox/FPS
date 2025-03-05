using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float range = 50;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem laser;
    [SerializeField] private Slider healthBar;
    float totalHealth = 100;
    float health;
    float level = 0;
    float damage = 9;
    float time = 0;
    float shootingInterval = 0.7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalHealth = GameManager.Instance.Save.data[0].e_totalHealth;
        damage = GameManager.Instance.Save.data[0].e_damage;
        level = GameManager.Instance.Save.data[0].e_level;
        health = totalHealth;
        healthBar.value = health / totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > shootingInterval&&!GameManager.Instance.paused)
        {
            time = 0;
            if ((player.position - transform.position).sqrMagnitude < range)
            {
                SoundManager.Instance.PlayenemyShootingSound();
                if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hitInfo, range, layerMask))
                {
                    if (hitInfo.collider.tag == "Player")
                    {
                        hitInfo.collider.GetComponent<PlayerStats>().TakeDamege(damage);
                        transform.localRotation = Quaternion.FromToRotation(new Vector3(0, 0, -1), (player.position - transform.position));
                        laser.Play();
                    }
                }
            }
        }
    }
    public void TakeDamege(float _damage)
    {
        health -= _damage;
        healthBar.value = health/totalHealth;
        if (health < 0)
        {
            player.GetComponent<PlayerStats>().AddScore((level + 5) * 3);
            Die();
        }
    }
    void Die()
    {
        GetComponent<suicide>().enabled = true;
        gameObject.tag = "Untagged";
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(5*transform.forward);
        laser.Clear();
        Destroy(this);
    }
}
