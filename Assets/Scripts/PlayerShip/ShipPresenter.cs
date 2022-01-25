using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

public class ShipPresenter : MonoBehaviour, IDamageable
{
    [SerializeField] private ShipView shipView;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private ShipModel shipModel = new ShipModel();
    [SerializeField] private TextMeshProUGUI hpBar;
    private bool canMove = false;

    #region life cycle
    private void Start()
    {
        shipModel.OnDeath.AddListener(OnDeathHandler);
        shipModel.OnImpact += OnImpactHandler;

        IObservable<long> updateLoop = Observable.EveryUpdate();
        ObserveMovement(updateLoop);
        ObserveShooting(updateLoop);
        ObserveCollisions();
    }

    private void ObserveMovement(IObservable<long> updateLoop)
    {
        Observable.EveryFixedUpdate()
            .Subscribe(_ => {
                if (!canMove) return;
                Vector3 movementValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                shipModel.SetVelocity(movementValue);
            })
            .AddTo(this);

        // Limit movement by asteroid field bounds
        updateLoop
            .Subscribe(_ =>
            {
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, -10, 10),
                    transform.position.y,
                    Mathf.Clamp(transform.position.z, -2, 12)
                );
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
        shipModel.OnDeath.RemoveListener(OnDeathHandler);
        shipModel.OnImpact -= OnImpactHandler;
    }
    #endregion

    #region event handlers
    private void OnImpactHandler()
    {
        shipView.Impact();
        hpBar.text = $"Ship status: {(int)(shipModel.hp * 100 / shipModel.maxHp)}%";
    }

    private void OnDeathHandler()
    {
        hpBar.text = $"Ship status: destroyed";
        shipView.Death();
    }
    #endregion

    #region api
    public void GetDamage(float damage)
    {
        shipModel.GetDamage(damage);
    }

    public void ToggleMovement(bool value)
    {
        shipModel.SetVelocity(Vector3.zero);
        canMove = value;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
    #endregion
}
