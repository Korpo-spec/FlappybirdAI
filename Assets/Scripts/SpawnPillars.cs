using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPillars : MonoBehaviour
{
    public GameObject pillars;
    public float spawnrate;
    public static GameControl instance;
    public List<GameObject> pillarsList = new List<GameObject>();
    private List<GameObject> birdList = new List<GameObject>();
    public GameObject bird;
    public int birdsPerGeneration;

    private float time = 0;

    private void Start()
    {
        time = spawnrate + 1;
        //Instantiate(bird);
    }
    void Update()
    {

        if (birdList.Count == 0)
        {
            pillarsList.Clear();
            GameObject[] pillars = GameObject.FindGameObjectsWithTag("CollumPair");
            foreach (GameObject pillar in pillars)
            {
                Destroy(pillar);
            }

            for (int i = 0; i < birdsPerGeneration; i++)
            {
                GameObject newBird = (GameObject)Instantiate(bird);
                birdList.Add(newBird);
            }
        }

        time += Time.deltaTime;
        if (time > spawnrate)
        {
            Transform positionPillar = this.transform;
            GameObject newGO = (GameObject)Instantiate(pillars, new Vector3(15, Random.Range(-2, 4), 1), Quaternion.identity);
            pillarsList.Add(newGO);
            Debug.Log(pillarsList.Count);
            
            time = 0;
        }
    }

    public void SpawnNewBird(GameObject deadBird)
    {

        birdList.Remove(deadBird);
    }
    
    public void RemoveFirst()
    {
        
        Debug.Log(spawnrate);
        Debug.Log(pillarsList.Count);
        
        try
        {
            pillarsList.RemoveAt(0);
            
        }
        catch
        {

        }
        
    }
    public Transform GetTransform()
    {
        try
        {
            return pillarsList[0].transform;
        }
        catch 
        {

            return this.transform;
        }
        
    }
}
