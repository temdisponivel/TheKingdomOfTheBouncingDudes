using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public Vector2 _distance = Vector2.right;
    public GameObject _bullet = null;
    public float _force = 1;
    public float _cooldDown = .5f;
    protected float _lastShootTime = 0f;

    virtual protected void Update()
    {
        Vector2 position, direction;
        Quaternion rotation;
        if (this._bullet != null && this.ShouldShoot(out position, out direction, out rotation))
        {
            GameObject bullet = GameObject.Instantiate(this._bullet, position, rotation) as GameObject;
            bullet.GetComponent<Rigidbody2D>().AddForce(direction * this._force, ForceMode2D.Impulse);
            this._lastShootTime = Time.time;
        }
    }

    virtual protected bool ShouldShoot(out Vector2 position, out Vector2 direction, out Quaternion rotation)
    {
        position = this.transform.position;
        direction = position;
        rotation = this.transform.rotation;
        return Input.GetButton("Fire") && Time.time - this._lastShootTime >= this._cooldDown;
    }
}
