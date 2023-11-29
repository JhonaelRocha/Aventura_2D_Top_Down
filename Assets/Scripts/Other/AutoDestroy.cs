using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timeToAutoDestroy;
    void Start()
    {
        StartCoroutine("DestroyObject");
    }
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(timeToAutoDestroy);
        Destroy(this.gameObject);
    }
    
}
