using UnityEngine;

public class MyGameNewDeckPopUpView : MonoBehaviour
{
	public MyGamePopUpViewModel popUpVM;
	public MyGameNewDeckPopUpViewModel newDeckPopUpVM;

	public MyGameNewDeckPopUpView ()
	{
		this.popUpVM = new MyGamePopUpViewModel ();
		this.newDeckPopUpVM = new MyGameNewDeckPopUpViewModel ();
	}
	void OnGUI()
	{
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Choisissez le nom de votre nouveau deck", popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					newDeckPopUpVM.name = GUILayout.TextField(newDeckPopUpVM.name, popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(newDeckPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(newDeckPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Créer le deck", popUpVM.centralWindowButtonStyle))
					{
						StartCoroutine(MyGameController.instance.createNewDeck());
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						MyGameController.instance.hideNewDeckPopUp();
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

