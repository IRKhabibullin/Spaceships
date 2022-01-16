using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ShipPresenter : MonoBehaviour, IDamageable
{
    [SerializeField] private ShipView shipView;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private ShipModel shipModel = new ShipModel();

    #region life cycle
    private void Start()
    {
        shipModel.OnDeath += OnDeathHandler;
        shipModel.OnImpact += OnImpactHandler;

        IObservable<long> updateLoop = Observable.EveryUpdate();
        ObserveMovement(updateLoop);
        ObserveShooting(updateLoop);
        ObserveCollisions();
    }

    private void ObserveMovement(IObservable<long> updateLoop)
    {
        updateLoop
            .Subscribe(_ => {
                Vector3 movementValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                shipModel.SetVelocity(movementValue);
            })
            .AddTo(this);

        shipModel.velocity
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(x => {
                shipView.Glide(x);
            })
            .AddTo(this);
    }

    private void ObserveShooting(IObservable<long> updateLoop)
    {
        // Detect shooting each <shipModel.gunReloadTime> seconds.
        // Reload time should be counted in the model probably, but this one is just to experiment with unirx
        updateLoop
            .Where(_ => Input.GetKey(KeyCode.Space))
            .ThrottleFirst(TimeSpan.FromSeconds(shipModel.gunReloadTime))
            .Subscribe(_ => {
                Instantiate(projectilePrefab, gun.position, projectilePrefab.transform.rotation);
            })
            .AddTo(this);
    }

    private void ObserveCollisions()
    {
        this.OnCollisionEnterAsObservable()
            .Subscribe(collision => {
                var target = collision.gameObject.GetComponent<IDamageable>();
                if (target != null)
                    target.GetDamage(shipModel.collisionDamage);
            })
            .AddTo(this);
    }

    private void OnDestroy()
    {
        shipModel.OnDeath -= OnDeathHandler;
        shipModel.OnImpact -= OnImpactHandler;
    }
    #endregion

    #region event handlers
    private void OnImpactHandler()
    {
        shipView.Impact();
    }

    private void OnDeathHandler()
    {
        Debug.Log("Game over");
        shipView.Death();
    }
    #endregion

    #region api
    public void GetDamage(float damage)
    {
        shipModel.GetDamage(damage);
    }
    #endregion
}
