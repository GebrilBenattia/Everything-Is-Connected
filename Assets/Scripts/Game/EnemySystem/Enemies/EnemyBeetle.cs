using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyBase
{
    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebLine _WebLine)
    {
        Instantiate(_webTrappedEffect, transform.position, Quaternion.identity);
        WebManager.instance.BreakWebLine(_WebLine);
    }

    protected override void EventOnDeath()
    {
    }
}
