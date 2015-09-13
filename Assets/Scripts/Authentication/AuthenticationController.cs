using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;


public class AuthenticationController : MonoBehaviour 
{
	
	public static AuthenticationController instance;
	private AuthenticationView view;
	private AuthenticationWindowPopUpView authenticationWindowView;
	
	public GUIStyle[] popUpVMStyle;
	public GUIStyle[] authenticationVMStyle;

	public GameObject blockObject;
	public GameObject transparentBackgroundObject;
	
	private GameObject mainBlock;
	private GameObject transparentBackground;
	private GameObject connectionButton;
	private GameObject inscriptionButton;

	private AuthenticationErrorPopUpView errorView;
	private AuthenticationAccountCreatedPopUpView accountCreatedView;

	private int widthScreen;
	private int heightScreen;

	private float worldHeight;
	private float worldWidth;

	private Rect centralWindow;

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
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
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
		yield return StartCoroutine(ApplicationModel.permanentConnexion ());
		if(ApplicationModel.username!=""&& !ApplicationModel.toDeconnect)
		{
			this.loadLevels();
		}
		else
		{
			view.authenticationVM.guiEnabled=true;
		}
	}
	public IEnumerator login()
	{
		authenticationWindowView.authenticationWindowPopUpVM.error=this.checkUsername(authenticationWindowView.authenticationWindowPopUpVM.username);
		if(authenticationWindowView.authenticationWindowPopUpVM.error=="")
		{
			authenticationWindowView.authenticationWindowPopUpVM.error=this.checkPasswordComplexity(authenticationWindowView.authenticationWindowPopUpVM.password);
		}
		if(authenticationWindowView.authenticationWindowPopUpVM.error=="")
		{
			authenticationWindowView.authenticationWindowPopUpVM.guiEnabled=false;
			yield return StartCoroutine(ApplicationModel.Login(authenticationWindowView.authenticationWindowPopUpVM.username,
			                                    authenticationWindowView.authenticationWindowPopUpVM.password,
			                                    authenticationWindowView.authenticationWindowPopUpVM.toMemorize));
			if(ApplicationModel.username!=""&& !ApplicationModel.toDeconnect)
			{
				this.loadLevels();
			}
			else
			{
				authenticationWindowView.authenticationWindowPopUpVM.error=ApplicationModel.error;
				ApplicationModel.error="";
				authenticationWindowView.authenticationWindowPopUpVM.guiEnabled=true;
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
		view.authenticationVM.guiEnabled = false;
		this.transparentBackground=Instantiate(this.transparentBackgroundObject) as GameObject;
		this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
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
		view.authenticationVM.guiEnabled = true;
		Destroy (this.transparentBackground);
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
		view.authenticationVM.password1="";
		view.authenticationVM.password2="";
		view.authenticationVM.email="";
		view.authenticationVM.username="";
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
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void accountCreatedPopUpResize()
	{
		accountCreatedView.popUpVM.centralWindow = this.centralWindow;
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
		this.connectionButton = GameObject.Find ("connectionButton");
		this.inscriptionButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Inscription";
		this.connectionButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Connectez-vous";

	}
	public void resize()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.35f * this.heightScreen);
		view.authenticationVM.resize (this.heightScreen);
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
		
		float mainBlockWidth = 8f;
		float mainBlockLeftMargin = (worldWidth-mainBlockWidth)/2f;
		float mainBlockRightMargin = mainBlockLeftMargin;
		float mainBlockUpMargin = 3f;
		float mainBlockDownMargin = 0.5f;
		
		float mainBlockHeight = worldHeight - mainBlockUpMargin-mainBlockDownMargin;
		Vector2 mainBlockOrigin = new Vector3 (-worldWidth/2f+mainBlockLeftMargin+mainBlockWidth/2f, -worldHeight / 2f + mainBlockDownMargin + mainBlockHeight / 2,0f);
		
		this.mainBlock.GetComponent<BlockController> ().resize(new Rect(mainBlockOrigin.x,mainBlockOrigin.y,mainBlockWidth,mainBlockHeight));

		float mainBlockGUIWidth = ((mainBlockWidth-2f) / (2f * Camera.main.GetComponent<Camera> ().orthographicSize)) * heightScreen;
		float mainBlockGUIHeight = ((mainBlockHeight)/(2f * Camera.main.GetComponent<Camera> ().orthographicSize))* heightScreen;
		float mainBlockGUIXOrigin = (((mainBlockOrigin.x - (mainBlockWidth-2f) / 2f) + worldWidth / 2f) / worldWidth) * widthScreen;
		float mainBlockGUIYOrigin = ((worldHeight / 2f-(mainBlockOrigin.y + mainBlockHeight / 2f)) / worldHeight) * heightScreen;

		view.authenticationScreenVM.setMainBlock(new Rect(mainBlockGUIXOrigin,mainBlockGUIYOrigin,mainBlockGUIWidth,mainBlockGUIHeight));
	}
	public IEnumerator createNewAccount()
	{
		view.authenticationVM.usernameError=this.checkUsername(view.authenticationVM.username);
		if(view.authenticationVM.usernameError=="")
		{
			view.authenticationVM.emailError=this.checkEmail(view.authenticationVM.email);
			if(view.authenticationVM.emailError=="")
			{
				view.authenticationVM.passwordError=this.checkPasswordEgality(view.authenticationVM.password1,view.authenticationVM.password2);
				if(view.authenticationVM.passwordError=="")
				{
					view.authenticationVM.passwordError=this.checkPasswordComplexity(view.authenticationVM.password1);
					if(view.authenticationVM.passwordError=="")
					{
						view.authenticationVM.guiEnabled = false;
						yield return StartCoroutine(ApplicationModel.createAccount(view.authenticationVM.username,view.authenticationVM.email,view.authenticationVM.password1));
						if(ApplicationModel.error!="")
						{
							this.displayErrorPopUp();
							errorView.errorPopUpVM.error=ApplicationModel.error;
							ApplicationModel.error="";
						}
						else
						{
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
		if(this.authenticationWindowView!=null)
		{
			this.login();
		}
		if(this.errorView!=null)
		{
			this.hideErrorPopUp();
		}
		if(this.accountCreatedView!=null)
		{
			this.hideAccountCreatedPopUp();
		}
	}
	public void escapePressed()
	{
		if(this.authenticationWindowView!=null)
		{
			this.hideAuthenticationWindowPopUp();
		}
		if(this.errorView!=null)
		{
			this.hideErrorPopUp();
		}
		if(this.accountCreatedView!=null)
		{
			this.hideAccountCreatedPopUp();
		}
	}
	private void loadLevels()
	{
		switch(ApplicationModel.tutorialStep)
		{
		case 0:
			Application.LoadLevel("Tutorial");	
			break;
		case 1:
			Application.LoadLevel("NewHomePage");
			break;
		case 2:
			Application.LoadLevel("NewMyGame");
			break;
		case 3:
			Application.LoadLevel("NewMyGame");
			break;
		case 4:
			Application.LoadLevel("NewHomePage");
			break;
		default:
			Application.LoadLevel("NewHomePage");	
			break;
		}
	}
}
