using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTouchToMove : MonoBehaviour
{
    Touch touch;
    Vector2 initPos;
    Vector2 direction;

    public CharacterController petController;
    Vector3 moveDirection;
    public float petSpeed = 5.0f;

    bool canMove = false;

    public float gravity = 9.8f;

    public float jumpForce = 3.0f;

    public float stopForce = 2f;

    public Animator petAnimator;

    public GameObject jumpeffect;

    public GameManager gameManager;

    private void Awake()
    {
        float bonusSpeed = 0.1f * PlayerPrefs.GetInt("speedLevel", 1);
        petSpeed += bonusSpeed;
    }

    void Update()
    {
        if (!gameManager.isGameEnded && gameManager.isGameStarted)
        {
            HandleInput();
            HandleMovement();
            HandleJump();

            ApplyGravity();
            MoveCharacter();
        }
    }

    void HandleInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            canMove = true;

            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    initPos = touch.position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    direction = touch.position - initPos;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    initPos = Input.mousePosition;
                }
                direction = (Vector2)Input.mousePosition - initPos;
            }

            if (petController.isGrounded)
            {
                moveDirection = new Vector3(direction.x, 0, direction.y).normalized * petSpeed;
                if (moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
            }
        }
        else
        {
            canMove = false;
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, Time.deltaTime * stopForce);
        }

        petAnimator.SetBool("canWalk", canMove);
    }

    void HandleMovement()
    {
        // Any additional movement logic can go here
    }

    void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && petController.isGrounded)
        {
            Jump();
        }
    }

    void ApplyGravity()
    {
        if (!petController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    void MoveCharacter()
    {
        petController.Move(moveDirection * Time.deltaTime);
    }

    public void Jump()
    {
        if (petController.isGrounded)
        {
            Instantiate(jumpeffect, transform.position, Quaternion.identity);
            moveDirection.y += jumpForce;
        }
    }
}
