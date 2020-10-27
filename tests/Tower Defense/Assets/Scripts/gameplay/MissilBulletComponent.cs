using System;
using UnityEngine;

//TODO: some of this class is too similar to cannonbullet, maybe it can share code or split it in more components
public class MissilBulletComponent : BulletComponent
{
    private Vector3 targetPosition;

    [SerializeField]
    private GameObject impactEffect = null;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float explosionAreaRadius = 1.5f;

    private float positionYOffset = 0.5f;

    public override void Shoot(Transform target)
    {
        targetPosition = target.position + Vector3.up * positionYOffset;
    }

    private void Update()
    {
        Vector3 dir = targetPosition - transform.position;
        float distance = speed * Time.deltaTime;

        if (dir.magnitude <= distance)
        {
            HitTarget();
            return;
        }

        transform.LookAt(targetPosition);
        transform.Translate(dir.normalized * distance, Space.World);
    }

    private void HitTarget()
    {
        GameObject particleEffect = Instantiate(impactEffect, transform.position, Quaternion.identity);
        ParticleSystem particles = particleEffect.GetComponentInChildren<ParticleSystem>();
#pragma warning disable CS0618 // Type or member is obsolete
        Destroy(particleEffect, particles.startLifetime + particles.duration);
#pragma warning restore CS0618 // Type or member is obsolete

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < explosionAreaRadius)
            {
                DestructibleComponent destructibleComponent = enemy.GetComponent<DestructibleComponent>();
                destructibleComponent.Hit(damage);
            }
        }

        Destroy(gameObject);
    }
}
