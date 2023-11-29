using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public bool isReady;
    public float scaleFactor = 20;
    private float startScaleFactor;
    public SpriteRenderer spriteRenderer;
    public GameObject shine;
    void Start()
    {
        startScaleFactor = scaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        shine.SetActive(isReady);
        if(scaleFactor < startScaleFactor)
        {
            isReady = false;
            scaleFactor++;
            float _scaleFactor = scaleFactor/startScaleFactor;
            transform.localScale = new Vector3(_scaleFactor,_scaleFactor,_scaleFactor);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        }
        else
        {
            isReady = true;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        }
    }
}
