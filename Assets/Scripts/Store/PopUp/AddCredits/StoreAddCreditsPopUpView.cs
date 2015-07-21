//using UnityEngine;
//
//public class StoreAddCreditsPopUpView : MonoBehaviour
//{
//	
//	public StorePopUpViewModel popUpVM;
//	public StoreAddCreditsPopUpViewModel addCreditsPopUpVM;
//	
//	public StoreAddCreditsPopUpView ()
//	{
//		this.popUpVM = new StorePopUpViewModel ();
//		this.addCreditsPopUpVM = new StoreAddCreditsPopUpViewModel ();
//	}
//	
//	void OnGUI()
//	{
//		GUI.depth = popUpVM.guiDepth;
//		GUI.enabled = addCreditsPopUpVM.guiEnabled;
//		GUILayout.BeginArea(popUpVM.centralWindow);
//		{
//			GUILayout.BeginVertical(popUpVM.centralWindowStyle);
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.Label ("Crédits à ajouter :",popUpVM.centralWindowTitleStyle);
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.Space(popUpVM.centralWindow.width*1/10);
//					addCreditsPopUpVM.credits=GUILayout.TextField(addCreditsPopUpVM.credits,10,popUpVM.centralWindowTextfieldStyle);
//					GUILayout.Space(popUpVM.centralWindow.width*1/10);
//				}
//				GUILayout.EndHorizontal();
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (GUILayout.Button("OK",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
//					{
//						StoreController.instance.addCreditsHandler();
//					}
//					GUILayout.FlexibleSpace();
//					if (GUILayout.Button("Annuler",popUpVM.centralWindowButtonStyle,GUILayout.Width (popUpVM.centralWindow.width*0.3f)))
//					{
//						StoreController.instance.hideAddCreditsPopUp();
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
