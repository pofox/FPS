using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private float totalHealth = 100;
    private float health;
    private float score = 0;
    private Vector3 lastCheckPoint = Vector3.one;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI ScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int index = Convert.ToInt16(!GameManager.Instance.firstGame);
        health = GameManager.Instance.Save.data[index].p_health;
        score = GameManager.Instance.Save.data[index].p_score;
        transform.position = GameManager.Instance.Save.data[index].p_position;
        transform.rotation = GameManager.Instance.Save.data[index].p_rotation;
        lastCheckPoint = GameManager.Instance.Save.data[index].p_lastCheckPoint;
        healthBar.fillAmount = health / totalHealth;
        ScoreText.text = "Score: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamege(float damege)
    {
        health -= damege;
        healthBar.fillAmount = health/totalHealth;
        if (health < 0)
        {
            Die();
        }
    }
    public void AddScore(float _score)
    {
        score += _score;
        ScoreText.text = "Score: " + score.ToString();
    }
    public void updateCheckPoint(Vector3 newCheckPoint)
    {
        lastCheckPoint = newCheckPoint;
    }
    void Die()
    {
        transform.position = lastCheckPoint;
        health = totalHealth;
        score -= 10;
        score = score > 0 ? score : 0;
    }
    private void OnDestroy()
    {
        GameManager.Instance.Save.data[1].p_health = health;
        GameManager.Instance.Save.data[1].p_score = score;
        GameManager.Instance.Save.data[1].p_position = transform.position;
        GameManager.Instance.Save.data[1].p_rotation = transform.rotation;
        GameManager.Instance.Save.data[1].p_lastCheckPoint = lastCheckPoint;
    }
}
