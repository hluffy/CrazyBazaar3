using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public Stat maxHealth;

    public Stat damage;

    public int currentHealth;

    public System.Action onHealthChanged;

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > maxHealth.GetValue())
        {
            currentHealth = maxHealth.GetValue();
        }

        onHealthChanged?.Invoke();
    }

    public virtual void DesreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        onHealthChanged?.Invoke();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void DoDamage(Stats _stats)
    {
        _stats.TakeDamage(damage.GetValue());
    }

    public virtual void TakeDamage(int _damage)
    {
        DesreaseHealthBy(_damage);

        DamageTextManager.instance.ShowDamageText(_damage, transform.position, Color.red);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
