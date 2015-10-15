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
		GUI.enabled = authenticationWindowPopUpVM.guiEnabled;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
				GUILayout.Label("Renseignez le formulaire suivant :",popUpVM.centralWindowTitleStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
				GUILayout.Label("Pseudo",popUpVM.centralWindowTitleStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				authenticationWindowPopUpVM.username=GUILayout.TextField(authenticationWindowPopUpVM.username,12,popUpVM.centralWindowTextfieldStyle,GUILayout.Height(popUpVM.centralWindow.height*5/100));
				GUILayout.Label ("le pseudo doit comporter doit comprendre au moins 3 caractères, vous pouvez utiliser des chiffres, des lettres et l'underscore",popUpVM.instructionsStyle);
				if(authenticationWindowPopUpVM.usernameError!="")
				{
					GUILayout.Label(authenticationWindowPopUpVM.usernameError,popUpVM.centralWindowErrorStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				}
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
				GUILayout.Label("Adresse email",popUpVM.centralWindowTitleStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				authenticationWindowPopUpVM.email=GUILayout.TextField(authenticationWindowPopUpVM.email,popUpVM.centralWindowTextfieldStyle,GUILayout.Height(popUpVM.centralWindow.height*5/100));
				if(authenticationWindowPopUpVM.emailError!="")
				{
					GUILayout.Label(authenticationWindowPopUpVM.emailError,popUpVM.centralWindowErrorStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				}
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
				GUILayout.Label("Mot de passe", popUpVM.centralWindowTitleStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				authenticationWindowPopUpVM.password1 = GUILayout.PasswordField(authenticationWindowPopUpVM.password1, "*"[0],popUpVM.centralWindowPasswordFieldStyle,GUILayout.Height(popUpVM.centralWindow.height*5/100));
				GUILayout.Label ("le mot de passe doit comprendre au moins 5 caractères, vous pouvez utiliser des lettres, des chiffres et les caractères sivants @_.",popUpVM.instructionsStyle);
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
				GUILayout.Label("Confirmez votre mot de passe", popUpVM.centralWindowTitleStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				authenticationWindowPopUpVM.password2 = GUILayout.PasswordField(authenticationWindowPopUpVM.password2, "*"[0],popUpVM.centralWindowPasswordFieldStyle,GUILayout.Height(popUpVM.centralWindow.height*5/100));
				if(authenticationWindowPopUpVM.passwordError!="")
				{
					GUILayout.Label(authenticationWindowPopUpVM.passwordError,popUpVM.centralWindowErrorStyle,GUILayout.Height(popUpVM.centralWindow.height*4/100));
				}
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
					if (GUILayout.Button("S'inscrire", popUpVM.centralWindowButtonStyle))
					{
						StartCoroutine(AuthenticationController.instance.createNewAccount ());
					}
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						AuthenticationController.instance.hideAuthenticationWindowPopUp();
					}
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(popUpVM.centralWindow.height*4/100);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();

		}
		GUILayout.EndArea();
	}
}

