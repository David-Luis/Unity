using System;
using UnityEngine;

public class LaserBulletComponent : BulletComponent
{
    private Transform target = null;

    [SerializeField]
    private GameObject impactEffect = null;

    [SerializeField]
    private GameObject startLaser = null;

    [SerializeField]
    private GameObject endLaser = null;

    public override void Shoot(Transform target)
    {
        this.target = target;
        HitTarget();
    }

    private void HitTarget()
    {
        GameObject particleEffect = Instantiate(impactEffect, transform.position, transform.rotation);
        ParticleSystem particles = particleEffect.GetComponent<ParticleSystem>();
#pragma warning disable CS0618 // Type or member is obsolete
        Destroy(particleEffect, particles.startLifetime + particles.duration);
#pragma warning restore CS0618 // Type or member is obsolete
        Debug.Log("DAMAGE: " + damage);
        DestructibleComponent destructibleComponent = target.GetComponent<DestructibleComponent>();
        destructibleComponent.Hit(damage);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable(Transform shootPosition)
    {
        if (target != null)
        {
            gameObject.SetActive(true);
            startLaser.transform.position = shootPosition.position;
            endLaser.transform.position = target.position;
        }
    }
}
