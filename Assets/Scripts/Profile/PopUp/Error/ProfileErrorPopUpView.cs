//using UnityEngine;
//
//public class ProfileErrorPopUpView : MonoBehaviour
//{
//	
//	public ProfilePopUpViewModel popUpVM;
//	public ProfileErrorPopUpViewModel errorPopUpVM;
//	
//	public ProfileErrorPopUpView ()
//	{
//		this.popUpVM = new ProfilePopUpViewModel ();
//		this.errorPopUpVM = new ProfileErrorPopUpViewModel ();
//	}
//	
//	void OnGUI()
//	{
//		GUI.depth = popUpVM.guiDepth;
//		GUI.enabled=true;
//		GUILayout.BeginArea(popUpVM.centralWindow);
//		{
//			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.Label (errorPopUpVM.error,popUpVM.centralWindowTitleStyle);
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
//					{
//						ProfileController.instance.reloadPage();
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
