using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using Facebook.Unity;
using System.Collections.Generic;


public class AuthenticationController : Photon.MonoBehaviour 
{
	public static AuthenticationController instance;

	public Sprite[] languagesSprites;
	
	private GameObject backOfficeController;
	private GameObject mainLogo;
	private GameObject chooseLanguageButton;
	private GameObject facebookButton;

	private GameObject loginPopUp;
	private bool isLoginPopUpDisplayed;
	private GameObject inscriptionPopUp;
	private bool isInscriptionPopUpDisplayed;
	private GameObject accountCreatedPopUp;
	private bool isAccountCreatedPopUpDisplayed;
	private GameObject lostLoginPopUp;
	private bool isLostLoginPopUpDisplayed;
	private GameObject passwordResetPopUp;
	private bool isPasswordResetPopUpDisplayed;

	private GameObject mainCamera;
	private GameObject backgroundCamera;
	private GameObject sceneCamera;

	void Start()
	{
		//Debug.Log("AWAKE"+gameObject.GetInstanceID());
		instance = this;
		this.initPlayer();
		this.initLanguage();
		this.initializeScene ();
		this.initializeBackOffice();
		this.initFacebookSDK();
		this.resize ();
		this.drawChooseLanguageButton();
		if(ApplicationModel.player.ToDeconnect)
		{
			ApplicationModel.player.ToDeconnect=false;
			this.displayLoginPopUp();
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(this.isConnectedToFB())
		{
			StartCoroutine(this.login());
		}
		else
		{
			StartCoroutine (this.checkPermanentConnection ());
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
		ApplicationModel.player=new Player();
		ApplicationModel.player.Username=lastUsername;
		ApplicationModel.player.IdLanguage=lastIDLanguage;
		ApplicationModel.player.ToDeconnect=lastToDeconnect;
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeAuthenticationController>();
		this.backOfficeController.GetComponent<BackOfficeAuthenticationController>().initialize();
	}
	private IEnumerator checkPermanentConnection()
	{
		yield return StartCoroutine(ApplicationModel.player.permanentConnexion ());
		if(ApplicationModel.player.Error=="" && ApplicationModel.player.Id!=-1)
		{
			this.connectToPhoton();
		}
		else
		{
			if(ApplicationModel.player.Error!="")
			{
				ApplicationModel.player.Error="";
			}
			this.displayLoginPopUp();
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public void initializeScene()
	{ 
		this.loginPopUp=GameObject.Find("loginPopUp");
		this.loginPopUp.SetActive(false);
		this.inscriptionPopUp=GameObject.Find("inscriptionPopUp");
		this.inscriptionPopUp.SetActive(false);
		this.accountCreatedPopUp=GameObject.Find("accountCreatedPopUp");
		this.accountCreatedPopUp.SetActive(false);
		this.lostLoginPopUp=GameObject.Find("lostLoginPopUp");
		this.lostLoginPopUp.SetActive(false);
		this.passwordResetPopUp=GameObject.Find("passwordResetPopUp");
		this.passwordResetPopUp.SetActive(false);
		this.mainCamera = gameObject;
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.mainLogo = GameObject.Find("mainLogo");
		this.chooseLanguageButton = GameObject.Find("chooseLanguageButton");
		this.facebookButton=GameObject.Find("FacebookButton");
	}
	private void connectToPhoton()
	{
		BackOfficeController.instance.changeLoadingScreenLabel (WordingAuthentication.getReference(0));
		PhotonNetwork.playerName = ApplicationModel.player.Username;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
	}
	public void facebookHandler()
	{
		StartCoroutine(this.login());
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
				StartCoroutine(this.login());
			}
		}
		this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError(error);
	}
	public IEnumerator login()
	{
		this.loginPopUp.SetActive(false);
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.Login());
		if(ApplicationModel.player.Error=="" && ApplicationModel.player.Id!=-1 && ApplicationModel.player.IsAccountActivated && ApplicationModel.player.IsAccountCreated)
		{
			this.connectToPhoton();
		}
		else if(ApplicationModel.player.Error!="")
		{
			this.loginPopUp.SetActive(true);
			this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError(ApplicationModel.player.Error);
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(!ApplicationModel.player.IsAccountCreated) // A MODIFIER
		{
			this.loginPopUp.SetActive(true);
			this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError("Compte non créé, veuillez créer le compte");
			BackOfficeController.instance.hideLoadingScreen();
		}
		else if(!ApplicationModel.player.IsAccountActivated) // A MODIFIER
		{
			this.loginPopUp.SetActive(true);
			this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError("Compte non activé, veuillez valider le mail");
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public void inscriptionHandler()
	{
		string login = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController>().getLogin();
		string password1 = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().getFirstPassword();
		string password2 = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().getSecondPassword();
		string email = this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController>().getEmail();
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
						StartCoroutine(this.createNewAccount(login,password1,email));
					}
				}
			}
		}
		this.inscriptionPopUp.GetComponent<InscriptionPopUpController>().setError(error);	
	}
	public void lostLoginHandler()
	{
		this.displayPasswordResetPopUp();
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
		this.facebookButton.transform.position= new Vector3(0f,ApplicationDesignRules.popUpWorldSize.y/2f-0.7f+0.25f+ApplicationDesignRules.roundButtonWorldSize.y/2f);

		if(this.isLoginPopUpDisplayed)
		{
			this.loginPopUpResize();
		}
		if(this.isInscriptionPopUpDisplayed)
		{
			this.inscriptionPopUpResize();
		}
		if(this.isAccountCreatedPopUpDisplayed)
		{
			this.accountCreatedPopUpResize();
		}
	}
	public void chooseLanguageHandler()
	{
		if(ApplicationModel.player.IdLanguage==1)
		{
			ApplicationModel.player.IdLanguage=0;
		}
		else
		{
			ApplicationModel.player.IdLanguage=1;
		}
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
		if(this.isPasswordResetPopUpDisplayed)
		{
			this.passwordResetPopUp.GetComponent<PasswordResetPopUpController>().computeLabels();
		}
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
		if(this.isInscriptionPopUpDisplayed)
		{
			this.hideInscriptionPopUp();
		}
		if(this.isAccountCreatedPopUpDisplayed)
		{
			this.hideAccountCreatedPopUp();
		}
		if(this.isLostLoginPopUpDisplayed)
		{
			this.hideLostLoginPopUp();
		}
		if(this.isPasswordResetPopUpDisplayed)
		{
			this.hidePasswordResetPopUp();
		}
		this.loginPopUp.transform.GetComponent<LoginPopUpController> ().reset(ApplicationModel.player.Username,false);
		this.isLoginPopUpDisplayed = true;
		this.loginPopUp.SetActive (true);
		this.loginPopUpResize ();
	}
	public void displayLostLoginPopUp()
	{
		if(this.isLoginPopUpDisplayed)
		{
			this.hideLoginPopUp();
		}
		this.lostLoginPopUp.transform.GetComponent<LostLoginPopUpController> ().reset(ApplicationModel.player.Username);
		this.isLostLoginPopUpDisplayed = true;
		this.lostLoginPopUp.SetActive (true);
		this.lostLoginPopUpResize ();
		this.displayFacebookButton(false);
	}
	public void displayInscriptionPopUp()
	{
		if(this.isLoginPopUpDisplayed)
		{
			this.hideLoginPopUp();
		}
		this.inscriptionPopUp.transform.GetComponent<InscriptionPopUpController> ().reset();
		this.isInscriptionPopUpDisplayed = true;
		this.inscriptionPopUp.SetActive (true);
		this.inscriptionPopUpResize ();
	}
	public void displayAccountCreatedPopUp()
	{
		if(this.isInscriptionPopUpDisplayed)
		{
			this.hideInscriptionPopUp();
		}
		this.accountCreatedPopUp.transform.GetComponent<AccountCreatedPopUpController> ().reset();
		this.isAccountCreatedPopUpDisplayed = true;
		this.accountCreatedPopUp.SetActive (true);
		this.accountCreatedPopUpResize ();
	}
	public void displayPasswordResetPopUp()
	{
		if(this.isLostLoginPopUpDisplayed)
		{
			this.hideLostLoginPopUp();
		}
		this.passwordResetPopUp.transform.GetComponent<PasswordResetPopUpController> ().reset();
		this.isPasswordResetPopUpDisplayed = true;
		this.passwordResetPopUp.SetActive (true);
		this.passwordResetPopUpResize ();
	}
	public void loginPopUpResize()
	{
		this.loginPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.7f, -2f);
		this.loginPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.loginPopUp.GetComponent<LoginPopUpController> ().resize ();
		this.displayFacebookButton(true);
		this.facebookButton.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAuthentication.getReference(10);
	}
	public void inscriptionPopUpResize()
	{
		this.inscriptionPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.7f, -2f);
		this.inscriptionPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.inscriptionPopUp.GetComponent<InscriptionPopUpController> ().resize ();
		this.displayFacebookButton(true);
		this.facebookButton.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAuthentication.getReference(11);
	}
	public void accountCreatedPopUpResize()
	{
		this.accountCreatedPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.7f, -2f);
		this.accountCreatedPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.accountCreatedPopUp.GetComponent<AccountCreatedPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void lostLoginPopUpResize()
	{
		this.lostLoginPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.7f, -2f);
		this.lostLoginPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.lostLoginPopUp.GetComponent<LostLoginPopUpController> ().resize ();
		this.displayFacebookButton(false);
	}
	public void passwordResetPopUpResize()
	{
		this.passwordResetPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y-0.7f, -2f);
		this.passwordResetPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.passwordResetPopUp.GetComponent<PasswordResetPopUpController> ().resize ();
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
	public void hideAccountCreatedPopUp()
	{
		this.accountCreatedPopUp.SetActive (false);
		this.isAccountCreatedPopUpDisplayed = false;
	}
	public void hideLostLoginPopUp()
	{
		this.lostLoginPopUp.SetActive (false);
		this.isLostLoginPopUpDisplayed = false;
	}
	public void hidePasswordResetPopUp()
	{
		this.passwordResetPopUp.SetActive (false);
		this.isPasswordResetPopUpDisplayed = false;
	}
	public void displayFacebookButton(bool value)
	{
		if(ApplicationDesignRules.isMobileScreen) // A modifer plus tard par IsMobileDevice
		{
			this.facebookButton.SetActive(value);
		}
		else
		{
			this.facebookButton.SetActive(false);
		}
	}
	public IEnumerator createNewAccount(string login, string password, string email)
	{
		this.inscriptionPopUp.SetActive(false);
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.createAccount(login,email,password));
		BackOfficeController.instance.hideLoadingScreen();
		this.inscriptionPopUp.SetActive(true);
		if(ApplicationModel.player.Error!="")
		{
			this.inscriptionPopUp.GetComponent<InscriptionPopUpController>().setError(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
		}
		else
		{
			this.displayAccountCreatedPopUp();
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
			this.loginHandler();
		}
		if(this.isInscriptionPopUpDisplayed)
		{
			this.inscriptionHandler();
		}
		if(this.isAccountCreatedPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
		if(this.isLostLoginPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
		if(this.isPasswordResetPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
	}
	public void escapePressed()
	{
		if(this.isInscriptionPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
		if(this.isAccountCreatedPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
		if(this.isLostLoginPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
		if(this.isPasswordResetPopUpDisplayed)
		{
			this.displayLoginPopUp();
		}
	}
	private void loadLevels()
	{
		if(ApplicationModel.player.TutorialStep!=-1)
		{
			switch(ApplicationModel.player.TutorialStep)
			{
			case 0:
				Application.LoadLevelAsync("Tutorial");	
				break;
			case 1:
				Application.LoadLevelAsync("NewStore");	
				break;
			case 2:
				Application.LoadLevelAsync("newMyGame");	
				break;
			case 3:case 4:
				Application.LoadLevelAsync("NewHomePage");
				break;
			default:
				Application.LoadLevelAsync("NewHomePage");
				break;
			}
		}
		else
		{
			Application.LoadLevelAsync("NewHomePage");
		}

	}
	void OnJoinedLobby()
	{
		this.loadLevels();
	}

	#region Facebook

	private bool isConnectedToFB()
	{
		if(ApplicationDesignRules.isMobileScreen)
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
		if(ApplicationDesignRules.isMobileScreen)
		{
			if(!FB.IsInitialized)
			{
				FB.Init(InitCallback,OnHideUnity);
			}
			else
			{
				FB.ActivateApp();
			}
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
		var perms = new List<string>(){"public_profile","email","user_friends"};
		FB.LogInWithReadPermissions(perms,AuthCallback);
	}
	private void AuthCallback(ILoginResult result)
	{
		if(FB.IsLoggedIn)
		{
			AccessToken aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			ApplicationModel.player.FacebookId=aToken.UserId;
			foreach (string perm in aToken.Permissions)
			{
				Debug.Log(perm);
			}
			FB.API("/me?fields=email",HttpMethod.GET,GraphResult =>
			{
				if(string.IsNullOrEmpty(GraphResult.Error)==false)
				{
					return;
				}
				ApplicationModel.player.Mail=GraphResult.ResultDictionary["email"] as string;
			});
		}
		else
		{
			Debug.Log("User cancelled login");
		}
	}
	#endregion facebook
}
