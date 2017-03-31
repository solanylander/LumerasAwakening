using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterAnimation : MonoBehaviour {

	public float topHeight;
	public float bottomHeight;
	public float destination;
	public float floatSpeed;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Mathf.Approximately (transform.localPosition.y, topHeight)) 
		{
			destination = bottomHeight;

		} else if (Mathf.Approximately (transform.localPosition.y, bottomHeight)) 
		{
			destination = topHeight;

		} 
			
		move (destination);
		
			
	}

	void move (float sendTo)
	{
		transform.localPosition= new Vector2 (transform.localPosition.x, Mathf.Lerp (transform.localPosition.y, sendTo, floatSpeed));
	}
}
