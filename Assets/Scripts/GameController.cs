using UnityEngine;
using System;
using UniRx.Triggers;
using UniRx;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float gWidth;
    [SerializeField] private float gDistance;
    [SerializeField] private LayerMask keepInBounds;
    public int maxLevel;

    private void Start()
    {
        this.OnTriggerExitAsObservable()
            .Where(collision => (keepInBounds.value & 1 << collision.gameObject.layer) != 0)
            .Subscribe(collision => {
                Destroy(collision.gameObject);
            })
            .AddTo(this);
    }

    public void GenerateAsteroid()
    {
        Instantiate(asteroidPrefab, new Vector3(UnityEngine.Random.Range(-gWidth, gWidth), 0f, gDistance), Quaternion.identity);
    }
}
