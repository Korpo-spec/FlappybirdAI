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
        Vector2 movement = new Vector2(-1 * movespeed, 0);

        transform.Translate(movement*Time.deltaTime);

        if (transform.position.x < -20)
        {
            Destroy(this.gameObject);
        }
        //if (transform.position.x < -3.6f)
        //{
        //    gameMaster.GetComponent<SpawnPillars>().RemoveFirst();
        //    Debug.Log("trying to remove");
        //}
    }

    
}
