using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEventHandler : MonoBehaviour
{
    public Action OnCastBegin;
    public void OnCastBeginHandler()
    {
        OnCastBegin?.Invoke();
    }

    public Action OnCast;
    public void OnCastHandler()
    {
        OnCast?.Invoke();
    }

    public Action OnCastEnd;
    public void OnCastEndHandler()
    {
        OnCastEnd?.Invoke();
    }

    public Action OnEvadeEnd;
    public void OnEvadeEndHandler()
    {
        OnEvadeEnd?.Invoke();
    }

    public void FootL()
    {

    }

    public void FootR()
    {

    }
}
