using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    private float startPos, lengthSprite;
    public float parallaxFactor;

    private void Start()
    {
        startPos = transform.position.x;
        lengthSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    public void Move(float delta)
    {

        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;
        transform.localPosition = newPos;
    }
}
