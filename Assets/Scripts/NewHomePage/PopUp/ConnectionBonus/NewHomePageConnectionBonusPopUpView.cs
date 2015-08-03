using UnityEngine;

public class NewHomePageConnectionBonusPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewHomePageConnectionBonusPopUpViewModel connectionBonusPopUpVM;
	
	public NewHomePageConnectionBonusPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.connectionBonusPopUpVM = new NewHomePageConnectionBonusPopUpViewModel ();
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
				GUILayout.Label ("Première connection de la journée, vous gagnez "+connectionBonusPopUpVM.bonus+" crédits",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewHomePageController.instance.hideConnectionBonusPopUp();
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


