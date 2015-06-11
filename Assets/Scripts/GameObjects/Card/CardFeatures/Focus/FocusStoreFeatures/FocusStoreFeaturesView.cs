using System;
using UnityEngine;
public class FocusStoreFeaturesView : MonoBehaviour
{

	public FocusStoreFeaturesViewModel focusStoreFeaturesVM;
	public CardFeaturesFocusViewModel cardFeaturesFocusVM;

	public FocusStoreFeaturesView ()
	{
		this.focusStoreFeaturesVM = new FocusStoreFeaturesViewModel ();
		this.cardFeaturesFocusVM = new CardFeaturesFocusViewModel ();
	}
	void OnGUI()
	{
		GUI.enabled = cardFeaturesFocusVM.guiEnabled;
		GUILayout.BeginArea (focusStoreFeaturesVM.cardFeaturesFocusRects[0]);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[0].width*1/20);
				if(GUILayout.Button("Désintégrer (+"+ focusStoreFeaturesVM.cardCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardController>().displaySellCardPopUp();
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[0].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusStoreFeaturesVM.cardFeaturesFocusRects[1]);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[1].width*1/20);
				if(focusStoreFeaturesVM.cardLevel<10)
				{
					if(ApplicationModel.credits<focusStoreFeaturesVM.nextLevelCost)
					{
						GUI.enabled=false;
					}
					if(GUILayout.Button("Passer au niveau suivant (-"+ focusStoreFeaturesVM.nextLevelCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
					{
						gameObject.GetComponent<CardController>().displayBuyXpCardPopUp();
					}
					GUI.enabled = cardFeaturesFocusVM.guiEnabled;
				}
				else
				{
					GUI.enabled=false;
					if(GUILayout.Button("La carte a atteint son niveau max", cardFeaturesFocusVM.buttonStyle))
					{
					}
					GUI.enabled = cardFeaturesFocusVM.guiEnabled;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[1].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusStoreFeaturesVM.cardFeaturesFocusRects[2]);
		{
			GUILayout.BeginHorizontal();
			{
				if(ApplicationModel.credits<focusStoreFeaturesVM.renameCost)
				{
					GUI.enabled=false;
				}
				GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[2].width*1/20);
				if(GUILayout.Button("Renommer la carte pour (-"+ focusStoreFeaturesVM.renameCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardController>().displayRenameCardPopUp();
				}
				GUI.enabled = cardFeaturesFocusVM.guiEnabled;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[2].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusStoreFeaturesVM.cardFeaturesFocusRects[3]);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[3].width*1/20);
				if(focusStoreFeaturesVM.isOnSale)
				{
					if(GUILayout.Button("La carte est mise en vente sur le bazar pour "+ focusStoreFeaturesVM.price+" crédits. Modifier ?", cardFeaturesFocusVM.buttonStyle))
					{
						gameObject.GetComponent<CardController>().displayEditSellCardPopUp();
					}
				}
				else
				{
					if(GUILayout.Button("Mettre la carte en vente sur le bazar", cardFeaturesFocusVM.buttonStyle))
					{
						gameObject.GetComponent<CardController>().displayputOnMarketCardPopUp();
					}
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[3].height*1/20);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (focusStoreFeaturesVM.cardFeaturesFocusRects[4]);
		{
			GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[4].height*1/20);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusStoreFeaturesVM.cardFeaturesFocusRects[4].width*1/20);
				GUI.enabled=focusStoreFeaturesVM.exitButtonEnabled;
				if(GUILayout.Button("Quitter", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardStoreController>().exitFocus();
				}
				GUI.enabled = cardFeaturesFocusVM.guiEnabled;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
}

