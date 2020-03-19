using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarsMovment : MonoBehaviour
{
    public float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(-1 * movespeed, 0);

        transform.Translate(movement*Time.deltaTime);

        if (transform.position.x < -20)
        {
            Destroy(this.gameObject);
        }
    }

    
}
