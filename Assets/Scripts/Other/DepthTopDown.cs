using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthTopDown : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 1000);
        }
        
    }
}
