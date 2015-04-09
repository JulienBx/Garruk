using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class AuthenticationController : MonoBehaviour {
	
	private AuthenticationView authenticationView;
	public static AuthenticationController instance;
	public AuthenticationManager instanceManager;
	public AuthenticationViewModel authenticationViewModel; 


	//styles
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle centralWindowTextFieldStyle;
	public GUIStyle centralWindowPasswordFieldStyle;
	public GUIStyle centralWindowToggleStyle;

	void Start (){
		instance = this;
		this.authenticationView = GetComponent<AuthenticationView>();
		this.authenticationView.initStyles(centralWindowStyle,centralWindowTitleStyle,centralWindowButtonStyle,centralWindowTextFieldStyle,centralWindowPasswordFieldStyle,centralWindowToggleStyle);
		this.instanceManager = GetComponent<AuthenticationManager>();
		 
		this.authenticationViewModel = instanceManager.createAuthenticationViewModel();
		this.authenticationView.authenticationViewModel = this.authenticationViewModel;
		StartCoroutine(this.checkPermanentConnexion());
	}

	private IEnumerator checkPermanentConnexion(){

		yield return StartCoroutine(instanceManager.permanentConnexion(this.authenticationViewModel));
		
		if (!this.authenticationViewModel.isRemembered){
			if(ApplicationModel.toDeconnect==false){
				Application.LoadLevel("HomePage");
			}
			else{
				ApplicationModel.toDeconnect=false;
				authenticationView.toDisplay = true;
			}
		}
		else{
			authenticationView.toDisplay = true;
		}
	}

	public IEnumerator Login() 
	{
		yield return StartCoroutine (AuthenticationManager.instance.Login(this.authenticationViewModel));
	}
}
