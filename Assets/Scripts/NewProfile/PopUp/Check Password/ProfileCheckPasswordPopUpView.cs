using UnityEngine;

public class ProfileCheckPasswordPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public ProfileCheckPasswordPopUpViewModel checkPasswordPopUpVM;
	
	public ProfileCheckPasswordPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.checkPasswordPopUpVM = new ProfileCheckPasswordPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), popUpVM.transparentStyle);
		{
		}
		GUILayout.EndArea ();
		GUI.enabled = popUpVM.guiEnabled;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Saisissez votre mot de passe",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
					checkPasswordPopUpVM.tempOldPassword = GUILayout.PasswordField(checkPasswordPopUpVM.tempOldPassword,'*',popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
				}
				GUILayout.EndHorizontal();
				if(checkPasswordPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label (checkPasswordPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewProfileController.instance.checkPasswordHandler(checkPasswordPopUpVM.tempOldPassword);
						
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Quitter",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewProfileController.instance.hideCheckPasswordPopUp();
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


