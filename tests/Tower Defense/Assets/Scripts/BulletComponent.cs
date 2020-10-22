using System;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private Transform target = null;

    [SerializeField]
    private GameObject impactEffect = null;

    [SerializeField]
    private float speed = 10f;

    public void Shoot(Transform target)
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
        Destroy(particleEffect, particles.startLifetime + particles.duration);

        Destroy(gameObject);
    }
}
