using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ProjectilePresenter : MonoBehaviour
{
    [SerializeField] private ProjectileModel projectileModel = new ProjectileModel();

    void Start()
    {
        this.OnCollisionEnterAsObservable()
            .Subscribe(collision => {
                var target = collision.gameObject.GetComponent<IDamageable>();
                if (target != null)
                    DealDamage(target);
            })
            .AddTo(this);
    }

    private void DealDamage(IDamageable target)
    {
        target.GetDamage(projectileModel.damage);
        Destroy(gameObject);
    }
}
