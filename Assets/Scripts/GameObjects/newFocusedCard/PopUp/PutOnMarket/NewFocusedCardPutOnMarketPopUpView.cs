using UnityEngine;

public class NewFocusedCardPutOnMarketPopUpView : MonoBehaviour
{
	
	public NewPopUpViewModel popUpVM;
	public NewFocusedCardPutOnMarketPopUpViewModel putOnMarketPopUpVM;
	
	public NewFocusedCardPutOnMarketPopUpView ()
	{
		this.popUpVM = new NewPopUpViewModel ();
		this.putOnMarketPopUpVM = new NewFocusedCardPutOnMarketPopUpViewModel ();
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
				GUILayout.Label("Choisir le prix en vente de la carte sur le bazar", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					putOnMarketPopUpVM.price = GUILayout.TextField(putOnMarketPopUpVM.price, 9,popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(putOnMarketPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label (putOnMarketPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Confirmer", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().putOnMarketCardHandler();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<NewFocusedCardController>().hidePutOnMarketPopUp();
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


