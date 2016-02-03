using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;


public class AuthenticationController : Photon.MonoBehaviour 
{

	public static AuthenticationController instance;
	private AuthenticationView view;
	private AuthenticationWindowPopUpView authenticationWindowView;
	
	public GameObject loadingScreenObject;
	public GUIStyle[] popUpVMStyle;
	public GUIStyle[] authenticationVMStyle;

	public GameObject blockObject;
	public GameObject transparentBackgroundObject;
	
	private GameObject loadingScreen;
	private GameObject mainBlock;
	private GameObject transparentBackground;
	private GameObject connectionButton;
	private GameObject inscriptionButton;
	private GameObject menuCamera;

	private AuthenticationErrorPopUpView errorView;
	private AuthenticationAccountCreatedPopUpView accountCreatedView;

	private Rect centralWindow;
	private Rect messageWindow;

	private bool isLoadingScreenDisplayed;

	void Start ()
	{
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <AuthenticationView>();
		this.initStyles ();
		this.initializeScene ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	void Update()
	{
		if (Screen.width != ApplicationDesignRules.widthScreen || Screen.height != ApplicationDesignRules.heightScreen) 
		{
			this.resize();
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			this.escapePressed();
		}
	}
	private IEnumerator initialization()
	{
		this.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.permanentConnexion ());
		if(ApplicationModel.player.Username!=""&& !ApplicationModel.player.ToDeconnect)
		{
			this.connectToPhoton();
		}
		else
		{
			this.hideLoadingScreen();
		}
	}
	private void connectToPhoton()
	{
		this.loadingScreen.GetComponent<LoadingScreenController> ().changeLoadingScreenLabel ("Connexion au lobby ...");
		PhotonNetwork.playerName = ApplicationModel.player.Username;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
	}
	public IEnumerator login()
	{
		this.view.authenticationVM.error=this.checkUsername(this.view.authenticationVM.username);
		if(this.view.authenticationVM.error=="")
		{
			this.view.authenticationVM.error=this.checkPasswordComplexity(this.view.authenticationVM.password);
		}
		if(this.view.authenticationVM.error=="")
		{
			this.displayLoadingScreen();
			this.view.authenticationVM.guiEnabled=false;
			yield return StartCoroutine(ApplicationModel.player.Login(this.view.authenticationVM.username,
			                                                   this.view.authenticationVM.password,
			                                                   this.view.authenticationVM.toMemorize));
			if(ApplicationModel.player.Username!=""&& !ApplicationModel.player.ToDeconnect)
			{
				this.connectToPhoton();
			}
			else
			{
				this.view.authenticationVM.error=ApplicationModel.player.Error;
				ApplicationModel.player.Error="";
				this.view.authenticationVM.guiEnabled=true;
				this.hideLoadingScreen();
			}
		}
	}
	public void displayAuthenticationWindow()
	{
		view.authenticationVM.guiEnabled = false;
		this.transparentBackground=Instantiate(this.transparentBackgroundObject) as GameObject;
		this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
		this.authenticationWindowView = Camera.main.gameObject.AddComponent <AuthenticationWindowPopUpView>();

		authenticationWindowView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			authenticationWindowView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		authenticationWindowView.popUpVM.initStyles();
		this.authenticationWindowPopUpResize ();
	}
	public void displayErrorPopUp()
	{
		//this.transparentBackground=Instantiate(this.transparentBackgroundObject) as GameObject;
		//this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
		this.errorView = Camera.main.gameObject.AddComponent <AuthenticationErrorPopUpView>();
		
		errorView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			errorView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		errorView.popUpVM.initStyles();
		this.errorPopUpResize ();
	}
	public void displayAccountCreatedPopUp()
	{
		view.authenticationVM.guiEnabled = false;
		this.transparentBackground=Instantiate(this.transparentBackgroundObject) as GameObject;
		this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
		this.accountCreatedView = Camera.main.gameObject.AddComponent <AuthenticationAccountCreatedPopUpView>();
		
		accountCreatedView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			accountCreatedView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		accountCreatedView.popUpVM.initStyles();
		this.accountCreatedPopUpResize ();
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		authenticationWindowView.authenticationWindowPopUpVM.guiEnabled = true;
		//Destroy (this.transparentBackground);
	}
	public void hideAuthenticationWindowPopUp()
	{
		Destroy (this.authenticationWindowView);
		view.authenticationVM.guiEnabled = true;
		Destroy (this.transparentBackground);
	}
	public void hideAccountCreatedPopUp()
	{
		Destroy (this.accountCreatedView);
		view.authenticationVM.guiEnabled = true;
		Destroy (this.transparentBackground);
	}
	public void authenticationWindowPopUpResize()
	{
		authenticationWindowView.popUpVM.centralWindow = this.centralWindow;
		authenticationWindowView.popUpVM.resize ();
	}
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.messageWindow;
		errorView.popUpVM.resize ();
	}
	public void accountCreatedPopUpResize()
	{
		accountCreatedView.popUpVM.centralWindow = this.messageWindow;
		accountCreatedView.popUpVM.resize ();
	}
	public void initStyles()
	{
		view.authenticationVM.styles=new GUIStyle[this.authenticationVMStyle.Length];
		for(int i=0;i<this.authenticationVMStyle.Length;i++)
		{
			view.authenticationVM.styles[i]=this.authenticationVMStyle[i];
		}
		view.authenticationVM.initStyles();
	}
	public void initializeScene()
	{ 
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.inscriptionButton = GameObject.Find ("inscriptionButton");
		this.inscriptionButton.AddComponent<AuthenticationInscriptionButtonController> ();
		this.connectionButton = GameObject.Find ("connectionButton");
		this.connectionButton.AddComponent<AuthenticationConnectionButtonController> ();
		this.inscriptionButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Connexion";
		this.connectionButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Inscription";
		this.menuCamera = GameObject.Find ("MenuCamera");
	}
	public void resize()
	{
		ApplicationDesignRules.widthScreen=Screen.width;
		ApplicationDesignRules.heightScreen=Screen.height;
		ApplicationDesignRules.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		ApplicationDesignRules.worldWidth = ((float)Screen.width/(float)Screen.height) * ApplicationDesignRules.worldHeight;
		if((float)ApplicationDesignRules.widthScreen/(float)ApplicationDesignRules.heightScreen<1f)
		{
			this.menuCamera.GetComponent<Camera> ().orthographicSize=5f*((float)ApplicationDesignRules.heightScreen/(float)ApplicationDesignRules.widthScreen);
		}
		else
		{
			this.menuCamera.GetComponent<Camera> ().orthographicSize=5f;
		}

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.70f * ApplicationDesignRules.heightScreen);
		this.messageWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.2f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
		view.authenticationVM.resize (ApplicationDesignRules.heightScreen);
		if(this.authenticationWindowView!=null)
		{
			this.authenticationWindowPopUpResize();
		}
		if(this.errorView!=null)
		{
			this.errorPopUpResize();
		}
		if(this.accountCreatedView!=null)
		{
			this.accountCreatedPopUpResize();
		}
		
		float mainBlockWidth = ApplicationDesignRules.worldWidth-1f;
		if(mainBlockWidth>8f)
		{
			mainBlockWidth=8f;
		}
		float mainBlockLeftMargin = (ApplicationDesignRules.worldWidth-mainBlockWidth)/2f;
		float mainBlockRightMargin = mainBlockLeftMargin;
		float mainBlockUpMargin = 3f;
		float mainBlockDownMargin = 2f;
		float mainBlockHeight = ApplicationDesignRules.worldHeight - mainBlockUpMargin - mainBlockDownMargin;

		this.mainBlock.GetComponent<NewBlockController> ().resize(mainBlockLeftMargin,mainBlockUpMargin,mainBlockWidth,mainBlockHeight);

		Vector2 mainBlockSize = this.mainBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 mainBlockOrigin = this.mainBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		Vector2 mainBlockUpperLeft = this.mainBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();

		float mainBlockGUIWidth = ((mainBlockSize.x-1f) / (2f * Camera.main.GetComponent<Camera> ().orthographicSize)) * ApplicationDesignRules.heightScreen;
		float mainBlockGUIHeight = ((mainBlockSize.y-1f)/(2f * Camera.main.GetComponent<Camera> ().orthographicSize))* ApplicationDesignRules.heightScreen;
		float mainBlockGUIXOrigin = 0.5f * ApplicationDesignRules.widthScreen-mainBlockGUIWidth/2f;
		float mainBlockGUIYOrigin = ((ApplicationDesignRules.worldHeight / 2f-(mainBlockOrigin.y + 0.5f+ (mainBlockSize.y-1f) / 2f)) / ApplicationDesignRules.worldHeight) * ApplicationDesignRules.heightScreen;

		view.authenticationScreenVM.setMainBlock(new Rect(mainBlockGUIXOrigin,mainBlockGUIYOrigin,mainBlockGUIWidth,mainBlockGUIHeight));
	}
	public IEnumerator createNewAccount()
	{
		this.authenticationWindowView.authenticationWindowPopUpVM.usernameError=this.checkUsername(this.authenticationWindowView.authenticationWindowPopUpVM.username);
		if(this.authenticationWindowView.authenticationWindowPopUpVM.usernameError=="")
		{
			this.authenticationWindowView.authenticationWindowPopUpVM.emailError=this.checkEmail(this.authenticationWindowView.authenticationWindowPopUpVM.email);
			if(this.authenticationWindowView.authenticationWindowPopUpVM.emailError=="")
			{
				this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=this.checkPasswordEgality(this.authenticationWindowView.authenticationWindowPopUpVM.password1,this.authenticationWindowView.authenticationWindowPopUpVM.password2);
				if(this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=="")
				{
					this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=this.checkPasswordComplexity(this.authenticationWindowView.authenticationWindowPopUpVM.password1);
					if(this.authenticationWindowView.authenticationWindowPopUpVM.passwordError=="")
					{
						this.authenticationWindowView.authenticationWindowPopUpVM.guiEnabled = false;
						yield return StartCoroutine(ApplicationModel.player.createAccount(this.authenticationWindowView.authenticationWindowPopUpVM.username,this.authenticationWindowView.authenticationWindowPopUpVM.email,this.authenticationWindowView.authenticationWindowPopUpVM.password1));
						if(ApplicationModel.player.Error!="")
						{
							this.displayErrorPopUp();
							errorView.errorPopUpVM.error=ApplicationModel.player.Error;
							ApplicationModel.player.Error="";
						}
						else
						{
							view.authenticationVM.username=this.authenticationWindowView.authenticationWindowPopUpVM.username;
							this.hideAuthenticationWindowPopUp();
							this.displayAccountCreatedPopUp();
						}
					}
				}
			}
		}
	}
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
		if(this.errorView!=null)
		{
			this.hideErrorPopUp();
		}
		else if(this.authenticationWindowView!=null)
		{
			StartCoroutine(this.createNewAccount());
		}
		if(this.accountCreatedView!=null)
		{
			this.hideAccountCreatedPopUp();
		}
	}
	public void escapePressed()
	{
		if(this.errorView!=null)
		{
			this.hideErrorPopUp();
		}
		else if(this.authenticationWindowView!=null)
		{
			this.hideAuthenticationWindowPopUp();
		}
		if(this.accountCreatedView!=null)
		{
			this.hideAccountCreatedPopUp();
		}
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
		//TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);    
		this.loadLevels();
		//print (PhotonNetwork.connectionState);
	}
	public void displayLoadingScreen()
	{
		Camera.main.gameObject.GetComponent <AuthenticationView> ().enabled = false;
		if(!isLoadingScreenDisplayed)
		{
			this.loadingScreen=Instantiate(this.loadingScreenObject) as GameObject;
			this.isLoadingScreenDisplayed=true;
		}
	}
	public void hideLoadingScreen()
	{
		Camera.main.gameObject.GetComponent <AuthenticationView> ().enabled = true;
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
		}
	}
}
