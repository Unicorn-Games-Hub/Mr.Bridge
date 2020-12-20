using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
	private Transform _platformHidingPoint;


	void Start()
	{
		_platformHidingPoint = GameObject.FindGameObjectWithTag ("HidingPoint").transform;
	}

	void Update()
	{
		if (GameController._instance._gameStates == GameController._states._ingame) 
		{
			if (transform.position.x < _platformHidingPoint.position.x)
			{
				ResetTheBridge ();
			}
		}
	}

	void ResetTheBridge()
	{
		GameController._instance.ReAdjustingTheBridge (this.transform);
	}
		
}
