using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;


public class AuthenticationController : MonoBehaviour 
{
	
	public static AuthenticationController instance;
	private AuthenticationView view;
	private AuthenticationWindowPopUpView authenticationWindowView;
	
	public GUIStyle[] popUpVMStyle;
	public GUIStyle[] authenticationVMStyle;

	private AuthenticationErrorPopUpView errorView;
	private AuthenticationAccountCreatedPopUpView accountCreatedView;


	void Start ()
	{
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <AuthenticationView>();
		this.initStyles ();
		this.resize ();
		StartCoroutine (this.initialization ());
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
	}
	public void hideAuthenticationWindowPopUp()
	{
		Destroy (this.authenticationWindowView);
		view.authenticationVM.guiEnabled = true;
	}
	public void hideAccountCreatedPopUp()
	{
		Destroy (this.accountCreatedView);
		view.authenticationVM.password1="";
		view.authenticationVM.password2="";
		view.authenticationVM.email="";
		view.authenticationVM.username="";
		view.authenticationVM.guiEnabled = true;
	}
	public void authenticationWindowPopUpResize()
	{
		authenticationWindowView.popUpVM.centralWindow = view.authenticationScreenVM.centralWindow;
		authenticationWindowView.popUpVM.resize ();
	}
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = view.authenticationScreenVM.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void accountCreatedPopUpResize()
	{
		accountCreatedView.popUpVM.centralWindow = view.authenticationScreenVM.centralWindow;
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
	public void resize()
	{
		view.authenticationScreenVM.resize ();
		view.authenticationVM.resize (view.authenticationScreenVM.heightScreen);
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
	}
	public IEnumerator createNewAccount()
	{
		view.authenticationVM.usernameError=this.checkUsername(view.authenticationVM.username);
		view.authenticationVM.emailError=this.checkEmail(view.authenticationVM.email);
		view.authenticationVM.passwordError=this.checkPasswordEgality(view.authenticationVM.password1,view.authenticationVM.password2);
		if(view.authenticationVM.passwordError=="")
		{
			view.authenticationVM.passwordError=this.checkPasswordComplexity(view.authenticationVM.password1);
		}
		if(view.authenticationVM.passwordError=="" && view.authenticationVM.emailError=="" && view.authenticationVM.usernameError=="")
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
			Application.LoadLevel("NewHomepage");
			break;
		case 2:
			Application.LoadLevel("NewMyGame");
			break;
		case 3:
			Application.LoadLevel("NewLobby");
			break;
		case 5:
			Application.LoadLevel("EndGame");
			break;
		case 6:
			Application.LoadLevel("NewStore");
			break;
		default:
			Application.LoadLevel("NewHomePage");	
			break;
		}
	}
}
