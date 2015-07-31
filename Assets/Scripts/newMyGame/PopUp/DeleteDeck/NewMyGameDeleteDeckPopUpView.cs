using UnityEngine;

public class NewMyGameDeleteDeckPopUpView : MonoBehaviour
{
	public NewPopUpViewModel popUpVM;
	public NewMyGameDeleteDeckPopUpViewModel deleteDeckPopUpVM;
	
	public NewMyGameDeleteDeckPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.deleteDeckPopUpVM = new NewMyGameDeleteDeckPopUpViewModel ();
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
				GUILayout.Label("Confirmez vous la suppression du deck "+deleteDeckPopUpVM.name, popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Confirmer", popUpVM.centralWindowButtonStyle,GUILayout.Width(0.2f*popUpVM.centralWindow.width)))
					{
						newMyGameController.instance.deleteDeckHandler();
					}
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle,GUILayout.Width(0.2f*popUpVM.centralWindow.width)))
					{
						newMyGameController.instance.hideDeleteDeckPopUp();
					}
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}

