using System;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float xParallaxValue;
    [SerializeField] private float yParallaxValue;
    private float spriteLength;
    private Camera camera;
    private Vector3 deltaMovement;
    private Vector3 lastCameraPosition;

    void Start()
    {
        camera = Camera.main;
        lastCameraPosition = camera.transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void LateUpdate()
    {
        deltaMovement = camera.transform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * xParallaxValue, deltaMovement.y * yParallaxValue);
        lastCameraPosition = camera.transform.position;

        if (camera.transform.position.x - transform.position.x >= spriteLength)
        {
            transform.position = new Vector3(camera.transform.position.x + spriteLength, transform.position.y);
        }
        else if (transform.position.x - camera.transform.position.x >= spriteLength)
        {
            transform.position = new Vector3(camera.transform.position.x - spriteLength, transform.position.y);
        }
    }
}
