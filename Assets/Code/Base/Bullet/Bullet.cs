using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public bool _autoDestroy = true;
    public bool _destroyOnCollision = true;
    public List<GameObject> _objectToDestroyWhenCollideWith = null;
    public List<string> _tagOfObjectToDestroyWhencollideWith = null;
    public float _lifeTime = 0;
    public float _maxDistance = 0;
    protected float _startTime = 0;
    protected Vector2 _startPosition = Vector2.zero;
    
    virtual protected void Start()
    {
        this._startTime = Time.time;
        this._startPosition = this.transform.position;
    }

    virtual protected void Update()
    {
        if (!this._autoDestroy)
        {
            return;
        }
        if (this._maxDistance > 0)
        {
            if (((Vector2)this.transform.position - this._startPosition).magnitude > this._maxDistance)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
        else if (this._lifeTime > 0)
        {
            if (Time.time - this._startTime >= this._lifeTime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    virtual protected void OnCollisionEnter2D()
    {
        if (!this._destroyOnCollision)
        {
            return;
        }
        if (this._objectToDestroyWhenCollideWith.Count == 0 && this._tagOfObjectToDestroyWhencollideWith.Count == 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    virtual protected void OnCollisionEnter2D(Collider2D collider)
    {
        if (!this._destroyOnCollision)
        {
            return;
        }
        bool onObject = this._objectToDestroyWhenCollideWith.Contains(collider.gameObject);
        bool onTag = this._tagOfObjectToDestroyWhencollideWith.Contains(collider.gameObject.tag);
        if (this._objectToDestroyWhenCollideWith.Count != 0 && this._tagOfObjectToDestroyWhencollideWith.Count != 0)
        {
            if (onObject && onTag)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
        else if (this._objectToDestroyWhenCollideWith.Count != 0 && onObject)
        {
            GameObject.Destroy(this.gameObject);
        }
        else if (this._tagOfObjectToDestroyWhencollideWith.Count != 0 && onTag)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
