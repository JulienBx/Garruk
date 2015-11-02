using UnityEngine;

public class NewStoreSelectCardTypePopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewStoreSelectCardTypePopUpViewModel selectCardTypePopUpVM;
	
	public NewStoreSelectCardTypePopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.selectCardTypePopUpVM = new NewStoreSelectCardTypePopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), popUpVM.transparentStyle);
		{
		}
		GUILayout.EndArea ();
		GUI.enabled = selectCardTypePopUpVM.guiEnabled;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Choisissez la classe :",popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					selectCardTypePopUpVM.cardTypeSelected = GUILayout.SelectionGrid(selectCardTypePopUpVM.cardTypeSelected,selectCardTypePopUpVM.cardTypes, 1, popUpVM.centralWindowSelGridStyle,GUILayout.Width(popUpVM.centralWindow.width*8f/10f));
					GUILayout.FlexibleSpace();
					
				}
				GUILayout.EndHorizontal();
				if(selectCardTypePopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(selectCardTypePopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewStoreController.instance.buyPackWidthCardTypeHandler();
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Annuler",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						NewStoreController.instance.hideSelectCardPopUp();
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


