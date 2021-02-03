using System.Collections;
using UnityEngine;


public class Mutex : MonoBehaviour
{
    private Object owner = null;
    public IEnumerator Lock(Object owner)
    {
        while(this.owner != null) yield return null;
        this.owner = owner;
    }

    public void Unlock(Object owner) {
        Debug.Assert(owner == this.owner);
        this.owner = null;
    }
}
