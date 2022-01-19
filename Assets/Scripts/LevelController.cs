using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AsteroidFieldController))]
[RequireComponent(typeof(Collider))]
public class LevelController : MonoBehaviour
{
    [SerializeField] private AsteroidFieldController afc;
    [SerializeField] private TextMeshProUGUI asteroidsCountText;
    [SerializeField] private Collider levelFinishLine;
    [SerializeField] private GameObject asteroidPrefab;

    [SerializeField] private float gPeriod;
    [Range(0, 1)]
    [SerializeField] private float gChance;
    [SerializeField] private ReactiveProperty<int> asteroidFieldSize; // number of asteroids on level. After all asteroids are out, level is passed

    [SerializeField] private int maxLevel;
    private int currentLevel;
    private IDisposable asteroidGenerator;
    private IDisposable levelFinishTrigger;

    void Start()
    {
        currentLevel = 0;
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

        ToNextLevel();
    }

    private void StartGeneration()
    {
        asteroidGenerator = Observable.EveryUpdate()
            .ThrottleFirst(TimeSpan.FromSeconds(gPeriod))
            .Subscribe(_ => {
                if (UnityEngine.Random.Range(0, 1) > gChance)
                    return;
                Instantiate(asteroidPrefab, new Vector3(UnityEngine.Random.Range(-afc.gWidth, afc.gWidth), 0f, afc.gDistance), Quaternion.identity);
                asteroidFieldSize.Value--;
            })
            .AddTo(this);
    }

    private void ActivateLevelFinishLine()
    {
        levelFinishTrigger = levelFinishLine.OnTriggerEnterAsObservable()
            .Where(collision => collision.gameObject.layer == LayerMask.NameToLayer("PlayerShip"))
            .Subscribe(collision => {
                collision.gameObject.GetComponent<ShipPresenter>().ResetPosition();
                ToNextLevel();
            })
            .AddTo(this);
    }

    public void RandomizeLevelVariables(int level)
    {
        asteroidFieldSize.Value = (int)UnityEngine.Random.Range(3 + level * 4, 5 + level * 5);
        gPeriod = UnityEngine.Random.Range(0.8f - level * 0.1f, 1 - level * 0.1f) * 3;
    }

    public void ToNextLevel()
    {
        levelFinishTrigger?.Dispose();

        if (currentLevel >= maxLevel)
        {
            Debug.Log("You won!");
            return;
        }
        currentLevel++;

        RandomizeLevelVariables(currentLevel);
        StartGeneration();
    }
}
