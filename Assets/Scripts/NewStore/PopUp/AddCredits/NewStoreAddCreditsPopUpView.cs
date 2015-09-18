using UnityEngine;

public class NewStoreAddCreditsPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewStoreAddCreditsPopUpViewModel addCreditsPopUpVM;
	
	public NewStoreAddCreditsPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.addCreditsPopUpVM = new NewStoreAddCreditsPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), popUpVM.transparentStyle);
		{
		}
		GUILayout.EndArea ();
		GUI.enabled = addCreditsPopUpVM.guiEnabled;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Cristaux à ajouter :",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(popUpVM.centralWindow.width*1/10);
					addCreditsPopUpVM.credits=GUILayout.TextField(addCreditsPopUpVM.credits,10,popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(popUpVM.centralWindow.width*1/10);
				}
				GUILayout.EndHorizontal();
				if(addCreditsPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(addCreditsPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewStoreController.instance.addCreditsHandler();
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Annuler",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewStoreController.instance.hideAddCreditsPopUp();
					}
					GUILayout.FlexibleSpace();
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


