using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public float health = 100f;
    private float fullHealth;
    CharAnimations charAnim;
    private bool charDead;
    private bool isPlayer;
    [SerializeField]
    private Image heath_UI;
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(heath_UI != null)
        {
            heath_UI.fillAmount = health / fullHealth;
        }
        if (health <= 0)
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
                        enemy.GetComponent<EnemyController>().enabled = false;
                    }
                }
            }
            else
            {
                GetComponent<EnemyController>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                transform.Find(ConstantNames.ENEMYFLOWINGHEARTHBAR).gameObject.SetActive(false);
            }

        }
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
    // Start is called before the first frame update
    void Start()
    {
        charAnim = GetComponent<CharAnimations>();
        isPlayer = gameObject.CompareTag(Tags.PLAYER_TAG);
        fullHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
