﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float flapforce;
    private int points = 0;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.velocity = Vector2.zero;

            rb2d.AddForce(new Vector2(0, flapforce));
            anim.Play("Flap");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collumn")
        {
            Destroy(this.gameObject);
            GameObject[] pillars = GameObject.FindGameObjectsWithTag("CollumPair");
            foreach (GameObject pillar in pillars)
            {
                Destroy(pillar);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PointsArea")
        {
            GameObject textUI = GameObject.Find("Text");
            
            points += 1;
            Debug.Log(points);
            textUI.GetComponent<UnityEngine.UI.Text>().text = points.ToString();
        }
    }

    private void NeuralNetwork()
    {



    }
}
