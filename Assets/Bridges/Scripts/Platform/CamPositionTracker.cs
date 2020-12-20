using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositionTracker : MonoBehaviour 
{
	public Transform _gameCam;
	//public Vector3 _offset;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3 (_gameCam.position.x, 0f, 0f);
	}
}
