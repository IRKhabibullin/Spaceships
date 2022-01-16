using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float maxSpeed;

    void Start()
    {
        _rb.velocity = new Vector3(0, 0, maxSpeed);
    }
}
