using UnityEngine;

public class ProfileChangePasswordPopUpView : MonoBehaviour
{
	
	public ProfilePopUpViewModel popUpVM;
	public ProfileChangePasswordPopUpViewModel changePasswordPopUpVM;
	
	public ProfileChangePasswordPopUpView ()
	{
		this.popUpVM = new ProfilePopUpViewModel ();
		this.changePasswordPopUpVM = new ProfileChangePasswordPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUI.enabled = popUpVM.guiEnabled;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Entrez votre nouveau mot de passe",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (popUpVM.centralWindow.width*0.05f);
					changePasswordPopUpVM.tempNewPassword = GUILayout.PasswordField(changePasswordPopUpVM.tempNewPassword,'*',popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space (popUpVM.centralWindow.width*0.05f);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Confirmer la saisie",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (popUpVM.centralWindow.width*0.05f);
					changePasswordPopUpVM.tempNewPassword2 = GUILayout.PasswordField(changePasswordPopUpVM.tempNewPassword2,'*',popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space (popUpVM.centralWindow.width*0.05f);
				}
				GUILayout.EndHorizontal();
				if(changePasswordPopUpVM.passwordsCheck!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label (changePasswordPopUpVM.passwordsCheck,popUpVM.centralWindowTitleStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						ProfileController.instance.editPasswordHandler();
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Quitter",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						ProfileController.instance.hideChangePasswordPopUp();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}


