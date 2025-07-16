using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    private float fullHealth;
    CharAnimations charAnim;
    private bool charDead;
    private bool isPlayer;
    [SerializeField]
    private Image heath_UI;
    private CharacterBase charBase;
    public event Action OnDeath;

    // Start is called before the first frame update
    void Start()
    {
        charAnim = GetComponent<CharAnimations>();
        charBase = GetComponent<CharacterBase>();
        isPlayer = gameObject.CompareTag(Tags.PLAYER_TAG);
        fullHealth = charBase.health;
    }
    public void playerReset()
    {
        if (isPlayer)
        {
            charBase.health = fullHealth;
            heath_UI.fillAmount = charBase.health / fullHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ApplyDamage(float damage)
    {
        charBase.health -= damage;
        if(heath_UI != null)
        {
            heath_UI.fillAmount = charBase.health / fullHealth;
        }
        if (charBase.health <= 0)
        {
            print(GetRootParentName(gameObject) + " died");
            charAnim.Died();
            charDead = true;
            if (isPlayer)
            {
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<ActionInput>().enabled = false;
                GameObject[] enemys = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
                if(enemys.Length != 0)
                {
                    foreach(GameObject enemy in enemys)
                    {
                        enemy.GetComponent<CharacterController>().enabled = false;
                    }
                }
            }
            else
            {
                GetComponent<CharacterController>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                if(transform.Find(ConstantNames.ENEMYFLOWINGHEARTHBAR) != null)
                {
                    transform.Find(ConstantNames.ENEMYFLOWINGHEARTHBAR).gameObject.SetActive(false);
                }
                    
            }

        }
    }
    public void DeadAnimationDone()
    {
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }
        Destroy(gameObject);
    }
    public bool IsCharDead()
    {
        return charDead;
    }
    //private string DieAnimation()
    //{
     //   charAnim.Died();
   // }

    string GetRootParentName(GameObject obj)
    {
        Transform currentTransform = obj.transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        return currentTransform.name;
    }
}
