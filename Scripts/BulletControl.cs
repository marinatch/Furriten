using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private float speed;

    public enum Direction { RIGHT, LEFT, UP, DOWN};
    public Direction dir;

    public void SetBullet(Direction _dir, float _speed, string _tag, int _layer)
    {
        dir = _dir;
        speed = _speed;
        gameObject.tag = _tag;
        gameObject.layer = _layer;

    }

    // Update is called once per frame
    void Update()
    {
        switch(dir)
        {
            case Direction.RIGHT:
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                break;
            case Direction.LEFT:
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                break;
            case Direction.UP:
                transform.Translate(Vector2.up * speed * Time.deltaTime);
                break;
            case Direction.DOWN:
                transform.Translate(Vector2.down * speed * Time.deltaTime);
                break;
        }
    }
}
