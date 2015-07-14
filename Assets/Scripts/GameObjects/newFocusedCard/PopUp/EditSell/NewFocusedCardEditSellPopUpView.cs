using UnityEngine;

public class NewFocusedCardEditSellPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewFocusedCardEditSellPopUpViewModel editSellPopUpVM;
	
	public NewFocusedCardEditSellPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.editSellPopUpVM = new NewFocusedCardEditSellPopUpViewModel ();
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
				GUILayout.Label("La carte est mise en vente sur le bazar pour "+ editSellPopUpVM.price+" crédits. Modifier ?", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Retirer du bazar", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().unsellCardHandler();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Modifier son prix", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().displayEditSellPriceCardPopUp();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().hideEditSellPopUp();
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


