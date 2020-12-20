using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHandler : MonoBehaviour
{
	private AudioSource _aud;

	void Awake()
	{
		DontDestroyOnLoad (this);
		if(FindObjectsOfType(GetType()).Length>1)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		Application.targetFrameRate = 60;
	}

	public void ControlTheBackgroundMusic()
	{
		_aud = transform.GetChild (0).GetComponent<AudioSource> ();

		if (PlayerPrefs.GetInt ("GameSound") == 0) 
		{
			_aud.mute = false;
		} 
		else
		{
			_aud.mute = true;
		}
	}
}
