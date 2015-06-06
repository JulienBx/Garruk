using UnityEngine;

public class AuthenticationWindowPopUpView : MonoBehaviour
{
	public AuthenticationPopUpViewModel popUpVM;
	public AuthenticationWindowPopUpViewModel authenticationWindowPopUpVM;
	
	public AuthenticationWindowPopUpView ()
	{
		this.popUpVM = new AuthenticationPopUpViewModel ();
		this.authenticationWindowPopUpVM = new AuthenticationWindowPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Saisissez vos identifiants", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label("Login", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					authenticationWindowPopUpVM.username = GUILayout.TextField(authenticationWindowPopUpVM.username, popUpVM.centralWindowTextfieldStyle, GUILayout.Height(popUpVM.centralWindow.height*10/100), GUILayout.Width(popUpVM.centralWindow.width*80/100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Mot de passe", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Space(3);
					authenticationWindowPopUpVM.password = GUILayout.PasswordField(authenticationWindowPopUpVM.password, "*"[0],popUpVM.centralWindowTextfieldStyle, GUILayout.Height(popUpVM.centralWindow.height*9/100),GUILayout.Width(popUpVM.centralWindow.width*80/100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					authenticationWindowPopUpVM.toMemorize = GUILayout.Toggle(authenticationWindowPopUpVM.toMemorize, "Memoriser vos identifiants", popUpVM.centralWindowToggleStyle, GUILayout.Width(popUpVM.centralWindow.width*60/100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				if(authenticationWindowPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(authenticationWindowPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUI.enabled = authenticationWindowPopUpVM.guiEnabled;
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Valider", popUpVM.centralWindowButtonStyle))
					{
						StartCoroutine(AuthenticationController.instance.login());
					}
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						AuthenticationController.instance.hideAuthenticationWindowPopUp();
					}
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}

