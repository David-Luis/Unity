using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField]
    private Transform shootPosition = null;

    [SerializeField]
    private GameObject bullet = null;

    private Transform target = null;

    [SerializeField]
    private float fireRate = 2f;

    private float fireCountDown = 0f;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target != null && fireCountDown <=0)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bulletGameObject = Instantiate(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
        BulletComponent bulletComponent = bulletGameObject.GetComponent<BulletComponent>();
        bulletComponent.Shoot(target);
    }
}
