using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimations : MonoBehaviour
{

    private Animator anim;
    private bool deffing = false;
    private bool inAction = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk(bool walking)
    {
        anim.SetBool(AnimatorParams.WALKING, walking);
    }   
    public void Win(bool win)
    {
        anim.SetBool(AnimatorParams.WIN, win);
    }    
    public void WalkingDirection(string directionParam, float direction)
    {
        anim.SetFloat(directionParam, direction);
    }
    
    public void AttackHead()
    {
        if (!inAction)
        {
            anim.SetTrigger(AnimatorParams.ATTACKHEAD);
            inAction = true;

        }
    }    
    public void AttackBody()
    {
        if (!inAction)
        {
            anim.SetTrigger(AnimatorParams.ATTACKBODY);
            inAction = true;
        }
    }
    public void Died()
    {   
        anim.SetBool(AnimatorParams.KNOCEDOUT, true);
        inAction = true;
    }
    public void DefOn()
    {
        if (!deffing)
        {
            anim.SetBool(AnimatorParams.DEF, true);
            deffing = true;
            inAction = true;
        }
    }    
    public void DefOff()
    {
        if (deffing)
        {
            anim.SetBool(AnimatorParams.DEF, false);
            deffing = false;
            inAction = false;
        }
    }
    public void ActionAnimationStart()
    {
        inAction = true;
    }
    public void ActionAnimationDone()
    {
        inAction = false;
    }
    public bool IsDoingAttackAndStuff()
    {
        return inAction;
    }

    public bool IsDefing()
    {
        return deffing;
    }

}
