using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    protected int _poweredBy = 0;

    public virtual void PowerOn()
    {
        if (_poweredBy++ == 0)
        {
            Activate();
        }
    }

    protected abstract void Activate();

    public virtual void PowerOff()
    {
        if (--_poweredBy == 0)
        {
            DeActivate();
        }

        //Security
        if (_poweredBy < 0)
            _poweredBy = 0;
    }

    protected abstract void DeActivate();
}
