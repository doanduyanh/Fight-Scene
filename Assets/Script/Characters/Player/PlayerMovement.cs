using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private InputManager inputManager;
    private CharAnimations charAnim;
    public float rotationSpeed = 90f;

    [SerializeField]
    private float playerSpeed = 20.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        charAnim = GetComponent<CharAnimations>();
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        RotateTowardEnemy();
        Move();

    }

    public void RotateTowardEnemy()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
        int nerestIndex = 0;
        for (int i = 0; i < enemys.Length; i++)
        {
            if (Vector3.Distance(enemys[nerestIndex].transform.position, transform.position) >= Vector3.Distance(enemys[i].transform.position, transform.position))
            {
                nerestIndex = i;
            }
        }
        //transform.LookAt(enemys[nerestIndex].transform.position);
        Vector3 direction = enemys[nerestIndex].transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    public void Move()
    {
        Vector2 movement = new(0, 0); 
        Vector3 move = new(0, 0, 0);
        if (!charAnim.IsDefing()) //wont allow to move if defending
        {
            movement = inputManager.GetPlayerMovement();
            move = transform.forward * movement.y + transform.right * movement.x;
            move.y = 0;
            move = move.normalized;

            Vector3 finalMove = (move * playerSpeed);
            controller.Move(finalMove * Time.deltaTime);
        }
        AnimateMovement(movement, move);

    }

    private void AnimateMovement(Vector2 movement, Vector3 move)
    {
        charAnim.WalkingDirection(AnimatorParams.WALKLR, movement.x);
        charAnim.WalkingDirection(AnimatorParams.WALKFB, movement.y);
        charAnim.Walk(move != Vector3.zero);
    }
}
