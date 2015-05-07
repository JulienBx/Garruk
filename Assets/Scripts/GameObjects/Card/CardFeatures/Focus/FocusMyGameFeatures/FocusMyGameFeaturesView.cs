using UnityEngine ;

public class FocusMyGameFeaturesView : MonoBehaviour
{
	public FocusMyGameFeaturesViewModel focusMyGameFeaturesVM;
	public CardFeaturesFocusViewModel cardFeaturesFocusVM;
	
	public FocusMyGameFeaturesView ()
	{
		this.focusMyGameFeaturesVM = new FocusMyGameFeaturesViewModel();
		this.cardFeaturesFocusVM = new CardFeaturesFocusViewModel ();
	}
	
	void OnGUI()
	{
		GUI.enabled = cardFeaturesFocusVM.guiEnabled;
		if(focusMyGameFeaturesVM.idOwner!=-1)
		{
			GUILayout.BeginArea (focusMyGameFeaturesVM.cardFeaturesFocusRects[0]);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[0].width*1/20);
					if(GUILayout.Button("Désintégrer (+"+ focusMyGameFeaturesVM.cardCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
					{
						gameObject.GetComponent<CardController>().displaySellCardPopUp();
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[0].height*1/20);
			}
			GUILayout.EndArea();
			GUILayout.BeginArea (focusMyGameFeaturesVM.cardFeaturesFocusRects[1]);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[1].width*1/20);
					if(focusMyGameFeaturesVM.cardLevel<10)
					{
						if(ApplicationModel.credits<focusMyGameFeaturesVM.nextLevelCost)
						{
							GUI.enabled=false;
						}
						if(GUILayout.Button("Passer au niveau suivant (-"+ focusMyGameFeaturesVM.nextLevelCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
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
				GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[1].height*1/20);
			}
			GUILayout.EndArea();
			GUILayout.BeginArea (focusMyGameFeaturesVM.cardFeaturesFocusRects[2]);
			{
				GUILayout.BeginHorizontal();
				{
					if(ApplicationModel.credits<focusMyGameFeaturesVM.renameCost)
					{
						GUI.enabled=false;
					}
					GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[2].width*1/20);
					if(GUILayout.Button("Renommer la carte pour (-"+ focusMyGameFeaturesVM.renameCost+" crédits)", cardFeaturesFocusVM.buttonStyle))
					{
						gameObject.GetComponent<CardController>().displayRenameCardPopUp();
					}
					GUI.enabled = cardFeaturesFocusVM.guiEnabled;
				}
				GUILayout.EndHorizontal();
				GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[2].height*1/20);
			}
			GUILayout.EndArea();
			GUILayout.BeginArea (focusMyGameFeaturesVM.cardFeaturesFocusRects[3]);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[3].width*1/20);
					if(focusMyGameFeaturesVM.canBeSold)
					{
						if(focusMyGameFeaturesVM.isOnSale)
						{
							if(GUILayout.Button("La carte est mise en vente sur le bazar pour "+ focusMyGameFeaturesVM.price+" crédits. Modifier ?", cardFeaturesFocusVM.buttonStyle))
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
					else
					{
						GUI.enabled=false;
						if(GUILayout.Button("Carte attaché à un deck, ne pouvant être mise en vente", cardFeaturesFocusVM.buttonStyle))
						{
						}
						GUI.enabled = cardFeaturesFocusVM.guiEnabled;
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[3].height*1/20);
			}
			GUILayout.EndArea();
			GUILayout.BeginArea (focusMyGameFeaturesVM.cardFeaturesFocusRects[4]);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[4].width*1/20);
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label("Victoires : "+focusMyGameFeaturesVM.nbWin,cardFeaturesFocusVM.statsStyle);
						GUILayout.Label("Défaites : "+focusMyGameFeaturesVM.nbLoose,cardFeaturesFocusVM.statsStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[4].height*1/20);
			}
			GUILayout.EndArea();
		}
		GUILayout.BeginArea (focusMyGameFeaturesVM.cardFeaturesFocusRects[5]);
		{
			GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[5].height*1/20);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (focusMyGameFeaturesVM.cardFeaturesFocusRects[5].width*1/20);
				if(GUILayout.Button("Retour", cardFeaturesFocusVM.buttonStyle))
				{
					gameObject.GetComponent<CardMyGameController>().exitFocus();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
}