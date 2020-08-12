using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;

public class Ghost : MonoBehaviour
{
    public float ghostSpeed = 3.9f;

    public Node startingPosition;

    public int scatterModeTimer1 = 7;
    public int chaseModeTimer1 = 20;
    public int scatterModeTimer2 = 7;
    public int chaseModeTimer2 = 20;
    public int scatterModeTimer3 = 5;
    public int chaseModeTimer3 = 20;
    public int scatterModeTimer4 = 5;

    private int modeChangeIteration = 1;
    private float modeChangeTimer = 0;

    public enum Mode
    {
        Chase,
        Scatter, 
        Frightened
    }

    Mode currentMode = Mode.Scatter;
    Mode previousMode;

    private GameObject PacMan;

    private Node currentNode, targetNode, previousNode;
    private UnityEngine.Vector2 direction, nextDirection;

    private void Start()
    {
        PacMan = GameObject.FindGameObjectWithTag("Player");

        Node node = GetNodeAtPosition(transform.localPosition);

        if(node != null)
        {
            currentNode = node;
        }

        direction = UnityEngine.Vector2.right;

        previousNode = currentNode;

        UnityEngine.Vector2 pacmanPosition = PacMan.transform.position;
        UnityEngine.Vector2 targetTile = new UnityEngine.Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));
        targetNode = GetNodeAtPosition(targetTile);
    }

    // Update is called once per frame
    void Update()
    {
        ModeUpdate();

        Move();


    }
    void Move()
    {
        if(targetNode != currentNode && targetNode != null)
        {
            if (OverShotTarget())
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);

                if(otherPortal != null)
                {
                    transform.localPosition = otherPortal.transform.position;

                    currentNode = otherPortal.GetComponent<Node>();
                }

                targetNode = ChooseNextNode();

                previousNode = currentNode;

                currentNode = null;
            }
            else
            {
                Debug.Log(direction);

                transform.localPosition += (UnityEngine.Vector3)(direction * ghostSpeed) * Time.deltaTime;
            }
        }
        
    }

    void ModeUpdate()
    {
        if(currentMode != Mode.Frightened)
        {
            modeChangeTimer += Time.deltaTime;

            if(modeChangeIteration == 1)
            {
                if(currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer1)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }

                if(currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer1)
                {
                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            } else if(modeChangeIteration == 2)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer2)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }

                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer2)
                {
                    modeChangeIteration = 3;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            } else if(modeChangeIteration == 3)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer3)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }

                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer3)
                {
                    modeChangeIteration = 4;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            } else if(modeChangeIteration == 4)
            {
                if(currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer4)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
            }
        } else if(currentMode == Mode.Frightened)
        {

        }
    }

    void ChangeMode (Mode m)
    {
        currentMode = m;
    }

    Node ChooseNextNode()
    {
        UnityEngine.Vector2 targetTile = UnityEngine.Vector2.zero;

        UnityEngine.Vector2 pacmanPosition = PacMan.transform.position;
        targetTile = new UnityEngine.Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));

        Node moveToNode = null;

        Node[] foundNodes = new Node[4];
        UnityEngine.Vector2[] foundNodesDirection = new UnityEngine.Vector2[4];

        int nodeCounter = 0;

        for (int i = 0; i < currentNode.neighours.Length; i++)
        {
            if(currentNode.validDirections[i] != direction * -1)
            {
                foundNodes[nodeCounter] = currentNode.neighours[i];
                foundNodesDirection[nodeCounter] = currentNode.validDirections[i];
                nodeCounter++;
            }
        }

        if(foundNodes.Length == 1)
        {
            moveToNode = foundNodes[0];
            direction = foundNodesDirection[0];
        }

        if(foundNodes.Length > 1)
        {
            float leastDistance = 10000f;
            for(int i = 0; i < foundNodes.Length; i++)
            {
                if(foundNodesDirection[i] != UnityEngine.Vector2.zero)
                {
                    float distance = GetDistance(foundNodes[i].transform.position, targetTile);

                    if(distance < leastDistance)
                    {
                        leastDistance = distance;
                        moveToNode = foundNodes[i];
                        direction = foundNodesDirection[i];
                    }
                }
            }
        }

        return moveToNode;
    }


    Node GetNodeAtPosition(UnityEngine.Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if(tile != null)
        {
            if(tile.GetComponent<Node>() != null)
            {
                return tile.GetComponent<Node>();
            }
        }

        return null;
    }

    GameObject GetPortal(UnityEngine.Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if(tile != null)
        {
            if (tile.GetComponent<Tiles>().isPortal)
            {
                GameObject otherPortal = tile.GetComponent<Tiles>().portalReceiver;
                return otherPortal;
            }
        }

        return null;
    }

    float LengthFromNode(UnityEngine.Vector2 targetPosition)
    {
        UnityEngine.Vector2 vec = targetPosition - (UnityEngine.Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.position);

        return nodeToSelf > nodeToTarget;
    }

    float GetDistance(UnityEngine.Vector2 posA, UnityEngine.Vector2 posB)
    {
        float dx = posA.x - posB.x;
        float dy = posA.y - posB.y;
        
        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        return distance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            return;
        }
    }
}
