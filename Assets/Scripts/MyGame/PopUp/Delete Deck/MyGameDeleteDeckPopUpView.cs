//using UnityEngine;
//
//public class MyGameDeleteDeckPopUpView : MonoBehaviour
//{
//	public MyGamePopUpViewModel popUpVM;
//	public MyGameDeleteDeckPopUpViewModel deleteDeckPopUpVM;
//	
//	public MyGameDeleteDeckPopUpView ()
//	{
//		this.popUpVM = new MyGamePopUpViewModel ();
//		this.deleteDeckPopUpVM = new MyGameDeleteDeckPopUpViewModel ();
//	}
//	void OnGUI()
//	{
//		GUI.depth = popUpVM.guiDepth;
//		GUILayout.BeginArea(popUpVM.centralWindow);
//		{
//			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.Label("Confirmez vous la suppression du deck "+deleteDeckPopUpVM.name, popUpVM.centralWindowTitleStyle);
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
//					if (GUILayout.Button("Confirmer", popUpVM.centralWindowButtonStyle))
//					{
//						StartCoroutine(MyGameController.instance.deleteDeck());
//					}
//					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
//					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
//					{
//						MyGameController.instance.hideDeleteDeckPopUp();
//					}
//					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
//				}
//				GUILayout.EndHorizontal();
//				GUILayout.FlexibleSpace();
//			}
//			GUILayout.EndVertical();
//		}
//		GUILayout.EndArea();
//	}
//}
//
