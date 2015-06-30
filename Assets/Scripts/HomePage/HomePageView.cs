using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageView : MonoBehaviour
{
	public HomePageViewModel homepageVM;
	public HomePageNotificationsViewModel notificationsVM;
	public HomePageScreenViewModel homepageScreenVM;
	public HomePageNewsViewModel newsVM;
	public HomePagePacksViewModel packsVM;
	public HomePageRankingsViewModel ranksVM;
	public HomePageCompetsViewModel competsVM;

	public bool canDisplay=false;

	public HomePageView ()
	{
		this.homepageVM = new HomePageViewModel ();
		this.notificationsVM = new HomePageNotificationsViewModel ();
		this.homepageScreenVM = new HomePageScreenViewModel ();
		this.newsVM = new HomePageNewsViewModel ();
		this.packsVM = new HomePagePacksViewModel ();
		this.ranksVM = new HomePageRankingsViewModel ();
		this.competsVM = new HomePageCompetsViewModel ();
	}
	void Update()
	{
		if (Screen.width != homepageScreenVM.widthScreen || Screen.height != homepageScreenVM.heightScreen) {
				HomePageController.instance.resize();
		}
	}
	void OnGUI()
	{
		GUI.enabled = homepageVM.guiEnabled;
		GUILayout.BeginArea(homepageScreenVM.blockTopLeft,homepageScreenVM.blockBorderStyle);
		{
			GUILayout.Label(notificationsVM.notificationsTitleLabel,homepageVM.titleStyle,GUILayout.Height(0.5f*notificationsVM.blocksHeight));
			GUILayout.Label(notificationsVM.labelNo,homepageVM.labelNoStyle,GUILayout.Height(0.5f*notificationsVM.blocksHeight));
			GUILayout.Space(2.5f*notificationsVM.blocksHeight);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUI.enabled=homepageVM.buttonsEnabled;
				if (notificationsVM.pageDebut>0){
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.notificationsPaginationBack();
					}
				}
				GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
				if(notificationsVM.pageFin>1)
				{
					for (int i = notificationsVM.pageDebut ; i < notificationsVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),notificationsVM.paginatorGuiStyle[i],
						                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
						                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
						{
							HomePageController.instance.notificationsPaginationSelect(i);
						}
						GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
					}
				}
				if (notificationsVM.nbPages>notificationsVM.pageFin)
				{
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.notificationsPaginationNext();
					}
				}
				GUI.enabled = homepageVM.guiEnabled;
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();

		for (int i=0;i<notificationsVM.finish-notificationsVM.start;i++)
		{
			GUILayout.BeginArea(notificationsVM.blocks[i]);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (notificationsVM.blocksWidth*0.02f);
					GUILayout.BeginVertical();
					{
						GUILayout.Space (notificationsVM.blocksHeight*0.05f);
						GUILayout.BeginHorizontal(GUILayout.Height(notificationsVM.blocksHeight*0.90f));
						{
							GUILayout.BeginHorizontal(homepageVM.profileBackgroundStyle);
							{
								GUI.enabled=homepageVM.buttonsEnabled;
								if(GUILayout.Button ("",notificationsVM.profilePicturesButtonStyle[i],
								                     GUILayout.Height (notificationsVM.blocksHeight*0.90f),
								                     GUILayout.Width (notificationsVM.blocksHeight*0.90f)))
								{
									ApplicationModel.profileChosen=notificationsVM.username[i];
									Application.LoadLevel("Profile");
								}
								GUI.enabled = homepageVM.guiEnabled;
								GUILayout.Space (notificationsVM.blocksWidth*0.01f);
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginVertical(homepageVM.profileBackgroundStyle, 
							                        GUILayout.Width (2*notificationsVM.blocksHeight*0.90f),
							                        GUILayout.Height(notificationsVM.blocksHeight*0.90f));
							{
								GUILayout.Label (notificationsVM.username[i]
								                 ,homepageVM.profileUsernameStyle);
								GUILayout.Label (notificationsVM.totalNbWins[i]+" V "
								                 +notificationsVM.totalNbLooses[i].ToString()+" D",
								                 homepageVM.profileInformationsStyle);
								GUILayout.Label ("R : "+notificationsVM.ranking[i].ToString()
								                 ,homepageVM.profileInformationsStyle);
								GUILayout.Label ("Div : "+notificationsVM.division[i].ToString()
								                 ,homepageVM.profileInformationsStyle);
							}
							GUILayout.EndVertical();
							GUILayout.Space (notificationsVM.blocksWidth*0.01f);
							GUILayout.BeginVertical();
							{
								GUILayout.Label (notificationsVM.content[i],notificationsVM.notificationContentStyle);
								GUILayout.BeginHorizontal();
								{
									if(notificationsVM.nonReadNotifications[i])
									{
										GUILayout.Label ("Nouveau !",notificationsVM.newStyle);
										GUILayout.Label ("\u00A0",notificationsVM.notificationDateStyle);
									}
									GUILayout.Label (notificationsVM.date[i].ToString("dd/MM/yyyy HH:mm"),
									                 notificationsVM.notificationDateStyle);
									GUILayout.FlexibleSpace();
								}
								GUILayout.EndHorizontal();
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndHorizontal();
						GUILayout.Space (notificationsVM.blocksHeight*0.05f);
					}
					GUILayout.EndVertical();
					GUILayout.Space (notificationsVM.blocksWidth*0.02f);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();      
		}
		GUILayout.BeginArea(homepageScreenVM.blockBottomLeft,homepageScreenVM.blockBorderStyle);
		{
			GUILayout.Label("Mon flux d'actualités",homepageVM.titleStyle,GUILayout.Height(0.5f*newsVM.blocksHeight));
			GUILayout.Label(newsVM.labelNo,homepageVM.labelNoStyle,GUILayout.Height(0.5f*newsVM.blocksHeight));
			GUILayout.Space(4.5f*newsVM.blocksHeight);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUI.enabled=homepageVM.buttonsEnabled;
				if (newsVM.pageDebut>0){
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.newsPaginationBack();
					}
				}
				GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
				if(newsVM.pageFin>1)
				{
					for (int i = newsVM.pageDebut ; i < newsVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),newsVM.paginatorGuiStyle[i],
						                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
						                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
						{
							HomePageController.instance.newsPaginationSelect(i);
						}
						GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
					}
				}
				if (newsVM.nbPages>newsVM.pageFin)
				{
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.newsPaginationNext();
					}
				}
				GUI.enabled = homepageVM.guiEnabled;
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
		
		for (int i=0;i<newsVM.finish-newsVM.start;i++)
		{
			GUILayout.BeginArea(newsVM.blocks[i]);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (newsVM.blocksWidth*0.02f);
					GUILayout.BeginVertical();
					{
						GUILayout.Space (newsVM.blocksHeight*0.05f);
						GUILayout.BeginHorizontal(GUILayout.Height(newsVM.blocksHeight*0.90f));
						{
							GUILayout.BeginHorizontal(homepageVM.profileBackgroundStyle);
							{
								GUI.enabled=homepageVM.buttonsEnabled;
								if(GUILayout.Button ("",newsVM.profilePicturesButtonStyle[i],
								                     GUILayout.Height (newsVM.blocksHeight*0.90f),
								                     GUILayout.Width (newsVM.blocksHeight*0.90f)))
								{
									ApplicationModel.profileChosen=newsVM.username[i];
									Application.LoadLevel("Profile");
								}
								GUI.enabled = homepageVM.guiEnabled;
								GUILayout.Space (newsVM.blocksWidth*0.01f);
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginVertical(homepageVM.profileBackgroundStyle, 
							                        GUILayout.Width (2*newsVM.blocksHeight*0.90f),
							                        GUILayout.Height(newsVM.blocksHeight*0.90f));
							{
								GUILayout.Label (newsVM.username[i]
								                 ,homepageVM.profileUsernameStyle);
								GUILayout.Label (newsVM.totalNbWins[i].ToString()+" V "
								                 +newsVM.totalNbLooses[i].ToString()+" D",
								                 homepageVM.profileInformationsStyle);
								GUILayout.Label ("R : "+newsVM.ranking[i].ToString()
								                 ,homepageVM.profileInformationsStyle);
								GUILayout.Label ("Div : "+newsVM.division[i].ToString()
								                 ,homepageVM.profileInformationsStyle);
							}
							GUILayout.EndVertical();
							GUILayout.Space (newsVM.blocksWidth*0.01f);
							GUILayout.BeginVertical();
							{
								GUILayout.Label (newsVM.content[i],newsVM.newsContentStyle);
								GUILayout.Label (newsVM.date[i].ToString("dd/MM/yyyy HH:mm"),
									                 newsVM.newsDateStyle);
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndHorizontal();
						GUILayout.Space (newsVM.blocksHeight*0.05f);
					}
					GUILayout.EndVertical();
					GUILayout.Space (newsVM.blocksWidth*0.02f);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();      
		}
		GUILayout.BeginArea(homepageScreenVM.blockBottomRight,homepageScreenVM.blockBorderStyle);
		{
			GUILayout.Label("Disponible en boutique",homepageVM.titleStyle,GUILayout.Height(0.125f*homepageScreenVM.blockBottomRightHeight));
			GUILayout.Label(packsVM.labelNo,packsVM.labelNoStyle,GUILayout.Height(0.125f*homepageScreenVM.blockBottomRightHeight));
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
		for (int i=0;i<packsVM.finish-packsVM.start;i++)
		{
			GUILayout.BeginArea(packsVM.blocks[i]);
			{
			
				GUILayout.FlexibleSpace();
				GUILayout.Label(packsVM.packsNames[i],packsVM.packNameStyle,GUILayout.Height(packsVM.blocksHeight*8f/100f));
				if(packsVM.packsNew[i])
				{
					GUILayout.Label ("Nouveau !",packsVM.newPackStyle,GUILayout.Height(packsVM.blocksHeight*1f/10f));
				}
				else
				{
					GUILayout.Label ("",packsVM.newPackStyle,GUILayout.Height(packsVM.blocksHeight*1f/10f));
				}
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("",packsVM.packPicturesButtonStyle[i],GUILayout.Height(packsVM.blocksHeight*55f/100f),GUILayout.Width(packsVM.blocksHeight*6f/10f)))
					{
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(packsVM.packsPrice[i].ToString()+" crédits",packsVM.packPriceStyle,GUILayout.Height(packsVM.blocksHeight*8f/100f));
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=homepageVM.buttonsEnabled;
					if(GUILayout.Button("Acheter",packsVM.buttonStyle,GUILayout.Height(packsVM.blocksHeight*15f/100f),GUILayout.Width(packsVM.blocksHeight*6f/10f)))
					{
						HomePageController.instance.buyPackHandler(i);
					}
					GUI.enabled = homepageVM.guiEnabled;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();      
		}
		GUILayout.BeginArea(homepageScreenVM.blockMiddleRight,homepageScreenVM.blockBorderStyle);
		{
			GUILayout.Label("Mes statistiques",homepageVM.titleStyle,GUILayout.Height(homepageScreenVM.blockBottomRightHeight*0.1f));
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Victoires : " + ranksVM.totalNbWins,ranksVM.informationsStyle,GUILayout.Height(homepageScreenVM.blockMiddleRightHeight*0.1f));
			GUILayout.Label("Défaites : " + ranksVM.totalNbLooses,ranksVM.informationsStyle,GUILayout.Height(homepageScreenVM.blockMiddleRightHeight*0.1f));
			if(ranksVM.ranking!="")
			{
				GUILayout.Label(ranksVM.ranking,ranksVM.informationsStyle,GUILayout.Height(homepageScreenVM.blockMiddleRightHeight*0.1f));
			}
			if(ranksVM.collectionRanking!="")
			{
				GUILayout.Label(ranksVM.collectionRanking,ranksVM.informationsStyle,GUILayout.Height(homepageScreenVM.blockMiddleRightHeight*0.1f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUI.enabled=homepageVM.buttonsEnabled;
				if(GUILayout.Button("Mon profil",ranksVM.buttonStyle,GUILayout.Height(0.1f*homepageScreenVM.blockBottomRightHeight),GUILayout.Width(0.4f*homepageScreenVM.blockMiddleRightWidth)))
				{
					Application.LoadLevel("Profile");
				}
				GUI.enabled = homepageVM.guiEnabled;
				GUILayout.Space (0.1f*homepageScreenVM.blockMiddleRightWidth);
				GUI.enabled=homepageVM.buttonsEnabled;
				if(GUILayout.Button("Ma collection",ranksVM.buttonStyle,GUILayout.Height(0.1f*homepageScreenVM.blockBottomRightHeight),GUILayout.Width(0.4f*homepageScreenVM.blockMiddleRightWidth)))
				{
					Application.LoadLevel("SkillBook");
				}
				GUI.enabled = homepageVM.guiEnabled;
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Vider mes cartes",ranksVM.buttonStyle,GUILayout.Height(0.1f*homepageScreenVM.blockBottomRightHeight),GUILayout.Width(0.5f*homepageScreenVM.blockBottomRightWidth)))
				{
					HomePageController.instance.cleanCardsHandler();
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(0.02f*homepageScreenVM.blockBottomRightHeight);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(homepageScreenVM.blockTopRight,homepageScreenVM.blockBorderStyle);
		{
			GUILayout.Label("Mes compétitions",homepageVM.titleStyle,GUILayout.Height(homepageScreenVM.blockBottomRightHeight*0.1f));
			GUILayout.FlexibleSpace();
			if(competsVM.competsNames.Count>0)
			{
				GUILayout.BeginHorizontal(GUILayout.Height(0.7f*homepageScreenVM.blockTopRightHeight));
				{
					for(int i=0;i<competsVM.competsNames.Count;i++)
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
						{
							GUILayout.Label(competsVM.competsNames[i],competsVM.nameStyle,GUILayout.Height(0.1f*homepageScreenVM.blockTopRightHeight));
							if(GUILayout.Button("",competsVM.competsButtonsStyle[i],GUILayout.Height(0.55f*homepageScreenVM.blockTopRightHeight),GUILayout.Width(0.55f*homepageScreenVM.blockTopRightHeight)))
							{
							}
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
					}
				}
				GUILayout.EndHorizontal();
			}
			else
			{
				GUILayout.Label(competsVM.labelNo,competsVM.labelNoStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUI.enabled=homepageVM.buttonsEnabled;
				if(GUILayout.Button("Rejoindre",ranksVM.buttonStyle,GUILayout.Height(0.125f*homepageScreenVM.blockBottomRightHeight),GUILayout.Width(0.5f*homepageScreenVM.blockMiddleRightWidth)))
				{
					Application.LoadLevel("Lobby");
				}
				GUI.enabled = homepageVM.guiEnabled;
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(0.02f*homepageScreenVM.blockBottomRightHeight);
		}
		GUILayout.EndArea();
	}
}

