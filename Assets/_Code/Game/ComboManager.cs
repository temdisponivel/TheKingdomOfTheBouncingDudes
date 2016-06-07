using UnityEngine;
using System.Collections;
using BounceDudes;
using System.Collections.Generic;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance = null;

    public int CurrentPointsMultiplier {
        get
        {
            return (this._killCount*this._killComboMultiplier) + (this._hitCount*this._hitComboMultiplier) +
                   (this._elementKillCount*this._elementkillComboMultiplier);
        }
    }

    [Header("Multipliers")]
    public int _killComboMultiplier = 2;
    public int _hitComboMultiplier = 2;
    public int _elementkillComboMultiplier = 2;

    [Header("Cool down")]
    public float _killComboCooldown = 3f;
    public float _hitComboCooldown = 3f;
    public float _elementKillComboCooldown = 3f;

    protected int _hitCount = 0;
    protected int _killCount = 0;
    protected int _elementKillCount = 0;

    protected float _lastKillTime = 0f;
    protected float _lastHitTime = 0f;
    protected float _lastElementKillTime = 0f;

    public int MaxHitComboCount { get; set; }
    
    private void Awake()
    {
        ComboManager.Instance = this;
    }

    private void Update()
    {
        if (Time.time - this._lastKillTime > this._killComboCooldown)
        {
            this._killCount = 0;
            this._lastKillTime = Time.time;
        }

        if (Time.time - this._lastHitTime > this._killComboCooldown)
        {
            this._hitCount = 0;
            this._lastHitTime = Time.time;
        }

        if (Time.time - this._elementKillCount > this._killComboCooldown)
        {
            this._elementKillCount = 0;
            this._lastElementKillTime = Time.time;
        }
    }

    public void AddKill()
    {
        this._killCount++;
        this._lastKillTime = Time.time;
        Debug.Log(string.Format("KILL {0}", this._killCount));
    }

    public void AddHit()
    {
        this._hitCount++;
        this._lastKillTime = Time.time;

        if (this.MaxHitComboCount < this._hitCount)
        {
            this.MaxHitComboCount = this._hitCount;
        }

        Debug.Log(string.Format("HIT {0}", this._hitCount));
    }

    public void AddElementKill()
    {
        this._elementKillCount++;
        this._lastElementKillTime = Time.time;

        Debug.Log(string.Format("ELEMENT KILL {0}", this._elementKillCount));
    }
}
