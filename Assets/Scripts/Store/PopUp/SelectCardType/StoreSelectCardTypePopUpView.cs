using UnityEngine;

public class StoreSelectCardTypePopUpView : MonoBehaviour
{
	
	public StorePopUpViewModel popUpVM;
	public StoreSelectCardTypePopUpViewModel selectCardTypePopUpVM;
	
	public StoreSelectCardTypePopUpView ()
	{
		this.popUpVM = new StorePopUpViewModel ();
		this.selectCardTypePopUpVM = new StoreSelectCardTypePopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
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
						StoreController.instance.getCardsWithCardTypeHandler();
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Annuler",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
					{
						StoreController.instance.hideSelectCardTypePopUp();
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


