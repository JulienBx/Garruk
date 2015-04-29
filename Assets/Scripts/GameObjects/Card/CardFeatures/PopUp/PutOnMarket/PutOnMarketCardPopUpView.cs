using UnityEngine;

public class PutOnMarketCardPopUpView : MonoBehaviour
{
	
	public CardPopUpViewModel popUpVM;
	public PutOnMarketCardPopUpViewModel putOnMarketPopUpVM;
	
	public PutOnMarketCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.putOnMarketPopUpVM = new PutOnMarketCardPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
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
						StartCoroutine(gameObject.GetComponent<CardController>().putOnMarketCard());
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().hideputOnMarketCardPopUp();
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

