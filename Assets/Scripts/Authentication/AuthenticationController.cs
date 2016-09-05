using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class AuthenticationController : Photon.MonoBehaviour 
{
	public static AuthenticationController instance;

	public Sprite[] languagesSprites;
	public GameObject soundControllerObject;
	public GameObject photonControllerObject;
	public GameObject serverControllerObject;

	private GameObject backOfficeController;
	private GameObject soundController;
	private GameObject photonController;
	private GameObject mainLogo;
	private GameObject chooseLanguageButton;
	private GameObject facebookButton;
	private GameObject quitButton;
	private GameObject serverController;

	private GameObject loginPopUp;
	private bool isLoginPopUpDisplayed;
	private GameObject inscriptionPopUp;
	private bool isInscriptionPopUpDisplayed;
	private GameObject authenticationMessagePopUp;
	private bool isAuthenticationMessagePopUpDisplayed;
	private GameObject offlineModePopUp;
	private bool isOfflineModePopUpDisplayed;
	private GameObject lostLoginPopUp;
	private bool isLostLoginPopUpDisplayed;
	private GameObject emailNonActivatedPopUp;
	private bool isEmailNonActivatedPopUpDisplayed;
	private GameObject existingAccountPopUp;
	private bool isExistingAccountPopUpDisplayed;
	private GameObject inscriptionFacebookPopUp;
	private bool isInscriptionFacebookPopUpDisplayed;
	private GameObject changePasswordPopUp;
	private bool isChangePasswordPopUpDisplayed;

	private GameObject mainCamera;
	private GameObject backgroundCamera;
	private GameObject sceneCamera;

	void Awake()
	{
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.initPlayer();
		this.initLanguage();
		this.initializeServerController();
		this.initializePhotonController();
		this.initializeBackOffice();
		this.initializeMusic();
		this.resize ();
		this.drawChooseLanguageButton();
		this.initFacebookSDK();
	}
	private void autoLogging()
	{
		if(ApplicationModel.player.ToDeconnect)
		{
            this.displayLoginPopUp();
			ApplicationModel.player.ToDeconnect=false;
			if(ApplicationModel.player.HasLostConnection)
			{
				ApplicationModel.player.HasLostConnection=false;
				this.loginPopUp.GetComponent<LoginPopUpController>().setError(WordingAuthentication.getReference(14));
			}
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(this.isConnectedToFB())
		{
            this.displayLoginPopUp();
			AccessToken aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			ApplicationModel.player.FacebookId=aToken.UserId;
            ApplicationModel.player.Username="";
			StartCoroutine(this.login());
		}
		else if(ApplicationModel.player.AutomaticConnection)
		{
            ApplicationModel.player.Username="";
            StartCoroutine (this.login ());
		}
		else
		{
            BackOfficeController.instance.hideLoadingScreen();
			this.displayLoginPopUp();
		}
	}
	private void initLanguage()
	{
		if(ApplicationModel.player.Username=="")
		{
			if(Application.systemLanguage==SystemLanguage.French)
			{
				ApplicationModel.player.IdLanguage=0;
			}
			else
			{
				ApplicationModel.player.IdLanguage=1;
			}
		}
	}
	private void initPlayer()
	{
		string lastUsername = ApplicationModel.player.Username;
		int lastIDLanguage = ApplicationModel.player.IdLanguage;
		bool lastToDeconnect = ApplicationModel.player.ToDeconnect;
		bool lastHasLostConnection = ApplicationModel.player.HasLostConnection;
		ApplicationModel.player=new Player();
		ApplicationModel.player.Username=lastUsername;
		ApplicationModel.player.IdLanguage=lastIDLanguage;
		ApplicationModel.player.ToDeconnect=lastToDeconnect;
		ApplicationModel.player.HasLostConnection=lastHasLostConnection;
		ApplicationModel.player.MacAdress=SystemInfo.deviceUniqueIdentifier;
        ApplicationModel.volBackOfficeFx = PlayerPrefs.GetFloat ("sfxVol", 0.5f) * ApplicationModel.volMaxBackOfficeFx;
        ApplicationModel.volMusic = PlayerPrefs.GetFloat ("musicVol", 0.5f) * ApplicationModel.volMaxMusic;
        ApplicationModel.player.AutomaticConnection=System.Convert.ToBoolean(PlayerPrefs.GetInt("automaticConnection",0));
        ApplicationModel.player.Username=ApplicationModel.Decrypt(PlayerPrefs.GetString("username",""));
		ApplicationModel.checkForSavedGame ();
	}
	private void initializeServerController()
	{
		this.serverController = GameObject.Find ("ServerController");
		if(this.serverController==null)
		{
			this.serverController=GameObject.Instantiate(this.serverControllerObject);
			this.serverController.name="ServerController";
			this.serverController.GetComponent<ServerController>().initialize();	
		}
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeAuthenticationController>();
		this.backOfficeController.GetComponent<BackOfficeAuthenticationController>().initialize();
	}
	private void initializeMusic()
	{
		this.soundController = GameObject.Find ("SoundController");
		if(this.soundController==null)
		{
			this.soundController=GameObject.Instantiate(this.soundControllerObject);
			this.soundController.name="SoundController";
			this.soundController.GetComponent<SoundController>().initialize();	
		}
	}
	private void initializePhotonController()
	{
		this.photonController = GameObject.Find ("PhotonController");
		if(this.photonController==null)
		{
			this.photonController=GameObject.Instantiate(this.photonControllerObject);
			this.photonController.name="PhotonController";
			this.photonController.GetComponent<PhotonController>().initialize();	
		}
	}
	public void initializeScene()
	{ 
		this.loginPopUp=GameObject.Find("loginPopUp");
		this.loginPopUp.SetActive(false);
		this.inscriptionPopUp=GameObject.Find("inscriptionPopUp");
		this.inscriptionPopUp.SetActive(false);
		this.offlineModePopUp = GameObject.Find ("offlineModePopUp");
		this.offlineModePopUp.SetActive (false);
		this.authenticationMessagePopUp=GameObject.Find("authenticationMessagePopUp");
		this.authenticationMessagePopUp.SetActive(false);
		this.emailNonActivatedPopUp=GameObject.Find("emailNonActivatedPopUp");
		this.emailNonActivatedPopUp.SetActive(false);
		this.lostLoginPopUp=GameObject.Find("lostLoginPopUp");
		this.lostLoginPopUp.SetActive(false);
		this.existingAccountPopUp=GameObject.Find("existingAccountPopUp");
		this.existingAccountPopUp.SetActive(false);
		this.inscriptionFacebookPopUp=GameObject.Find("inscriptionFacebookPopUp");
		this.inscriptionFacebookPopUp.SetActive(false);
		this.changePasswordPopUp=GameObject.Find("changePasswordPopUp");
		this.changePasswordPopUp.SetActive(false);
		this.mainCamera = gameObject;
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.mainLogo = GameObject.Find("mainLogo");
		this.chooseLanguageButton = GameObject.Find("chooseLanguageButton");
		this.facebookButton=GameObject.Find("FacebookButton");
		this.quitButton=GameObject.Find("QuitButton");
		this.quitButton.AddComponent<AuthenticationQuitButtonController>();
	}
	private void connectToPhoton()
	{
		BackOfficeController.instance.changeLoadingScreenLabel (WordingAuthentication.getReference(0));
		photonController.GetComponent<PhotonView>().viewID=1;
		PhotonNetwork.playerName = ApplicationModel.player.Username;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
	}
	public void facebookHandler()
	{
		SoundController.instance.playSound(9);
		this.facebookLogin();
	}
	public void loginHandler()
	{
		string login = this.loginPopUp.transform.GetComponent<LoginPopUpController>().getLogin();
		string password = this.loginPopUp.transform.GetComponent<LoginPopUpController> ().getPassword();
		string error = this.checkLogin(login);
		if(error=="")
		{
			error=this.checkPasswordComplexity(password);
			if(error=="")
			{
				ApplicationModel.player.Username=login;
				ApplicationModel.player.Password=password;
				ApplicationModel.player.ToRememberLogins=this.loginPopUp.transform.GetComponent<LoginPopUpController> ().getRememberMe();
				ApplicationModel.player.FacebookId="";
				StartCoroutine(this.login());
				SoundController.instance.playSound(12);
			}
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError(error);
	}
	public IEnumerator login()
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.Login());
		Debug.Log (ApplicationModel.player.IsOnline);
		if (ApplicationModel.player.Error != "") 
		{
			this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError (ApplicationModel.player.Error);
			SoundController.instance.stopPlayingSound ();
			SoundController.instance.playSound (13);
			BackOfficeController.instance.hideLoadingScreen ();
		} 
		else if (!ApplicationModel.player.IsAccountCreated) 
		{
			this.hideLoginPopUp ();
			this.hideInscriptionPopUp ();
			this.displayInscriptionFacebookPopUp (ApplicationModel.player.Mail);
			BackOfficeController.instance.hideLoadingScreen ();
		} 
		else if (!ApplicationModel.player.IsAccountActivated) 
		{
			this.hideLoginPopUp ();
			this.displayEmailNonActivatedPopUp (ApplicationModel.player.Mail);
			BackOfficeController.instance.hideLoadingScreen ();
		} 
		else if (ApplicationModel.player.ToChangePassword) 
		{
			this.hideLoginPopUp ();
			this.displayChangePasswordPopUp ();
			BackOfficeController.instance.hideLoadingScreen ();
		} 
		else
		{
			if (ApplicationModel.player.IsOnline) 
			{
				ApplicationModel.Save ();
				this.connectToPhoton ();
			} 
			else 
			{
				BackOfficeController.instance.hideLoadingScreen ();
				this.hideLoginPopUp ();
				this.displayOfflineModePopUp ();
			}
		}
	}
	public void inscriptionHandler()
	{
		string login = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController>().getLogin();
		string password1 = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().getFirstPassword();
		string password2 = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().getSecondPassword();
		string email = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController>().getEmail();
		bool tooAccepted = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController>().getTouAccepted();
		string error = this.checkUsername(login);
		if(error=="")
		{
		 	error=this.checkPasswordEgality(password1,password2);
			if(error=="")
			{
				error=this.checkPasswordComplexity(password1);
				if(error=="")
				{
					error=this.checkEmail(email);
					if(error=="")
					{
						error=this.checkTouAccepted(tooAccepted);
						if(error=="")
						{
							ApplicationModel.player.Username=login;
							ApplicationModel.player.Password=password1;
							ApplicationModel.player.Mail=email;
							ApplicationModel.player.IsAccountActivated=false;
							StartCoroutine(this.createNewAccount());
						}
					}
				}
			}
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.inscriptionPopUp.GetComponent<InscriptionPopUpController>().setError(error);	
	}
	public IEnumerator createNewAccount()
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.createAccount());
		if(ApplicationModel.player.Error!="")
		{
			SoundController.instance.playSound(13);
			this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().setError(ApplicationModel.player.Error);
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(!ApplicationModel.player.IsAccountActivated)
		{
			this.hideInscriptionPopUp();
			this.displayAuthenticationMessagePopUp(1);
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public void inscriptionFacebookHandler()
	{
		string login = this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController>().getLogin();
		string password1 = this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController> ().getFirstPassword();
		string password2 = this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController> ().getSecondPassword();
		string email = this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController>().getEmail();
		bool touAccepted = this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController>().getTouAccepted();
		string error = this.checkUsername(login);
		if(error=="")
		{
		 	error=this.checkPasswordEgality(password1,password2);
			if(error=="")
			{
				error=this.checkPasswordComplexity(password1);
				if(error=="")
				{
					error=this.checkEmail(email);
					if(error=="")
					{
						error=this.checkTouAccepted(touAccepted);
						if(error=="")
						{
							ApplicationModel.player.Username=login;
							ApplicationModel.player.Password=password1;
							if(email!=ApplicationModel.player.Mail)
							{
								ApplicationModel.player.IsAccountActivated=false;
							}
							else
							{
								ApplicationModel.player.IsAccountActivated=true;
							}
							ApplicationModel.player.Mail=email;
							StartCoroutine(this.createNewFacebookAccount());
						}
					}
				}
			}
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.inscriptionPopUp.GetComponent<InscriptionPopUpController>().setError(error);	
	}
	public IEnumerator createNewFacebookAccount()
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.createAccount());
		if(ApplicationModel.player.Error=="" && ApplicationModel.player.Id!=-1 && ApplicationModel.player.IsAccountActivated)
		{
			this.connectToPhoton();
		}
		else if(ApplicationModel.player.Error!="")
		{
			SoundController.instance.playSound(13);
			this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController> ().setError(ApplicationModel.player.Error);
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(!ApplicationModel.player.IsAccountActivated)
		{
			this.hideInscriptionFacebookPopUp();
			this.displayAuthenticationMessagePopUp(1);
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public void lostLoginHandler()
	{
		ApplicationModel.player.Username=this.lostLoginPopUp.GetComponent<LostLoginPopUpController>().getLogin();
		StartCoroutine(this.lostLogin());
	}
	public IEnumerator lostLogin()
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.lostLogin());
		if(ApplicationModel.player.Error=="")
		{
			this.displayAuthenticationMessagePopUp(2);
			this.hideLostLoginPopUp();
		}
		else 
		{
			SoundController.instance.playSound(13);
			this.lostLoginPopUp.GetComponent<LostLoginPopUpController>().setError(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen();
	}
	public void changePasswordHandler()
	{
		string password1 = this.changePasswordPopUp.transform.GetComponent<AuthenticationChangePasswordPopUpController> ().getFirstPassword();
		string password2 = this.changePasswordPopUp.transform.GetComponent<AuthenticationChangePasswordPopUpController> ().getSecondPassword();
	 	string error=this.checkPasswordEgality(password1,password2);
		if(error=="")
		{
			error=this.checkPasswordComplexity(password1);
			if(error=="")
			{
				ApplicationModel.player.Password=password1;
				StartCoroutine(this.editPassword());
			}
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.changePasswordPopUp.GetComponent<AuthenticationChangePasswordPopUpController>().setError(error);
	}
	private IEnumerator editPassword()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.player.editPassword());
		if(ApplicationModel.player.Error=="")
		{
			this.hideChangePasswordPopUp();
			this.connectToPhoton();
		}
		else
		{
			SoundController.instance.playSound(13);
			this.changePasswordPopUp.GetComponent<AuthenticationChangePasswordPopUpController>().setError(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
			BackOfficeController.instance.hideLoadingScreen ();
		}
	}
	public void emailNonActivatedHandler()
	{
		string email = this.emailNonActivatedPopUp.transform.GetComponent<EmailNonActivatedPopUpController>().getEmail();
		string error = this.checkEmail(email);
		if(error=="")
		{
			ApplicationModel.player.Mail=email;
			StartCoroutine(this.emailNonActivated());
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.emailNonActivatedPopUp.GetComponent<EmailNonActivatedPopUpController>().setError(error);	
	}
	private IEnumerator emailNonActivated()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.player.sentNewEmail());
		if(ApplicationModel.player.Error=="")
		{
			this.hideEmailNonActivatedPopUp();
			this.displayAuthenticationMessagePopUp(3);
		}
		else
		{
			SoundController.instance.playSound(13);
			this.emailNonActivatedPopUp.GetComponent<EmailNonActivatedPopUpController>().setError(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public void existingAccountHandler()
	{
		string login = this.existingAccountPopUp.transform.GetComponent<ExistingAccountPopUpController>().getLogin();
		string password = this.existingAccountPopUp.transform.GetComponent<ExistingAccountPopUpController> ().getPassword();
		string error = this.checkLogin(login);
		if(error=="")
		{
			error=this.checkPasswordComplexity(password);
			if(error=="")
			{
				ApplicationModel.player.Username=login;
				ApplicationModel.player.Password=password;
				StartCoroutine(this.existingAccount());
			}
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.existingAccountPopUp.transform.GetComponent<ExistingAccountPopUpController> ().setError(error);
	}
	private IEnumerator existingAccount()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.player.linkAccount());
		if(ApplicationModel.player.Error=="")
		{
			this.hideExistingAccountPopUp();
			this.displayLoginPopUp();
			StartCoroutine(this.login());
		}
		else
		{
			SoundController.instance.playSound(13);
			this.existingAccountPopUp.GetComponent<ExistingAccountPopUpController>().setError(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
			BackOfficeController.instance.hideLoadingScreen ();
		}
	}
	public void resize()
	{
		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraAuthenticationPosition;
		this.mainLogo.transform.localScale=ApplicationDesignRules.mainLogoScale;
		this.mainLogo.transform.position=new Vector3(0f,ApplicationDesignRules.worldHeight/2f-ApplicationDesignRules.mainLogoWorldSize.y/2f-0.25f,0f);
		this.chooseLanguageButton.transform.localScale=ApplicationDesignRules.roundButtonScale;
		this.chooseLanguageButton.transform.position= new Vector3(0f,-ApplicationDesignRules.worldHeight/2f+0.25f+ApplicationDesignRules.roundButtonWorldSize.y/2f);
		this.facebookButton.transform.localScale=ApplicationDesignRules.popUpScale;
		this.facebookButton.transform.position= new Vector3(0f,ApplicationDesignRules.popUpWorldSize.y/2f+0.25f+ApplicationDesignRules.roundButtonWorldSize.y/2f);
		this.quitButton.transform.position=new Vector3(-ApplicationDesignRules.worldWidth/2f+0.05f+ApplicationDesignRules.roundButtonWorldSize.x/2f, ApplicationDesignRules.worldHeight/2f-0.05f-ApplicationDesignRules.roundButtonWorldSize.y/2f,0f);
		this.quitButton.SetActive(!ApplicationDesignRules.isMobileScreen);

		if(this.isLoginPopUpDisplayed)
		{
			this.loginPopUpResize();
		}
		if(this.isInscriptionPopUpDisplayed)
		{
			this.inscriptionPopUpResize();
		}
		if(this.isAuthenticationMessagePopUpDisplayed)
		{
			this.authenticationMessagePopUpResize();
		}
		if(this.isLostLoginPopUpDisplayed)
		{
			this.lostLoginPopUpResize();
		}
		if(this.isEmailNonActivatedPopUpDisplayed)
		{
			this.emailNonActivatedPopUpResize();
		}
		if(this.isExistingAccountPopUpDisplayed)
		{
			this.existingAccountPopUpResize();
		}
		if(this.isInscriptionFacebookPopUpDisplayed)
		{
			this.inscriptionFacebookPopUpResize();
		}
		if(this.isChangePasswordPopUpDisplayed)
		{
			this.changePasswordPopUpResize();
		}
		if (this.isOfflineModePopUpDisplayed) 
		{
			this.offlineModePopUpResize ();
		}
	}
	public void chooseLanguageHandler()
	{
		SoundController.instance.playSound(9);
		if(ApplicationModel.player.IdLanguage==1)
		{
			StartCoroutine(this.chooseLanguage(0));
		}
		else
		{
			StartCoroutine(this.chooseLanguage(1));
		}
	}
	public IEnumerator chooseLanguage(int idLanguage)
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.chooseLanguage(idLanguage));
		if(ApplicationModel.player.Error=="")
		{
			this.drawChooseLanguageButton();
			if(this.isLoginPopUpDisplayed)
			{
				this.loginPopUp.GetComponent<LoginPopUpController>().computeLabels();
			}
			if(this.isInscriptionPopUpDisplayed)
			{
				this.inscriptionPopUp.GetComponent<InscriptionPopUpController>().computeLabels();
			}
			if(this.isLostLoginPopUpDisplayed)
			{
				this.lostLoginPopUp.GetComponent<LostLoginPopUpController>().computeLabels();
			}
			if(this.isAuthenticationMessagePopUpDisplayed)
			{
				this.authenticationMessagePopUp.GetComponent<AuthenticationMessagePopUpController>().computeLabels();
			}
			if(this.isEmailNonActivatedPopUpDisplayed)
			{
				this.emailNonActivatedPopUp.GetComponent<EmailNonActivatedPopUpController>().computeLabels();
			}
			if(this.isInscriptionFacebookPopUpDisplayed)
			{
				this.inscriptionFacebookPopUp.GetComponent<InscriptionFacebookPopUpController>().computeLabels();
			}
			if(this.isExistingAccountPopUpDisplayed)
			{
				this.existingAccountPopUp.GetComponent<ExistingAccountPopUpController>().computeLabels();
			}
			if(this.isChangePasswordPopUpDisplayed)
			{
				this.changePasswordPopUp.GetComponent<AuthenticationChangePasswordPopUpController>().computeLabels();
			}
			if(this.isOfflineModePopUpDisplayed)
			{
				this.offlineModePopUp.GetComponent<OfflineModePopUpController>().computeLabels();
			}
		}
		else
		{
			ApplicationModel.player.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen();
	}
	public void drawChooseLanguageButton()
	{
		if(ApplicationModel.player.IdLanguage==0)
		{
			this.chooseLanguageButton.GetComponent<SpriteRenderer>().sprite=this.languagesSprites[1];
		}
		else
		{
			this.chooseLanguageButton.GetComponent<SpriteRenderer>().sprite=this.languagesSprites[0];
		}
	}
	public void displayLoginPopUp()
	{
		this.loginPopUp.transform.GetComponent<LoginPopUpController> ().reset(ApplicationModel.player.Username,false);
		this.isLoginPopUpDisplayed = true;
		this.loginPopUp.SetActive (true);
		this.loginPopUpResize ();
	}
	public void displayLostLoginPopUp()
	{
		this.lostLoginPopUp.transform.GetComponent<LostLoginPopUpController> ().reset(ApplicationModel.player.Username);
		this.isLostLoginPopUpDisplayed = true;
		this.lostLoginPopUp.SetActive (true);
		this.lostLoginPopUpResize ();
		this.displayFacebookButton(false);
	}
	public void displayInscriptionPopUp()
	{
		this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().reset();
		this.isInscriptionPopUpDisplayed = true;
		this.inscriptionPopUp.SetActive (true);
		this.inscriptionPopUpResize ();
	}
	public void displayAuthenticationMessagePopUp(int idMessage)
	{
		this.authenticationMessagePopUp.transform.GetComponent<AuthenticationMessagePopUpController> ().reset(idMessage);
		this.isAuthenticationMessagePopUpDisplayed = true;
		this.authenticationMessagePopUp.SetActive (true);
		this.authenticationMessagePopUpResize ();
	}
	public void displayOfflineModePopUp()
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.offlineModePopUp.transform.GetComponent<OfflineModePopUpController> ().reset();
		this.isOfflineModePopUpDisplayed = true;
		this.offlineModePopUp.SetActive (true);
		this.offlineModePopUpResize ();
	}
	public void displayEmailNonActivatedPopUp(string mail)
	{
		this.emailNonActivatedPopUp.transform.GetComponent<EmailNonActivatedPopUpController> ().reset(mail);
		this.isEmailNonActivatedPopUpDisplayed = true;
		this.emailNonActivatedPopUp.SetActive (true);
		this.emailNonActivatedPopUpResize ();
	}
	public void displayExistingAccountPopUp()
	{
		this.existingAccountPopUp.transform.GetComponent<ExistingAccountPopUpController> ().reset();
		this.isExistingAccountPopUpDisplayed = true;
		this.existingAccountPopUp.SetActive (true);
		this.existingAccountPopUpResize ();
	}
	public void displayInscriptionFacebookPopUp(string mail)
	{
		this.inscriptionFacebookPopUp.transform.GetComponent<InscriptionFacebookPopUpController> ().reset(mail);
		this.isInscriptionFacebookPopUpDisplayed = true;
		this.inscriptionFacebookPopUp.SetActive (true);
		this.inscriptionFacebookPopUpResize ();
	}
	public void displayChangePasswordPopUp()
	{
		this.changePasswordPopUp.transform.GetComponent<AuthenticationChangePasswordPopUpController> ().reset();
		this.isChangePasswordPopUpDisplayed = true;
		this.changePasswordPopUp.SetActive (true);
		this.changePasswordPopUpResize ();
	}
	public void loginPopUpResize()
	{
		this.loginPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.loginPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.loginPopUp.GetComponent<LoginPopUpController> ().resize ();
		this.displayFacebookButton(true);
		this.facebookButton.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAuthentication.getReference(10);
	}
	public void inscriptionPopUpResize()
	{
		this.inscriptionPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.inscriptionPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.inscriptionPopUp.GetComponent<InscriptionPopUpController> ().resize ();
		this.displayFacebookButton(true);
		this.facebookButton.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAuthentication.getReference(11);
	}
	public void authenticationMessagePopUpResize()
	{
		this.authenticationMessagePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.authenticationMessagePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.authenticationMessagePopUp.GetComponent<AuthenticationMessagePopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void offlineModePopUpResize()
	{
		this.offlineModePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.offlineModePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.offlineModePopUp.GetComponent<OfflineModePopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void lostLoginPopUpResize()
	{
		this.lostLoginPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.lostLoginPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.lostLoginPopUp.GetComponent<LostLoginPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void emailNonActivatedPopUpResize()
	{
		this.emailNonActivatedPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.emailNonActivatedPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.emailNonActivatedPopUp.GetComponent<EmailNonActivatedPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void inscriptionFacebookPopUpResize()
	{
		this.inscriptionFacebookPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.inscriptionFacebookPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.inscriptionFacebookPopUp.GetComponent<InscriptionFacebookPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void existingAccountPopUpResize()
	{
		this.existingAccountPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.existingAccountPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.existingAccountPopUp.GetComponent<ExistingAccountPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void changePasswordPopUpResize()
	{
		this.changePasswordPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.5f, -2f);
		this.changePasswordPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.changePasswordPopUp.GetComponent<AuthenticationChangePasswordPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void hideLoginPopUp()
	{
		this.loginPopUp.SetActive (false);
		this.isLoginPopUpDisplayed = false;
	}
	public void hideInscriptionPopUp()
	{
		this.inscriptionPopUp.SetActive (false);
		this.isInscriptionPopUpDisplayed = false;
	}
	public void hideLostLoginPopUp()
	{
		this.lostLoginPopUp.SetActive (false);
		this.isLostLoginPopUpDisplayed = false;
	}
	public void hideAuthenticationMessagePopUp()
	{
		this.authenticationMessagePopUp.SetActive (false);
		this.isAuthenticationMessagePopUpDisplayed = false;
	}
	public void hideOfflineModePopUp()
	{
		this.offlineModePopUp.SetActive (false);
		this.isOfflineModePopUpDisplayed = false;
		BackOfficeController.instance.displayLoadingScreen ();
		this.loadLevels ();
	}
	public void hideEmailNonActivatedPopUp()
	{
		this.emailNonActivatedPopUp.SetActive (false);
		this.isEmailNonActivatedPopUpDisplayed = false;
	}
	public void hideInscriptionFacebookPopUp()
	{
		this.inscriptionFacebookPopUp.SetActive (false);
		this.isInscriptionFacebookPopUpDisplayed = false;
	}
	public void hideExistingAccountPopUp()
	{
		this.existingAccountPopUp.SetActive (false);
		this.isExistingAccountPopUpDisplayed = false;
	}
	public void hideChangePasswordPopUp()
	{
		this.changePasswordPopUp.SetActive (false);
		this.isChangePasswordPopUpDisplayed = false;
	}
	public void displayFacebookButton(bool value)
	{
		if(ApplicationDesignRules.isMobileDevice) // A remplacer
		{
			this.facebookButton.SetActive(value);
		}
		else
		{
			this.facebookButton.SetActive(false);
		}
	}
	public string checkLogin(string login)
	{
		if(login=="")
		{
			return WordingAuthentication.getReference(1);
		} 
		return "";
	}
	public string checkTouAccepted(bool touAccepted)
	{
		if(!touAccepted)
		{
			return WordingAuthentication.getReference(15);
		} 
		return "";
	}
	public string checkUsername(string username)
	{
		if(username=="")
		{
			return WordingAuthentication.getReference(1);
		}
		else if(username.Length<4 )
		{
			return WordingAuthentication.getReference(2);
		}
		else if(!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
		{
			return WordingAuthentication.getReference(3);
		}
		else if(username.Length>12)
		{
			return WordingAuthentication.getReference(12);
		}      
		return "";
	}
	public string checkPasswordEgality (string password1, string password2)
	{
		if(password1=="")
		{
			return WordingAuthentication.getReference(4);
		}
		else if(password2=="")
		{
			return WordingAuthentication.getReference(5);
		}
		else if(password1!=password2)
		{
			return WordingAuthentication.getReference(6);
		}
		return "";
	}
	public string checkPasswordComplexity(string password)
	{
		if(password.Length<5)
		{
			return WordingAuthentication.getReference(7);
		}
		else if(!Regex.IsMatch(password, @"^[a-zA-Z0-9_.@]+$"))
		{
			return WordingAuthentication.getReference(8);
		} 
		return "";
	}
	public string checkEmail(string email)
	{
		if(!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
		{
				return WordingAuthentication.getReference(9);
		}
		return "";
	}
	public void returnPressed()
	{
		if(this.isLoginPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.loginHandler();
		}
		if(this.isInscriptionPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.inscriptionHandler();
		}
		if(this.isLostLoginPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.lostLoginHandler();
		}
		if(this.isEmailNonActivatedPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.emailNonActivatedHandler();
		}
		if(this.isChangePasswordPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.changePasswordHandler();
		}
		if (this.isOfflineModePopUpDisplayed) 
		{
			SoundController.instance.playSound (8);
			this.hideOfflineModePopUp ();
		}
	}
	public void escapePressed()
	{
		if(this.isInscriptionPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.displayLoginPopUp();
			this.hideInscriptionPopUp();
		}
		if(this.isLostLoginPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.displayLoginPopUp();
			this.hideLostLoginPopUp();
		}
		if(this.isEmailNonActivatedPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.displayLoginPopUp();
			this.hideEmailNonActivatedPopUp();
		}
		if(this.isChangePasswordPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.displayLoginPopUp();
			this.hideChangePasswordPopUp();
		}
		if (this.isOfflineModePopUpDisplayed) 
		{
			SoundController.instance.playSound (8);
			this.hideOfflineModePopUp ();
		}
	}
	private void loadLevels()
	{
		SoundController.instance.playMusic(new int[]{0,1,2});
		if(ApplicationModel.player.TutorialStep!=-1)
		{
			switch(ApplicationModel.player.TutorialStep)
			{
			case 0:
                BackOfficeController.instance.loadScene("Tutorial");	
				break;
			case 1:
				StartCoroutine(BackOfficeController.instance.joinTutorialGame());
				break;
			case 2: case 3:
				BackOfficeController.instance.loadScene("NewHomePage");	
				break;
			case 4:
				BackOfficeController.instance.loadScene("NewStore");
				break;
			case 5:
				BackOfficeController.instance.loadScene("newMyGame");
				break;
			case 6:
				BackOfficeController.instance.loadScene("NewHomePage");	
				break;
			default:
				BackOfficeController.instance.loadScene("NewHomePage");
				break;
			}
		}
        else if(ApplicationModel.player.HasLostConnectionDuringGame)
        {
            BackOfficeController.instance.loadScene("EndGame");
        }
		else
		{
            BackOfficeController.instance.loadScene("NewHomePage");
		}

	}
	void OnJoinedLobby()
	{
		this.loadLevels();
	}
	private void retrievePlayerData()
	{
		
	}

	#region Facebook

	private bool isConnectedToFB()
	{
		if(ApplicationDesignRules.isMobileDevice) // a remplacer
		{
			return FB.IsLoggedIn;
		}
		else
		{
			return false;
		}
	}
	private void initFacebookSDK()
	{
		if(ApplicationDesignRules.isMobileDevice) // a remplacer
		{
			if(!FB.IsInitialized)
			{
				FB.Init(InitCallback,OnHideUnity);
			}
			else
			{
				FB.ActivateApp();
				this.autoLogging();
			}
		}
		else
		{
			this.autoLogging();
		}
	}
	private void InitCallback()
	{
		if(FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		else
		{
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
		this.autoLogging();
	}
	private void OnHideUnity(bool isGameShown)
	{
		if(!isGameShown)
		{
			Time.timeScale=0;
		}
		else
		{
			Time.timeScale=1;
		}
	}
	private void facebookLogin()
	{
		BackOfficeController.instance.displayLoadingScreen();
		var perms = new List<string>(){"public_profile","email","user_friends"};
		FB.LogInWithReadPermissions(perms,AuthCallback);
	}
	private void AuthCallback(ILoginResult result)
	{
		if(FB.IsLoggedIn)
		{
			AccessToken aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			ApplicationModel.player.FacebookId=aToken.UserId;
//			foreach (string perm in aToken.Permissions)
//			{
//				//Debug.Log(perm);
//			}
			FB.API("/me?fields=email",HttpMethod.GET,GraphResult =>
			{
				if(string.IsNullOrEmpty(GraphResult.Error)==false)
				{
					return;
				}
				ApplicationModel.player.Mail=GraphResult.ResultDictionary["email"] as string;
				StartCoroutine(this.login());
			});
		}
		else
		{
			BackOfficeController.instance.hideLoadingScreen();
			//Debug.Log("User cancelled login");
		}
	}
	#endregion facebook
}
