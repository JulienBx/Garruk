using UnityEngine;

public class NewFocusedCardNewCardTypePopUpView : MonoBehaviour
{
	
	public NewFocusedCardNewCardTypePopUpViewModel cardNewCardTypePopUpVM;
	public NewPopUpViewModel popUpVM;
	
	public NewFocusedCardNewCardTypePopUpView ()
	{
		this.cardNewCardTypePopUpVM = new NewFocusedCardNewCardTypePopUpViewModel ();
		this.popUpVM = new NewPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = cardNewCardTypePopUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow,popUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label("Félicications !! Vous avez débloqué la classe :",popUpVM.centralWindowTitleStyle);
			GUILayout.Label (cardNewCardTypePopUpVM.newCardType,popUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width(popUpVM.centralWindow.width/3f)))
				{
					gameObject.GetComponent<NewFocusedCardController>().hideNewCardTypePopUp();
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


