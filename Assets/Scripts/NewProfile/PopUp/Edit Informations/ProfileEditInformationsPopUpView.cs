using UnityEngine;

public class ProfileEditInformationsPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public ProfileEditInformationsPopUpViewModel editInformationsPopUpVM;
	
	public ProfileEditInformationsPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.editInformationsPopUpVM = new ProfileEditInformationsPopUpViewModel ();
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
				GUILayout.Label ("Prénom",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
					editInformationsPopUpVM.tempFirstName = GUILayout.TextField(editInformationsPopUpVM.tempFirstName,popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Nom",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
					editInformationsPopUpVM.tempSurname = GUILayout.TextField(editInformationsPopUpVM.tempSurname,popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Mail",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
					editInformationsPopUpVM.tempMail = GUILayout.TextField(editInformationsPopUpVM.tempMail,popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
				}
				GUILayout.EndHorizontal();
				if(editInformationsPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label (editInformationsPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						//NewProfileController.instance.updateUserInformationsHandler();
						
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Quitter",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						//NewProfileController.instance.hideEditInformationsPopUp();
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


