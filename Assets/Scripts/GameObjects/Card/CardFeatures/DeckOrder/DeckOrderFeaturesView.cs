//using System;
//using UnityEngine;
//
//public class DeckOrderFeaturesView : MonoBehaviour
//{
//	public DeckOrderFeaturesViewModel deckOrderFeaturesVM;
//	
//	public DeckOrderFeaturesView ()
//	{
//		this.deckOrderFeaturesVM = new DeckOrderFeaturesViewModel ();
//	}
//	void OnGUI()
//	{
//		GUI.enabled = deckOrderFeaturesVM.guiEnabled;
//		GUI.enabled = deckOrderFeaturesVM.buttonsEnabled;
//		if(deckOrderFeaturesVM.displayLeftArrow)
//		{
//			GUILayout.BeginArea (deckOrderFeaturesVM.leftArrowRect);
//			{
//				if(GUILayout.Button("",deckOrderFeaturesVM.leftArrowStyle))
//				{
//					gameObject.GetComponent<CardController> ().changeDeckOrder(true);
//				}
//			}
//			GUILayout.EndArea ();
//		}
//		if(deckOrderFeaturesVM.displayRightArrow)
//		{
//			GUILayout.BeginArea (deckOrderFeaturesVM.rightArrowRect);
//			{
//				if(GUILayout.Button("",deckOrderFeaturesVM.rightArrowStyle))
//				{
//					gameObject.GetComponent<CardController> ().changeDeckOrder(false);
//				}
//			}
//			GUILayout.EndArea ();
//		}
//		GUI.enabled = deckOrderFeaturesVM.guiEnabled;
//		GUILayout.BeginArea (deckOrderFeaturesVM.deckOrderNameRect);
//		{
//			GUILayout.Label(deckOrderFeaturesVM.deckOrderName,deckOrderFeaturesVM.titleStyle);
//		}
//		GUILayout.EndArea ();
//	}
//}
//
