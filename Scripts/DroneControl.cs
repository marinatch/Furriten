using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    public float speed;
    public List<Transform> waypoints;
    private int currentPoint;
    public GameObject bullet;
    private float fireRate;
    public float countRate;

    public int lives;

    public List<GameObject> fxAudios;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireRate += Time.deltaTime;
        if(fireRate > countRate)
        {
            
            GameObject newSound = Instantiate(fxAudios[0]);
            Destroy(newSound, 3);

            fireRate = 0;
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
            newBullet.GetComponent<BulletControl>().SetBullet(BulletControl.Direction.DOWN, 8, "KillPlayer", 10);
            Destroy(newBullet, 5);
        }

       
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].position, speed * Time.deltaTime);
        if(transform.position == waypoints[currentPoint].position)
        {
            currentPoint++;
            if(currentPoint >= waypoints.Count)
            {
                currentPoint = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "KillEnemies")
        {
            Destroy(other.gameObject);
            lives--;
            if(lives <= 0)
            {
                GameObject newSound = Instantiate(fxAudios[1]);
                Destroy(newSound, 3);
                //hay que destuir todos los componentes del Drone, no solo el drone. Por eso se destuye el Padre
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
