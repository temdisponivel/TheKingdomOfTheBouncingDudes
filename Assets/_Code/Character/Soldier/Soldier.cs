using System;
using System.Linq;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {
		public string _soldierName = "Vicenzito";

        protected int _ammunitionPosition = -1;
        
        public int AmmunitionPosition { get { return this._ammunitionPosition; } set { this._ammunitionPosition = value; } }

        private bool _shooted = false;
        
        public override void Start()
        {
            base.Start();

            if (!_shooted && !_isSpecial)
                this.TurnIntoAmmunition();
        }

        public override void Shoot()
        {
            if (!this._isSpecial)
                this.transform.rotation = Weapon.Instance.WeaponRotation;

            _transitioning = false;

            this.ConvertSpeed();

            this._shooted = true;

            this.TurnIntoProjectile();

            this.RigidBody.AddForce(this.transform.up * this._maxSpeed * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            string collTag = collision.gameObject.tag;

            if (collTag == TagAndLayer.BASE)
                return;

            if (collTag == TagAndLayer.ENEMY_BASE)
            {
                EffectManager.Instance.CreateDieEffect(this.transform);
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.HP -= 1;
            }
            else if (collTag == TagAndLayer.WALL || collTag == TagAndLayer.BASE)
            {
                EffectManager.Instance.CreateWallHitEffect(this.transform);
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
                if (this._onBarrel || _transitioning)
                    return;

                Character monster = collider.gameObject.GetComponent<Character>();
                this.HP -= 1;
                monster.HP -= 1;

                EffectManager.Instance.CreateHitEffect(monster.transform);

                if (monster.HP <= 0)
                {
                    EffectManager.Instance.CreateDieEffect(monster.transform);
                    EffectManager.Instance.CreateSmokeEffect(monster.transform);
                    LevelManager.Instance.KillEnemy(monster);
                    ComboManager.Instance.AddKill();
                    ComboManager.Instance.AddElementKill();
                }
                else
                {
                    ComboManager.Instance.AddHit();
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!_onBarrel)
                return;

            this.transform.position = Weapon.Instance._idlePoint.transform.position;
            this.transform.rotation = Weapon.Instance.WeaponRotation;
        }

        public void OnBarrel()
        {
            this.CurrentSortingOrder = this._spriteOnBarrelOrder;
            this._onBarrel = true;
        }

        public override void Recycle()
        {
            Debug.Log("RECYCLE");
            this._shooted = false;
            this._transitioning = true;
            this.Start();
            this._hp = _hpBkp;
            this.transform.rotation = _rotationBkp;
            this.transform.localScale = _scaleBkp;
            AmmunitionClip.Instance.AddAmmunition(this.gameObject, true);
        }
    }
}