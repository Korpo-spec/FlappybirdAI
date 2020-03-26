using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdRotation : MonoBehaviour
{
    public Rigidbody2D rbParent;
    public float roataionSpeed;
    


 
    // Update is called once per frame
    void Update()
    {
        if (rbParent.velocity.y < 0)
        {
            if (transform.rotation.z > -0.5)
            {
                
                transform.Rotate(Vector3.back, 90 * Time.deltaTime);
                
            }
            
        }
        else if (rbParent.velocity.y > 0)
        {
            
            
            // Dampen towards the target rotation
            transform.rotation = Quaternion.Euler(0, 0, 25f);
            
        }

    }
}
