using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidView : MonoBehaviour
{
    void Start()
    {
        
    }

    public void Impact()
    {
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
