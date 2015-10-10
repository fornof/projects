using UnityEngine;
using System.Collections;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected bool dead;
    protected float health;
    public event System.Action OnDeath;
    protected virtual void Start() {
        health = startingHealth;
    }
     void TakeHit(float damage, RaycastHit hit)
    {
        health -= damage;
        if (health <= 0 && !dead) {
            Die();
         }
    }
    protected void Die()
    {

        dead = true;
        
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

    void IDamageable.TakeHit(float damage, RaycastHit hit)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }
}
