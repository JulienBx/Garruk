using UnityEngine;

public class NewMyGameNewDeckPopUpView : MonoBehaviour
{
	public NewPopUpViewModel popUpVM;
	public NewMyGameNewDeckPopUpViewModel newDeckPopUpVM;
	
	public NewMyGameNewDeckPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.newDeckPopUpVM = new NewMyGameNewDeckPopUpViewModel ();
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
				GUILayout.Label("Choisissez le nom de votre nouvelle équipe", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
					newDeckPopUpVM.name = GUILayout.TextField(newDeckPopUpVM.name, popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.1f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(newDeckPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(newDeckPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Créer l'équipe", popUpVM.centralWindowButtonStyle,GUILayout.Width(0.2f*popUpVM.centralWindow.width)))
					{
						newMyGameController.instance.createNewDeckHandler();
					}
					GUILayout.Space(0.2f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle,GUILayout.Width(0.2f*popUpVM.centralWindow.width)))
					{
						newMyGameController.instance.hideNewDeckPopUp();
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

