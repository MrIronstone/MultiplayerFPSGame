using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject Door;
    private Vector3 StartingPosition;
    private float speed = 1;
    bool pressed = false;
    bool exit = false;

    Vector3 targetposition;

    private void Awake()
    {
        StartingPosition = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // pressure plate y sini 0 yap
        pressed = true;
        targetposition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.05f, gameObject.transform.position.z);
    }

    private void OnTriggerExit(Collider other)
    {
        pressed = false;
        exit = true;
        targetposition = StartingPosition;
    }

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        if (pressed)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, targetposition, step);
        }

        if (exit)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, targetposition, step);
        }


    }

}
