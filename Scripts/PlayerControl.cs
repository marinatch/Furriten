using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public enum PlayerStates { INIT, NORMAL, TELEPORT, FINISH}
    public PlayerStates state;

    public float speed, jumpForce;
    private Rigidbody2D rigid;

    public Transform p1, p2;
    public LayerMask groundMask;
    private bool isGrounded;

    public int totalJumps;
    private int currentJumps;

    public GameObject bullet;

    public int currentWorld;

    public int lives;

    public Animator anim;

    public List<GameObject> fxAudios;

    //private bool hasTeleport;

    public int score;
    public float time;

    //private bool LevelComplete;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        string keySave = "World" + currentWorld;
        PlayerPrefs.SetInt(keySave, 1);

        //LevelComplete = false;
    }

    private void ShootSystem()
    {
        GameObject newSound = Instantiate(fxAudios[0]);
        Destroy(newSound, 3);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletControl>().SetBullet(BulletControl.Direction.RIGHT, 15, "KillEnemies", 10);
            Destroy(newBullet, 5);
        }
    }

    private void Movment()
    {
        isGrounded = Physics2D.OverlapArea(p1.position, p2.position, groundMask);

        anim.SetBool("grounded", isGrounded);
        anim.SetBool("moving", Input.GetAxis("Horizontal") != 0);

        rigid.velocity = new Vector2(/*Input.GetAxis("Horizontal") **/ speed, rigid.velocity.y);
        if (isGrounded == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                GameObject newSound = Instantiate(fxAudios[1]);
                Destroy(newSound, 3);

                currentJumps = 0;
                rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else //doble salto
        {
            if (Input.GetButtonDown("Jump") && currentJumps < totalJumps)
            {
                GameObject newSound = Instantiate(fxAudios[1]);
                Destroy(newSound, 3);

                currentJumps++;
                //resetar la vilocidad en i, para evitar el problema que ocuure cuando la velocidad negativa al momento de cayer es mayor de la velocidad del salto. En resultado no salta el Player.
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

        }
    }

    /*private void ActiveTeleport()
    {
        if(Input.GetKeyDown(KeyCode.T) && hasTeleport == true)
        {
            hasTeleport = false;
            state = PlayerStates.TELEPORT;
            //para parar el tiempo
            Time.timeScale = 0;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerStates.INIT:
                anim.SetBool("grounded", true);
                if(Input.GetButtonDown("Jump"))
                {
                    state = PlayerStates.NORMAL;
                }
                break;
            case PlayerStates.NORMAL:
                time += Time.deltaTime;
                ShootSystem();
                Movment();
               // ActiveTeleport();
                break;
            case PlayerStates.TELEPORT:
                //pon el jugador donde pulso con el mouse
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    transform.position = mousePos;
                    Time.timeScale = 1;
                    state = PlayerStates.NORMAL;
                }
                break;
            case PlayerStates.FINISH:
                rigid.velocity *= 0;
                anim.SetBool("moving", false);
                anim.SetBool("grounded", true);
                break;
        }

        
        
    }

    private void DeadPlayer()
    {
        Time.timeScale = 0;
       GetComponent<ScoreControl>().SetScore(score, currentWorld);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "DeadPlayer")
        {
            DeadPlayer();
        }
        if(other.tag == "KillPlayer")
        {
            lives--;
            if(lives <= 0)
            {
                DeadPlayer();

            }

        }
        /*if(other.tag == "Teleport")
        {
            Destroy(other.gameObject);
            hasTeleport = true;
        }*/
        if (other.tag == "Coin")
        {
            score += other.GetComponent<CoinScript>().value;
            Destroy(other.gameObject);
        }
        if (other.tag == "Finish")
        {
            //LevelComplete = true;
            state = PlayerStates.FINISH;
            GetComponent<ScoreControl>().SetScore(score, (int)time);
            
            /*PlayerPrefs.SetInt("World" + (currentWorld + 1), 1);
            DeadPlayer();*/
        }
    }

}
