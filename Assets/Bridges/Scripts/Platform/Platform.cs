using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour 
{
	private Transform _hidingPoint;
	private float _animationSpeed=8f;

	[Header("controlled by GameController")]
	private float _targetYpos=-10f;
	public bool _animateIt = false;



	// Use this for initialization
	void Start ()
	{
		_hidingPoint = GameObject.FindGameObjectWithTag ("HidingPoint").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameController._instance._gameStates == GameController._states._ingame) 
		{
			if (_hidingPoint.position.x > transform.position.x) 
			{
				ResetThisPlatform ();
			}

			//
			if (_animateIt) 
			{
				if (transform.position.y < _targetYpos) 
				{
					transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, _targetYpos, 0f), Time.deltaTime * _animationSpeed);
				}
				else 
				{
					_animateIt = false;
				}
			}
		}
	}

	void ResetThisPlatform()
	{
		GameController._instance.PlatformNewPos (this.transform);
	}
}
