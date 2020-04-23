using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdRotation : MonoBehaviour
{
    public Rigidbody2D rbParent;
    public float roataionSpeed;
    


 
    // Update is called once per frame
    void Update()//rotera fågeln beroende på velocity
    {
        if (rbParent.velocity.y < 0)// om fågeln är påväg nedåt vinkla nedåt
        {
            if (transform.rotation.z > -0.5)//stoppar rotationen så att fåglarna inte snörrar runt
            {
                
                transform.Rotate(Vector3.back, 90 * Time.deltaTime);
                
            }
            
        }
        else if (rbParent.velocity.y > 0)//vinklar upp fågeln när man har en positiv valocity
        {
            
            
            
            transform.rotation = Quaternion.Euler(0, 0, 25f);//sätter rotationen till en bra flap rotation
            
        }

    }
}
