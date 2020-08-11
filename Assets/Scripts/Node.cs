using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] neighours;
    public Vector2[] validDirections;

    // Start is called before the first frame update
    void Start()
    {
        validDirections = new Vector2[neighours.Length];

        for (int i = 0; i < neighours.Length; i++)
        {
            Node neighbour = neighours[i];
            Vector2 tempVector = neighbour.transform.localPosition - transform.localPosition;

            validDirections[i] = tempVector.normalized;
        }
    }

}
