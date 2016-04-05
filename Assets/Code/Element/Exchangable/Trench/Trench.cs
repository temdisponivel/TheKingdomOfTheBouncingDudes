using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a trench that shoots objects from time to time.
    /// </summary>
    public class Trench : Exchangable
    {
        public GameObject _shootPoint = null;
        public GameObject[] _targetsEnemy = null;
        public GameObject[] _targetsPlayer = null;
        public GameObject[] _projectilesPlayer = null;
        public GameObject[] _projectilesMonsters = null;
        public float _coolDown = 0f;

        protected GameObject[] _targets = null;
        protected GameObject[] _projectiles = null;
        protected float _lastTimeShoot = 0;
        protected int _currentIndexSoldier = 0;
        protected int _currentIndexTarget = 0;

        public void Update()
        {
            if (this._projectiles != null && this._targets != null && Time.time - this._lastTimeShoot >= this._coolDown)
            {
                this._currentIndexTarget = (this._currentIndexTarget + 1) % this._targets.Length;
                this._currentIndexSoldier = (this._currentIndexSoldier + 1) % this._projectiles.Length;
                GameObject target = this._targets[this._currentIndexTarget];
                GameObject soldier = (GameObject)GameObject.Instantiate(this._projectiles[this._currentIndexSoldier], _shootPoint.transform.position, _shootPoint.transform.rotation);
                soldier.transform.rotation = Quaternion.LookRotation(Vector3.forward, (target.transform.position - this.transform.position).normalized);
                this._lastTimeShoot = Time.time;
            }
        }

        public override void Add(GameObject gameObject)
        {
            base.Add(gameObject);
            if (gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
                this._projectiles = this._projectilesPlayer;
                this._targets = this._targetsEnemy;
            }
            else
            {
                this._projectiles = this._projectilesMonsters;
                this._targets = this._targetsPlayer;
            }
        }
    }
}
