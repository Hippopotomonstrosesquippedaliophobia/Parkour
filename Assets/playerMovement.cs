using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = 19.62f;
    public float jumpHeight = 3f;

    // Uses sphere at bottom of character to see if the player is touching the ground
    public Transform groundCheck;
    public Transform wallCheck;
    public float groundDistance = 0.4f;
    public float wallDistance = 2f;
    public LayerMask groundMask;

    public RaycastHit wallTest;
    public Ray wallLeft, wallRight;

    Vector3 velocity;
    bool isGrounded;
    bool isNearWall;
    bool run;
    bool doubleJump;
    bool wallRan = false;
    bool wallAttached = false;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isNearWall = Physics.CheckSphere(wallCheck.position, wallDistance, groundMask);

        //Check if player is holding shift then changes speed
        playerRun();

        //Allows player to jump if grounded, or jump one time after being airbourne
        playerJump();

        //Moves player relative to look direction
        playerMove();

        playerWallRun();

        //Gravity
        playerFall();

    }

    void playerRun()
    {
        //Run if shift is held down
        if (Input.GetKey("left shift"))
            run = true;
        else
            run = false;

        //Check if player is running and change speed accordingly
        if (run)
            speed = 20f;
        else
            speed = 10f;
    }

    void playerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Relative to direction movement
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void playerJump()
    {
        if (isGrounded)
        {
            wallRan = false;
            doubleJump = isGrounded; //Reset doubleJump after every landing
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Let player Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded && doubleJump) // Allows one jump after going into air
        {
            velocity.y = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
            doubleJump = false; // reset until grounded again
        }
    }

    void playerFall()
    {
        velocity.y -= gravity * Time.deltaTime;
        // Speeds up over time falling
        controller.Move(velocity * Time.deltaTime);
    }

    void playerWallRun()
    {
        bool wl = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out wallTest, Mathf.Infinity, groundMask);
        bool wr = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out wallTest, Mathf.Infinity, groundMask);

        if (isNearWall)
        {
            wallLeft = new Ray(transform.position, Vector3.left);
            wallRight = new Ray(transform.position, Vector3.right);

            if (Physics.Raycast(wallLeft, out wallTest) && wallTest.distance < 1.5f)
            {
                Debug.Log("Hit left");
                onWall("left");
            }
            if (Physics.Raycast(wallRight, out wallTest) && wallTest.distance < 1.5f)
            {
                Debug.Log("Hit right");
                onWall("right");
            }
        }
    }

    void onWall(string side)
    {        
        wallAttached = true;
        wallRan = true;
        isGrounded = true;
        checkWallAttachment();
    }

    void checkWallAttachment()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
            Debug.Log("Jumped off");
        }
    }
}
