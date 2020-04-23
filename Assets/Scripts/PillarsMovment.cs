using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarsMovment : MonoBehaviour
{
    public float movespeed;
    private GameObject gameMaster;
    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(-1 * movespeed, 0);// hur pelaren ska flyttas

        transform.Translate(movement*Time.deltaTime);//flyttningen

        if (transform.position.x < -20)//förstör pelare
        {
            Destroy(this.gameObject);
        }
        if (transform.position.x < -3.6f)// ta bort pelaren från pelar listan när den är vid fågeln
        {
            gameMaster.GetComponent<SpawnPillars>().RemoveFirst();
            
        }
    }

    
}
