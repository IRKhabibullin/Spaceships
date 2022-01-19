using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class AsteroidFieldController : MonoBehaviour
{
    public float gWidth;
    public float gDistance;

    [SerializeField] private LayerMask keepInBounds;

    private void Start()
    {
        this.OnTriggerExitAsObservable()
            .Where(collision => (keepInBounds.value & 1 << collision.gameObject.layer) != 0)
            .Subscribe(collision => {
                Destroy(collision.gameObject);
            })
            .AddTo(this);
    }
}
