using UnityEngine;

public class StatHandler : MonoBehaviour 
{
	public static StatHandler _instance{ set; get;}

	public int _noOfGemsToSavePlayer = 20;
	public int _interstitialAdsCounter=0;
	public int _noOfGamesPlayed=0;

	public bool _tutorialShown = false;
	public bool _countSession=true;
	public bool _restartTheGame=false;

	void Awake()
	{
		if (_instance != null) {
			return;
		} else {
			_instance = this;
		}
	}

}
