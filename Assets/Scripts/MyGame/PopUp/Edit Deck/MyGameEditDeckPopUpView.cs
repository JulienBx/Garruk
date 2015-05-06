using UnityEngine;

public class MyGameEditDeckPopUpView : MonoBehaviour
{
	public MyGamePopUpViewModel popUpVM;
	public MyGameEditDeckPopUpViewModel editDeckPopUpVM;
	
	public MyGameEditDeckPopUpView ()
	{
		this.popUpVM = new MyGamePopUpViewModel ();
		this.editDeckPopUpVM = new MyGameEditDeckPopUpViewModel ();
	}
	void OnGUI()
	{
		GUILayout.BeginArea(popUpVM.centralWindow);
		{
			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Modifiez le nom de votre deck "+editDeckPopUpVM.oldName, popUpVM.centralWindowTitleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					editDeckPopUpVM.newName = GUILayout.TextField(editDeckPopUpVM.newName, popUpVM.centralWindowTextfieldStyle);
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
				}
				GUILayout.EndHorizontal();
				if(editDeckPopUpVM.error!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(editDeckPopUpVM.error,popUpVM.centralWindowErrorStyle);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Renommer", popUpVM.centralWindowButtonStyle))
					{
						StartCoroutine(MyGameController.instance.editDeck());
					}
					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
					{
						MyGameController.instance.hideEditDeckPopUp();
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

