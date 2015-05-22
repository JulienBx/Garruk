using UnityEngine;

public class BuyCardPopUpView : MonoBehaviour
{
		
	public CardPopUpViewModel popUpVM;
	public BuyCardPopUpViewModel buyPopUpVM;

	public BuyCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.buyPopUpVM = new BuyCardPopUpViewModel ();
	}

	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Confirmer l'achat de la carte (coûte "+buyPopUpVM.price+ " crédits)", popUpVM.centralWindowTitleStyle);
				GUILayout.Space(0.02f*popUpVM.centralWindow.height);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f*popUpVM.centralWindow.width);
					if (GUILayout.Button("Acheter",popUpVM.centralWindowButtonStyle)) // also can put width here
					{
						gameObject.GetComponent<CardController>().buyCard();
					}
					GUILayout.Space(0.04f*popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler",popUpVM.centralWindowButtonStyle)) // also can put width here
					{
						gameObject.GetComponent<CardController>().hideBuyCardPopUp();
					}
					GUILayout.Space(0.03f*popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
	}
	GUILayout.EndArea();
	}
}


