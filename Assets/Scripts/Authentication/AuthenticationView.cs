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

	void OnGUI()
	{
		GUI.enabled = authenticationVM.guiEnabled;
		GUILayout.BeginArea (authenticationScreenVM.mainBlock);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label("ou connectez-vous",authenticationVM.titleStyle,GUILayout.Height(authenticationScreenVM.mainBlock.height*7/100));
			GUILayout.FlexibleSpace();
			GUILayout.Label("Login", authenticationVM.labelStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				authenticationVM.username = GUILayout.TextField(authenticationVM.username, authenticationVM.textFieldStyle, GUILayout.Height(authenticationScreenVM.mainBlock.height*10/100), GUILayout.Width(authenticationScreenVM.mainBlock.width*80/100));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Mot de passe", authenticationVM.labelStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.Space(3);
				authenticationVM.password = GUILayout.PasswordField(authenticationVM.password, "*"[0],authenticationVM.textFieldStyle, GUILayout.Height(authenticationScreenVM.mainBlock.height*10/100),GUILayout.Width(authenticationScreenVM.mainBlock.width*80/100));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				authenticationVM.toMemorize = GUILayout.Toggle(authenticationVM.toMemorize, "Memoriser vos identifiants", authenticationVM.toggleStyle, GUILayout.Width(authenticationScreenVM.mainBlock.width*60/100));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			if(authenticationVM.error!="")
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label(authenticationVM.error,authenticationVM.errorStyle);
			}
			GUILayout.FlexibleSpace();
			GUI.enabled = authenticationVM.guiEnabled;
		}
		GUILayout.EndArea();
	}
}
