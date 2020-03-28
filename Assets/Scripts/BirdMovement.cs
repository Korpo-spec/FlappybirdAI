using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float flapforce;
    private int points = 0;
    public Animator anim;
    public GameObject pillarListGameObject;
    
    private float deltaX;
    private float deltaY;
    private SpawnPillars sp;
    
    
    // Start is called before the first frame update
    void Start()
    {
        pillarListGameObject = GameObject.Find("GameMaster");
        sp = pillarListGameObject.GetComponent<SpawnPillars>();
        rb2d = GetComponent<Rigidbody2D>();
        int[] layers = { 3, 2, 2 };
        NeuralNetwork(layers);
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform pillarTrans = sp.GetTransform();
        deltaX = pillarTrans.position.x - transform.position.x;

        deltaY = pillarTrans.position.y - transform.position.y;

        //Debug.Log(deltaX + "deltaX");
        //Debug.Log(deltaY + "deltaY");
        float[] inputsss = { deltaX, deltaY, rb2d.velocity.y};
        float[] output = FeedForward(inputsss);
        //Debug.Log(output[0]);
        //Debug.Log(output[1]);

        if (output[0] > output[1])
        {
            Flap();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.velocity = Vector2.zero;

            rb2d.AddForce(new Vector2(0, flapforce));
            anim.Play("Flap");
        }
        
    }

    private void Flap()
    {
        rb2d.velocity = Vector2.zero;

        rb2d.AddForce(new Vector2(0, flapforce));
        anim.Play("Flap");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Collumn")
        {
            Destroy(this.gameObject);

            points = 0;
            sp.SpawnNewBird(this.gameObject);
        }
        if (collision.gameObject.tag == "PointsArea")
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
    private int[] activations;//layers
    public void NeuralNetwork(int[] layers)
    {

        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();
        var result = sp.GetWeightsBiases();
        if (result.pastGenExists)
        {
            biases = result.bias;
            weights = result.weight;
            Debug.Log("setting bias");
        }
        

    }

    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
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
        return (float)Math.Tanh(value);
    }

    public float[] FeedForward(float[] inputs)
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
