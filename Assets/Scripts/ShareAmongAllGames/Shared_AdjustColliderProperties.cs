﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Shared_AdjustColliderProperties : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		AdjustCollidersize ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// adjusts collider size to the size of the sprite.
	// works just for 2d boxcollider now, need to add for circle and other colliders as well
	public void AdjustCollidersize(){
		try{
			Vector2 S = GetComponent<SpriteRenderer>().bounds.size;
			if(GetComponent<BoxCollider2D>()){
				GetComponent<BoxCollider2D>().size = S;
				//				GetComponent<BoxCollider2D>().center = new Vector2 ((S.x / 2), 0);
			}else if(GetComponent<CircleCollider2D>()){
				GetComponent<CircleCollider2D>().radius = S.x/2;
			}
		} catch (Exception e) {
			Debug.Log (e.ToString());
		}
	}


	public void SetColliderSize(Vector2 new_size){
		if(GetComponent<BoxCollider2D>()){
			GetComponent<BoxCollider2D>().size = new_size;
			//				GetComponent<BoxCollider2D>().center = new Vector2 ((S.x / 2), 0);
		}else if(GetComponent<CircleCollider2D>()){
			GetComponent<CircleCollider2D>().radius = new_size.x/2;
		}
	}


}
