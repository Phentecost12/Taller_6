using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bribery : TrapsFather
{

    
    // Start is called before the first frame update+7
    protected override void DoSomething()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(Disable) return;
        Enemy enemycontroller = collision.gameObject.GetComponent<Enemy>();
        if (enemycontroller != null)
        {
            enemycontroller.OnBribery(this);
            transform.parent = enemycontroller.transform;
            transform.position = Vector2.zero;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if(Disable) return;
    }
}
