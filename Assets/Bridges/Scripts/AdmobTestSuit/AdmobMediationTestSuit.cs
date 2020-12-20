using System;
using UnityEngine;
using GoogleMobileAdsMediationTestSuite.Api;
using UnityEngine.UI;

public class AdmobMediationTestSuit : MonoBehaviour 
{
	public Text _mediationTestOutputText;

	private static string outputMessage = string.Empty;

	public static string OutputMessage
	{
		set { outputMessage = value; }
	}

	void Start()
	{
		MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;
	}

	public void ShowMediationTestSuite()
	{
		#if UNITY_ANDROID
		string _appID ="ca-app-pub-8979435940516933~4617274737";
		#elif UNITY_IPHONE
		string _appID ="ca-app-pub-8979435940516933~4964936701";
		#else
		string _appID ="unexpected_platform";
		#endif
		MediationTestSuite.Show(_appID);
		ShowMediationTestOutput ();
	}

	public void HandleMediationTestSuiteDismissed(object sender,EventArgs args)
	{
		MonoBehaviour.print ("HandleMediationTestSuiteDismissed event received");
	}

	void ShowMediationTestOutput()
	{
		_mediationTestOutputText.text = outputMessage;
	}
}
