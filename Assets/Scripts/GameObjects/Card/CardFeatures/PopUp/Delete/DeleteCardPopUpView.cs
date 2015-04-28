using UnityEngine;

public class DeleteCardPopUpView : MonoBehaviour
{
	
	public CardPopUpViewModel popUpVM;
	public DeleteCardPopUpViewModel deletePopUpVM;
	
	public DeleteCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.deletePopUpVM = new DeleteCardPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Confirmer la désintégration de la carte (rapporte " + deletePopUpVM.price + " crédits)", 
				                popUpVM.centralWindowTitleStyle);
				
				GUILayout.Space(0.02f * popUpVM.centralWindow.height);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Désintégrer", popUpVM.centralWindowButtonStyle))
					{
						StartCoroutine(gameObject.GetComponent<CardController>().deleteCard());
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().hideDeleteCardPopUp();
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


