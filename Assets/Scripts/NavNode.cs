using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    public List<NavNode> adjacentNodes;
    public List<bool> shouldJump;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        for (int i = 0; i < adjacentNodes.Count; i++)
        {
            Gizmos.color = Color.white;
            if (shouldJump[i])
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position, adjacentNodes[i].transform.position - ((adjacentNodes[i].transform.position - transform.position) / 2f));
        }
    }
}
