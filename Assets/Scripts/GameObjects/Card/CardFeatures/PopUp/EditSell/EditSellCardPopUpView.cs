using UnityEngine;

public class EditSellCardPopUpView : MonoBehaviour
{
	
	public CardPopUpViewModel popUpVM;
	public EditSellCardPopUpViewModel editSellPopUpVM;
	
	public EditSellCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.editSellPopUpVM = new EditSellCardPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("La carte est mise en vente sur le bazar pour "+ editSellPopUpVM.price+" crédits. Modifier ?", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Retirer du bazar", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().unsellCard();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Modifier son prix", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().displayEditSellPriceCardPopUp();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().hideEditSellCardPopUp();
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


