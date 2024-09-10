using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : EnemyBase
{
    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebSegment _WebSegment)
    {
        TakeDamage(_WebSegment.damage);
    }

    protected override void EventOnDeath()
    {
    }
}
