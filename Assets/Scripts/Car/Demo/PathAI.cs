using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAI : MonoBehaviour
{
    public Color _lineColor;

    private List<Transform> _nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = _lineColor;

        Transform[] l_pathTransforms = GetComponentsInChildren<Transform>();
        _nodes = new List<Transform>();

        for (int i = 0; i < l_pathTransforms.Length; i++)
        {
            if (l_pathTransforms[i] != transform)
            {
                _nodes.Add(l_pathTransforms[i]);
            }
        }

        for (int i = 0; i < _nodes.Count; i++)
        {
            Vector3 l_currentNode = _nodes[i].position;
            Vector3 l_previosNode = _nodes[0].position;

            //if (i > 0) // if is not the first node
            //{
            //    l_previosNode = _nodes[i - 1].position;
            //}
            //else if (i == 0 && _nodes.Count > 1) // if is first node
            //{
            //    l_previosNode = _nodes[_nodes.Count - 1].position;
            //}

            if (i != 0)
            {
                l_previosNode = _nodes[i - 1].position;
            }
            Gizmos.DrawLine(l_previosNode, l_currentNode);
            Gizmos.DrawSphere(l_currentNode, 0.3f);
        }
    }
}
