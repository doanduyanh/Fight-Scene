using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInput : CharacterBase
{
    private UnityEngine.CharacterController controller;
    private InputManager inputManager;
    private CharAnimations charAnim;


    /*~ActionInput()
    {
        DefUnsubscribe();
    }*/

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<UnityEngine.CharacterController>();
        charAnim = GetComponent<CharAnimations>();
        inputManager = InputManager.Instance;
        //DefSubscribe();

    }
    //Didn't make the final cut 
    /*public void DefUnsubscribe()
    {
        inputManager.UnsubPlayerDefOff(DefOff);
        inputManager.UnsubPlayerDefOn(DefOn);
    }
    public void DefSubscribe()
    {
        inputManager.SubPlayerDefOff(DefOff);
        inputManager.SubPlayerDefOn(DefOn);
    }*/

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    public void Attack()
    {
        if (inputManager.GetPlayerAttackHead())
        {
            charAnim.AttackHead();
            AttackPattern pattern = attackPatterns.Find(ap => ap.animationTrigger == AnimatorParams.ATTACKHEAD);
            if (pattern != null)
            {
                attackPointsToActive = pattern.attackPoints;
            }
        }
        else if (inputManager.GetPlayerAttackBody())
        {
            charAnim.AttackBody();
            AttackPattern pattern = attackPatterns.Find(ap => ap.animationTrigger == AnimatorParams.ATTACKBODY);
            if (pattern != null)
            {
                attackPointsToActive = pattern.attackPoints;
            }
        }
    }

    /*private void DefOn(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        charAnim.DefOn();
    }

    private void DefOff(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        charAnim.DefOff();
    }*/


}
