using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMapScript : MonoBehaviour
{
    public float tiltAngle = 20.0f;
    public float smoothTurnAmount = 600.0f;

    float verticalTilt;
    float horizontalTilt;

    public bool canTurn = false;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canTurn == true)
        {
            verticalTilt += Input.GetAxis("Vertical");
            horizontalTilt += Input.GetAxis("Horizontal") * -1;

            if (verticalTilt > tiltAngle)
            {
                verticalTilt = tiltAngle;
            }
            else if (verticalTilt < (tiltAngle * -1))
            {
                verticalTilt = (tiltAngle * -1);
            }

            if (horizontalTilt > tiltAngle)
            {
                horizontalTilt = tiltAngle;
            }
            else if (horizontalTilt < (tiltAngle * -1))
            {
                horizontalTilt = (tiltAngle * -1);
            }

            Quaternion targetRotation = Quaternion.Euler(verticalTilt, 0, horizontalTilt);

            transform.rotation = targetRotation;
        }

    }
}
