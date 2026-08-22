using System;
using Player;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private int startPointIndex;
    private int targetPointIndex;
    private PlayerCharacter playerCharacter;
    private void Start()
    {
        targetPointIndex = startPointIndex;
        transform.position = movePoints[targetPointIndex].position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[targetPointIndex].position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoints[targetPointIndex].position) < 0.1f)
        {
            targetPointIndex++;
            if (targetPointIndex >= movePoints.Length)
            {
                targetPointIndex = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            playerCharacter = other.gameObject.GetComponent<PlayerCharacter>();
            if (playerCharacter != null)
            {
                playerCharacter.GetPhysicsControl().SetInterpolationMode(RigidbodyInterpolation2D.Extrapolate);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy)
            {
                other.transform.SetParent(null);
                if (playerCharacter != null)
                {
                    playerCharacter.GetPhysicsControl().SetInterpolationMode(RigidbodyInterpolation2D.Interpolate);
                }
            }
        }
    }
}
