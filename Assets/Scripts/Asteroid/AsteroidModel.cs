using System;

[Serializable]
public class AsteroidModel
{
    public float collisionDamage;
    public float maxSpeed;
    public event Action OnDeath;

    public void GetDamage(float damage)
    {
        OnDeath?.Invoke();
    }
}
