//using UnityEngine;
//
//public class HomePageConnectionBonusPopUpView : MonoBehaviour
//{
//	
//	public HomePagePopUpViewModel popUpVM;
//	public HomePageConnectionBonusPopUpViewModel connectionBonusPopUpVM;
//	
//	public HomePageConnectionBonusPopUpView ()
//	{
//		this.popUpVM = new HomePagePopUpViewModel ();
//		this.connectionBonusPopUpVM = new HomePageConnectionBonusPopUpViewModel ();
//	}
//	
//	void OnGUI()
//	{
//		GUI.depth = popUpVM.guiDepth;
//		GUILayout.BeginArea(popUpVM.centralWindow);
//		{
//			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.Label ("Première connection de la journée, vous gagnez "+connectionBonusPopUpVM.bonus+" crédits",popUpVM.centralWindowTitleStyle);
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
//					{
//						HomePageController.instance.hideConnectionBonusPopUp();
//					}
//					GUILayout.FlexibleSpace();
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
//
