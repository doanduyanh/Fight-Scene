using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private UnityEngine.CharacterController controller;
    private InputManager inputManager;
    private CharAnimations charAnim;
    public float rotationSpeed = 90f;

    [SerializeField]
    private float playerSpeed = 20.0f;

    private void Start()
    {
        controller = GetComponent<UnityEngine.CharacterController>();
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
        if (enemys.Length==0)
        {
            charAnim.Win(true);
            return;
        }
        else
        {
            charAnim.Win(false);
        }

        int nearestIndex = -1;
        float nearestValueDistance = -1;
        for (int i = 0; i < enemys.Length; i++)
        {
            HealthScript tempHC = enemys[i].GetComponent<HealthScript>();
            if (tempHC != null && !tempHC.IsCharDead())
            {
                if (nearestValueDistance >= Vector3.Distance(enemys[i].transform.position, transform.position) || nearestValueDistance == -1)
                {
                    nearestIndex = i;
                    nearestValueDistance = Vector3.Distance(enemys[i].transform.position, transform.position);
                }
            }
        }
        if (nearestIndex != -1)
        {
            //transform.LookAt(enemys[nerestIndex].transform.position);
            Vector3 direction = enemys[nearestIndex].transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 euler = newRotation.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            transform.rotation = Quaternion.Euler(euler);
        }
    }
    public void Move()
    {
        Vector2 movement = new(0, 0); 
        Vector3 move = new(0, 0, 0);
        //if (!charAnim.IsDefing()) //wont allow to move if defending //cut this feature for difficuty and time scope
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
