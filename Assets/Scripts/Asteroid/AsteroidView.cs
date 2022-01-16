using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    void Start()
    {
        
    }

    public void SetVelocity(Vector3 newValue)
    {
        _rb.velocity = newValue;
    }

    public void Impact()
    {
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
