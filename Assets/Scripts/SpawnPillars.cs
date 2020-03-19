using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPillars : MonoBehaviour
{
    public GameObject pillars;
    public float spawnrate;
    public static GameControl instance;

    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > spawnrate)
        {
            Transform positionPillar = this.transform;
            Instantiate(pillars, new Vector3(15, Random.Range(-3, 3), 1), Quaternion.identity);
            time = 0;
        }
    }
}
