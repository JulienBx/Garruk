using UnityEngine;

public class AuthenticationAccountCreatedPopUpView : MonoBehaviour
{
	
	public AuthenticationPopUpViewModel popUpVM;
	public AuthenticationAccountCreatedPopUpViewModel accountCreatedPopUpVM;
	
	public AuthenticationAccountCreatedPopUpView ()
	{
		this.popUpVM = new AuthenticationPopUpViewModel ();
		this.accountCreatedPopUpVM = new AuthenticationAccountCreatedPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth-1;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label (accountCreatedPopUpVM.label,popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						AuthenticationController.instance.hideAccountCreatedPopUp();
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


