using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : EnemyBase
{
    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebLine _WebLine)
    {
        Instantiate(_webTrappedEffect, transform.position, Quaternion.identity);
        TakeDamage(_WebLine.damage);
    }

    protected override void EventOnDeath()
    {
    }
}
