using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DifficultyLevel", order = 1)]
public class DifficultyLevel : ScriptableObject
{
    public float duration;

    public float crabMoveSpeed, slamTelegraphTime, slamRecoveryDelay, slamRecoverTime;
    public float slamAttackDuration, throwTelegraphTime1, throwTelegraphTime2;
    public int throwMinAmount, throwMaxAmount;
    public float laserXDuration, laserXStartDelay, laserXEndDelay;
    public float laserFollowDuration, laserFollowSpeed, laserFollowStartDelay;
    public float timeBetweenAttacks;
    public int level;
    public void SetDifficulty()
    {
        CrabAI c = CrabAI.Instance;
        c.moveSpeed = crabMoveSpeed;
        c.slamTelegraphTime = slamTelegraphTime;
        c.slamRecoveryDelay = slamRecoveryDelay;
        c.slamRecoverTime = slamRecoverTime;

        c.slamAttackDuration = slamAttackDuration;
        c.throwTelegraphTime1 = throwTelegraphTime1;
        c.throwTelegraphTime2 = throwTelegraphTime2;
        c.throwMaxAmount = throwMaxAmount;
        c.throwMinAmount = throwMinAmount;

        c.laserXDuration = laserXDuration;
        c.laserXStartDelay = laserXStartDelay;
        c.laserXEndDelay = laserXEndDelay;

        c.laserFollowDuration = laserFollowDuration;
        c.laserFollowSpeed = laserFollowSpeed;
        c.laserFollowStartDelay = laserFollowStartDelay;
        c.timeBetweenAttacks = timeBetweenAttacks;
        c.availableMoves = level;
        Debug.Log("Level up!");

        UIController.Instance.Level++;
    }
}
