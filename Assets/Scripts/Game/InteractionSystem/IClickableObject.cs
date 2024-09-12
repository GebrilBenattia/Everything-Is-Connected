using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickableObject
{
    public void EventOnLeftButtonDown(RaycastHit _HitInfo);
    public void EventOnLeftButtonUp();
}