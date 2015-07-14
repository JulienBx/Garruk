using UnityEngine;

public class NewFocusedCardErrorPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewFocusedCardErrorPopUpViewModel errorPopUpVM;
	
	public NewFocusedCardErrorPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.errorPopUpVM = new NewFocusedCardErrorPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), popUpVM.transparentStyle);
		{
		}
		GUILayout.EndArea ();
		GUI.enabled = popUpVM.guiEnabled;GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label (errorPopUpVM.error,popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						gameObject.GetComponent<NewFocusedCardController>().hideErrorPopUp();
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


