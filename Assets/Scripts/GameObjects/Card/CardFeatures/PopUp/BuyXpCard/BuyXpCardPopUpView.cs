//using UnityEngine;
//
//public class BuyXpCardPopUpView : MonoBehaviour
//{
//	
//	public CardPopUpViewModel popUpVM;
//	public BuyXpCardPopUpViewModel buyXpPopUpVM;
//	
//	public BuyXpCardPopUpView ()
//	{
//		this.popUpVM = new CardPopUpViewModel ();
//		this.buyXpPopUpVM = new BuyXpCardPopUpViewModel ();
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
//				GUILayout.Label("Confirmer la montée de niveau de la carte (coûte " + buyXpPopUpVM.price + " crédits)",popUpVM.centralWindowTitleStyle);
//				GUILayout.Space(0.02f * popUpVM.centralWindow.height);
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.Space(0.03f * popUpVM.centralWindow.width);
//					if (GUILayout.Button("Acheter", popUpVM.centralWindowButtonStyle))
//					{
//						gameObject.GetComponent<CardController>().buyXpCard();
//					}
//					GUILayout.Space(0.04f * popUpVM.centralWindow.width);
//					if (GUILayout.Button("Annuler", popUpVM.centralWindowButtonStyle))
//					{
//						gameObject.GetComponent<CardController>().hideBuyXpCardPopUp();
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
//
