using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageView : MonoBehaviour
{
	public HomePageViewModel homepageVM;
	public NotificationsViewModel notificationsVM;
	public HomePageScreenViewModel homepageScreenVM;
	public NewsViewModel newsVM;

	public bool canDisplay=false;

	public HomePageView ()
	{
		this.homepageVM = new HomePageViewModel ();
		this.notificationsVM = new NotificationsViewModel ();
		this.homepageScreenVM = new HomePageScreenViewModel ();
		this.newsVM = new NewsViewModel ();
	}
	void Update()
	{
		if (Screen.width != homepageScreenVM.widthScreen || Screen.height != homepageScreenVM.heightScreen) {
				HomePageController.instance.resize();
		}
	}
	void OnGUI()
	{
		GUILayout.BeginArea(homepageScreenVM.blockTopLeft,homepageScreenVM.blockBorderStyle);
		{
			GUILayout.Label(notificationsVM.notificationsTitleLabel,homepageVM.titleStyle,GUILayout.Height(0.5f*notificationsVM.blocksHeight));
			GUILayout.Label(notificationsVM.labelNo,homepageVM.labelNoStyle,GUILayout.Height(0.5f*notificationsVM.blocksHeight));
			GUILayout.Space(2.5f*notificationsVM.blocksHeight);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if (notificationsVM.pageDebut>0){
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.pagination(0,0);
					}
				}
				GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
				for (int i = notificationsVM.pageDebut ; i < notificationsVM.pageFin ; i++)
				{
					if (GUILayout.Button(""+(i+1),notificationsVM.paginatorGuiStyle[i],
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.pagination(0,1,i);
					}
					GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
				}
				if (notificationsVM.nbPages>notificationsVM.pageFin)
				{
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.pagination(0,2);
					}
				}
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
								if(GUILayout.Button ("",notificationsVM.profilePicturesButtonStyle[i],
								                     GUILayout.Height (notificationsVM.blocksHeight*0.90f),
								                     GUILayout.Width (notificationsVM.blocksHeight*0.90f)))
								{
									ApplicationModel.profileChosen=notificationsVM.username[i];
									Application.LoadLevel("Profile");
								}
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
				if (newsVM.pageDebut>0){
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.pagination(1,0);
					}
				}
				GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
				for (int i = newsVM.pageDebut ; i < newsVM.pageFin ; i++)
				{
					if (GUILayout.Button(""+(i+1),newsVM.paginatorGuiStyle[i],
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.pagination(1,1,i);
					}
					GUILayout.Space(homepageScreenVM.widthScreen*0.01f);
				}
				if (newsVM.nbPages>newsVM.pageFin)
				{
					if (GUILayout.Button("...",homepageVM.paginationStyle,
					                     GUILayout.Height(homepageScreenVM.heightScreen*3/100),
					                     GUILayout.Width (homepageScreenVM.widthScreen*2/100)))
					{
						HomePageController.instance.pagination(1,2);
					}
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
		
		for (int i=newsVM.start;i<newsVM.finish;i++)
		{
			GUILayout.BeginArea(newsVM.blocks[i-newsVM.start]);
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
								if(GUILayout.Button ("",newsVM.profilePicturesButtonStyle[i],
								                     GUILayout.Height (newsVM.blocksHeight*0.90f),
								                     GUILayout.Width (newsVM.blocksHeight*0.90f)))
								{
									ApplicationModel.profileChosen=newsVM.news[i].User.Username;
									Application.LoadLevel("Profile");
								}
								GUILayout.Space (newsVM.blocksWidth*0.01f);
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginVertical(homepageVM.profileBackgroundStyle, 
							                        GUILayout.Width (2*newsVM.blocksHeight*0.90f),
							                        GUILayout.Height(newsVM.blocksHeight*0.90f));
							{
								GUILayout.Label (newsVM.news[i].User.Username
								                 ,homepageVM.profileUsernameStyle);
								GUILayout.Label (newsVM.news[i].User.TotalNbWins+" V "
								                 +newsVM.news[i].User.TotalNbLooses+" D",
								                 homepageVM.profileInformationsStyle);
								GUILayout.Label ("R : "+newsVM.news[i].User.Ranking
								                 ,homepageVM.profileInformationsStyle);
								GUILayout.Label ("Div : "+newsVM.news[i].User.Division
								                 ,homepageVM.profileInformationsStyle);
							}
							GUILayout.EndVertical();
							GUILayout.Space (newsVM.blocksWidth*0.01f);
							GUILayout.BeginVertical();
							{
								GUILayout.Label (newsVM.news[i].Content,newsVM.newsContentStyle);
								GUILayout.Label (newsVM.news[i].News.Date.ToString("dd/MM/yyyy HH:mm"),
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
	}


}

