using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class AsteroidPresenter : MonoBehaviour, IDamageable
{
    [SerializeField] private AsteroidModel asteroidModel = new AsteroidModel();
    [SerializeField] private AsteroidView asteroidView;

    #region life cycle
    private void Start()
    {
        asteroidView.SetVelocity(new Vector3(0, 0, -asteroidModel.maxSpeed));
        asteroidModel.OnDeath += OnDeathHandler;
        asteroidModel.OnImpact += OnImpactHandler;

        this.OnCollisionEnterAsObservable()
            .Subscribe(collision => {
                var target = collision.gameObject.GetComponent<IDamageable>();
                if (target != null)
                    target.GetDamage(asteroidModel.collisionDamage);
            })
            .AddTo(this);
    }

    private void OnDestroy()
    {
        asteroidModel.OnDeath -= OnDeathHandler;
        asteroidModel.OnImpact -= OnImpactHandler;
    }
    #endregion

    #region event handlers
    private void OnImpactHandler()
    {
        asteroidView.Impact();
    }

    private void OnDeathHandler()
    {
        asteroidView.Death();
    }
    #endregion

    #region api
    public void GetDamage(float damage)
    {
        asteroidModel.GetDamage(damage);
    }
    #endregion
}
