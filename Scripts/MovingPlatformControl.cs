using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformControl : MonoBehaviour
{
    public float speed;
    public List<Transform> waypoints;
    private int currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].position, speed * Time.deltaTime);
        if (transform.position == waypoints[currentPoint].position)
        {
            currentPoint++;
            if (currentPoint >= waypoints.Count)
            {
                currentPoint = 0;
            }
        }
    }
}
