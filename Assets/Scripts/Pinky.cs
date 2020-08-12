using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class Pinky : MonoBehaviour
{
    public float ghostSpeed = 3.9f;

    private Transform target;
    private UnityEngine.Vector2 direction = UnityEngine.Vector2.zero;

    private void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
        Move();
        UpdateOrientation();
    }

    void CheckDirection()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            return;
        }
    }


    void UpdateOrientation()
    {
        if (direction == UnityEngine.Vector2.left)
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
    void Move()
    {
        transform.localPosition += (UnityEngine.Vector3)(direction * ghostSpeed) * Time.deltaTime;
    }
}
