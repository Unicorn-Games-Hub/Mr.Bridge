using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController _instance{ set; get; }

	public float _followSpeed;
	public Vector3 _camOffset;
	public bool _canfollow=false;

	private Transform _player;

	void Awake()
	{
		if (_instance != null) {
			return;
		} else {
			_instance = this;
		}
	}

	// Use this for initialization
	void Start () 
	{
		_player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (GameController._instance._gameStates==GameController._states._ingame)
		{
			if (_canfollow) 
			{
				transform.position = Vector3.Lerp (transform.position, new Vector3 (_player.position.x,0f ,0f), Time.deltaTime * _followSpeed);
			}

		}
	}
}
