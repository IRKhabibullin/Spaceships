using System;
using UniRx;
using UnityEngine;

public class ShipPresenter : MonoBehaviour
{
    [SerializeField] private ShipView shipView;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private ShipModel shipModel = new ShipModel();

    void Start()
    {
        shipModel.velocity
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(x => {
                shipView.Glide(x);
            })
            .AddTo(this);

        var updateLoop = Observable.EveryUpdate();

        // Calculate movement every frame
        updateLoop
            .Subscribe(_ => CalculateMovement())
            .AddTo(this);

        // Detect shooting each N seconds.
        // Reload time should be counted in the model probably, but this one is just to experiment with unirx
        updateLoop
            .Where(_ => Input.GetKey(KeyCode.Space))
            .ThrottleFirst(TimeSpan.FromSeconds(shipModel.gunReloadTime))
            .Subscribe(_ => Shoot())
            .AddTo(this);
    }

    private void CalculateMovement()
    {
        Vector3 movementValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        shipModel.ChangeVelocity(movementValue);
    }

    private void Shoot()
    {
        var pp = gun.position;
        Instantiate(projectilePrefab, pp, projectilePrefab.transform.rotation);
    }
}
