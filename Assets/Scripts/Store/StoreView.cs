//using System;
//using UnityEngine;
//
//public class StoreView : MonoBehaviour
//{
//
//	public StoreViewModel storeVM;
//	public StoreScreenViewModel storeScreenVM;
//	public StorePacksViewModel packsVM;
//
//	public StoreView ()
//	{
//		this.storeVM = new StoreViewModel ();
//		this.storeScreenVM = new StoreScreenViewModel ();
//		this.packsVM = new StorePacksViewModel ();
//	}
//	void Update()
//	{
//		if (Screen.width != storeScreenVM.widthScreen || Screen.height != storeScreenVM.heightScreen) {
//			StoreController.instance.resize();
//		}
//		if(Input.GetKeyDown(KeyCode.Return)) 
//		{
//			StoreController.instance.returnPressed();
//		}
//		if(Input.GetKeyDown(KeyCode.Escape)) 
//		{
//			StoreController.instance.escapePressed();
//		}
//	}
//	void OnGUI()
//	{
//		GUI.enabled=storeVM.guiEnabled;
//		if(!this.storeVM.hideGUI)
//		{
//			GUILayout.BeginArea(storeScreenVM.blockMain);
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					GUILayout.Label("Choisissez votre pack",storeVM.titleStyle,GUILayout.Width(storeScreenVM.blockMainHeight+3f/5f*(storeScreenVM.blockMainWidth-storeScreenVM.blockMainHeight)),GUILayout.Height(storeScreenVM.blockMainHeight*1f/10f));
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndHorizontal();
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					for(int i=0;i<packsVM.finish-packsVM.start;i++)
//					{
//						GUILayout.BeginVertical(GUILayout.Width(storeScreenVM.blockMainHeight*1/4));
//						{
//							GUILayout.Label (packsVM.packNames[i],packsVM.packNameStyle,GUILayout.Height(storeScreenVM.blockMainHeight*1/12));
//							if(packsVM.isNew[i])
//							{
//								GUILayout.Label("Nouveau !",packsVM.packNewStyle,GUILayout.Height(storeScreenVM.blockMainHeight*1/15));
//							}
//							else
//							{
//								GUILayout.Label("",packsVM.packNewStyle,GUILayout.Height(storeScreenVM.blockMainHeight*1/15));
//							}
//							if(GUILayout.Button("",packsVM.packPictureStyles[i],GUILayout.Width(storeScreenVM.blockMainHeight*1/4),GUILayout.Height(storeScreenVM.blockMainHeight*1/4)))
//							{
//							}
//							GUILayout.Label(packsVM.packPrices[i].ToString()+ " crédits",packsVM.packPriceStyle,GUILayout.Height(storeScreenVM.blockMainHeight*1/12));
//							if(ApplicationModel.credits>=packsVM.packPrices[i])
//							{
//								if(packsVM.guiEnabled[i])
//								{
//									if(i==0)
//									{
//										GUI.enabled=storeVM.buttonsEnabled[0];
//									}
//									else
//									{
//										GUI.enabled=storeVM.buttonsEnabled[1];
//									}
//								}
//								else
//								{
//									GUI.enabled=false;
//								}
//							}
//							else
//							{
//								GUI.enabled=false;
//							}
//							if(GUILayout.Button("Acheter",packsVM.packBuyButtonStyle,GUILayout.Height(storeScreenVM.blockMainHeight*1/12)))
//							{
//								StoreController.instance.buyPackHandler(i);
//							}
//							GUI.enabled=storeVM.guiEnabled;
//						}
//						GUILayout.EndVertical();
//						GUILayout.FlexibleSpace();
//					}
//				}
//				GUILayout.EndHorizontal();
//				GUILayout.FlexibleSpace();
//				GUI.enabled=storeVM.buttonsEnabled[1];
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (packsVM.pageDebut>0)
//					{
//						if (GUILayout.Button("...",storeVM.paginationStyle,
//						                     GUILayout.Height(storeScreenVM.heightScreen*3/100),
//						                     GUILayout.Width (storeScreenVM.widthScreen*2/100)))
//						{
//							StoreController.instance.paginationBack();
//						}
//					}
//					GUILayout.Space(storeScreenVM.widthScreen*0.01f);
//					if(packsVM.pageFin>1)
//					{
//						for (int i = packsVM.pageDebut ; i < packsVM.pageFin ; i++)
//						{
//							if (GUILayout.Button(""+(i+1),packsVM.paginatorGuiStyle[i],
//							                     GUILayout.Height(storeScreenVM.heightScreen*3/100),
//							                     GUILayout.Width (storeScreenVM.widthScreen*2/100)))
//							{
//								StoreController.instance.paginationSelect(i);
//							}
//							GUILayout.Space(storeScreenVM.widthScreen*0.01f);
//						}
//					}
//					if (packsVM.nbPages>packsVM.pageFin)
//					{
//						if (GUILayout.Button("...",storeVM.paginationStyle,
//						                     GUILayout.Height(storeScreenVM.heightScreen*3/100),
//						                     GUILayout.Width (storeScreenVM.widthScreen*2/100)))
//						{
//							StoreController.instance.paginationNext();
//						}
//					}
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndHorizontal();
//				GUI.enabled=storeVM.guiEnabled;
//				GUILayout.FlexibleSpace();
//				GUI.enabled=storeVM.canAddCredits;
//				GUI.enabled=storeVM.buttonsEnabled[1];
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if(GUILayout.Button("Approvisionnez votre portefeuille",storeVM.buttonAddCreditsStyle,GUILayout.Width(storeScreenVM.blockMainHeight+3f/5f*(storeScreenVM.blockMainWidth-storeScreenVM.blockMainHeight)),GUILayout.Height(storeScreenVM.blockMainHeight*1f/8f)))
//					{
//						StoreController.instance.displayAddCreditsPopUp();
//					}
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndHorizontal();
//				GUI.enabled=storeVM.guiEnabled;
//				GUILayout.FlexibleSpace();
//			}
//			GUILayout.EndArea();
//		}
//		if(storeVM.areMoreThan1CardDisplayed)
//		{
//			GUILayout.BeginArea(storeScreenVM.blockMain);
//			{
//				GUILayout.Space (storeScreenVM.blockMain.height*8/10);
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if(GUILayout.Button("Quitter",storeVM.buttonAddCreditsStyle,GUILayout.Width(storeScreenVM.blockMainWidth*1/8f),GUILayout.Height(storeScreenVM.blockMainHeight*1f/8f)))
//					{
//						StoreController.instance.displayMainGUI();
//					}
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndHorizontal();
//			}
//			GUILayout.EndArea();
//		}
//	}
//	
//}
