using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStructure
{
    public float damage;
    public float attackSpeed;
    public float health;
    public float movementSpeed;
    public float attackDelay;

    public CharStructure()
    {
        this.damage = BaseCharStat.DAMAGE;
        this.attackSpeed = BaseCharStat.ATTACKSPEED;
        this.health = BaseCharStat.HEALH;
        this.movementSpeed = BaseCharStat.MOVEMENTSPEED;
        this.attackDelay = BaseCharStat.ATTACKDELAY;
    }
    public void CharCookingDone(List<float> dataList)
    {
        if (dataList.Count != 5)
        {
            return;
        }
        damage = dataList[0];
        attackSpeed = dataList[1];
        health = dataList[2];
        movementSpeed = dataList[3];
        attackDelay = dataList[4];
    }
    public List<float> CharToListData()
    {
        return new List<float>{ damage, attackSpeed, health, movementSpeed, attackDelay };
    }
}


public class CharGenerator
{

    public int level;

    public CharStructure GenerateEnemyByWeight(int weight)
    {
        CharStructure charCooking = new CharStructure();
        List<float> charCookingAsList = charCooking.CharToListData();
        List<float> perWeightCharStatGrow = PerWeightCharStatGrow.WEIGHTMATRIX;
        List<float> weightDistribute = new List<float> { 0,0,0,0,0};



        ////////Char cooking algorithm:base stat + Weitgh Distribute (level * 2 for enough variaty of noticeable stat) * Difficulti multiplier(Easy/Normal/Hard) * PerWeightCharStatGrow///////
        for (int i = 1; i< weight*2; i++)
        {
            int improving = Random.Range(0, 5);
            weightDistribute[improving] += 1;
        }
        for (int i = 0; i< charCookingAsList.Count; i++)
        {
            charCookingAsList[i] += weightDistribute[i] * perWeightCharStatGrow[i];
        }
        charCooking.CharCookingDone(charCookingAsList);
        return charCooking;
    }
}
