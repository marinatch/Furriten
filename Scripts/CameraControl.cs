using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 finalPos = target.position;
        finalPos.x += offset.x;
        finalPos.y += offset.y;
        finalPos.z = -10;

        transform.position = Vector3.Lerp(transform.position, finalPos, 3 * Time.deltaTime);
    }
}
