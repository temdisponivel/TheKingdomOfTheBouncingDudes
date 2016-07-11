using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents the base of the player.
    /// </summary>
    public class Base : MonoBehaviour
    {
        public int _hp = 10;
		public int HP { get { return _hp; } set { LoseHP (value); } }
        public int _startedHp;

		public SpriteRenderer _baseSprite;

        public void Start()
        {
            this._startedHp = this._hp;
        }

		protected void LoseHP(int value){
			
			_hp = value;

			if (_baseSprite == null)
				return;

			Color baseGotHitColor = _baseSprite.color;
			baseGotHitColor.r = 1;
			baseGotHitColor.b = 0;
			baseGotHitColor.g = 0;

			_baseSprite.DOColor(baseGotHitColor, 0.3f).OnComplete(()=> { 
				baseGotHitColor = _baseSprite.color;
				baseGotHitColor.r = 1;
				baseGotHitColor.b = 1;
				baseGotHitColor.g = 1;
				_baseSprite.DOColor(baseGotHitColor, 0.2f);

			} );

			if (_hp <= 0) { 
				LevelManager.Instance.GameOver(); 
			} 
		}
    }
}