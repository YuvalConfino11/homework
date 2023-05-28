using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxNew : MonoBehaviour
{
    private float lengthSpriteX, startPosX;
    public GameObject cam;
    public float parallaxEffectX;

    private void Start()
    {
        startPosX = transform.position.x;
        lengthSpriteX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float tempX = cam.transform.position.x * (1 - parallaxEffectX);
        float distanceX = cam.transform.position.x * parallaxEffectX;
        transform.position = new Vector3(startPosX + distanceX, transform.position.y, transform.position.z);
        if (tempX > startPosX + lengthSpriteX)
        {
            startPosX += lengthSpriteX;
        }
        else if (tempX < startPosX - lengthSpriteX)
        {
            startPosX -= lengthSpriteX;
        }
    }
}
