using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationView : MonoBehaviour
{
	public AuthenticationScreenViewModel authenticationScreenVM;
	public AuthenticationViewModel authenticationVM;

	public AuthenticationView()
	{
		this.authenticationScreenVM= new AuthenticationScreenViewModel();
		this.authenticationVM = new AuthenticationViewModel ();
	}
	void Update()
	{
		if (Screen.width != authenticationScreenVM.widthScreen || Screen.height != authenticationScreenVM.heightScreen) 
		{
			AuthenticationController.instance.resize();
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			AuthenticationController.instance.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			AuthenticationController.instance.escapePressed();
		}
	}

	void OnGUI()
	{
		GUI.enabled = authenticationVM.guiEnabled;
		GUILayout.BeginArea (authenticationScreenVM.mainBlock);
		{
			if(GUILayout.Button("Connectez vous",authenticationVM.buttonStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*7/100)))
			{
				AuthenticationController.instance.displayAuthenticationWindow();
			}
			GUILayout.Label("ou inscrivez-vous",authenticationVM.titleStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*7/100));
			GUILayout.Label("Pseudo",authenticationVM.labelStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			authenticationVM.username=GUILayout.TextField(authenticationVM.username,14,authenticationVM.textFieldStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			if(authenticationVM.usernameError!="")
			{
				GUILayout.Label(authenticationVM.usernameError,authenticationVM.errorStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			}
			GUILayout.Label("Adresse email",authenticationVM.labelStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			authenticationVM.email=GUILayout.TextField(authenticationVM.email,authenticationVM.textFieldStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			if(authenticationVM.emailError!="")
			{
				GUILayout.Label(authenticationVM.emailError,authenticationVM.errorStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			}
			GUILayout.Label("Mot de passe", authenticationVM.labelStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			authenticationVM.password1 = GUILayout.PasswordField(authenticationVM.password1, "*"[0],authenticationVM.passwordFieldStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			GUILayout.Label("Confirmez votre mot de passe", authenticationVM.labelStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			authenticationVM.password2 = GUILayout.PasswordField(authenticationVM.password2, "*"[0],authenticationVM.passwordFieldStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			if(authenticationVM.passwordError!="")
			{
				GUILayout.Label(authenticationVM.passwordError,authenticationVM.errorStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*5/100));
			}
			GUILayout.Space(authenticationScreenVM.mainBlock.height*5/100);
			if(GUILayout.Button("Inscription",authenticationVM.buttonStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*7/100)))
			{
				StartCoroutine(AuthenticationController.instance.createNewAccount());
			}
		}
		GUILayout.EndArea();
	}
}
