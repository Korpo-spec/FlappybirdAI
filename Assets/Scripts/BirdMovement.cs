using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BirdMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float flapforce;
    public int points = 0;
    public Animator anim;//animator objektet
    public GameObject pillarListGameObject;
    
    private float deltaX;
    private float deltaY;
    private SpawnPillars spawnPillarsScript;

    
    // Start is called before the first frame update
    void Start()
    {
        pillarListGameObject = GameObject.Find("GameMaster");//hämta in GameMaster objektet
        spawnPillarsScript = pillarListGameObject.GetComponent<SpawnPillars>();//get spawnpillars scriptet
        rb2d = GetComponent<Rigidbody2D>();// hämta ridgidbody 2D på fågeln
        int[] layers = { 2, 2, 2 };//Hur många lager med hur många neutroner som finns i nätverket
        NeuralNetwork(layers);//Set up för neural network

        
    }

    // Update is called once per frame
    void Update()
    {

        if (spawnPillarsScript.isAIOn)//kollar om AI är på
        {
            Transform pillarTrans = spawnPillarsScript.GetTransform();//hämta transform från den första/närmsta pelaren
            deltaX = pillarTrans.position.x - transform.position.x;// tar fram deltaX
            deltaY = pillarTrans.position.y - transform.position.y;// tar fram deltaY

            float[] inputs = { deltaX, deltaY };// skapar inputs och inputar till "hjärnan"
            float[] output = ActivateNeurons(inputs);// activerar hjärnan


            if (output[0] > output[1])// kollar om vi ska flap eller inte
            {
                Flap();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))// om Ai är av kolla om man klickar space
            {
                Flap();
            }
        }
        
        
        
    }

    
    
    private void Flap()
    {
        rb2d.velocity = Vector2.zero;// sätt velocity till 0

        rb2d.AddForce(new Vector2(0, flapforce));// lägg till en force upp (flapforce)
        anim.Play("Flap");//spela flap animationen
    }

    private void OnTriggerEnter2D(Collider2D collision)// när fågeln colliderar
    {

        if (collision.gameObject.tag == "Collumn")// om det är en collumn ta bort objectet och evaluate bird för att se vilken som är bäst
        {
            spawnPillarsScript.EvaluateBird(this.gameObject);
            points = 0;
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "PointsArea")// om det är points area som den colliderar med så ändras points texten
        {
            GameObject textUI = GameObject.Find("Text");
            
            points += 1;
            
            textUI.GetComponent<UnityEngine.UI.Text>().text = points.ToString();
            
        }
    }


    private int[] layers;//layers    
    private float[][] neurons;//neurons    
    public float[][] biases;//biasses    
    public float[][][] weights;//weights  
    
    public int chanceOfMutation;
    public float mutationValue;
    
    public void NeuralNetwork(int[] layers)//Initierar neuralnetwork
    {

        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)//skapar antalet lager
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();
        var result = spawnPillarsScript.GetWeightsBiases();
        if (result.pastGenExists)
        {
            for (int x = 0; x < biases.Length; x++)
            {
                for (int y = 0; y < biases[x].Length; y++)
                {
                    biases[x][y] = result.bias[x][y];
                }
            }
            //biases = result.bias;
            for (int x = 0; x < weights.Length; x++)
            {
                for (int y = 0; y < weights[x].Length; y++)
                {
                    for (int z = 0; z < weights[x][y].Length; z++)
                    {
                        weights[x][y][z] = result.weight[x][y][z];
                    }
                }
            }
            //weights = result.weight;
            
            Mutate();
        }
        

    }
    bool firstTime = true;
    private void Mutate()
    {

        for (int x = 0; x < biases.Length; x++)
        {
            for (int y = 0; y < biases[x].Length; y++)
            {
                if (UnityEngine.Random.Range(0f, 100f) <= chanceOfMutation)//kollar om den biasen ska muteras
                {
                    biases[x][y] += UnityEngine.Random.Range(-mutationValue, mutationValue);//randomizar med hur mycket den muterrar
                }

            }
        }

        for (int x = 0; x < weights.Length; x++)
        {
            for (int y = 0; y < weights[x].Length; y++)
            {
                for (int z = 0; z < weights[x][y].Length; z++)
                {
                    if (UnityEngine.Random.Range(0f, 100f) <= chanceOfMutation)//gör samma som innan fast med weights
                    {
                        float randomNumber = UnityEngine.Random.Range(-mutationValue, mutationValue);
                        
                        weights[x][y][z] += randomNumber;
                        
                        

                    }
                   
                }
            }
        }

        


    }

    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();//skapa en neutron lista
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);//lägger till antalet neuroner som ska finnas i lagret neuron 
        }
        neurons = neuronsList.ToArray();
    }

    private void InitBiases()
    {
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            float[] bias = new float[layers[i]];
            for (int j = 0; j < layers[i]; j++)
            {
                bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
            }
            biasList.Add(bias);
        }
        biases = biasList.ToArray();
    }

    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    public float activate(float value)
    {
        return (float)Math.Tanh(value);//En matematisk formel som jag inte kommer ihåg
    }

    public float[] ActivateNeurons(float[] inputs)//hjärnan
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }
        for (int i = 1; i < layers.Length; i++)
        {
            int layer = i - 1;
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = activate(value + biases[i][j]);
            }
        }
        return neurons[neurons.Length - 1];
    }
}
