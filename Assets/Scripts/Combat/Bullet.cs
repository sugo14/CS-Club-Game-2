using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletState bulletState;


    void Start()
    {
        
    }


    void Update()
    {
        //move bullet
        transform.position += bulletState.velocity * Time.deltaTime;

        //rotate sprite to velocity direction
        float angle = Mathf.Atan2(bulletState.velocity.y, bulletState.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //apply gravity
        bulletState.velocity.y += Physics2D.gravity.y * Time.deltaTime;




    }


    public void  setup(BulletState state)
    {
        bulletState = state;
    }
}
