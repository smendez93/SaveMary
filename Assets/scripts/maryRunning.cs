﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maryRunning : MonoBehaviour 
{

    public bool onFloor = true;
    public float width = 8.0f;
    public float center = 0.0f;
    public float height = 0.05f;
	public float speed = 0.1f;
    public bool isAlive = true;

    private float blockHeight;
	private Collider2D maryCollider;
	private Rigidbody2D maryRigidbody;
	private bool isJumping;

	// Use this for initialization
	void Start () 
	{
		blockHeight = GameObject.Find("platform").transform.localScale.y;
		maryCollider = GetComponent<BoxCollider2D>();
		maryRigidbody = GetComponent<Rigidbody2D>();
		isJumping = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// move to the right
		transform.Translate (speed, 0, 0);
        if (!isAlive)
        {
            speed = 0;
        }

		if(Input.GetKeyDown(KeyCode.Z) && !isJumping && isAlive)
		{
			isJumping = true;
			maryRigidbody.constraints = RigidbodyConstraints2D.FreezeAll ^ RigidbodyConstraints2D.FreezePositionY;	// Unfreeze Y
			maryRigidbody.AddForce(new Vector2(0.0f, 200.0f));
		}

        if (transform.position.x >= center + width / 2 && !isJumping)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        
        if (transform.position.x <= center - width / 2 && !isJumping)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "resting")
        {
			height = coll.gameObject.transform.localScale.y;
			transform.position.Set(0.0f, height, 0.0f);
            center = coll.gameObject.transform.position.x;
            width = coll.gameObject.transform.localScale.x;
        }
		else if (coll.gameObject.tag == "ground")
		{
			height = 0.05f;
			center = 0.0f;
			width = 8.0f;
		}
        else if (coll.gameObject.tag == "falling")
        {
            isAlive = false;
        }

		isJumping = false;
		maryRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        /*else if (coll.gameObject.tag == "wall")
		{
			transform.Rotate(0.0f, 180.0f, 0.0f);
		}*/
    }

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == "water")
		{
			if(other.bounds.center.y + other.bounds.extents.y > maryCollider.bounds.center.y + maryCollider.bounds.extents.y)
			{
				isAlive = false;
			}
		}
	}
}
