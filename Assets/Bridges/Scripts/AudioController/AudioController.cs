using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour 
{
	public static AudioController _instance{ set; get; }
	//
	public AudioSource aud_Score;
	//
	public AudioSource aud_Gem;
	//
	public AudioSource aud_Button;

	//
	public AudioSource aud_BridgeLanding;
	//
	public AudioSource aud_PerfectLanding;
	//
	public AudioSource aud_failed;
	//
	public AudioSource aud_Spark;
	//
	public AudioSource aud_camMove;

	//
	public AudioSource aud_BridgeCreation;
	public AudioClip[] _bridgeCreationSounds;

	void Awake()
	{
		if (_instance != null)
		{
			return;
		} 
		else
		{
			_instance = this;
		}
	}




	public void PlayGemSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_Gem.Play ();
		}
	}

	public void PlayScoreSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_Score.Play ();
		}
	}

	public void PlayButtonSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_Button.Play ();
		}
	}

	public void PlayFailedSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_failed.Play ();
		}
	}

	public void PlayBridgeLandingSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_BridgeLanding.Play ();
		}
	}

	public void PlayPerfectLandingSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_PerfectLanding.Play ();
		}
	}

	public void PlayBridgeCreationSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_BridgeCreation.clip = _bridgeCreationSounds [Random.Range (0, _bridgeCreationSounds.Length)];
			aud_BridgeCreation.Stop ();
			aud_BridgeCreation.Play ();
		}
	}

	public void PlaySparkSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_Spark.Stop ();
			aud_Spark.Play ();
		}
	}

	public void PlayCameraMoveSound()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0)
		{
			aud_camMove.Stop ();
			aud_camMove.Play ();
		}
	}
}
