using UnityEngine;

public class LaserComponent : ShootComponent
{
    private LaserBulletComponent laserBullerComponent = null;

    private void Start()
    {
        
    }

    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);

        if (laserBullerComponent == null)
        {
            laserBullerComponent = Instantiate(bullet, transform.position, transform.rotation).GetComponent<LaserBulletComponent>();
        }

        if (target != null)
        {
            laserBullerComponent.Enable(shootPosition);
        }
        else
        {
            laserBullerComponent.Disable();
        }
    }

    protected override void Shoot()
    {
        laserBullerComponent.Shoot(target);
    }
}
