using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    public void Glide(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }

    public void Impact()
    {

    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
