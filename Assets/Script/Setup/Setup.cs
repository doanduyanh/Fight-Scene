using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIndex
{
    public const int MAINMENU = 0;
    public const int ARENA = 1;
    public const int ARENAMULTI = 2;
    public const int ARENATEAM = 3;
    public const int ARENATEAM50 = 4;
}
public class Tags
{
    public const string PLAYER_TAG = "Player";
    public const string ENEMY_TAG = "Enemy";
    public const string ALLIES_TAG = "Allies";
}
public class AnimatorParams
{
    public const string WALKLR = "WalkLR";
    public const string WALKFB = "WalkFB";
    public const string WALKING = "Walking";
    public const string WIN = "Win";

    public const string ATTACKHEAD = "AttackHead";
    public const string ATTACKBODY = "AttackBody";

    public const string DEF = "Def";

    public const string KNOCEDOUT = "KnockedOut";
}
public class ConstantNames
{
    public const string ENEMYFLOWINGHEARTHBAR = "HealthBarCanvas";
}

public class BaseCharStat
{
    public const float DAMAGE = 10;
    public const float ATTACKSPEED = 1f;
    public const float HEALH = 50;
    public const float MOVEMENTSPEED = 2;
    public const float ATTACKDELAY = 2.5f;
}
public class PerWeightCharStatGrow
{
    /*public const float DAMAGE = 7;
    public const float ATTACKSPEED = .15f;
    public const float HEALH = 25;
    public const float MOVEMENTSPEED = .25f;
    public const float ATTACKDELAY = -.2f;*/
    public static readonly List<float> WEIGHTMATRIX = new List<float> { 5, .25f, 37, .4f, -.3f };
    
}