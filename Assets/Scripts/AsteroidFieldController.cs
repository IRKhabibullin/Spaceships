using UnityEngine;
using UniRx.Triggers;
using UniRx;
using TMPro;
using System;
using UnityEngine.Events;

public class AsteroidFieldController : MonoBehaviour
{
    public float gWidth;
    public float gDistance;

    [SerializeField] private ReactiveProperty<int> asteroidFieldSize; // number of asteroids on level. After all asteroids are out, level is passed
    [SerializeField] private TextMeshProUGUI asteroidsCountText;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Collider levelFinishLine;
    [SerializeField] private LayerMask keepInBounds;
    public UnityEvent onFinishLineReached;

    private IDisposable asteroidGenerator;
    private IDisposable levelFinishTrigger;

    private void Start()
    {
        asteroidFieldSize = new ReactiveProperty<int>(1);
        asteroidFieldSize
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(x =>
            {
                asteroidsCountText.text = $"Asteroids left: {x}";
                if (x <= 0)
                {
                    asteroidGenerator?.Dispose();
                    ActivateLevelFinishLine();
                }
            }).AddTo(this);

        this.OnTriggerExitAsObservable()
            .Where(collision => (keepInBounds.value & 1 << collision.gameObject.layer) != 0)
            .Subscribe(collision => {
                Destroy(collision.gameObject);
            })
            .AddTo(this);
    }

    public void StartGeneration(LevelParameters levelParams)
    {
        asteroidFieldSize.Value = levelParams.asteroidsCount;
        asteroidGenerator = Observable.EveryUpdate()
            .ThrottleFirst(TimeSpan.FromSeconds(levelParams.generationRate))
            .Subscribe(_ => {
                Instantiate(asteroidPrefab, new Vector3(UnityEngine.Random.Range(-gWidth, gWidth), 0f, gDistance), Quaternion.identity);
                asteroidFieldSize.Value--;
            })
            .AddTo(this);
    }

    private void ActivateLevelFinishLine()
    {
        levelFinishTrigger = levelFinishLine.OnTriggerEnterAsObservable()
            .Where(collision => collision.gameObject.layer == LayerMask.NameToLayer("PlayerShip"))
            .Subscribe(collision => {
                collision.gameObject.GetComponentInParent<ShipPresenter>().ResetPosition();
                levelFinishTrigger?.Dispose();
                onFinishLineReached?.Invoke();
            })
            .AddTo(this);
    }
}
