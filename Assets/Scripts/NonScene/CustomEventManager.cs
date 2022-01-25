using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CustomEventManager
{
    public static UnityEvent<bool> OnInputPermissionChanged = new UnityEvent<bool>();

    public static void ChangeInputPermission(bool status)
    {
        OnInputPermissionChanged?.Invoke(status);
    }
    
}
