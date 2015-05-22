using System;
using UnityEngine;

public class StoreView : MonoBehaviour
{

	public StoreViewModel storeVM;
	public StoreScreenViewModel storeScreenVM;

	public StoreView ()
	{
		this.storeVM = new StoreViewModel ();
		this.storeScreenVM = new StoreScreenViewModel ();
	}
	void Update()
	{
		if (Screen.width != storeScreenVM.widthScreen || Screen.height != storeScreenVM.heightScreen) {
			StoreController.instance.resize();
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			StoreController.instance.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			StoreController.instance.escapePressed();
		}
	}
	void OnGUI()
	{
		GUI.enabled=storeVM.guiEnabled;
		if(!this.storeVM.hideGUI)
		{
			GUILayout.BeginArea(storeScreenVM.blockMain);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("Choisissez votre pack",storeVM.titleStyle,GUILayout.Width(storeScreenVM.blockMainHeight+3f/5f*(storeScreenVM.blockMainWidth-storeScreenVM.blockMainHeight)),GUILayout.Height(storeScreenVM.blockMainHeight*1f/10f));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (ApplicationModel.credits>=storeVM.cardCreationCost)
					{
						GUI.enabled=storeVM.guiEnabled;
					}
					else
					{
						GUI.enabled=false;
					}
					if (GUILayout.Button("Pack 1 carte \n\n\n\n\n\n\n\n\n\n"+ storeVM.cardCreationCost+" crédits",storeVM.button1CardStyle,
					                     GUILayout.Width(storeScreenVM.blockMainHeight*1/4),
					                     GUILayout.Height(storeScreenVM.blockMainHeight*1/2)))
					{
						StartCoroutine(StoreController.instance.createRandomCard());
					}
					GUILayout.FlexibleSpace();
					if (ApplicationModel.credits>=storeVM.fiveCardsCreationCost)
					{
						GUI.enabled=storeVM.guiEnabled;
					}
					else
					{
						GUI.enabled=false;
					}
					if (GUILayout.Button("Pack 5 cartes \n\n\n\n\n\n\n\n\n\n"+ storeVM.fiveCardsCreationCost+" crédits",storeVM.button5CardStyle,
					                     GUILayout.Width(storeScreenVM.blockMainHeight*1/4),
					                     GUILayout.Height(storeScreenVM.blockMainHeight*1/2)))
					{
						StartCoroutine(StoreController.instance.create5RandomCard());
					}
					GUILayout.FlexibleSpace();
					if (ApplicationModel.credits>=storeVM.cardWithCardTypeCreationCost)
					{
						GUI.enabled=storeVM.guiEnabled;
					}
					else
					{
						GUI.enabled=false;
					}
					if (GUILayout.Button("Pack 1 carte \nde classe \n\n\n\n\n\n\n\n\n"+ storeVM.cardWithCardTypeCreationCost+" crédits",storeVM.button1CardWithCardTypeStyle,
					                     GUILayout.Width(storeScreenVM.blockMainHeight*1/4),
					                     GUILayout.Height(storeScreenVM.blockMainHeight*1/2)))
					{
						StoreController.instance.displaySelectCardPopUp(false);	
					}
					GUILayout.FlexibleSpace();
					if (ApplicationModel.credits>=storeVM.fiveCardsWidthCardTypeCreationCost)
					{
						GUI.enabled=storeVM.guiEnabled;
					}
					else
					{
						GUI.enabled=false;
					}
					if (GUILayout.Button("Pack 5 cartes \nde classe \n\n\n\n\n\n\n\n\n"+ storeVM.fiveCardsWidthCardTypeCreationCost+" crédits",storeVM.button5CardWithCardTypeStyle,
					                     GUILayout.Width(storeScreenVM.blockMainHeight*1/4),
					                     GUILayout.Height(storeScreenVM.blockMainHeight*1/2)))
					{
						StoreController.instance.displaySelectCardPopUp(true);		
					}
					GUILayout.FlexibleSpace();
				}
				GUI.enabled=storeVM.guiEnabled;
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUI.enabled=storeVM.canAddCredits;
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("Approvisionnez votre portefeuille",storeVM.buttonAddCreditsStyle,GUILayout.Width(storeScreenVM.blockMainHeight+3f/5f*(storeScreenVM.blockMainWidth-storeScreenVM.blockMainHeight)),GUILayout.Height(storeScreenVM.blockMainHeight*1f/8f)))
					{
						StoreController.instance.displayAddCreditsPopUp();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
		}
		if(storeVM.are5CardsDisplayed)
		{
			GUILayout.BeginArea(storeScreenVM.blockMain);
			{
				GUILayout.Space (storeScreenVM.blockMain.height*8/10);
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("Quitter",storeVM.buttonAddCreditsStyle,GUILayout.Width(storeScreenVM.blockMainWidth*1/8f),GUILayout.Height(storeScreenVM.blockMainHeight*1f/8f)))
					{

					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
	}
	
}
