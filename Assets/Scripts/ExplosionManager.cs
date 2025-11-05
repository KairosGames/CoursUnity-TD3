using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    List<ParticleSystem> explosionEffects = new ();

    void Start()
    {
        for (int i= 0; i < 20; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();
            explosionEffects.Add(particleSystem);
        }
    }

    public void PlayExplosion(Vector3 position)
    {
        ParticleSystem explosion = new ParticleSystem();
        bool isFound = false;

        foreach (ParticleSystem particleSystem in explosionEffects)
            if (!particleSystem.isPlaying)
            {
                explosion = particleSystem;
                isFound = true;
                break;
            }
        
        if (isFound)
        {
            explosion.transform.position = position;
            explosion.Play();
            return;
        }

        GameObject go = Instantiate(prefab, transform);
        explosion = go.GetComponent<ParticleSystem>();
        explosionEffects.Add(explosion);
        explosion.transform.position = position;
        explosion.Play();
    }
}
