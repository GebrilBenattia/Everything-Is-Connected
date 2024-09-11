using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyBase
{
    // ######################################### FUNCTIONS ########################################

    protected override void EventOnWebCollision(WebSegment _WebSegment)
    {
        // TODO: Destroy web segment
    }

    protected override void EventOnDeath()
    {
    }
}
