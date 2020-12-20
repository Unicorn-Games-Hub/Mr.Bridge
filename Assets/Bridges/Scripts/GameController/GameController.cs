using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
	//creating the instance of gamecontroller so that it can be called easily from other classes
	public static GameController _instance{ set; get; }

	//"Game state handler"
	public enum _states
	{
		_mainMenu,
		_ingame,
		_gameOver,
		_noTutorial
	}
	public  _states _gameStates;

	[Header("Game Scene Name")]
	public string _sceneName;

	[Header("Refrence to cameras")]
	public GameObject _menuCam;
	public GameObject _gameCam;

	[Header("Refrence to character shop")]
	public GameObject _characterShop;

	[Header("Refrences to gameUI")]
	public GameObject _mainMenuUI;
	public GameObject _ingameUI;
	public GameObject _gemInfoContainer;
	public GameObject _gameOverUI;

	[Header("Refrences Menu texts")]
	public Text _menuScoreText;
	public Text _menuBestScoreText;
	public Text _menuNewBestIndicator;

	[Header("Refrences inGame texts")]
	public Text _ingameScoreText;
	public Text _ingameGemText;
	public GameObject _ingameBestIndicator;
	private bool _newBestShown=false;

	[Header("Refrences to gameOver texts")]
	public Text _gameOverScoreText;
	public Text _gameOverBestScoreText;
	public Text _gameOverGemCounterText;
	public Text _gameOverNewBestIndicator;

	[Header("Gem and Score tracker")]
	private int _scoreCounter;
	private int _bestScoreCounter;
	private int _gemCounter;
	private bool _isPerfect=false;

	[Header("For Scrolling MenuUI")]
	private float _targetXpos;
	private float _camTargetPos;
	public Transform _mainMenuContainer;
	public float _swipeSpeed=5f;
	public float _homeXpos=0f;
	public float _shopXpos = -800f;

	[Header("For Save Menu Logic")]
	private float _timer=1f;
	private float _countDownTime=3f;
	private float _countDownTimer;
	public float _speedOfSlider=5f;
	public Text _countDownText;
	public Image _sliderImage;

	[Header("Audio Setting")]
	public Sprite[] _audioSprite;
	public Image[] _audioButtonImage;

	[Header("For custom ads of own games")]
	private int _messageSelector;
	private int _gameSelector;
	public string[] _message;
	public Text _displayText;
	public Text _gameName;
	public Image _gameLogoImage;

	private string _androidUrl;
	private string _iosUrl;

	[System.Serializable]
	public class customGameInfo
	{
		public string _gameName;
		public Sprite _gameIcon;
		public string _playstoreLink;
		public string _appstoreLink;
	}
	public List<customGameInfo> _customAds = new List<customGameInfo> ();

	//refrence to the player
	private Transform  _player;
	public bool _movePlayer = false;
	public float _playerSpeed=5f;

	[Header("Platform and gameplay logic")]
	public Transform _platformContainer;
	public float _initialPlatformYpos = -40f;
	public float _platformYpos = -10f;
	public float _minXpos;
	public float _maxXpos;
	private float _Xpos;
	//
	public float _minXsize;
	public float _maxXsize;
	private float _Xsize;
	//
	private int _platformSelector=1;
	//private int _bridgeSelector;
	private int _platformForBridge=0;
	private Vector2  _bridgeChildSize;
	private float _newchildPos;
	private int _childSelector=0;
	[Range(1f,10f)]
	public float _bridgeGrowTime=5f;
	public float _waitTimeToGrow=0.1f;
	float _bridgeTimer;
	//public bool _timeToRotateTheBridge=false;
	public float _bridgeRotationSpeed=10f;
	private float _bridgeRotationTime;
	//for 180 rotation starting from 90
	private float _newRotationTimer=1f;
	private float _timerValue=0.02f;
	public bool _createNewBridge = true;
	private bool _growBridge=false;
	private bool _IsnewCycleReady = false;
	private Transform _lastPlatform;
	private Transform _generationPoint;
	private bool _playerIsSaved=false;

	enum _ingameStates
	{
		_waitForInput,
		_createBridge,
		_rotateBridge,
		_moveToCenter,
		_moveToEnd,
		_startNewCycle,
		_rotateBridgeForGameOver
	}
	private _ingameStates _newStates;

	[Header("Game camera adjustment")]
	public Vector3 _camOffset;
	public bool _moveCamera=false;

	//for screen fader
	private GameObject _fader;

	[Header("Consecutive perfect score")]
	private int _consecutivePerfectScoreCounter=0;
	private int _perfectDisplayTextSelector;
	public Text _perfectScoreDisplayText;
	private string _tempString;
	public string[] _encouragingTexts;

	[Header("For color and effects")]
	//for bridge
	private Material[] _matArray;
	public Material[] _bridgeMat;

	//for platform
	private Material[] _platformMatArray;
	public Material[] _platformMat;

	//for center
	private Material[] _centerMatArray;
	public Material[] _centerMat;

	[Header("Random color generation")]
	//for random bridge color
	private float _red, _green, _blue;
	private float _bridgeMinColorValue=0f;
	private float _bridgeMaxColorValue=255f;
	private float _alpha=255f;
	private Color _bridgeCol;
	private int _glowMatSelector=0;
	public Material[] _glowMaterials;

	//particle effect for bridge landing
	private ParticleSystem.MainModule _module;
	private ParticleSystem.MainModule _module1;
	public ParticleSystem _landingParticle;

	[Header("Ads handeler")]
	public int _deathNoToShowInterstitialAds=5;

	[Header("Tutorial")]
	public GameObject _tutorialUI;
	public Sprite[] _handGesture;
	public Image _gestureIndicator;
	private bool _showTutorial=false;
	private bool _isTutorialInputReady=false;
	private float _currentBridgeHeight;
	private float _distanceBetweenTwoPlatforms;

	//for playing the bridge creation audio
	private float _soundTimer=0f;
	//
	public bool _deleteGameStat=false;

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

	// Use this for initialization
	void Start ()
	{
		//disable the bool while building
		if (_deleteGameStat) 
		{
			PlayerPrefs.DeleteAll ();
		}
		_gameStates = _states._mainMenu;
		_menuCam.SetActive (true);
		_gameCam.SetActive (false);
		_characterShop.SetActive (false);
		_mainMenuUI.SetActive (true);
		_ingameUI.SetActive (false);
		_gameOverUI.SetActive (false);
		_player = GameObject.FindGameObjectWithTag ("Player").transform;
		_generationPoint = GameObject.FindGameObjectWithTag ("GenerationPoint").transform;
		_fader=GameObject.FindGameObjectWithTag("Fader");
		_bridgeChildSize=_platformContainer.GetChild(0).GetChild(4).GetChild(0).localScale;
		_player.GetComponent<Rigidbody> ().isKinematic = true;
		UpdateMenuUIContents ();

		if (StatHandler._instance._restartTheGame)
		{
			AdjustmentsForNewGame ();
			StatHandler._instance._restartTheGame = false;
		}

		//for counting the sessions only once on every game open
		if (StatHandler._instance._countSession) 
		{
			PlayerPrefs.SetInt ("TotalSession", PlayerPrefs.GetInt ("TotalSession") + 1);
			FirebaseHandler._instance.TrackingGameSession (PlayerPrefs.GetInt ("TotalSession"));
			StatHandler._instance._countSession = false;
		}
	}

	void UpdateMenuUIContents()
	{
		_menuScoreText.text = "Last Score : "+ PlayerPrefs.GetInt ("LastScore").ToString ();
		_menuBestScoreText.text = "Best Score : "+PlayerPrefs.GetInt ("HighScore").ToString ();
		_gameOverNewBestIndicator.gameObject.SetActive(false);
		_ingameBestIndicator.SetActive (false);
		_tutorialUI.SetActive (false);

		if (PlayerPrefs.GetInt ("HighScore") != 0) {
			if (PlayerPrefs.GetInt ("LastScore") == PlayerPrefs.GetInt ("HighScore")) {
				_menuNewBestIndicator.gameObject.SetActive (true);
			} else {
				_menuNewBestIndicator.gameObject.SetActive (false);
			}
		}
		//adjusting the audio sprite
		ChangeTheAudioSprite ();
		_fader.GetComponent<Animator> ().SetTrigger("initialFade");
	}
		
	// Update is called once per frame
	void Update () 
	{
		//for main menu
		if (_gameStates == _states._mainMenu)
		{
			if(Mathf.Round (_mainMenuContainer.GetComponent<RectTransform> ().anchoredPosition.x)!=_targetXpos)
			{
				_mainMenuContainer.GetComponent<RectTransform> ().anchoredPosition = Vector3.Lerp (_mainMenuContainer.GetComponent<RectTransform> ().anchoredPosition,
					new Vector3 (_targetXpos, 0f, 0f), Time.deltaTime * _swipeSpeed);
				_menuCam.transform.position = Vector3.Lerp (_menuCam.transform.position,
					new Vector3 (_camTargetPos, 0f, -10f), Time.deltaTime * _swipeSpeed);
			}
		}

		if (_gameStates == _states._ingame) {
			if (_createNewBridge) {
				if (_newStates == _ingameStates._waitForInput) {
					if (!_growBridge && Input.GetMouseButtonDown (0)) {
						_growBridge = true;
					}

					if (_growBridge && Input.GetMouseButton (0) && !_showTutorial) {
						_newStates = _ingameStates._createBridge;
					}
				}
			}

			if (_newStates == _ingameStates._createBridge) {
				ManageBridgeChildPosition ();
				if (Input.GetMouseButtonUp (0) && !_showTutorial) {
					_newStates = _ingameStates._rotateBridge;
				}
				//lets play audio from here
				_soundTimer += Time.deltaTime;
				if (_soundTimer >= 0.05f) {
					AudioController._instance.PlayBridgeCreationSound ();
					_soundTimer = 0f;
				}
			}


			if (_newStates == _ingameStates._rotateBridge) 
			{
				Vector3 _curBridge = _platformContainer.GetChild(_platformForBridge).GetChild(4).transform.eulerAngles;
				_bridgeRotationTime += _timerValue * _bridgeRotationSpeed;
					
				if (_bridgeRotationTime > 1f) {
					_bridgeRotationTime = 1f;
				}
				_curBridge.z = _bridgeRotationTime * -90f;
				_platformContainer.GetChild(_platformForBridge).GetChild(4).transform.eulerAngles = _curBridge;
					
				if (_bridgeRotationTime == 1f) {
					_bridgeRotationTime = 0f;
					//checking perfect landing or not
					Bounds _perfectBound = _platformContainer.GetChild (_platformSelector).GetChild (0).GetChild (1).GetComponent<BoxCollider> ().bounds;

					if (_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.position.x > _perfectBound.min.x &&
						_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.position.x < _perfectBound.max.x) {
						//
						_consecutivePerfectScoreCounter++;

						if (_consecutivePerfectScoreCounter > 1) {
							ConsecutiveScoreController ();
						} 
						//
						StartColoringThePlatform (_platformContainer.GetChild (_platformSelector).gameObject);
						//
						_isPerfect = true;
						//add perfect landing score and play the perfect audioClip from here
						AddScore (2);
						//Playing perfect score sound
						AudioController._instance.PlayPerfectLandingSound ();
						//move player to center of the platform
						_newStates = _ingameStates._moveToCenter;
					} else {
						_isPerfect = false;
						_consecutivePerfectScoreCounter = 0;
					}
					//for checking the normal landing
					Bounds _normalBound = _platformContainer.GetChild (_platformSelector).GetChild (0).GetChild (0).GetComponent<BoxCollider> ().bounds;

					if (_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.position.x > _normalBound.min.x &&
						_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.position.x < _normalBound.max.x) {
						if (!_isPerfect) {
							AddScore (1);
							AudioController._instance.PlayScoreSound ();
						}
						//
						PlayLandingParticleEffect ();
						_newStates = _ingameStates._moveToCenter;
					}
					if (_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.position.x < _normalBound.min.x ||
						_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.position.x > _normalBound.max.x) {
						_newStates = _ingameStates._moveToEnd;
					}
					_player.transform.GetComponent<Rigidbody> ().isKinematic = false;
					_movePlayer = true;
				}
			}


			//for moving the player
			//moving player to the center of the platform
			//0 for cener and 1 for end
			if (_newStates == _ingameStates._moveToCenter) {
				MoveThePlayer (_platformContainer.GetChild (_platformSelector).transform, 0);
			}
			//moving player to the end of the bridge
			if (_newStates == _ingameStates._moveToEnd) {
				MoveThePlayer (_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform, 1);
			}
			//for rotating the bridge for gameOver
			if (_newStates == _ingameStates._rotateBridgeForGameOver) {
				Vector3 _bridgeRotaion = _platformContainer.GetChild(_platformForBridge).GetChild(4).transform.eulerAngles;
				_newRotationTimer += _timerValue * _bridgeRotationSpeed;
				if (_newRotationTimer > 2f) {
					_newRotationTimer = 2f;
				}
				_bridgeRotaion.z = _newRotationTimer * -90f;
				_platformContainer.GetChild(_platformForBridge).GetChild(4).transform.eulerAngles = _bridgeRotaion;
				if (_newRotationTimer == 2) {
					_newRotationTimer = 1f;
					StartCoroutine (DelayTheGameOver ());
					_gameStates = _states._gameOver;
				}
			}
				
			if (_isTutorialInputReady) 
			{
				_newStates = _ingameStates._createBridge;
			}

		} 


		//for new cycle
		if (_newStates == _ingameStates._startNewCycle) 
		{
			if (_IsnewCycleReady) 
			{
				if (_platformForBridge < _platformContainer.childCount - 1) 
				{
					_platformForBridge++;
				}
				else
				{
					_platformForBridge=0;
				}
				_childSelector = 0;
				_newchildPos = 0f;
				ManageTheBridgePosition ();
				_newStates = _ingameStates._waitForInput;
				_IsnewCycleReady = false;
			}
		}
			
		if (_gameStates == _states._gameOver) 
		{
				if (_gameOverUI.transform.GetChild (0).gameObject.activeInHierarchy) {
					if (_timer > 0) {
						_timer -= Time.deltaTime / _speedOfSlider;
						_sliderImage.fillAmount = _timer;
						_countDownTimer = _timer * _countDownTime % _countDownTime;
						_countDownText.text = Mathf.Round (_countDownTimer).ToString ("0");
					} else {
						_timer = 0f;
						ShowGameOverUI ();
					}
				}
			}


	}
		
	IEnumerator DelayTheGameOver()
	{
		yield return new WaitForSeconds (0.3f);
		GameOver ();
	}

	private Vector3 _playerCurPos;

	void MoveThePlayer(Transform _targetPos,int _source)
	{
		_player.GetComponent<TrailRenderer>().enabled=true;

		if (Mathf.Abs(_player.transform.position.x)<Mathf.Abs(_targetPos.position.x)) 
		{
			_player.GetComponent<Rigidbody> ().isKinematic = false;
			_playerCurPos = _player.transform.position;
			_playerCurPos.x += 0.02f *_playerSpeed;
			_player.transform.position=new Vector3(_playerCurPos.x,_player.transform.position.y,0f);
		} 
		else
		{
			_player.GetComponent<Rigidbody> ().isKinematic = true;
			if (_source == 0) 
			{
				if (_platformContainer.transform.childCount-1 > _platformSelector) 
				{
					_platformSelector++;
				}
				else
				{
					_platformSelector = 0;
				}
				_platformContainer.GetChild (_platformSelector).GetComponent<Platform> ()._animateIt = true;
				_platformContainer.GetChild (_platformSelector).gameObject.SetActive (true);
				_player.GetComponent<Rigidbody> ().isKinematic = true;
				//new cycle is started from here
				_IsnewCycleReady=true;
				StartCoroutine(ControlTheCamera());
				//
				AudioController._instance.PlayCameraMoveSound();
				_newStates = _ingameStates._startNewCycle;

				//
				if (_showTutorial) {
					StartCoroutine (StartNewTutorialInput ());
				}
			} 
			else 
			{
				AudioController._instance.PlayFailedSound ();
				//bridge is further rotated and game over is called
				_player.GetComponent<SphereCollider> ().isTrigger = true;
				_player.GetComponent<Rigidbody> ().isKinematic = false;
				_newStates = _ingameStates._rotateBridgeForGameOver;
			}
		}
	}
		

	void PlayLandingParticleEffect()
	{
		_module = _landingParticle.main;
		_module1 = _landingParticle.transform.GetChild (0).GetComponent<ParticleSystem> ().main;
		_module1.startColor = _module.startColor = _bridgeCol;
		_landingParticle.transform.position = new Vector3 (_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild(_childSelector).position.x, 0f, 0f);
		_landingParticle.Stop ();
		_landingParticle.Play ();
	}

	//
	IEnumerator ControlTheCamera()
	{
		CameraController._instance._canfollow = true;
		yield return new WaitForSeconds (1f);
		CameraController._instance._canfollow = false;
	}
		
	#region Platform logic
	//this is called when we click the play button
	void InitialPlatformAdjustMent()
	{
		RandomizeTheGlowMaterial ();
		//first platform is always at 0f,0f,0f at the start of the game
		_platformContainer.GetChild (0).transform.position = new Vector3 (0f, _platformYpos, 0f);
		//
		AdjustThePlatformColor (_platformContainer.GetChild (0).GetChild (0).GetChild (0).gameObject);
		InitialCenterColor ();

		for (int i = 0; i < _platformContainer.childCount; i++)
		{
			if (i > 0)
			{
				_Xpos = Random.Range (_minXpos,_maxXpos);
				_platformContainer.GetChild (i).transform.position = new Vector3 (_platformContainer.GetChild (i - 1).transform.position.x + _Xpos, _initialPlatformYpos, 0f);
				ResetThePlatformColor (_platformContainer.GetChild(i).GetChild(0).GetChild(0).gameObject);
				CheckTheGemProbablity (_platformContainer.GetChild (i).transform);
			}
			ManagePlatformSize (_platformContainer.GetChild(i).transform);
			//
			_platformContainer.GetChild (i).gameObject.SetActive (false);
		}
		//
		_platformContainer.GetChild (0).gameObject.SetActive (true);
		//for animating the childs
		StartCoroutine (AnimateChild1 ());
		_lastPlatform = _platformContainer.GetChild (_platformContainer.childCount - 1).transform;

		//for deactivating all the bridges before game start
		for (int i = 0; i < _platformContainer.childCount; i++)
		{
			for (int j = 0; j < _platformContainer.GetChild(i).GetChild(4).childCount; j++) 
			{
				_platformContainer.GetChild(i).GetChild (4).GetChild (j).gameObject.SetActive (false);
			}
		}
			
		RandomizeTheBridgeColor ();
	}

	IEnumerator AnimateChild1()
	{
		_platformContainer.GetChild (_platformSelector).gameObject.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		_platformContainer.GetChild (_platformSelector).GetComponent<Platform> ()._animateIt = true;
	}
		
	//for randomizing the size of each platform
	void ManagePlatformSize(Transform _curPlatform)
	{
		//for getting the random x size of the platform
		_Xsize = Random.Range (_minXsize, _maxXsize);
		//now change the scale of the platform according to the _Xsize we obtained
			_curPlatform.GetChild(0).GetChild(0).localScale=new Vector3(
			_Xsize,
			_curPlatform.GetChild(0).GetChild(0).localScale.y,
			_curPlatform.GetChild(0).GetChild(0).localScale.z
		);
		//for adjusting the fornt and end point of the platform 
		float _platformXsize;
		float _platformYsize;
		_platformXsize = _curPlatform.GetChild (0).GetChild (0).localScale.x / 2;
		_platformYsize= _curPlatform.GetChild (0).GetChild (0).localScale.y / 2;
		_curPlatform.GetChild (1).transform.localPosition = new Vector3 (-_platformXsize,_platformYsize,0f);
		_curPlatform.GetChild (2).transform.localPosition = new Vector3 (_platformXsize,_platformYsize,0f);
	}


	//positioning the platform 
	public void PlatformNewPos(Transform _newPlatform)
	{
		_newPlatform.gameObject.SetActive (false);
		_Xpos = Random.Range (_minXpos, _maxXpos);
		_newPlatform.position = new Vector3 (_lastPlatform.position.x+_Xpos,_initialPlatformYpos,0f);
		_lastPlatform = _newPlatform;
		ManagePlatformSize (_newPlatform);
		ResetThePlatformColor (_newPlatform.GetChild (0).GetChild (0).gameObject);
		ResetTheCenterColor (_newPlatform.GetChild (0).GetChild (1).gameObject);
		CheckTheGemProbablity (_newPlatform);

		ReAdjustingTheBridge (_lastPlatform);
	}
		
	void CheckTheGemProbablity(Transform _platformForGem)
	{
		if (!_showTutorial) 
		{
			if (Random.Range (0, 30) < 5) 
			{
				_platformForGem.GetChild (3).gameObject.SetActive (true);
			}
		}
	}
		
	void ManageTheBridgePosition()
	{
		_platformContainer.GetChild (_platformForBridge).GetChild (4).transform.position = new Vector3 (
			_platformContainer.GetChild(_platformForBridge).GetChild(2).transform.position.x+(_bridgeChildSize.x/2f),
			_platformContainer.GetChild(_platformForBridge).GetChild(2).transform.position.y-(_bridgeChildSize.y/3f),
			0f
		);

		_platformContainer.GetChild (_platformForBridge).GetChild (4).gameObject.SetActive (true);

		if (_showTutorial)
		{
			CalculationsForTutorial ();
		}
	}

	void ManageBridgeChildPosition()
	{
		if (_showTutorial)
		{
			CalculatingBridgeHeight ();
		}

		//for controlling the color of bridge
		_matArray = _platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).gameObject.GetComponent<Renderer> ().materials;
		_matArray [0] = _bridgeMat [0];
		_matArray [1] = _bridgeMat [1];
		_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).gameObject.GetComponent<Renderer> ().materials = _matArray;


		_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild(_childSelector).gameObject.SetActive(true);

		if (_childSelector < _platformContainer.GetChild(_platformForBridge).GetChild(4).childCount-1) 
		{
			_bridgeTimer +=_timerValue*_bridgeGrowTime;
			if (_bridgeTimer >= _waitTimeToGrow) 
			{
				_newchildPos += _bridgeChildSize.x;
				_childSelector++;
				_bridgeTimer = 0;
			}
		} 
		else 
		{
			_newStates = _ingameStates._rotateBridge;
		}
			
		_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.localPosition = new Vector3 (0f,_newchildPos,0f);
	}
		
	public void ReAdjustingTheBridge(Transform _oldBridge)
	{
		_oldBridge.GetChild(4).gameObject.SetActive (false);
		_oldBridge.GetChild(4).transform.rotation= Quaternion.Euler (0f, 0f, 0f);

		for (int i = 0; i < _oldBridge.GetChild(4).childCount; i++) 
		{
			_oldBridge.GetChild(4).GetChild (i).transform.localPosition = Vector3.zero;
			_oldBridge.GetChild(4).GetChild (i).gameObject.SetActive (false);
		}
	}

	void ResetThePlatformInformation()
	{
		//reseting the value of platformselector ot zero
		_platformSelector=0;
		//reseting the position of platform to (0f,0f,0f) and then adjusting its pos and size again
		for (int i = 0; i < transform.childCount; i++) 
		{
			transform.GetChild (i).transform.position = Vector3.zero;
		}
	}
	#endregion

	public void Play()
	{
		StartFading ();
		AudioController._instance.PlayButtonSound ();
		StartCoroutine (LoadTheGame ());
		//for firebase
		PassingGameStartInfoToAnalytics("home_screen");
	}

	IEnumerator LoadTheGame()
	{
		yield return new WaitForSeconds (0.3f);
		AdjustmentsForNewGame ();
	}
		
	void AdjustmentsForNewGame()
	{
		_gameStates = _states._ingame;
		InitialPlatformAdjustMent ();
		ManageTheBridgePosition ();
		HideMenuContents ();
		_player.GetComponent<PlayerController> ().ChangePlayerSkin ();
		_player.GetComponent<Rigidbody> ().isKinematic = false;

		//firebase game play counter
		StatHandler._instance._noOfGamesPlayed+=1;
		FirebaseHandler._instance.TrackingGamePlay (StatHandler._instance._noOfGamesPlayed);
		FirebaseHandler._instance.TrackingScreenStart ();
	}
		
	void HideMenuContents()
	{
		_menuCam.SetActive (false);
		_gameCam.SetActive (true);
		_mainMenuUI.SetActive (false);
		_characterShop.SetActive (false);

		if (!_showTutorial) 
		{
			_ingameUI.SetActive (true);
		} 
		else 
		{
			_gemInfoContainer.SetActive (false);
			_tutorialUI.SetActive (true);
		}
	}

	void AddScore(int _score)
	{
		if (!_showTutorial) 
		{
			_scoreCounter += _score;
			_ingameScoreText.text = _scoreCounter.ToString ();
			_bestScoreCounter = PlayerPrefs.GetInt ("HighScore");
			if (_scoreCounter > _bestScoreCounter) {
				PlayerPrefs.SetInt ("HighScore", _scoreCounter);
				_gameOverNewBestIndicator.gameObject.SetActive (true);
				if (!_newBestShown) {
					StartCoroutine (ShownewBestScoreAnimation ());
				}
			}
			ShowScoreAnimation (_tempString, _score);
		}
	}

	IEnumerator ShownewBestScoreAnimation()
	{
		_ingameBestIndicator.SetActive (true);
		_ingameBestIndicator.GetComponent<Animator> ().SetBool ("_showNewBest", true);
		yield return new WaitForSeconds (1.2f);
		_ingameBestIndicator.GetComponent<Animator> ().SetBool ("_showNewBest", false);
		_newBestShown = true;
	}

	void ConsecutiveScoreController()
	{
		_perfectDisplayTextSelector = Random.Range (0, _encouragingTexts.Length);
		_tempString = _encouragingTexts [_perfectDisplayTextSelector];
	}

	void ShowScoreAnimation(string _displayText,int _val)
	{
		_perfectScoreDisplayText.text=_displayText+"\n"+"+"+_val.ToString();
		_perfectScoreDisplayText.gameObject.GetComponent<Animator> ().SetTrigger ("_addScore");
		_tempString = "";
	}
		
	//only called when player touches the gem
	public void AddGem(int _gem)
	{
		_gemCounter += 1;
		PlayerPrefs.SetInt ("TotalGems", PlayerPrefs.GetInt ("TotalGems") + _gem);
		_ingameGemText.text = PlayerPrefs.GetInt ("TotalGems").ToString ();
		//play button sound
		AudioController._instance.PlayGemSound ();

		//for counting all the gems collected for gameAnalytics
		PlayerPrefs.SetInt("TotalCollectedGems",PlayerPrefs.GetInt("TotalCollectedGems")+1);
	}
		
	void GameOver()
	{
		_ingameUI.SetActive (false);
		//
		_player.GetComponent<TrailRenderer>().enabled=false;
		//
		for (int i = 0; i < _gameOverUI.transform.childCount; i++)
		{
			_gameOverUI.transform.GetChild (i).gameObject.SetActive (false);
		}
		//
		PlayerPrefs.SetInt ("LastScore", _scoreCounter);
		//
		_gameOverScoreText.text = "Score : "+ _scoreCounter.ToString ();
		_gameOverBestScoreText.text = "Best : " + PlayerPrefs.GetInt ("HighScore").ToString ();
		_gameOverGemCounterText.text = _gemCounter.ToString();
		_gameOverUI.SetActive (true);

		if (Application.internetReachability==NetworkReachability.NotReachable||_playerIsSaved) 
		{
			//show gameover ui
			ShowGameOverUI();
		} 
		else 
		{
			//show other games ui and save me ui when there is internet connection
			ShowSaveMeUI();
		}
	}

	void ShowSaveMeUI()
	{
		_gameOverUI.transform.GetChild (0).gameObject.SetActive (true);
	}

	void ShowGameOverUI()
	{
		AdmobManager._instance.DestroyBannerAds();
		//for interstitialAds
		if (StatHandler._instance._interstitialAdsCounter < _deathNoToShowInterstitialAds) {
			StatHandler._instance._interstitialAdsCounter++;
		}
		else 
		{
			AdmobManager._instance.ShowInterstitialAds ();
			StatHandler._instance._interstitialAdsCounter = 0;
		}
		_gameOverUI.transform.GetChild (0).gameObject.SetActive (false);
		_gameSelector = Random.Range (0, _customAds.Count);
		_messageSelector = Random.Range (0, _message.Length);
		_displayText.text = _message [_messageSelector];
		_gameName.text = _customAds [_gameSelector]._gameName;
		_gameLogoImage.GetComponent<Image> ().sprite = _customAds [_gameSelector]._gameIcon;
		_androidUrl=_customAds[_gameSelector]._playstoreLink;
		_iosUrl = _customAds [_gameSelector]._appstoreLink;
		_gameOverUI.transform.GetChild (1).gameObject.SetActive (true);
		_playerIsSaved=false;
		_gemCounter = 0;
		_newBestShown = false;
		//for preventing the calcutation player gravity after gameOVer
		StartCoroutine (FreezeThePlayerPosition ());
		PlayerPrefs.SetInt("TotalGamesPlayed",PlayerPrefs.GetInt("TotalGamesPlayed")+1);
		FirebaseHandler._instance.TrackingScreenEnd ();
		FirebaseHandler._instance.TrackingGameOver(PlayerPrefs.GetInt ("TotalGems"),_scoreCounter,PlayerPrefs.GetInt ("HighScore"));
	}

	IEnumerator FreezeThePlayerPosition ()
	{
		yield return new WaitForSeconds (0.5f);
		_player.GetComponent<Rigidbody> ().isKinematic = true;
	}

	public void OpenGameLink()
	{
		#if UNITY_ANDROID
		Application.OpenURL(_androidUrl);
		#elif UNITY_IOS||UNITY_IPHONE
		Application.OpenURL(_iosUrl);
		#endif
		//play button sound
		AudioController._instance.PlayButtonSound ();
	}
		
	public void SaveThePlayer()
	{
		//play button sound
		AudioController._instance.PlayButtonSound ();
		//for showing reward video ads
		AdmobManager._instance.ShowRewardVideoAds();
	}

	//for closing the save menu before its time up
	public void CloseTheSaveUI()
	{
		//play button sound
		AudioController._instance.PlayButtonSound ();
		_gameOverUI.transform.GetChild (0).gameObject.SetActive (false);
		ShowGameOverUI ();
	}

	//this function is only called when the player is saved by watching the ads
	public void PlayerSaved()
	{
		StartFading ();
		StartCoroutine (DelaySave ());
		//for tracking the number of times player saved via video ads
		PlayerPrefs.SetInt ("PlayerSavedFor", PlayerPrefs.GetInt ("PlayerSavedFor") + 1);
	}
		
	IEnumerator DelaySave()
	{
		_player.GetComponent<Rigidbody> ().isKinematic = true;
		yield return new WaitForSeconds (0.3f);
		BehindThePlayerSave ();
		//first relocate the player to the previous platform and reset the bridge
		_playerIsSaved=true;
		_gameOverUI.SetActive (false);
		_ingameUI.SetActive (true);
	}
		
	void BehindThePlayerSave()
	{
		_player.GetComponent<SphereCollider> ().isTrigger = false;
		_platformContainer.GetChild(_platformForBridge).GetChild(4).transform.rotation=Quaternion.Euler(0f,0f,0f);
		_player.transform.position = new Vector3 (
			_platformContainer.GetChild(_platformForBridge).transform.position.x,
			0.4f,
			0f
		);
		for (int i = 0; i < _platformContainer.GetChild(_platformForBridge).GetChild(4).childCount; i++)
		{
			_platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (i).gameObject.SetActive (false);
		}
		_bridgeTimer = 0f;
		_childSelector = 0;
		_newchildPos = 0f;
		_player.GetComponent<Rigidbody> ().isKinematic = false;
		_newStates = _ingameStates._waitForInput;
		_gameStates = _states._ingame;
	}

	public void Home()
	{
		StartFading ();
		//play button sound
		AudioController._instance.PlayButtonSound ();
		StartCoroutine (GoToHome ());
	}

	IEnumerator GoToHome()
	{
		yield return new WaitForSeconds (0.4f);
		SceneManager.LoadScene (_sceneName);
	}

	//call this method only from restart button of game over menu
	public void Restart()
	{
		AudioController._instance.PlayButtonSound ();
		StartCoroutine (DelayRestart ());
		//passing info to analytics handler
		PassingGameStartInfoToAnalytics("end_restart");
	}

	void PassingGameStartInfoToAnalytics(string _from)
	{
		FirebaseHandler._instance.TrackingGameStart (PlayerPrefs.GetInt ("TotalGems"), _from);
	}
	 
	IEnumerator DelayRestart()
	{
		_showTutorial = false;
		_isTutorialInputReady = false;
		StartFading ();
		yield return new WaitForSeconds (0.2f);
		_newStates = _ingameStates._waitForInput;
		ResetTheGame ();
		_gemInfoContainer.SetActive (true);
	}
		
	//reloding the scene for restart or you can just use restart method above if you dont want to restart using scene reload
	public void RestartWithLevelReload()
	{
		StatHandler._instance._restartTheGame = true;
		AudioController._instance.PlayButtonSound ();
		StartCoroutine (WaitForButtonSoundBeforeRestart ());
	}

	IEnumerator WaitForButtonSoundBeforeRestart()
	{
		yield return new WaitForSeconds (0.2f);
		SceneManager.LoadScene (_sceneName);
	}

	//reseting the positions and rotation of platform along with bridge instead of reloading the level
	void ResetTheGame()
	{
		_gameOverUI.SetActive(false);
		_ingameUI.SetActive (true);
		_player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		_player.transform.position = new Vector3 (0f, 0.4f, 0f);
		_player.GetComponent<SphereCollider> ().isTrigger = false;
		_gameCam.transform.position = Vector3.zero;
		_gameStates = _states._ingame;
		//reseting the position of the bridge
		for (int i = 0; i < _platformContainer.childCount; i++) 
		{
			_platformContainer.GetChild(i).GetChild(4).transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		}
		//reseting all the variables value 
		_platformSelector = 1;
		_platformForBridge=0;
		_bridgeTimer = 0f;
		_childSelector = 0;
		_scoreCounter = 0;
		_newchildPos = 0f;
		_timer = 1f;
		_ingameScoreText.text = _scoreCounter.ToString ();

		//adjusting the platforms and bridges
		InitialPlatformAdjustMent ();
		ManageTheBridgePosition ();
		_player.GetComponent<Rigidbody> ().isKinematic = false;

		if (_platformContainer.GetChild (_platformForBridge).GetChild (3).gameObject.activeInHierarchy) 
		{
			_platformContainer.GetChild (_platformForBridge).GetChild (3).gameObject.SetActive (false);
		}
		//disabling the new best indicator
		_gameOverNewBestIndicator.gameObject.SetActive(false);
	}


	public void RateUs()
	{
		//play button sound
		AudioController._instance.PlayButtonSound ();
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id="+Application.identifier);
		#elif UNITY_IOS||UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/id"+Application.identifier);
		#endif
		//
		FirebaseHandler._instance.TrackingRating();
	}

	public void ChangeTheAudioSettings()
	{
		if (PlayerPrefs.GetInt ("GameSound") == 0) {
			PlayerPrefs.SetInt ("GameSound", 1);
		} else if (PlayerPrefs.GetInt ("GameSound") == 1) 
		{
			PlayerPrefs.SetInt ("GameSound", 0);
		}
		//play button sound
		AudioController._instance.PlayButtonSound ();
		ChangeTheAudioSprite ();
	}

	void ChangeTheAudioSprite()
	{
		for (int i = 0; i < _audioButtonImage.Length; i++)
		{
			_audioButtonImage [i].sprite = _audioSprite [PlayerPrefs.GetInt ("GameSound")];
		}
		GameObject.FindGameObjectWithTag ("Manager").GetComponent<ManagerHandler> ().ControlTheBackgroundMusic ();
	}

	#region scrollable menu ui
	public void GoToShop()
	{
		_characterShop.SetActive (true);
		if (Mathf.Round (_mainMenuContainer.GetComponent<RectTransform> ().anchoredPosition.x) > _shopXpos) 
		{
			_targetXpos = _shopXpos;
			_camTargetPos = 10f;
		}
		//play button sound
		AudioController._instance.PlayButtonSound ();
	}

	public void BackToMenuFromShop()
	{
		if (Mathf.Round (_mainMenuContainer.GetComponent<RectTransform> ().anchoredPosition.x) < _homeXpos) 
		{
			_targetXpos = _homeXpos;
			_camTargetPos = 0f;
		}
		StartCoroutine (WaitBeforeClosingShop());
		//play button sound
		AudioController._instance.PlayButtonSound ();
	}

	IEnumerator WaitBeforeClosingShop()
	{
		yield return new WaitForSeconds (0.5f);
		_characterShop.SetActive (false);
	}
	#endregion 


	//for fading in and fading out
	void StartFading()
	{
		_fader.GetComponent<Animator> ().SetBool ("startFading", true);
		StartCoroutine (FadeOut ());
	}

	IEnumerator FadeOut()
	{
		yield return new WaitForSeconds (0.3f);
		_fader.GetComponent<Animator> ().SetBool ("startFading", false);
	}


	#region platform color and effects
	void RandomizeTheBridgeColor ()
	{
		_red = Random.Range (_bridgeMinColorValue, _bridgeMaxColorValue);
		_green=Random.Range (_bridgeMinColorValue, _bridgeMaxColorValue);
		_blue=Random.Range (_bridgeMinColorValue, _bridgeMaxColorValue);
		_bridgeCol = new Color (_red / 255f, _green / 255f, _blue / 255f, _alpha);
		_bridgeMat [0].color =_bridgeCol;
	}

	void RandomizeTheGlowMaterial()
	{
		_glowMatSelector = Random.Range (0, _glowMaterials.Length);
		_bridgeMat [1] = _platformMat [1] = _centerMat [1] = _glowMaterials [_glowMatSelector];
	}

	void InitialCenterColor()
	{
		for (int i = 0; i < _platformContainer.childCount; i++) 
		{
			_centerMatArray = _platformContainer.GetChild(i).GetChild(0).GetChild(1).GetComponent<Renderer> ().materials;
			_centerMatArray [0] = _centerMat [0];
			_centerMatArray [1] = _centerMat [0];
			_platformContainer.GetChild (i).GetChild (0).GetChild (1).GetComponent<Renderer> ().materials = _centerMatArray;
		}
	}
		
	void StartColoringThePlatform(GameObject _receivedObj)
	{
		StartCoroutine (ShowSomeDelayEffect (_receivedObj));
	}

	IEnumerator ShowSomeDelayEffect (GameObject _newObj)
	{
		AudioController._instance.PlaySparkSound();
		_centerMatArray = _newObj.transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().materials;
		_centerMatArray [0] = _centerMat [0];
		_centerMatArray [1] = _centerMat [1];
		_newObj.transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().materials = _centerMatArray;
		yield return new WaitForSeconds (0.1f);
		AdjustThePlatformColor (_newObj.transform.GetChild(0).GetChild(0).gameObject);
	}
		
	//only color on perfect landing
	void AdjustThePlatformColor(GameObject _toBeColored)
	{
		_platformMatArray=_toBeColored.GetComponent<Renderer> ().materials;
		_platformMatArray [0] = _platformMat [0];
		_platformMatArray [1] = _platformMat [1];
		_toBeColored.GetComponent<Renderer> ().materials = _platformMatArray;
	}

	void ResetThePlatformColor(GameObject _toBeRemoved)
	{
		_platformMatArray=_toBeRemoved.GetComponent<Renderer> ().materials;
		_platformMatArray [0] = _platformMat [0];
		_platformMatArray [1] = _platformMat [0];
		_toBeRemoved.GetComponent<Renderer> ().materials = _platformMatArray;
	}

	void ResetTheCenterColor(GameObject _centerToBeRemoved)
	{
		_centerMatArray = _centerToBeRemoved.GetComponent<Renderer> ().materials;
		_centerMatArray [0] = _centerMat [0];
		_centerMatArray [1] = _centerMat [0];
		_centerToBeRemoved.GetComponent<Renderer> ().materials = _centerMatArray;
	}
	#endregion


	public void ShowTutorial()
	{
		if (!StatHandler._instance._tutorialShown) 
		{
			Play ();
			StartCoroutine (StartTutorialBot ());
			_showTutorial = true;
		} 
		else
		{
			Play ();
		}
	}

	IEnumerator StartTutorialBot ()
	{
		StatHandler._instance._tutorialShown = true;
		yield return new WaitForSeconds (1.5f);
		_isTutorialInputReady = true;
		_gestureIndicator.sprite = _handGesture [1];
	}

	//
	IEnumerator StartNewTutorialInput ()
	{
		yield return new WaitForSeconds (0.8f);
		_gestureIndicator.sprite = _handGesture [1];
		_isTutorialInputReady = true;
	}
		
	public void SkipTutorial()
	{
		AudioController._instance.PlayButtonSound ();
		_gameStates = _states._noTutorial;
		_showTutorial = false;
		_isTutorialInputReady = false;
		StartCoroutine (DisablingTutorial());
	}

	IEnumerator DisablingTutorial()
	{
		_tutorialUI.SetActive (false);
		yield return new WaitForSeconds (1f);
		StartCoroutine (DelayRestart ());
	}
		
	 void DisableTutorialInput()
	{
		_isTutorialInputReady = false;
		_gestureIndicator.sprite = _handGesture [0];
		_currentBridgeHeight = 0f;
		_distanceBetweenTwoPlatforms = 0f;
		_newStates = _ingameStates._rotateBridge;
	}
		
	void CalculationsForTutorial()
	{
		_distanceBetweenTwoPlatforms = _platformContainer.GetChild (_platformSelector).GetChild (0).GetChild (1).position.x
		- _platformContainer.GetChild (_platformForBridge).GetChild (2).position.x;
	}

	void CalculatingBridgeHeight()
	{
		if (_distanceBetweenTwoPlatforms>_currentBridgeHeight) 
		{
			_currentBridgeHeight = _platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (_childSelector).transform.localPosition.y - _platformContainer.GetChild(_platformForBridge).GetChild(4).GetChild (0).transform.localPosition.y;
		}
		else 
		{
			DisableTutorialInput ();
		}
	}
}
