using UnityEngine;
using UniRx;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float gWidth;
    [SerializeField] private float gDistance;
    [SerializeField] private float gPeriod;

    [Range(0, 1)]
    [SerializeField] private float gChance;

    void Start()
    {
        Observable.EveryUpdate()
            .ThrottleFirst(TimeSpan.FromSeconds(gPeriod))
            .Subscribe(_ => GenerateAsteroid())
            .AddTo(this);
    }

    private void GenerateAsteroid()
    {
        if (UnityEngine.Random.Range(0, 1) > gChance)
            return;
        Instantiate(asteroidPrefab, new Vector3(UnityEngine.Random.Range(-gWidth, gWidth), 0f, gDistance), Quaternion.identity);
    }
}
