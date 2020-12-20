using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public ParticleSystem _gemParticle;

	public void ChangePlayerSkin()
	{
		for (int i = 0; i < transform.childCount; i++) 
		{
			transform.GetChild (i).gameObject.SetActive (false);
		}
		transform.GetChild (PlayerPrefs.GetInt ("CurrentChar")).gameObject.SetActive (true);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Gem"))
		{
			_gemParticle.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y, 0f);
			_gemParticle.Stop ();
			_gemParticle.Play ();
			GameController._instance.AddGem (1);
			col.gameObject.SetActive (false);
		}
	}
}
