using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : Car
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
