using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableGround : MonoBehaviour, IClickableObject
{
    // ######################################### FUNCTIONS ########################################

    public void EventOnLeftButtonDown(RaycastHit _HitInfo)
    {
        WebManager.instance.spiderController.SetTargetPos(_HitInfo.point);
        WebManager.instance.DeselectNewsNode();
    }

    public void EventOnLeftButtonUp()
    {
    }
}
