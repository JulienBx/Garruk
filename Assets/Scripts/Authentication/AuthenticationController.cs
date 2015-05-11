using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class AuthenticationController : MonoBehaviour {
	
	public static AuthenticationController instance;
	private AuthenticationView view;
	private AuthenticationWindowPopUpView authenticationWindowView;
	
	public GUIStyle[] popUpVMStyle;

	void Start ()
	{
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <AuthenticationView>();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine(ApplicationModel.permanentConnexion ());
		if(ApplicationModel.username!=""&& !ApplicationModel.toDeconnect)
		{
			Application.LoadLevel("Homepage");
		}
		else
		{
			displayAuthenticationWindow();
		}
	}
	public IEnumerator login()
	{
		if(authenticationWindowView.authenticationWindowPopUpVM.username!="" || authenticationWindowView.authenticationWindowPopUpVM.password!="")
		{
			yield return StartCoroutine(ApplicationModel.Login(authenticationWindowView.authenticationWindowPopUpVM.username,
			                                    authenticationWindowView.authenticationWindowPopUpVM.password,
			                                    authenticationWindowView.authenticationWindowPopUpVM.toMemorize));
			if(ApplicationModel.username!=""&& !ApplicationModel.toDeconnect)
			{
				Application.LoadLevel("Homepage");
			}
			else
			{
				authenticationWindowView.authenticationWindowPopUpVM.error=ApplicationModel.error;
				ApplicationModel.error="";
			}
		}
		else
		{
			authenticationWindowView.authenticationWindowPopUpVM.error ="Veuillez saisir vos identifiants";
		}
	}
	public void displayAuthenticationWindow()
	{
		this.authenticationWindowView = Camera.main.gameObject.AddComponent <AuthenticationWindowPopUpView>();

		authenticationWindowView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			authenticationWindowView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		authenticationWindowView.popUpVM.initStyles();
		this.authenticationWindowPopUpResize ();
	}
	public void hideAuthenticationWindowPopUp()
	{
		Destroy (this.authenticationWindowView);
	}
	public void authenticationWindowPopUpResize()
	{
		authenticationWindowView.popUpVM.centralWindow = view.authenticationScreenVM.centralWindow;
		authenticationWindowView.popUpVM.resize ();
	}
	public void resize()
	{
		view.authenticationScreenVM.resize ();
		if(this.authenticationWindowView!=null)
		{
			this.authenticationWindowPopUpResize();
		}
	}
}
