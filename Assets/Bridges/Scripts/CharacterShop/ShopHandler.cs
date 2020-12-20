using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour 
{
	//for keeping track of selected character
	private int _charIndex;

	//for rotating the character
	private float _curAngle;
	[Header("Refrence to the characterContainer")]
	public Transform _characterContainer;

	[Header("Refrence to the buy button")]
	public Button _buyButton;

	//for keeping track of total number of gem collected
	private int _avaliableGems;
	public Text _gemText;

	[Header("Text for showing the error")]
	public Text _ErrorText;

	[System.Serializable]
	public class _characterDetail
	{
		public string _characterName;
		public int _characterID;
		public int _characterPrice;
	}
	//list for class we created above to store the multiple value of character
	public List<_characterDetail> charShop = new List<_characterDetail> ();

	//for keeping track of the name of the current character
	private string _currentCharName;

	//for keeping track of character id and the amount of gem required to buy that character
	private int _currentID,_currentPrice;

	private string _status;

	// Use this for initialization
	void Start () 
	{
		//_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		_avaliableGems = PlayerPrefs.GetInt ("TotalGems");
		_ErrorText.enabled = false;
		_charIndex = PlayerPrefs.GetInt ("CurrentChar");
		_currentCharName = charShop [_charIndex]._characterName;
		_currentID = charShop [_charIndex]._characterID;
		_currentPrice = charShop [_charIndex]._characterPrice;
		UpdateShopStatistics ();
		AdjustChildrenPos ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (_characterContainer.gameObject.activeInHierarchy)
		{
			_curAngle += 1f;
			_characterContainer.transform.GetChild (_charIndex).transform.localRotation = Quaternion.Euler (0f,
				_curAngle, 45f);
		}
	}
		
	public void GoLeft()
	{
		if (_charIndex > 0) 
		{
			_charIndex--;
		} else 
		{
			_charIndex = _characterContainer.transform.childCount - 1;
		}

		AdjustChildrenPos ();
		//
		BuySelectedCharacter(charShop [_charIndex]._characterID,charShop [_charIndex]._characterPrice,charShop [_charIndex]._characterName);
		//
		AudioController._instance.PlayButtonSound ();
	}

	public void GoRight()
	{
		if (_charIndex < _characterContainer.transform.childCount - 1) 
		{
			_charIndex++;
		}
		else
		{
			_charIndex = 0;
		}
		AdjustChildrenPos ();
		BuySelectedCharacter(charShop [_charIndex]._characterID,charShop [_charIndex]._characterPrice,charShop [_charIndex]._characterName);
		//
		AudioController._instance.PlayButtonSound ();
	}

	void AdjustChildrenPos()
	{
		for (int i = 0; i < _characterContainer.transform.childCount; i++)
		{
			_characterContainer.transform.GetChild(i).gameObject.SetActive(false);
		}
		_characterContainer.transform.GetChild(_charIndex).gameObject.SetActive(true);
	}
		
	void BuySelectedCharacter(int _ID,int _price,string _name)
	{
		_currentCharName = _name;
		_currentID = _ID;
		_currentPrice = _price;
		_buyButton.transform.GetChild (0).transform.GetChild (1).GetComponent<Text> ().text = _currentPrice.ToString ();
		UpdateShopStatistics ();
	}
		
	public void BuyThisCharacter()
	{
		if (PlayerPrefs.GetInt (_currentCharName+_currentID) != 1 && _avaliableGems >= _currentPrice)
		{
			PlayerPrefs.SetInt (_currentCharName+ _currentID, 1);
			_avaliableGems -= _currentPrice;
			PlayerPrefs.SetInt ("TotalGems", _avaliableGems);
		} 
		else if (PlayerPrefs.GetInt (_currentCharName+ _currentID) != 1 && _avaliableGems < _currentPrice) 
		{
			StartCoroutine (ShowPurchaseError ());
		}
		else if(PlayerPrefs.GetInt(_currentCharName+_currentID)==1&&PlayerPrefs.GetInt("CurrentChar")!=_currentID)
		{
			PlayerPrefs.SetInt ("CurrentChar",_currentID);
		}
		UpdateShopStatistics ();
		//
		AudioController._instance.PlayButtonSound ();
	}


	void UpdateShopStatistics()
	{
		//for making first character always purchased and selected on the shop
		if (PlayerPrefs.GetInt ("A" + 0) == 0) 
		{
			PlayerPrefs.SetInt ("A" + 0, 1);
		}
		
		if (PlayerPrefs.GetInt (_currentCharName + _charIndex) == 1)
		{
			_buyButton.transform.GetChild (0).gameObject.SetActive (false);
			_buyButton.transform.GetChild (1).gameObject.SetActive (true);
			//for purchased ones
			if (PlayerPrefs.GetInt ("CurrentChar" )== _currentID)
			{
				//selected one
				_buyButton.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text="Selected";
				_status = "Selected";
			} 
			else
			{
				//unselected one
				_buyButton.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text="Select";
				_status = "Unlocked";
			}
		}
		else 
		{
			//for unpurchased one
			_buyButton.transform.GetChild (0).gameObject.SetActive (true);
			_buyButton.transform.GetChild (1).gameObject.SetActive (false);
			_status = "Locked";
		}
		_gemText.text = PlayerPrefs.GetInt ("TotalGems").ToString ();
		//_player.ChangePlayerSkin ();


		//
		FirebaseHandler._instance.TrackingCharacter(_currentID,_status,charShop [_charIndex]._characterPrice);
	}
		
	IEnumerator ShowPurchaseError()
	{
		_ErrorText.color = Color.red;
		_ErrorText.text = "Not enought gems";
		_ErrorText.enabled = true;
		yield return new WaitForSeconds (0.5f);
		_ErrorText.enabled = false;
	}
}
