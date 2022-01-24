using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    void Start()
    {
        Observable.EveryUpdate()
            .Subscribe(_ => {
                transform.Rotate(0, 0, 50 * Time.deltaTime);
            })
            .AddTo(this);
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
