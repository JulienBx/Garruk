using UnityEngine;

public class NewMenuDisconnectedPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewMenuDisconnectedPopUpViewModel disconnectPopUpVM;
	
	public NewMenuDisconnectedPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.disconnectPopUpVM = new NewMenuDisconnectedPopUpViewModel ();
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
				GUILayout.Label("Souhaitez-vous quitter le jeu ?",popUpVM.centralWindowTitleStyle);
				
				GUILayout.Space(0.02f * popUpVM.centralWindow.height);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Confirmer", popUpVM.centralWindowButtonStyle))
					{
						newMenuController.instance.logOutLink();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						newMenuController.instance.hideDisconnectedPopUp();
					}
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
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


