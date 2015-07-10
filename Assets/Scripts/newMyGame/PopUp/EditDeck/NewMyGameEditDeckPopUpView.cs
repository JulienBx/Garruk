using UnityEngine;

public class NewMyGameEditDeckPopUpView : MonoBehaviour
{
	public NewPopUpViewModel popUpVM;
	public NewMyGameEditDeckPopUpViewModel editDeckPopUpVM;
	
	public NewMyGameEditDeckPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.editDeckPopUpVM = new NewMyGameEditDeckPopUpViewModel ();
	}
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), popUpVM.transparentStyle);
		{
		}
		GUILayout.EndArea ();
		GUI.enabled = editDeckPopUpVM.guiEnabled;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Modifiez le nom de votre deck "+editDeckPopUpVM.oldName, popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					editDeckPopUpVM.newName = GUILayout.TextField(editDeckPopUpVM.newName, popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(editDeckPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(editDeckPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Renommer", popUpVM.centralWindowButtonStyle,GUILayout.Width(0.2f*popUpVM.centralWindow.width)))
					{
						newMyGameController.instance.editDeckHandler();
					}
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle,GUILayout.Width(0.2f*popUpVM.centralWindow.width)))
					{
						newMyGameController.instance.hideEditDeckPopUp();
					}
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}

