using UnityEngine;

public class SellCardPopUpView : MonoBehaviour
{
	
	public CardPopUpViewModel popUpVM;
	public SellCardPopUpViewModel sellPopUpVM;
	
	public SellCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.sellPopUpVM = new SellCardPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Confirmer la désintégration de la carte (rapporte " + sellPopUpVM.price + " crédits)", 
				                popUpVM.centralWindowTitleStyle);
				
				GUILayout.Space(0.02f * popUpVM.centralWindow.height);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Désintégrer", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().sellCard();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().hideSellCardPopUp();
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


