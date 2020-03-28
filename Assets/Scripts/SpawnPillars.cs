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
    private List<GameObject> deadBirds = new List<GameObject>();
    private GameObject bestOfPastGeneration;
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
        Debug.Log(birdList.Count);
    }


    private float[][][] weightsOfBest;
    private float[][] biasesOfBest;
    public void SpawnNewBird(GameObject deadBird)
    {
        if (birdList.Count == 1)
        {
            bestOfPastGeneration = deadBird;
            weightsOfBest = bestOfPastGeneration.GetComponent<BirdMovement>().weights;
            biasesOfBest = bestOfPastGeneration.GetComponent<BirdMovement>().biases;
            Debug.Log("best of gen");
        }
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

    public (float[][][] weight,float[][] bias, bool pastGenExists) GetWeightsBiases()
    {
        if (weightsOfBest == null)
        {
            
            return (null, null , false);
            
        }
        else
        {
            return (weightsOfBest, biasesOfBest, true);
        }
       
    }
}
