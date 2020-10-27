using System;
using UnityEngine;

public class CannonBulletComponent : BulletComponent
{
    private Transform target = null;

    [SerializeField]
    private GameObject impactEffect = null;

    [SerializeField]
    private float speed = 10f;

    public override void Shoot(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 dir = target.position - transform.position;
        float distance = speed * Time.deltaTime;

        if (dir.magnitude <= distance)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distance, Space.World);
    }

    private void HitTarget()
    {
        GameObject particleEffect = Instantiate(impactEffect, transform.position, transform.rotation);
        ParticleSystem particles = particleEffect.GetComponent<ParticleSystem>();
#pragma warning disable CS0618 // Type or member is obsolete
        //Destroy(particleEffect, particles.startLifetime + particles.duration);
#pragma warning restore CS0618 // Type or member is obsolete

        DestructibleComponent destructibleComponent = target.GetComponent<DestructibleComponent>();
        destructibleComponent.Hit(damage);

        Destroy(gameObject);
    }
}
