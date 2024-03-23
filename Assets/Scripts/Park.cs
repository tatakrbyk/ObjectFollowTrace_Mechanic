using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : MonoBehaviour
{
    public Route route;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem FX;
    private ParticleSystem.MainModule fxMainModule;

    private void Start()
    {
        fxMainModule = FX.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Car car))
        {
            if(car.route == route)
            {
                Game.Instance.OnCarEntersPark?.Invoke(route);
                StarFX();
            }
        }
    }

    private void StarFX()
    {
        fxMainModule.startColor = route.carColor;
        FX.Play();
            
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
