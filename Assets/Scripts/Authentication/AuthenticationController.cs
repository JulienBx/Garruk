using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;


public class AuthenticationController : Photon.MonoBehaviour 
{

	public static AuthenticationController instance;
	
	private GameObject backOfficeController;
	private GameObject createAccountButton;

	private GameObject loginPopUp;
	private bool isLoginPopUpDisplayed;

	private GameObject mainCamera;
	private GameObject backgroundCamera;
	private GameObject sceneCamera;

	void Awake()
	{
		instance = this;
		this.initializeScene ();
		this.initializeBackOffice();
		StartCoroutine (this.initialization ());
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeAuthenticationController>();
		this.backOfficeController.GetComponent<BackOfficeAuthenticationController>().initialize();
	}
	private IEnumerator initialization()
	{
		this.resize ();
		yield return StartCoroutine(ApplicationModel.player.permanentConnexion ());
		this.displayLoginPopUp();
		if(ApplicationModel.player.Username!=""&& !ApplicationModel.player.ToDeconnect)
		{
			this.connectToPhoton();
		}
		else
		{
			ApplicationModel.player.ToDeconnect=false;
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public void initializeScene()
	{ 
		this.createAccountButton = GameObject.Find ("createAccountButton");
		this.createAccountButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingAuthentication.getReference(0);
		this.loginPopUp=GameObject.Find("loginPopUp");
		this.loginPopUp.SetActive(false);
		this.mainCamera = gameObject;
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.sceneCamera = GameObject.Find ("sceneCamera");
	}
	private void connectToPhoton()
	{
		BackOfficeController.instance.changeLoadingScreenLabel ("Connexion au lobby ...");
		PhotonNetwork.playerName = ApplicationModel.player.Username;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
	}
	public void loginHandler()
	{
		string login = this.loginPopUp.transform.GetComponent<LoginPopUpController>().getLogin();
		string password = this.loginPopUp.transform.GetComponent<LoginPopUpController> ().getPassword();
		string error = this.checkUsername(login);
		if(error=="")
		{
			error=this.checkPasswordComplexity(password);
			if(error=="")
			{
				StartCoroutine(this.login(login,password,this.loginPopUp.transform.GetComponent<LoginPopUpController> ().getRememberMe()));
			}
		}
		this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError(error);
	}
	public IEnumerator login(string login, string password, bool rememberMe)
	{
		this.loginPopUp.SetActive(false);
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.Login(login,password,rememberMe));
		if(ApplicationModel.player.Error=="")
		{
			this.connectToPhoton();
		}
		else
		{
			this.loginPopUp.SetActive(true);
			this.loginPopUp.transform.GetComponent<LoginPopUpController> ().setError(ApplicationModel.player.Error);
			BackOfficeController.instance.hideLoadingScreen();
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
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
		if(this.isLoginPopUpDisplayed)
		{
			this.loginPopUpResize();
		}
	}
	public void displayLoginPopUp()
	{
		this.loginPopUp.transform.GetComponent<LoginPopUpController> ().reset(ApplicationModel.player.Username,false);
		this.isLoginPopUpDisplayed = true;
		this.loginPopUp.SetActive (true);
		this.loginPopUpResize ();
	}
	public void loginPopUpResize()
	{
		this.loginPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.loginPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.loginPopUp.GetComponent<LoginPopUpController> ().resize ();
	}
	public void hideLoginPopUp()
	{
		this.loginPopUp.SetActive (false);
		this.isLoginPopUpDisplayed = false;
	}
//	public IEnumerator createNewAccount()
//	{
//		this.authenticationWindowView.authenticationWindowPopUpVM.usernameError=this.checkUsername(this.authenticationWindowView.authenticationWindowPopUpVM.username);
//		if(this.authenticationWindowView.authenticationWindowPopUpVM.usernameError=="")
//		{
//			this.authenticationWindowView.authenticationWindowPopUpVM.emailError=this.checkEmail(this.authenticationWindowView.authenticationWindowPopUpVM.email);
//			if(this.authenticationWindowView.authenticationWindowPopUpVM.emailError=="")
//			{
//				this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=this.checkPasswordEgality(this.authenticationWindowView.authenticationWindowPopUpVM.password1,this.authenticationWindowView.authenticationWindowPopUpVM.password2);
//				if(this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=="")
//				{
//					this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=this.checkPasswordComplexity(this.authenticationWindowView.authenticationWindowPopUpVM.password1);
//					if(this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=="")
//					{
//						this.authenticationWindowView.authenticationWindowPopUpVM.guiEnabled = false;
//						yield return StartCoroutine(ApplicationModel.player.createAccount(this.authenticationWindowView.authenticationWindowPopUpVM.username,this.authenticationWindowView.authenticationWindowPopUpVM.email,this.authenticationWindowView.authenticationWindowPopUpVM.password1));
//						if(ApplicationModel.player.Error!="")
//						{
//							BackOfficeController.instance.displayErrorPopUp(ApplicationModel.player.Error);
//							ApplicationModel.player.Error="";
//						}
//						else
//						{
//							view.authenticationVM.username=this.authenticationWindowView.authenticationWindowPopUpVM.username;
//							this.hideAuthenticationWindowPopUp();
//							this.displayAccountCreatedPopUp();
//						}
//					}
//				}
//			}
//		}
//	}
	public string checkUsername(string username)
	{
		if(username=="")
		{
			return "Veuillez saisir un pseudo";
		}
		else if(username.Length<4 )
		{
			return "Le pseudo doit comporter au moins 3 caractères";
		}
		else if(!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
		{
			return "Vous ne pouvez pas utiliser de caractères spéciaux";
		}   
		return "";
	}
	public string checkPasswordEgality (string password1, string password2)
	{
		if(password1=="")
		{
			return "Veuillez saisir un mot de passe";
		}
		else if(password2=="")
		{
			return "Veuillez confirmer votre mot de passe";
		}
		else if(password1!=password2)
		{
			return "Les deux mots de passes doivent être identiques";
		}
		return "";
	}
	public string checkPasswordComplexity(string password)
	{
		if(password.Length<5)
		{
			return "Le mot de passe doit comporter au moins 5 caractères";
		}
		else if(!Regex.IsMatch(password, @"^[a-zA-Z0-9_.@]+$"))
		{
			return "Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .";
		} 
		return "";
	}
	public string checkEmail(string email)
	{
		if(!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
		{
				return "Veuillez saisir une adresse email valide";
		}
		return "";
	}
	public void returnPressed()
	{
//		if(this.authenticationWindowView!=null)
//		{
//			StartCoroutine(this.createNewAccount());
//		}
//		if(this.accountCreatedView!=null)
//		{
//			this.hideAccountCreatedPopUp();
//		}
	}
	public void escapePressed()
	{
//		if(this.authenticationWindowView!=null)
//		{
//			this.hideAuthenticationWindowPopUp();
//		}
//		if(this.accountCreatedView!=null)
//		{
//			this.hideAccountCreatedPopUp();
//		}
	}
	private void loadLevels()
	{
		if(ApplicationModel.player.TutorialStep!=-1)
		{
			switch(ApplicationModel.player.TutorialStep)
			{
			case 0:
				Application.LoadLevel("Tutorial");	
				break;
			case 1:
				Application.LoadLevel("NewStore");	
				break;
			case 2:
				Application.LoadLevel("newMyGame");	
				break;
			case 3:case 4:
				Application.LoadLevel("NewHomePage");
				break;
			default:
				Application.LoadLevel("NewHomePage");
				break;
			}
		}
		else
		{
			Application.LoadLevel("NewHomePage");
		}
	}
	void OnJoinedLobby()
	{
		this.loadLevels();
	}
}
