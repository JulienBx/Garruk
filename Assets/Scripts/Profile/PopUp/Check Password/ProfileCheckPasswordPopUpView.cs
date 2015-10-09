//using UnityEngine;
//
//public class ProfileCheckPasswordPopUpView : MonoBehaviour
//{
//	
//	public ProfilePopUpViewModel popUpVM;
//	public ProfileCheckPasswordPopUpViewModel checkPasswordPopUpVM;
//	
//	public ProfileCheckPasswordPopUpView ()
//	{
//		this.popUpVM = new ProfilePopUpViewModel ();
//		this.checkPasswordPopUpVM = new ProfileCheckPasswordPopUpViewModel ();
//	}
//	
//	void OnGUI()
//	{
//		GUI.depth = popUpVM.guiDepth;
//		GUI.enabled = popUpVM.guiEnabled;
//		GUILayout.BeginArea(popUpVM.centralWindow);
//		{
//			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.Label ("Saisissez votre mot de passe",popUpVM.centralWindowTitleStyle);
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
//					checkPasswordPopUpVM.tempOldPassword = GUILayout.PasswordField(checkPasswordPopUpVM.tempOldPassword,'*',popUpVM.centralWindowTextfieldStyle);
//					GUILayout.Space(popUpVM.centralWindow.width*0.05f);
//				}
//				GUILayout.EndHorizontal();
//				if(checkPasswordPopUpVM.error!="")
//				{
//					GUILayout.FlexibleSpace();
//					GUILayout.Label (checkPasswordPopUpVM.error,popUpVM.centralWindowTitleStyle);
//				}
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
//					{
//						ProfileController.instance.checkPasswordHandler(checkPasswordPopUpVM.tempOldPassword);
//						
//					}
//					GUILayout.FlexibleSpace();
//					if (GUILayout.Button("Quitter",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
//					{
//						ProfileController.instance.hideCheckPasswordPopUp();
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
