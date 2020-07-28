using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class PacManController : MonoBehaviour
{

    public float pacSpeed = 4.0f;
    private UnityEngine.Vector2 direction = UnityEngine.Vector2.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Move();
        UpdateOrientation();
    }
    
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction= UnityEngine.Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = UnityEngine.Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = UnityEngine.Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = UnityEngine.Vector2.up;
        }
    }
    
    void UpdateOrientation()
    {
        if(direction == UnityEngine.Vector2.left)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction == UnityEngine.Vector2.right)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction == UnityEngine.Vector2.up)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (direction == UnityEngine.Vector2.down)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }


    }
    void Move()
    {
        transform.localPosition += (UnityEngine.Vector3)(direction * pacSpeed) * Time.deltaTime;
    }
}
