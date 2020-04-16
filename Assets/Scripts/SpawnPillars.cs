using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPillars : MonoBehaviour
{
    public GameObject pillars;
    public float spawnrate;
    public static GameControl instance;


    public List<GameObject> pillarsList = new List<GameObject>();
    private List<GameObject> birdList = new List<GameObject>();
    


    private GameObject bestOfPastGeneration;
    public GameObject bird;
    public int birdsPerGeneration;

    private float time = 0;

    public Button btn;
    private void Start()
    {
        time = spawnrate + 1;
        
        btn.onClick.AddListener(AiOnOff);

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
            if (isAIOn)
            {
                for (int i = 0; i < birdsPerGeneration; i++)
                {
                    GameObject newBird = (GameObject)Instantiate(bird);
                    birdList.Add(newBird);
                }
            }
            else
            {
                GameObject newBird = (GameObject)Instantiate(bird);
                birdList.Add(newBird);
            }
            
        }

        time += Time.deltaTime;
        if (time > spawnrate)
        {
            Transform positionPillar = this.transform;
            GameObject newGO = (GameObject)Instantiate(pillars, new Vector3(10, Random.Range(-2, 4), 1), Quaternion.identity);
            pillarsList.Add(newGO);
            Debug.Log(pillarsList.Count);
            
            time = 0;
        }
        
    }
    private bool isOn = false;
    public bool isAIOn {
        get
        {
            return isOn;
        }
    }
    
    private void AiOnOff()
    {
        isOn = !isOn;//ändrar isOn till det motsatta av isOn alltså om den e true blir den false

        if (isOn)//byter färg på knappen beroende på om AI är på eller inte
        {
            var colors = btn.colors;
            colors.pressedColor = Color.green;
            colors.normalColor = Color.green;
            colors.selectedColor = Color.green;
            btn.colors = colors;
        }
        else
        {
            var colors = btn.colors;
            colors.pressedColor = Color.red;
            colors.normalColor = Color.red;
            colors.selectedColor = Color.red;
            btn.colors = colors;
        }
        

        
      
    }


    private float[][][] weightsOfBest;
    private float[][] biasesOfBest;
    private int bestPointOfAllGen = 0;
    public void SpawnNewBird(GameObject deadBird)
    {
        if (birdList.Count == 1)
        {
            bestOfPastGeneration = deadBird;
            int bestPointOfPastGeneration = bestOfPastGeneration.GetComponent<BirdMovement>().points;
            if (bestPointOfPastGeneration > bestPointOfAllGen)
            {
                bestPointOfAllGen = bestPointOfPastGeneration;
                weightsOfBest = bestOfPastGeneration.GetComponent<BirdMovement>().weights;
                biasesOfBest = bestOfPastGeneration.GetComponent<BirdMovement>().biases;
            }
            
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
