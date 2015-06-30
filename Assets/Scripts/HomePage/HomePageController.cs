using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageController : MonoBehaviour 
{

	public GameObject MenuObject;
	public GameObject TutorialObject;

	public GUIStyle[] notificationsVMStyle;
	public GUIStyle[] newsVMStyle;
	public GUIStyle[] homepageScreenVMStyle;
	public GUIStyle[] homepageVMStyle;
	public GUIStyle[] packsVMStyle;
	public GUIStyle[] ranksVMStyle;
	public GUIStyle[] competsVMStyle;
	public GUIStyle[] popUpVMStyle;
	public GUISkin tutorialStyles;

	private float timer;
	private float timer2;
	private bool isDataLoaded=false;
	public int refreshInterval;
	public int sliderRefreshInterval;
	public int totalNbResultLimit;
	private GameObject tutorial;
	
	private HomePageView view;
	public static HomePageController instance;
	private HomePageModel model;

	private HomePageConnectionBonusPopUpView connectionBonusView;
	
	void Start () 
	{
		instance = this;
		this.model = new HomePageModel ();
		this.view = Camera.main.gameObject.AddComponent <HomePageView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization());
	}
	
	void Update()
	{
		
		timer += Time.deltaTime;
		timer2 += Time.deltaTime;
		
		if (timer > refreshInterval) 
		{
			StartCoroutine(this.refreshNonReadsNotifications());

		}
		if (timer2 > sliderRefreshInterval) 
		{
			this.timer2=this.timer2-this.sliderRefreshInterval;
			view.packsVM.chosenPage++;
			if(view.packsVM.chosenPage>view.packsVM.nbPages-1)
			{
				view.packsVM.chosenPage=0;
			}
			this.displayPacksPage();
		}
	}
	private IEnumerator refreshNonReadsNotifications()
	{
		this.timer=this.timer-this.refreshInterval;
		yield return StartCoroutine(model.player.countNonReadsNotifications(this.totalNbResultLimit));
		MenuObject.GetComponent<MenuController>().setNbNotificationsNonRead(model.player.nonReadNotifications);
	}
	
	private IEnumerator initialization()
	{
		yield return StartCoroutine (model.getData (this.totalNbResultLimit));
		this.initLabelsNo ();
		this.initStyles ();
		this.initVM ();
		this.toLoadData (true);
		this.isDataLoaded = true;
		if(model.player.TutorialStep==1)
		{
			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
			this.tutorial.AddComponent<HomePageTutorialController>();
			this.tutorial.GetComponent<HomePageTutorialController>().launchSequence(0);
			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
		}
		else if(model.player.ConnectionBonus>0)
		{
			this.displayConnectionBonusPopUp(model.player.ConnectionBonus);
		}
	}
	private void toLoadData(bool firstLoad=false)
	{
		this.resize ();
		this.manageNonReadsNotifications (firstLoad);
	}
	
	private void manageNonReadsNotifications(bool firstload){
		this.computeNonReadsNotifications ();
		this.initLabelTitle ();
		StartCoroutine(this.updateReadNotifications (firstload));
	}
	
	private void computeNonReadsNotifications()
	{
		view.notificationsVM.nbNonReadNotifications = 0;
		for(int i=0;i<model.notifications.Count;i++)
		{
			if(i==model.notificationSystemIndex)
			{
				view.notificationsVM.nbNonReadNotifications++;
			}
			else if(!model.notifications[i].Notification.IsRead)
			{
				view.notificationsVM.nbNonReadNotifications++;
			}
		}
	}
	private void initLabelsNo()
	{
		if(model.notifications.Count==0)
		{
			view.notificationsVM.labelNo="Vous n'avez pas de notification";
		}
		if(model.news.Count==0)
		{
			view.newsVM.labelNo="Il n'y a pas d'actualités à afficher";
		}
		if(model.packs.Count==0)
		{
			view.packsVM.labelNo="Aucun pack n'est à la une";
		}
		if(model.player.NbGamesCup==0&& model.player.NbGamesDivision==0)
		{
			view.competsVM.labelNo="Aucune compétition en cours";
		}
	}
	private void initLabelTitle()
	{
		if(view.notificationsVM.nbNonReadNotifications>1)
		{
			view.notificationsVM.notificationsTitleLabel="Mes notifications - "+view.notificationsVM.nbNonReadNotifications+" nouveautés";
		}
		else if(view.notificationsVM.nbNonReadNotifications==1)
		{
			view.notificationsVM.notificationsTitleLabel="Mes notifications - 1 nouveauté";
		}
		else if(view.notificationsVM.nbNonReadNotifications==0)
		{
			view.notificationsVM.notificationsTitleLabel="Mes notifications";
		}
	}
	private void initVM()
	{
		view.ranksVM.totalNbWins = model.player.TotalNbWins;
		view.ranksVM.totalNbLooses = model.player.TotalNbLooses;
		view.ranksVM.division = model.player.Division;
		if(model.player.Ranking!=0)
		{
			view.ranksVM.ranking="Classement : " + model.player.Ranking.ToString()+" ("+ model.player.RankingPoints.ToString() + " pts)";
		}
		else
		{
			view.ranksVM.ranking="";
		}
		if(model.player.CollectionRanking!=0)
		{
			view.ranksVM.collectionRanking="Class collection : " + model.player.CollectionRanking.ToString()+ " ("+ model.player.CollectionPoints.ToString() + " pts)";
		}
		else
		{
			view.ranksVM.collectionRanking="";
		}
		if(model.player.NbGamesDivision>0)
		{
			view.competsVM.competsNames.Add (model.currentDivision.Name);
			view.competsVM.competsButtonsStyle.Add (new GUIStyle());
			view.competsVM.competsButtonsStyle[view.competsVM.competsButtonsStyle.Count-1].normal.background=model.currentDivision.texture;
			StartCoroutine(model.currentDivision.setPicture());
		}
		if(model.player.NbGamesCup>0)
		{
			view.competsVM.competsNames.Add (model.currentCup.Name);
			view.competsVM.competsButtonsStyle.Add (new GUIStyle());
			view.competsVM.competsButtonsStyle[view.competsVM.competsButtonsStyle.Count-1].normal.background=model.currentCup.texture;
			StartCoroutine(model.currentCup.setPicture());
		}
		for (int i=0;i<view.competsVM.competsButtonsStyle.Count;i++)
		{
			view.competsVM.competsButtonsStyle[i].border.top=3;
			view.competsVM.competsButtonsStyle[i].border.bottom=3;
			view.competsVM.competsButtonsStyle[i].border.left=3;
			view.competsVM.competsButtonsStyle[i].border.right=3;
		}
	}
	private void initPagination()
	{
		view.notificationsVM.nbPages = Mathf.CeilToInt(((float)model.notifications.Count) / (3f*(float)view.notificationsVM.elementPerRow));
		view.notificationsVM.pageDebut = 0 ;
		if (view.notificationsVM.nbPages>10){
			view.notificationsVM.pageFin = 10 ;
		}
		else
		{
			view.notificationsVM.pageFin = view.notificationsVM.nbPages;
		}
		view.notificationsVM.paginatorGuiStyle = new GUIStyle[view.notificationsVM.nbPages];
		for (int i = 0; i < view.notificationsVM.nbPages; i++) 
		{ 
			if (i==0)
			{
				view.notificationsVM.paginatorGuiStyle[i]=view.homepageVM.paginationActivatedStyle;
			}
			else{
				view.notificationsVM.paginatorGuiStyle[i]=view.homepageVM.paginationStyle;
			}
		}
		view.newsVM.nbPages = Mathf.CeilToInt(((float)model.news.Count) / (5f*(float)view.newsVM.elementPerRow));
		view.newsVM.pageDebut = 0 ;
		if (view.newsVM.nbPages>10){
			view.newsVM.pageFin = 10 ;
		}
		else
		{
			view.newsVM.pageFin = view.newsVM.nbPages;
		}
		view.newsVM.paginatorGuiStyle = new GUIStyle[view.newsVM.nbPages];
		for (int i = 0; i < view.newsVM.nbPages; i++) { 
			if (i==0){
				view.newsVM.paginatorGuiStyle[i]=view.homepageVM.paginationActivatedStyle;
			}
			else
			{
				view.newsVM.paginatorGuiStyle[i]=view.homepageVM.paginationStyle;
			}
		}
		view.packsVM.nbPages = Mathf.CeilToInt(((float)model.packs.Count) / (float)view.packsVM.elementPerRow);
	}
	private void initStyles()
	{
		view.homepageScreenVM.styles=new GUIStyle[this.homepageScreenVMStyle.Length];
		for(int i=0;i<this.homepageScreenVMStyle.Length;i++)
		{
			view.homepageScreenVM.styles[i]=this.homepageScreenVMStyle[i];
		}
		view.homepageScreenVM.initStyles();
		view.notificationsVM.styles=new GUIStyle[this.notificationsVMStyle.Length];
		for(int i=0;i<this.notificationsVMStyle.Length;i++)
		{
			view.notificationsVM.styles[i]=this.notificationsVMStyle[i];
		}
		view.notificationsVM.initStyles();
		view.homepageVM.styles=new GUIStyle[this.homepageVMStyle.Length];
		for(int i=0;i<this.homepageVMStyle.Length;i++)
		{
			view.homepageVM.styles[i]=this.homepageVMStyle[i];
		}
		view.homepageVM.initStyles();
		view.newsVM.styles=new GUIStyle[this.newsVMStyle.Length];
		for(int i=0;i<this.newsVMStyle.Length;i++)
		{
			view.newsVM.styles[i]=this.newsVMStyle[i];
		}
		view.newsVM.initStyles();
		view.packsVM.styles=new GUIStyle[this.packsVMStyle.Length];
		for(int i=0;i<this.packsVMStyle.Length;i++)
		{
			view.packsVM.styles[i]=this.packsVMStyle[i];
		}
		view.packsVM.initStyles();
		view.ranksVM.styles=new GUIStyle[this.ranksVMStyle.Length];
		for(int i=0;i<this.ranksVMStyle.Length;i++)
		{
			view.ranksVM.styles[i]=this.ranksVMStyle[i];
		}
		view.ranksVM.initStyles();
		view.competsVM.styles=new GUIStyle[this.competsVMStyle.Length];
		for(int i=0;i<this.competsVMStyle.Length;i++)
		{
			view.competsVM.styles[i]=this.competsVMStyle[i];
		}
		view.competsVM.initStyles();
	}
	public void resize()
	{
		view.homepageScreenVM.resize ();
		view.homepageVM.resize (view.homepageScreenVM.heightScreen);
		view.notificationsVM.resize (view.homepageScreenVM.heightScreen);
		view.homepageVM.resize (view.homepageScreenVM.heightScreen);
		view.newsVM.resize (view.homepageScreenVM.heightScreen);
		view.packsVM.resize (view.homepageScreenVM.heightScreen);
		view.ranksVM.resize (view.homepageScreenVM.heightScreen);
		view.competsVM.resize (view.homepageScreenVM.heightScreen);

		view.notificationsVM.elementPerRow = 1;
		view.notificationsVM.blocksWidth = view.homepageScreenVM.blockTopLeftWidth / view.notificationsVM.elementPerRow;
		view.notificationsVM.blocksHeight = (view.homepageScreenVM.blockBottomLeftHeight+view.homepageScreenVM.blockTopLeftHeight) / 10f;
		view.notificationsVM.blocks=new Rect[3*view.notificationsVM.elementPerRow];
		for(int i=0;i<view.notificationsVM.blocks.Length;i++)
		{
			view.notificationsVM.blocks[i]=new Rect(view.homepageScreenVM.blockTopLeft.xMin+(i%view.notificationsVM.elementPerRow)*view.notificationsVM.blocksWidth,
			                                    view.homepageScreenVM.blockTopLeft.yMin+0.5f*view.notificationsVM.blocksHeight+Mathf.FloorToInt(i/view.notificationsVM.elementPerRow)*view.notificationsVM.blocksHeight,
			                                    view.notificationsVM.blocksWidth,
			                                    view.notificationsVM.blocksHeight);
		}
		view.newsVM.elementPerRow = 1;
		view.newsVM.blocksWidth = view.homepageScreenVM.blockBottomLeftWidth / view.newsVM.elementPerRow;
		view.newsVM.blocksHeight = view.notificationsVM.blocksHeight;
		view.newsVM.blocks=new Rect[5*view.newsVM.elementPerRow];
		for(int i=0;i<view.newsVM.blocks.Length;i++)
		{
			view.newsVM.blocks[i]=new Rect(view.homepageScreenVM.blockBottomLeft.xMin+(i%view.newsVM.elementPerRow)*view.newsVM.blocksWidth,
			                               view.homepageScreenVM.blockBottomLeft.yMin+0.5f*view.newsVM.blocksHeight+Mathf.FloorToInt(i/view.newsVM.elementPerRow)*view.newsVM.blocksHeight,
			                                        view.newsVM.blocksWidth,
			                                        view.newsVM.blocksHeight);
		}
		view.packsVM.elementPerRow = 2;
		view.packsVM.blocksWidth = view.homepageScreenVM.blockBottomRightWidth / view.packsVM.elementPerRow;
		view.packsVM.blocksHeight = view.homepageScreenVM.blockBottomRightHeight*0.8f;
		view.packsVM.blocks=new Rect[view.packsVM.elementPerRow];
		for(int i=0;i<view.packsVM.blocks.Length;i++)
		{
			view.packsVM.blocks[i]=new Rect(view.homepageScreenVM.blockBottomRight.xMin+(i%view.packsVM.elementPerRow)*view.packsVM.blocksWidth,
			                               view.homepageScreenVM.blockBottomRight.yMin+0.2f*view.packsVM.blocksHeight+Mathf.FloorToInt(i/view.packsVM.elementPerRow)*view.packsVM.blocksHeight,
			                               view.packsVM.blocksWidth,
			                               view.packsVM.blocksHeight);
		}
		view.packsVM.chosenPage = 0;
		view.notificationsVM.chosenPage = 0;
		view.newsVM.chosenPage = 0;
		this.initPagination ();
		this.displayNotificationsPage ();
		this.displayNewsPage ();
		this.displayPacksPage ();
		if(this.tutorial!=null)
		{
			this.tutorial.GetComponent<HomePageTutorialController>().resize();
		}
	}
	private IEnumerator updateReadNotifications(bool firstLoad)
	{
		IList<int> tempList = new List<int> ();
		for (int i=view.notificationsVM.start;i<view.notificationsVM.finish;i++)
		{
			if(i==model.notificationSystemIndex)
			{
				tempList.Add (i);
				model.notificationSystemIndex=-1;
			}
			else if(!model.notifications[i].Notification.IsRead)
			{
				tempList.Add (i);
			}
		}
		if(firstLoad)
		{
			MenuObject.GetComponent<MenuController>().setNbNotificationsNonRead(view.notificationsVM.nbNonReadNotifications-tempList.Count);
		}
		if(tempList.Count>0)
		{
			yield return StartCoroutine(model.updateReadNotifications (tempList,this.totalNbResultLimit));
			MenuObject.GetComponent<MenuController>().setNbNotificationsNonRead(model.player.nonReadNotifications);
		}
		yield break;
	}
	public void displayNotificationsPage()
	{	
		view.notificationsVM.start = view.notificationsVM.chosenPage*(view.notificationsVM.elementPerRow*3);
		if (model.notifications.Count < (3*view.notificationsVM.elementPerRow*(view.notificationsVM.chosenPage+1)))
		{
			view.notificationsVM.finish = model.notifications.Count;
		}
		else
		{
			view.notificationsVM.finish = (view.notificationsVM.chosenPage+1)*(3 * view.notificationsVM.elementPerRow);
		}
		view.notificationsVM.username = new List<string> ();
		view.notificationsVM.totalNbWins = new List<int> ();
		view.notificationsVM.totalNbLooses = new List<int> ();
		view.notificationsVM.ranking = new List<int> ();
		view.notificationsVM.division = new List<int> ();
		view.notificationsVM.content = new List<string> ();
		view.notificationsVM.date = new List<DateTime> ();
		view.notificationsVM.profilePicturesButtonStyle = new List<GUIStyle> ();
		view.notificationsVM.nonReadNotifications = new List<bool> ();
		for(int i =view.notificationsVM.start;i<view.notificationsVM.finish;i++)
		{
			view.notificationsVM.username.Add (model.notifications[i].SendingUser.Username);
			view.notificationsVM.totalNbWins.Add (model.notifications[i].SendingUser.TotalNbWins);
			view.notificationsVM.totalNbLooses.Add (model.notifications[i].SendingUser.TotalNbLooses);
			view.notificationsVM.ranking.Add (model.notifications[i].SendingUser.Ranking);
			view.notificationsVM.division.Add (model.notifications[i].SendingUser.Division);
			view.notificationsVM.content.Add (model.notifications[i].Content);
			view.notificationsVM.date.Add (model.notifications[i].Notification.Date);
			view.notificationsVM.profilePicturesButtonStyle.Add (new GUIStyle());
			view.notificationsVM.profilePicturesButtonStyle[i-view.notificationsVM.start].normal.background=model.notifications[i].SendingUser.texture;
			StartCoroutine(model.notifications[i].SendingUser.setProfilePicture());
			if(!model.notifications[i].Notification.IsRead || i==model.notificationSystemIndex)
			{
				view.notificationsVM.nonReadNotifications.Add(true);
			}
			else
			{
				view.notificationsVM.nonReadNotifications.Add(false);
			}
		}
	}
	public void displayNewsPage()
	{	
		view.newsVM.start = view.newsVM.chosenPage*(view.newsVM.elementPerRow*5);
		if (model.news.Count < (5*view.newsVM.elementPerRow*(view.newsVM.chosenPage+1)))
		{
			view.newsVM.finish = model.news.Count;
		}
		else
		{
			view.newsVM.finish = (view.newsVM.chosenPage+1)*(5 * view.newsVM.elementPerRow);
		}
		view.newsVM.username = new List<string> ();
		view.newsVM.totalNbWins = new List<int> ();
		view.newsVM.totalNbLooses = new List<int> ();
		view.newsVM.ranking = new List<int> ();
		view.newsVM.division = new List<int> ();
		view.newsVM.content = new List<string> ();
		view.newsVM.date = new List<DateTime> ();
		view.newsVM.profilePicturesButtonStyle = new List<GUIStyle> ();
		for(int i =view.newsVM.start;i<view.newsVM.finish;i++)
		{
			view.newsVM.username.Add (model.news[i].User.Username);
			view.newsVM.totalNbWins.Add (model.news[i].User.TotalNbWins);
			view.newsVM.totalNbLooses.Add (model.news[i].User.TotalNbLooses);
			view.newsVM.ranking.Add (model.news[i].User.Ranking);
			view.newsVM.division.Add (model.news[i].User.Division);
			view.newsVM.content.Add (model.news[i].Content);
			view.newsVM.date.Add (model.news[i].News.Date);
			view.newsVM.profilePicturesButtonStyle.Add (new GUIStyle());
			view.newsVM.profilePicturesButtonStyle[i-view.newsVM.start].normal.background=model.news[i].User.texture;
			StartCoroutine(model.news[i].User.setProfilePicture());
		}
	}
	public void displayPacksPage()
	{	
		view.packsVM.start = view.packsVM.chosenPage*view.packsVM.elementPerRow;
		if (model.packs.Count < (view.packsVM.elementPerRow*(view.packsVM.chosenPage+1)))
		{
			view.packsVM.finish = model.packs.Count;
		}
		else
		{
			view.packsVM.finish = (view.packsVM.chosenPage+1)*view.packsVM.elementPerRow;
		}
		view.packsVM.packsNames = new List<string> ();
		view.packsVM.packsNew = new List<bool> ();
		view.packsVM.packsPrice = new List<int> ();
		view.packsVM.packPicturesButtonStyle = new List<GUIStyle> ();
		for(int i =view.packsVM.start;i<view.packsVM.finish;i++)
		{
			view.packsVM.packsNames.Add (model.packs[i].Name);
			view.packsVM.packsNew.Add (model.packs[i].New);
			view.packsVM.packsPrice.Add (model.packs[i].Price);
			view.packsVM.packPicturesButtonStyle.Add (new GUIStyle());
			view.packsVM.packPicturesButtonStyle[i-view.packsVM.start].normal.background=model.packs[i].texture;
			StartCoroutine(model.packs[i].setPicture());
		}
	}
	public void buyPackHandler(int index)
	{
		ApplicationModel.packToBuy = model.packs [view.packsVM.start + index].Id;
		Application.LoadLevel ("Store");
	}
	public void newsPaginationBack()
	{
		view.newsVM.pageDebut = view.newsVM.pageDebut-10;
		view.newsVM.pageFin = view.newsVM.pageDebut+10;
	}
	public void newsPaginationSelect(int chosenPage)
	{
		view.newsVM.paginatorGuiStyle[view.newsVM.chosenPage]=view.homepageVM.paginationStyle;
		view.newsVM.chosenPage=chosenPage;
		view.newsVM.paginatorGuiStyle[chosenPage]=view.homepageVM.paginationActivatedStyle;
		this.displayNewsPage();
	}
	public void newsPaginationNext()
	{
		view.newsVM.pageDebut = view.newsVM.pageDebut+10;
		view.newsVM.pageFin= Mathf.Min(view.newsVM.pageFin+10, view.newsVM.nbPages);
	}
	public void notificationsPaginationBack()
	{
		view.notificationsVM.pageDebut = view.notificationsVM.pageDebut-10;
		view.notificationsVM.pageFin = view.notificationsVM.pageDebut+10;
	}
	public void notificationsPaginationSelect(int chosenPage)
	{
		view.notificationsVM.paginatorGuiStyle[view.notificationsVM.chosenPage]=view.homepageVM.paginationStyle;
		view.notificationsVM.chosenPage=chosenPage;
		view.notificationsVM.paginatorGuiStyle[chosenPage]=view.homepageVM.paginationActivatedStyle;
		this.displayNotificationsPage();
		this.manageNonReadsNotifications (false);
	}
	public void notificationsPaginationNext()
	{
		view.notificationsVM.pageDebut = view.notificationsVM.pageDebut+10;
		view.notificationsVM.pageFin= Mathf.Min(view.notificationsVM.pageFin+10, view.notificationsVM.nbPages);
	}
	public void cleanCardsHandler()
	{
		StartCoroutine (this.cleanCards ());
	}
	private IEnumerator cleanCards()
	{
		this.setGui (false);
		yield return StartCoroutine (model.player.cleanCards ());
		this.setGui (true);
		if(model.player.CollectionRanking!=0)
		{
			view.ranksVM.collectionRanking="Class collection : " + model.player.CollectionRanking.ToString()+ " ("+ model.player.CollectionPoints.ToString() + " pts)";
		}
		else
		{
			view.ranksVM.collectionRanking="";
		}
	}
	public void setGui(bool value)
	{
		view.homepageVM.guiEnabled = value;
		this.setButtonsGui (value);
	}
	public void setButtonsGui(bool value)
	{
		view.homepageVM.buttonsEnabled=value;
	}
	public IEnumerator endTutorial()
	{
		MenuController.instance.setButtonsGui (false);
		yield return StartCoroutine (model.player.setTutorialStep (2));
		Application.LoadLevel ("MyGame");
	}
	public void displayConnectionBonusPopUp(int connectionBonus)
	{
		this.setGui (false);
		this.connectionBonusView= Camera.main.gameObject.AddComponent <HomePageConnectionBonusPopUpView>();
		connectionBonusView.connectionBonusPopUpVM.bonus = connectionBonus.ToString();
		connectionBonusView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			connectionBonusView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		connectionBonusView.popUpVM.initStyles();
		this.connectionBonusPopUpResize ();
	}
	public void hideConnectionBonusPopUp()
	{
		this.setGui (true);
		Destroy (this.connectionBonusView);
	}
	public void connectionBonusPopUpResize()
	{
		connectionBonusView.popUpVM.centralWindow = view.homepageScreenVM.centralWindow;
		connectionBonusView.popUpVM.resize ();
	}
}

