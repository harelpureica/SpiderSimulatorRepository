using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followplayer : MonoBehaviour
{
    Transform player;
    void Start()
    {
        player=FindObjectOfType<RobotOneMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= player.transform.position;
    }
}
