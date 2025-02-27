﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalling : MonoBehaviour {


	public float maxSize = 1f;
	public float minSize = .9f;
	public float growFactor = .5f;
	public float waitTime = .1f;
	public bool shouldScale ;
	public Vector3 original_scale;
	Coroutine scaling;
	void Start()
	{
		original_scale = transform.localScale;
		//transform.localScale = new Vector3 (1, 1, 1) * maxSize;
//		minSize = .5f * maxSize;
//		growFactor = .5f * (maxSize - minSize);
		if (shouldScale && scaling == null) {
			scaling = StartCoroutine (Scale ());
		}
        Debug.Log("Should Scale = " + shouldScale);
	}


	IEnumerator Scale()
	{
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while (shouldScale)
        {
            while (maxSize > transform.localScale.x)
            {
                transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
                //Debug.Log("increasing size" + transform.localScale)  ;
            }
            while (minSize < transform.localScale.x)
            {
                transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
                //Debug.Log("decreasing size" + transform.localScale);
            }
            yield return new WaitForSeconds(waitTime);
        }
		//yield return new WaitForSeconds(waitTime);
		//if (shouldScale) {
		//	scaling = StartCoroutine (Scale ());
		//} else {
			transform.localScale = new Vector3(1,1,1) * maxSize;
		//}
	}

    IEnumerator Scale(float maxSize, float minSize)
    {
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors

        while (maxSize > transform.localScale.x)
        {
            transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            yield return null;
        }
        while (minSize < transform.localScale.x)
        {
            transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);
        if (shouldScale)
        {
            scaling = StartCoroutine(Scale(maxSize, minSize));
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1) * maxSize;
        }
    }
   
    // used in shape match game only
    public void SetScale(bool value, int GO)
    {
        float maxSize;
        if (GO <= 3)
        {
            maxSize = 5 - 0.7f * GO;
        }
        else if (GO > 3 && GO <= 5)
        {
            maxSize = 5 - 0.6f * GO;
        }
        else
        {
            maxSize = 5 - 0.5f * GO;
        }
        float minSize = maxSize - 0.5f + 0.02f * GO;
        shouldScale = value;
        if (shouldScale)
        {
            StartCoroutine(Scale(maxSize, minSize));
        }
        else if (scaling != null && (!shouldScale))
        {
            StopCoroutine(scaling);
        }
        //		transform.localScale = new Vector3(1,1,1) * maxSize;
    }


    public void SetScale(bool value){
//		StopCoroutine(Scale());

		shouldScale = value;
		if (shouldScale && scaling == null) {
            original_scale = transform.localScale;
             scaling = StartCoroutine(Scale());
        } else if(scaling != null && (!shouldScale)){
			Debug.Log ("Stopping to scale");
            StopCoroutine (scaling);
            scaling = null;
            transform.localScale = original_scale;
        }
//		transform.localScale = new Vector3(1,1,1) * maxSize;
	}

    public void SetScale(bool val, float max_size, float min_size) 
    {
        shouldScale = val;
        if (shouldScale)
        {
            scaling = StartCoroutine(Scale(maxSize, minSize));
        } else if (scaling != null && (!shouldScale))
        {
            
            StopCoroutine(scaling);
        }
    }
    public void SetScaleForLevelScreen(bool value)
    {
        float maxSize = 1.5f;

        float minSize = maxSize - 0.5f;
        shouldScale = value;
        if (shouldScale)
        {
            StartCoroutine(Scale(maxSize, minSize));
        }
        else if (scaling != null && (!shouldScale))
        {
            StopCoroutine(scaling);
        }
        //		transform.localScale = new Vector3(1,1,1) * maxSize;
    }

    public void Flip()
    {
        Debug.Log("Scalling flipped");
        SetScale(!shouldScale);
    }

    public void Flip(float Max_size, float Min_size)
    {
        SetScale(!shouldScale, Max_size, Min_size);
    }
}
