using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;

public class PacManController : MonoBehaviour
{

    public float pacSpeed = 4.0f;

    public Sprite idleSprite;
    private UnityEngine.Vector2 direction = UnityEngine.Vector2.zero;
    private UnityEngine.Vector2 nextDirection;

    private Node currentNode, previousNode, targetNode;

    // private  AudioSource audioSrc;
    //void Awake()
    //{
    //  audioSrc = GetComponent<AudioSource>();
    //}
    private void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
            Debug.Log(currentNode);
        }

        direction = UnityEngine.Vector2.left;
        ChangePosition(direction);
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();

        Move();

        UpdateOrientation();

        UpdateAnimationState();

        ConsumePellet();
        //if (!audioSrc.isPlaying)
        //{
         //   audioSrc.Play();
        //}
       
    }
    
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(UnityEngine.Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(UnityEngine.Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(UnityEngine.Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(UnityEngine.Vector2.up);
        }
    }

    void UpdateOrientation()
    {
        if(direction == UnityEngine.Vector2.left)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction == UnityEngine.Vector2.right)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction == UnityEngine.Vector2.up)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        if (direction == UnityEngine.Vector2.down)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void ChangePosition(UnityEngine.Vector2 d)
    {
        if (d != direction)
            nextDirection = d;

        if (currentNode != null)
        {
            Node moveToNode = CanMove(d);
            if (moveToNode != null)
            {
                direction = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null)
        {
            if(nextDirection == direction * -1)
            {
                direction *= -1;

                Node tempNode = targetNode;

                targetNode = previousNode;

                previousNode = tempNode;
            }

            if (OvershotTarget())
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                Node moveToNode = CanMove(nextDirection);

                if (moveToNode != null)
                    direction = nextDirection;

                if (moveToNode == null)
                    moveToNode = CanMove(direction);

                if(moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direction = UnityEngine.Vector2.zero;
                }
            }
            else
            {
                transform.localPosition += (UnityEngine.Vector3)(direction * pacSpeed) * Time.deltaTime;
            }
        }
    }

    //void MoveToNode(UnityEngine.Vector2 d)
    //{
    //    Node moveToNode = CanMove(d);
    //    if (moveToNode != null)
    //    {
    //        transform.localPosition = moveToNode.transform.position;
    //        currentNode = moveToNode;
    //    }
    //}
    
    void UpdateAnimationState()
    {
        if(direction == UnityEngine.Vector2.zero)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }
        else
        {
            GetComponent<Animator>().enabled = true;
        }
    }
    Node CanMove (UnityEngine.Vector2 d)
    {
        Node moveToNode = null;

        for(int i = 0; i < currentNode.neighours.Length; i++)
        {
            if(currentNode.validDirections[i] == d)
            {
                moveToNode = currentNode.neighours[i];
                break;
            }
        }
        return moveToNode;
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Coin"))
    //    {
    //        Destroy(other.gameObject);
    //    }
    //}

    void ConsumePellet()
    {
        GameObject o = GetTileAtPosition(transform.position);

        if(o != null)
        {
            Tiles tile = o.GetComponent<Tiles>();

            if(tile != null)
            {
                if (!tile.didConsume && (tile.isPellet || tile.isSuperPellet)){
                    o.GetComponent<SpriteRenderer>().enabled = false;
                    tile.didConsume = true;
                }
            }
        }
    }

    GameObject GetTileAtPosition(UnityEngine.Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x);
        int tileY = Mathf.RoundToInt(pos.y);

        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
            return tile;

        return null;
    }

    Node GetNodeAtPosition ( UnityEngine.Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if(tile != null)
        {
            return tile.GetComponent<Node>();
        }

        return null;

    }

    bool OvershotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    }

    float LengthFromNode (UnityEngine.Vector2 targetPosition)
    {
        UnityEngine.Vector2 vec = targetPosition - (UnityEngine.Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

}
