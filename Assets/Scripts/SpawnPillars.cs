using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPillars : MonoBehaviour
{
    public GameObject pillars;//prefab för pillars

    public float spawnrate;

    


    public List<GameObject> pillarsList = new List<GameObject>(); //Lista över alla pillars som finns i scenen
    private List<GameObject> birdList = new List<GameObject>(); //Lista över alla levande fåglar
    


    private GameObject bestOfPastGeneration; //Den fågel som var bäst i förra generationen
    
    public GameObject bird; //bird prefab
    
    public int birdsPerGeneration;//Antalet birds som ska spawnas varje runda

    private float time = 0;

    public Button btn;//tar in knappen
    private void Start()
    {
        time = spawnrate + 1; // gör så att pillars spawnar direkt
        
        btn.onClick.AddListener(AiOnOff);//länkar knappen till metoden som ska köras

    }
    void Update()
    {

        if (birdList.Count == 0) //om birdsList är tom ta bort alla pillars och spawna in en ny bird/många om AI är på
        {
            pillarsList.Clear();
            GameObject[] pillars = GameObject.FindGameObjectsWithTag("CollumPair");// tar in alla pillars
            foreach (GameObject pillar in pillars)
            {
                Destroy(pillar);//försör alla pillars som hittades
            }
            if (isAIOn)//om AI är på spawna antalet birds som birdsPerGeneration vill
            {
                for (int i = 0; i < birdsPerGeneration; i++)
                {
                    GameObject newBird = (GameObject)Instantiate(bird);
                    birdList.Add(newBird);// lägg till birden i birdlist
                }
            }
            else
            {
                GameObject newBird = (GameObject)Instantiate(bird);
                birdList.Add(newBird);// lägg till birden i birdlist
            }
            
        }

        time += Time.deltaTime;// ökar timern
        if (time > spawnrate)//håller spawnraten
        {
            
            GameObject newGO = (GameObject)Instantiate(pillars, new Vector3(10, Random.Range(-2, 4), 1), Quaternion.identity);// spawnar en pillar med random höjd
            pillarsList.Add(newGO);//lägger till pillar i pillarsList
            
            
            time = 0;//säter timer till 0 så att spawnrate funkar
        }
        
    }

    public bool isAIOn { get; private set; } = false;// get set för isAIOn

    private void AiOnOff()
    {
        isAIOn = !isAIOn;//ändrar isOn till det motsatta av isOn alltså om den e true blir den false

        if (isAIOn)//byter färg på knappen beroende på om AI är på eller inte
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
    private float[][] biasesOfBest; //weights och biases för de bästa fåglarna

    private int bestPointOfAllGen = 0; 
    public void EvaluateBird(GameObject deadBird)
    {
        if (birdList.Count == 1 && isAIOn)// om det är den sista fågeln och AI är på
        {
            bestOfPastGeneration = deadBird; // den sista som dog blir bestOfPastGeneration
            int bestPointOfPastGeneration = bestOfPastGeneration.GetComponent<BirdMovement>().points; //hämtar dess poäng
            if (bestPointOfPastGeneration > bestPointOfAllGen) //om poängen e bättre än alla andra fåglar gör dessa fåglars biases och weights till de nya som används till arv
            {
                bestPointOfAllGen = bestPointOfPastGeneration;
                weightsOfBest = bestOfPastGeneration.GetComponent<BirdMovement>().weights;
                biasesOfBest = bestOfPastGeneration.GetComponent<BirdMovement>().biases;
            }
            
        }
        birdList.Remove(deadBird);// ta bort fågeln från listan
    }
    
    public void RemoveFirst()// metod för att försöka ta bort den första pelaren i listan
    {
        
        
        
        try
        {
            pillarsList.RemoveAt(0);
            
        }
        catch
        {

        }
        
    }
    public Transform GetTransform() // finns för att returna den närmaste pelarens transform
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

    public (float[][][] weight,float[][] bias, bool pastGenExists) GetWeightsBiases()// metod för att returera de bästa weights och biases
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
