using System;
using UnityEngine;

[Serializable]
public class AsteroidModel
{
    public float hp;
    public event Action OnDeath;
    public event Action OnImpact;

    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnDeath?.Invoke();
        }
        else
        {
            OnImpact?.Invoke();
        }
    }
}
