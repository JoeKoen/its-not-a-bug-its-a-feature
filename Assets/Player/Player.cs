﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float jumpForce = 500.0f;
    public float pushForce = 500.0f;
    public float hp = 100.0f;
    public Rigidbody rigidBody;
    public GameObject pfxPrefab;
    public GameObject xploPrefab;
    public GameplayMgr gameplayMgr;
    public CameraController cameraController;

    private MeshRenderer renderObject;
    private GameObject pfxAttached;
    private float Score = 0.0f;
	// Use this for initialization
	void Start ()
    {
        renderObject = GetComponent<MeshRenderer>();

        if (pfxPrefab)
        {
            pfxAttached = Instantiate(pfxPrefab, gameObject.transform.position, Quaternion.AngleAxis(  0.0f, Vector3.up) );
            pfxAttached.transform.localScale *= 0.5f;
            pfxAttached.transform.parent = gameObject.transform;
            pfxAttached = Instantiate(pfxPrefab, gameObject.transform.position, Quaternion.AngleAxis( 90.0f, Vector3.up));
            pfxAttached.transform.localScale *= 0.5f;
            pfxAttached.transform.parent = gameObject.transform;
            pfxAttached = Instantiate(pfxPrefab, gameObject.transform.position, Quaternion.AngleAxis(180.0f, Vector3.up));
            pfxAttached.transform.localScale *= 0.5f;
            pfxAttached.transform.parent = gameObject.transform;
            pfxAttached = Instantiate(pfxPrefab, gameObject.transform.position, Quaternion.AngleAxis(270.0f, Vector3.up));
            pfxAttached.transform.localScale *= 0.5f;
            pfxAttached.transform.parent = gameObject.transform;
        }
    }

    public void UpdateScore()
    {
        Score += -rigidBody.velocity.y;

        gameplayMgr.SetScore((int)Score);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddForce(new Vector3(0.0f, jumpForce, 0.0f));
        }

        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddForce(new Vector3(0.0f, 0.0f, pushForce));// (gameObject.transform.forward * 50.0f );
        }

        if (Input.GetKey(KeyCode.S))
        {
            rigidBody.AddForce(new Vector3(0.0f, 0.0f, -pushForce));//(gameObject.transform.forward * -50.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.AddForce(new Vector3(-pushForce, 0.0f, 0.0f));//(gameObject.transform.right * -50.0f);
            //rigidBody.AddTorque(new Vector3(0.0f, -2.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.AddForce(new Vector3(pushForce, 0.0f, 0.0f));//(gameObject.transform.right * 50.0f);
            //rigidBody.AddTorque(new Vector3(0.0f, 2.0f, 0.0f));
        }
        /*
                if( rigidBody.velocity.magnitude < 0.1f )
                {
                    if(pfxAttached.activeSelf==false)
                    pfxAttached.SetActive(true);
                }
                else
                {
                    if (pfxAttached.activeSelf == false)
                        pfxAttached.SetActive(true);
                }
        */


        UpdateScore();
    }

    private IEnumerator Flash()
    {
        renderObject.material.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        renderObject.material.color = Color.white;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        string tag = other.tag;

        if( tag == "Pain" )
        {
            StartCoroutine(Flash());
        }

        if (tag == "Mover")
        {
            gameObject.transform.parent = other.transform.parent;

            hp -= 50.0f;
            if( hp <= 0.0f )
            {
                // hit
                StartCoroutine(Flash());
            }
            else
            {
                // die
                GameObject xplogo = Instantiate(xploPrefab, gameObject.transform.position, Quaternion.identity);
                xplogo.transform.localScale *= 1.0f;

                Destroy(gameObject);
                cameraController.GameOver();
                gameplayMgr.GameOver();
            }
        }

        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
    }


    void OnCollisionExit(Collision collision)
    {
        GameObject other = collision.gameObject;
        string tag = other.tag;

        if (tag == "Mover")
        {
            gameObject.transform.parent = null;
        }

        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
    }
}
