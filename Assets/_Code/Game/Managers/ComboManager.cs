using UnityEngine;
using System.Collections;
using BounceDudes;
using System.Collections.Generic;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance = null;

    public Text TextCombo;

    [Header("Multipliers")]
    public int _killComboMultiplier = 1;
    public int _hitComboMultiplier = 1;
    public int _elementkillComboMultiplier = 1;
    public int _fontSizeMultiplierPerIncrement = 1;

    [Header("Cool down")]
    public float _comboTextCoolDown = 1f;
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

    public int LastComboShown { get; set; }

    public float _lastComboShownTime = 0;

    public float _fontSizeBkp;

    private void Awake()
    {
        ComboManager.Instance = this;
        _fontSizeBkp = this.TextCombo.fontSize;
		this.TextCombo.text = string.Empty;
    }

    private void Update()
    {
        if (Time.time - this._lastKillTime > this._killComboCooldown)
        {
            this._killCount = 0;
            this._lastKillTime = Time.time;
            this.UpdateInfo();
        }

        if (Time.time - this._lastHitTime > this._killComboCooldown)
        {
            this._hitCount = 0;
            this._lastHitTime = Time.time;
            this.UpdateInfo();
        }

        if (Time.time - this._elementKillCount > this._killComboCooldown)
        {
            this._elementKillCount = 0;
            this._lastElementKillTime = Time.time;
            this.UpdateInfo();
        }
    }

    public void AddKill()
    {
        this._killCount++;
        this._lastKillTime = Time.time;

        this.UpdateInfo();
    }

    public void AddHit()
    {
        this._hitCount++;
        this._lastKillTime = Time.time;

        this.UpdateInfo();
    }

    public void AddElementKill()
    {
        this._elementKillCount++;
        this._lastElementKillTime = Time.time;

        this.UpdateInfo();
    }

    public void UpdateInfo()
    {
        var current = this._killCount + this._hitCount + this._elementKillCount;
        if (current > LastComboShown)
        {
            this.TextCombo.text = string.Format("{0} x", current);
           // var size = this.TextCombo.fontSize;
            //this.TextCombo.fontSize = (int)Mathf.Clamp(size + (Mathf.Abs(current - LastComboShown) * _fontSizeMultiplierPerIncrement), _fontSizeBkp, 30);
            this.LastComboShown = current;
            this._lastComboShownTime = Time.time;
        }
        else
        {
            if (Time.time - _lastComboShownTime > _comboTextCoolDown)
            {
                this.TextCombo.text = string.Empty;
                LastComboShown = 0;
                this.TextCombo.fontSize = (int)_fontSizeBkp;
            }
        }

        if (current > MaxHitComboCount)
            MaxHitComboCount = current;
    }
}
