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
				GUILayout.Label("Unité sur le marché pour "+ editSellPopUpVM.price+" cristaux. Modifier ?", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Retirer du marché", popUpVM.centralWindowButtonStyle))
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
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
	}
}


