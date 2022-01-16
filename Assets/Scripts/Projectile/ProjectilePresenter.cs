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
                TryImpact(collision.gameObject);
            })
            .AddTo(this);
    }

    private void TryImpact(GameObject target)
    {
        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.GetDamage(projectileModel.damage);
        Destroy(gameObject);
    }
}
