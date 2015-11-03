using UnityEngine;

public class NewCardTypePopUpView : MonoBehaviour
{
	
	public NewCardTypePopUpViewModel cardNewCardTypePopUpVM;
	public NewPopUpViewModel popUpVM;
	
	public NewCardTypePopUpView ()
	{
		this.cardNewCardTypePopUpVM = new NewCardTypePopUpViewModel ();
		this.popUpVM = new NewPopUpViewModel ();
	}
	void OnGUI()
	{
		GUI.depth = cardNewCardTypePopUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow,popUpVM.centralWindowStyle);
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label("Félicications !! Vous avez débloqué la faction :",popUpVM.centralWindowTitleStyle);
			GUILayout.Label (cardNewCardTypePopUpVM.newCardType,popUpVM.centralWindowTitleStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width(popUpVM.centralWindow.width/3f)))
				{
					MenuController.instance.hideNewCardTypePopUp();
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


