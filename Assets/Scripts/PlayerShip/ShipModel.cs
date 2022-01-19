using System;
using UniRx;
using UnityEngine;

[Serializable]
public class ShipModel
{
    public float maxHp;
    public float hp;
    public float collisionDamage;
    public event Action OnDeath;
    public event Action OnImpact;
    public ReactiveProperty<Vector3> velocity { get; private set; }
    public float maxSpeed;
    public float gunReloadTime;

    public ShipModel()
    {
        velocity = new ReactiveProperty<Vector3>();
        hp = maxHp;
    }

    public void SetVelocity(Vector3 newValue)
    {
        velocity.Value = newValue * maxSpeed;
    }

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
