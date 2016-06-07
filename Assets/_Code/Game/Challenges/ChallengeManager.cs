using UnityEngine;
using System.Collections;
using BounceDudes;

public class ChallengeManager 
{
    public static bool ValidateCompletion(Challenge challenge)
    {
        switch (challenge._type)
        {
            case ChallengeType.KillXEnemy:
                return ChallengeManager.ValidateKillEnemy(challenge._x);
            case ChallengeType.MakeAComboWithXHits:
                return ChallengeManager.ValidateCombo(challenge._x);
            case ChallengeType.MakeXDamage:
                return ChallengeManager.ValidateDamage(challenge._x);
            case ChallengeType.ShootMaxXSoldier:
                return ChallengeManager.ValidateShoot(challenge._x);
            case ChallengeType.StayWithXHP:
                return ChallengeManager.ValidateXHP(challenge._x);
            default:
                return false;
        }
    }

    protected static bool ValidateKillEnemy(int x)
    {
        return LevelManager.Instance.EnemiesKilled >= x;
    }

    protected static bool ValidateShoot(int x)
    {
        return Weapon.Instance.ShootCount < x;
    }

    protected static bool ValidateDamage(int x)
    {
        return (LevelManager.Instance._enemyBase._startedHp - LevelManager.Instance._enemyBase.HP) >= x;
    }

    protected static bool ValidateXHP(int x)
    {
        return LevelManager.Instance._playerBase.HP >= x;
    }

    protected static bool ValidateCombo(int x)
    {
        return ComboManager.Instance.MaxHitComboCount >= x;
    }
}
