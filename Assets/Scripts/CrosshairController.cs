using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[SelectionBase]
public class CrosshairController : MonoBehaviour
{
    public event Action<Vector3> OnSlingshotFired;

    [SerializeField] List<EnemyHandler> potentialLockOnEnemies = new List<EnemyHandler>();
    [SerializeField] GameData gameData;
    [SerializeField] EnemyHandler enemyToLock;
    [SerializeField] EnemyHandler lockedOnEnemy;

    bool isLockedOn = false;
    bool isLockChanging = false;
    float enemyLockTimer;
    float nextAllowedFire;

    public bool IsLockedOn => isLockedOn;

    public void Fire()
    {
        if (nextAllowedFire > Time.time)
            return;

        nextAllowedFire = Time.time + gameData.CooldownBetweenShotsInSeconds;

        Debug.Log("Slingshot fired to : " + lockedOnEnemy, this);
        AkSoundEngine.PostEvent("SLG_Fire", gameObject);

        if (lockedOnEnemy)
            lockedOnEnemy.HitEnemy();
    }

    private void Update()
    {
        if (isLockedOn)
        {
            //If the enemy is out of the potentially locked enemis but we are still locked on them, begin delocking them
            if (!isLockChanging && !potentialLockOnEnemies.Contains(lockedOnEnemy))
            {
                isLockChanging = true;
                enemyLockTimer = Time.time + gameData.delockDelay;
            }
            //if we were delocking the enemy, but we got it back before the delay
            else if (isLockChanging && potentialLockOnEnemies.Contains(lockedOnEnemy))
            {
                isLockChanging = false;
            }
            //If the enemy is still lockable and the lock on delay is complete
            else if (lockedOnEnemy == null || (isLockChanging && Time.time > enemyLockTimer))
            {
                lockedOnEnemy = null;
                isLockChanging = false;
                isLockedOn = false;
            }
        }
        else
        {
            //If we are not locking on to anyone and one is available
            if (!isLockChanging && potentialLockOnEnemies.Count != 0)
            {
                isLockChanging = true;
                enemyToLock = potentialLockOnEnemies[0];
                enemyLockTimer = Time.time + gameData.lockOnDelay;
                return;
            }
            //If we were locking on to someone but it got away before
            else if (isLockChanging && !potentialLockOnEnemies.Contains(enemyToLock))
            {
                isLockChanging = false;
                enemyToLock = null;
            }
            //If the enemy is still lockable and the lock on delay is complete
            else if (isLockChanging && Time.time > enemyLockTimer)
            {
                lockedOnEnemy = enemyToLock;
                enemyToLock = null;
                isLockChanging = false;
                isLockedOn = true;
            }
        }
    }

    public void AddEnemyToPotentialLockList(EnemyHandler enemyHandler)
    {
        if (!potentialLockOnEnemies.Contains(enemyHandler))
            potentialLockOnEnemies.Add(enemyHandler);
    }

    public void RemoveEnemyFromPotentialLockList(EnemyHandler enemyHandler)
    {
        if (potentialLockOnEnemies.Contains(enemyHandler))
            potentialLockOnEnemies.Remove(enemyHandler);
    }
}