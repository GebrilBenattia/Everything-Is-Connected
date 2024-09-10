using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : EnemyBase
{
    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision()
    {
        TakeDamage(1);
    }

    protected override void EventOnDeath()
    {
    }
}
