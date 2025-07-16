using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackPattern
{
    public float moveDistance;
    public string animationTrigger;
    public GameObject[] attackPoints;
}
public class CharacterBase : MonoBehaviour
{

    [HideInInspector]
    protected GameObject[] attackPointsToActive;


    public List<AttackPattern> attackPatterns;
    public float damage = 1f;
    public float health = 100f;

    public void ActivateAttackPoints()
    {
        foreach (GameObject at in attackPointsToActive)
        {
            at.SetActive(true);
        }
    }
    public void DeactivateAttackPoints()
    {
        foreach (GameObject at in attackPointsToActive)
        {
            at.SetActive(false);
        }
    }
}
