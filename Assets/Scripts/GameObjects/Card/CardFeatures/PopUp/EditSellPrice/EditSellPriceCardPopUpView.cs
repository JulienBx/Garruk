using UnityEngine;

public class EditSellPriceCardPopUpView : MonoBehaviour
{
	
	public CardPopUpViewModel popUpVM;
	public EditSellPriceCardPopUpViewModel editSellPricePopUpVM;
	
	public EditSellPriceCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.editSellPricePopUpVM = new EditSellPriceCardPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Changer le prix de vente de la carte sur le bazar", popUpVM.centralWindowTitleStyle);
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
						gameObject.GetComponent<CardController>().editSellPriceCard();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().hideEditSellPriceCardPopUp();
					}
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}


