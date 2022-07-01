using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 9;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float maintainedXMomentum;

    private float maintainedZMomentum;

    private Vector3 maintainedRightTransform;

    private Vector3 maintainedForwardTransform;

    Vector3 velocity;
    bool isGrounded;

    private Vector3 move = new Vector3(0,0,0);

    public MouseLook mouseMovements;

    public float walkMultiplier = 1f;




    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown("left shift")) {
            speed = 4.5f;
        }
        if (Input.GetKeyUp("left shift")) {
            speed = 9f;
        }

        if (Input.GetKeyDown("c")) {
            speed = 6f;
            
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (isGrounded) {
            move = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1);
          // move = transform.right * x + transform.forward * z;
        }
        else if (!isGrounded && (maintainedXMomentum != 0 || maintainedZMomentum != 0)) {
            if ((x == -1 && Input.GetAxis("Mouse X") < 0) || (x == 1 && Input.GetAxis("Mouse X") > 0)) {
                maintainedForwardTransform = transform.forward;
                Debug.Log(maintainedForwardTransform);
            }
            move = maintainedRightTransform * maintainedXMomentum + maintainedForwardTransform * maintainedZMomentum;
        }



        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            maintainedXMomentum = x;
            maintainedZMomentum = z;
            maintainedRightTransform = transform.right;
            maintainedForwardTransform = transform.forward;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += 2*gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
