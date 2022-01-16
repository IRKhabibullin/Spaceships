using UnityEngine;

public class AsteroidPresenter : MonoBehaviour, IDamageable
{
    [SerializeField] private AsteroidModel asteroidModel = new AsteroidModel();
    [SerializeField] private AsteroidView asteroidView;

    void Start()
    {
        asteroidModel.OnDeath += Death;
        asteroidModel.OnImpact += Impact;
    }

    private void OnDestroy()
    {
        asteroidModel.OnDeath -= Death;
        asteroidModel.OnImpact -= Impact;
    }

    public void GetDamage(float damage)
    {
        asteroidModel.GetDamage(damage);
    }

    public void Death()
    {
        asteroidView.Death();
    }

    public void Impact()
    {
        asteroidView.Impact();
    }
}
