using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]private int currentHealth;
    [SerializeField]private int maxHealth;
    [SerializeField] private bool isHit;

    private float invincibleTime;
    private float maxInvincibleTime;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        maxInvincibleTime = 2f;
        invincibleTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (isHit)
        {
            invincibleTime += Time.deltaTime;

            if(invincibleTime > maxInvincibleTime)
            {
                isHit = false;
                invincibleTime = 0;
            }
        }
    }

    public void damagePlayer(int damage)
    {
        if (!isHit)
        {
            currentHealth -= damage;
            isHit = true;
        }
        
    }

    public void healPlayer(int health)
    {
        currentHealth += health;
    }
}
