using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    public abstract void trigger();
    public bool isDestroyed = false;
    public Guid id = new Guid();
}
