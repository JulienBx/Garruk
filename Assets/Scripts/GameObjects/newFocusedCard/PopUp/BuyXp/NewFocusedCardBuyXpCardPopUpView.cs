using UnityEngine;

public class NewFocusedCardBuyXpView: MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewFocusedCardBuyXpPopUpViewModel buyXpPopUpVM;
	
	public NewFocusedCardBuyXpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.buyXpPopUpVM = new  NewFocusedCardBuyXpPopUpViewModel ();
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
				GUILayout.Label("Confirmer la montée de niveau de l'unité (coûte " + buyXpPopUpVM.price + " cristaux)",popUpVM.centralWindowTitleStyle);
				GUILayout.Space(0.02f * popUpVM.centralWindow.height);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Acheter", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().buyXpCardHandler();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().hideBuyXpPopUp();
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


