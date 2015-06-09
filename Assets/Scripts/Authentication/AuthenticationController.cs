using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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
		if(authenticationWindowView.authenticationWindowPopUpVM.username!="" || authenticationWindowView.authenticationWindowPopUpVM.password!="")
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
		else
		{
			authenticationWindowView.authenticationWindowPopUpVM.error ="Veuillez saisir vos identifiants";
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
		view.authenticationVM.guiEnabled = true;
		Application.LoadLevel("Homepage");
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
		view.authenticationVM.usernameError="";
		view.authenticationVM.emailError="";
		view.authenticationVM.passwordError="";
		if(this.checkUsername() && this.checkEmail() && this.checkPassword() )
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
	public bool checkUsername()
	{
		if(view.authenticationVM.username=="")
		{
			view.authenticationVM.usernameError="Veuillez saisir un pseudo";
			return false;
		}
		else if(view.authenticationVM.username.Length<4)
		{
			view.authenticationVM.usernameError="Le pseudo doit comporter au moins 3 caractères";
			return false;
		}
		return true;
	}
	public bool checkPassword()
	{
		if(view.authenticationVM.password1=="")
		{
			view.authenticationVM.passwordError="Veuillez saisir un mot de passe";
			return false;
		}
		else if(view.authenticationVM.password2=="")
		{
			view.authenticationVM.passwordError="Veuillez confirmer votre mot de passe";
			return false;
		}
		else if(view.authenticationVM.password1!=view.authenticationVM.password2)
		{
			view.authenticationVM.passwordError="Les deux mots de passes doivent être identiques";
			return false;
		}
		else if(view.authenticationVM.password1.Length<6)
		{
			view.authenticationVM.passwordError="Le mot de passe doit comporter au moins 6 caractères";
			return false;
		}
		return true;
	}
	public bool checkEmail()
	{
		if(view.authenticationVM.email=="")
		{
			view.authenticationVM.emailError="Veuillez saisir un email";
			return false;
		}
		else if(view.authenticationVM.email.Length<7 ||
			   !view.authenticationVM.email.Contains("@") || 
			   !view.authenticationVM.email.Substring(view.authenticationVM.email.Length - 4, 4).Contains("."))
		{
				view.authenticationVM.emailError="Veuillez saisir une adresse email valide";
				return false;
		}
		return true;
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
				Application.LoadLevel("Homepage");
			break;
		case 2:
			Application.LoadLevel("MyGame");
			break;
		case 3:
			Application.LoadLevel("Lobby");
			break;
		default:
			Application.LoadLevel("HomePage");	
			break;
		}
	}
}
