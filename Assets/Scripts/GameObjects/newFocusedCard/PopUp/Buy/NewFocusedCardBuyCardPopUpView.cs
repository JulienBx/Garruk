using UnityEngine;

public class NewFocusedCardBuyPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewFocusedCardBuyPopUpViewModel buyPopUpVM;
	
	public NewFocusedCardBuyPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.buyPopUpVM = new NewFocusedCardBuyPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), popUpVM.transparentStyle);
		{
		}
		GUILayout.EndArea ();
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
						gameObject.GetComponent<NewFocusedCardController>().buyCardHandler();
					}
					GUILayout.Space(0.04f*popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler",popUpVM.centralWindowButtonStyle)) // also can put width here
					{
						gameObject.GetComponent<NewFocusedCardController>().hideBuyPopUp();
					}
					GUILayout.Space(0.03f*popUpVM.centralWindow.width);
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


