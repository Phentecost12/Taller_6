using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingCannon : TrapsFather
{
    //[SerializeField] BowlingBall Projectile;
    GameObject target;

    protected override void DoSomething()
    {
        BowlingBall Projectile =  Bullet_Manager.Instance.GetBowlingBall();
        Projectile.config(bullet_Damage,bullet_Power);
        Projectile.transform.position = transform.position;
        target = _Enemy_Inside[0].gameObject;
        Vector2 EnemyLocation = target.transform.position - transform.position;
        EnemyLocation.Normalize();
        Projectile.Launch(EnemyLocation);
    }
}
