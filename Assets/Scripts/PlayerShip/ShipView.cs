using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    /// <summary>
    /// Set velocity of the ship
    /// </summary>
    public void Glide(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }
}
