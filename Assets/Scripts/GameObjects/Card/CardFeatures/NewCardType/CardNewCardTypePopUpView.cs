using UnityEngine;

public class CardNewCardTypePopUpView : MonoBehaviour
{
	
	public CardNewCardTypePopUpViewModel cardNewCardTypePopUpVM;
	
	public CardNewCardTypePopUpView ()
	{
		this.cardNewCardTypePopUpVM = new CardNewCardTypePopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = cardNewCardTypePopUpVM.guiDepth;
		GUILayout.BeginArea(cardNewCardTypePopUpVM.centralWindow,cardNewCardTypePopUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label("Félicications !! Vous avez débloqué la classe :",cardNewCardTypePopUpVM.centralWindowTitleStyle);
			GUILayout.Label (cardNewCardTypePopUpVM.newCardType,cardNewCardTypePopUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("OK",cardNewCardTypePopUpVM.centralWindowButtonStyle,GUILayout.Width(cardNewCardTypePopUpVM.centralWindow.width/3f)))
				{
					gameObject.GetComponent<CardController>().hideNewCardTypePopUp();
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


