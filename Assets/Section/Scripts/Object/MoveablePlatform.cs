using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoint;
    [SerializeField] private Transform oneWayPlatform;

    private int currentWayPointIndex;
    private int moveSpeed = 2;

    private void Start()
    {
        oneWayPlatform.position = wayPoint[0].position;
    }

    private void Update()
    {
        oneWayPlatform.position = Vector3.MoveTowards(oneWayPlatform.position, wayPoint[currentWayPointIndex].position, moveSpeed * Time.deltaTime);
        if (oneWayPlatform.position == wayPoint[currentWayPointIndex].position)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= wayPoint.Length)
                currentWayPointIndex = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (wayPoint.Length <= 1)
            return;
        for (int i = 0; i < wayPoint.Length - 1; i++)
        {
            Gizmos.DrawLine(wayPoint[i].position, wayPoint[i + 1].position);
        }
        Gizmos.DrawLine(wayPoint[0].position, wayPoint[wayPoint.Length - 1].position);

    }
}
