using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnemy : MonoBehaviour
{
    public List<Transform> waypoints;

    public Vector3 GetWaypoint(int index)
    {
        if (index >= 0 && index < waypoints.Count)
        {
            return waypoints[index].position;
        }
        return Vector3.zero;
    }

    public int WaypointCount => waypoints.Count;
}
