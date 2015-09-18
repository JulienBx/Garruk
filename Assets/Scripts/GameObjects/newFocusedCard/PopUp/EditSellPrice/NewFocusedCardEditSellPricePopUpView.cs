using UnityEngine;

public class NewFocusedCardEditSellPricePopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewFocusedCardEditSellPricePopUpViewModel editSellPricePopUpVM;
	
	public NewFocusedCardEditSellPricePopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.editSellPricePopUpVM = new NewFocusedCardEditSellPricePopUpViewModel ();
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
				GUILayout.Label("Changer le prix de vente de l'unité sur le marché", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					editSellPricePopUpVM.price = GUILayout.TextField(editSellPricePopUpVM.price, 9,popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(editSellPricePopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(editSellPricePopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Confirmer", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().editSellPriceCardHandler();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().hideEditSellPricePopUp();
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


