using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public GameObject player;
    private float moveX, moveY;
    public float speed;
    public float limitMaxX, limitMinX, limitMaxY, limitMinY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x == Mathf.Clamp(player.transform.position.x, limitMinX, limitMaxX))
        {
            moveX = Mathf.Lerp(transform.position.x, player.transform.position.x, speed);
        }
        if(player.transform.position.y == Mathf.Clamp(player.transform.position.y, limitMinY, limitMaxY))
        {
            moveY = Mathf.Lerp(transform.position.y, player.transform.position.y, speed);
        }
        transform.position = new Vector3(moveX, moveY, transform.position.z);
    }
}
