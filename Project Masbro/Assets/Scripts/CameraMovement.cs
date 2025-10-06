using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject groundZero;
    private GameObject target;
    private Vector3 targetPos;
    private float playerXPos;
    private float playerYPos;
    private float offsetY = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target == null) return;
        }

        playerXPos = target.transform.position.x;
        playerYPos = target.transform.position.y;

        targetPos = new Vector3(playerXPos, Mathf.Clamp(playerYPos, groundZero.transform.position.y + offsetY, 100), -10);

        transform.position = targetPos;
    }
}
