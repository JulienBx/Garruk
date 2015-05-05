using UnityEngine;

public class RenameCardPopUpView : MonoBehaviour
{
	
	public CardPopUpViewModel popUpVM;
	public RenameCardPopUpViewModel renamePopUpVM;
	
	public RenameCardPopUpView ()
	{
		this.popUpVM = new CardPopUpViewModel ();
		this.renamePopUpVM = new RenameCardPopUpViewModel ();
	}
	
	void OnGUI()
	{
		GUI.depth = popUpVM.guiDepth;
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Renommer la carte pour " + renamePopUpVM.price + " crédits", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					renamePopUpVM.newTitle = GUILayout.TextField(renamePopUpVM.newTitle, 14, popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(renamePopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label (renamePopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Confirmer", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().renameCard();
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						gameObject.GetComponent<CardController>().hideRenameCardPopUp();;
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


