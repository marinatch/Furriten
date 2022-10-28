using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroideControl : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigid;
    public Transform rayPoint;
    private RaycastHit2D hitWalls;
    public LayerMask wallMask;

    public List<GameObject> fxAudios;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hitWalls = Physics2D.Raycast(rayPoint.position, Vector2.right * speed, 0.1f, wallMask);
        if(hitWalls == true)
        {
            speed *= -1;
            if(speed >0)
            {
                //mirar hacia la derecha
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                //mirar hacia la ezquierda
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        rigid.velocity = new Vector2(speed, rigid.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "KillEnemies")
        {
            GameObject newSound = Instantiate(fxAudios[0]);
            Destroy(newSound, 3);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
