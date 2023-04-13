using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GridUpdater : MonoBehaviour
{
    private GridGraph grid;

    void Start()
    {
        // Get the existing Grid Graph
        AstarData data = AstarPath.active.data;
        // Update the graph with the new settings
        AstarPath.active.Scan();
    }
}