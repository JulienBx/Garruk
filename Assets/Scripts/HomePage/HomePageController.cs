using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageController : MonoBehaviour {

	public GameObject MenuObject;

	public GUIStyle[] notificationsVMStyle;
	public GUIStyle[] newsVMStyle;
	public GUIStyle[] homepageScreenVMStyle;
	public GUIStyle[] homepageVMStyle;

	private float timer;
	private bool isDataLoaded=false;
	public int refreshInterval;
	public int totalNbResultLimit;
	
	private HomePageView view;
	public static HomePageController instance;
	private HomePageModel model;
	
	void Start () {
		instance = this;
		this.model = new HomePageModel ();
		this.view = Camera.main.gameObject.AddComponent <HomePageView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization());
	}
	void Update(){
		
		timer += Time.deltaTime;
		
		if (timer > refreshInterval) 
		{
			timer=timer-refreshInterval;
			StartCoroutine(model.player.countNonReadsNotifications(this.totalNbResultLimit));
		}
		if(model.player.nonReadNotifications>0)
		{
			MenuObject.GetComponent<MenuController>().setNbNotificationsNonRead(
				model.player.nonReadNotifications+view.notificationsVM.displayedNonReadNotifications);
		}
		else if(isDataLoaded)
		{
			MenuObject.GetComponent<MenuController>().setNbNotificationsNonRead(view.notificationsVM.nbNonReadNotifications);
		}
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine (model.getData (this.totalNbResultLimit));
		view.notificationsVM.notifications=model.notifications;
		view.newsVM.news = this.filterNews (model.news,model.player.Id);
		this.initLabelsNo ();
		this.picturesInitialization ();
		this.initStyles ();
		this.toLoadData ();
		this.isDataLoaded = true;
	}
	private void toLoadData(){
		this.resize ();
		this.manageNonReadsNotifications ();
	}
	private void manageNonReadsNotifications(){
		this.computeNonReadsNotifications ();
		this.initLabelTitle ();
		this.updateReadNotifications ();
	}
	private void picturesInitialization(){

		view.notificationsVM.profilePicturesButtonStyle=new List<GUIStyle>();
		view.newsVM.profilePicturesButtonStyle = new List<GUIStyle> ();
		
		for(int i =0;i<model.notifications.Count;i++)
		{
			view.notificationsVM.profilePicturesButtonStyle.Add(new GUIStyle());
			view.notificationsVM.profilePicturesButtonStyle[i].normal.background=model.notifications[i].SendingUser.texture;
			StartCoroutine(model.notifications[i].SendingUser.setProfilePicture());
		}
		for(int i =0;i<model.news.Count;i++)
		{
			view.newsVM.profilePicturesButtonStyle.Add(new GUIStyle());
			view.newsVM.profilePicturesButtonStyle[i].normal.background=model.news[i].User.texture;
			StartCoroutine(model.news[i].User.setProfilePicture());
		}
	}
	private IList<DisplayedNews> filterNews(IList<DisplayedNews> newsToFilter, int playerId)
	{
		IList<DisplayedNews> filterednews = new List<DisplayedNews>();
		bool toAdd = true;
		for (int i=0;i<newsToFilter.Count;i++)
		{
			if(newsToFilter[i].News.IdNewsType==1)
			{
				if(newsToFilter[i].Users[0].Id!=playerId)
				{
					toAdd=true;
					for(int j=0;j<filterednews.Count;j++)
					{
						if (filterednews[j].Users[0].Id==newsToFilter[i].User.Id
						    && filterednews[j].User.Id==newsToFilter[i].Users[0].Id)
						{
							toAdd=false;
							break;
						}
					}
					if(toAdd)
					{
						filterednews.Add (newsToFilter[i]);
					}
				}
			}
			else if(newsToFilter[i].News.IdNewsType!=1)
			{
				filterednews.Add (model.news[i]);
			}
		}
		return filterednews;
	}
	private void computeNonReadsNotifications()
	{
		bool firstSystemNotification=false;
		view.notificationsVM.nonReadNotifications = new List<bool> ();
		view.notificationsVM.nbNonReadNotifications = 0;

		for(int i=0;i<model.notifications.Count;i++)
		{
			if(!model.notifications[i].Notification.IsRead && 
			   model.notifications[i].Notification.IdNotificationType!=1)
			{
				view.notificationsVM.nonReadNotifications.Add (true);
				view.notificationsVM.nbNonReadNotifications++;
			}
			else if(model.notifications[i].Notification.IdNotificationType==1 &&
			        !firstSystemNotification && !model.player.readnotificationsystem)
			{
				firstSystemNotification=true;
				view.notificationsVM.nonReadNotifications.Add (true);
				view.notificationsVM.nbNonReadNotifications++;
			}
			else
			{
				view.notificationsVM.nonReadNotifications.Add (false);
			}
		}
	}
	private void initLabelsNo()
	{
		if(model.notifications.Count==0)
		{
			view.notificationsVM.labelNo="Vous n'avez pas de notification";
		}
		if(view.newsVM.news.Count==0)
		{
			view.newsVM.labelNo="Il n'y a pas d'actualités à afficher";
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
	private void initPagination()
	{
		view.notificationsVM.nbPages = Mathf.CeilToInt((view.notificationsVM.notifications.Count-1) / (10*view.notificationsVM.elementPerRow))+1;
		view.notificationsVM.pageDebut = 0 ;
		if (view.notificationsVM.nbPages>10){
			view.notificationsVM.pageFin = 9 ;
		}
		else{
			view.notificationsVM.pageFin = view.notificationsVM.nbPages;
		}
		view.notificationsVM.paginatorGuiStyle = new GUIStyle[view.notificationsVM.nbPages];
		for (int i = 0; i < view.notificationsVM.nbPages; i++) { 
			if (i==0){
				view.notificationsVM.paginatorGuiStyle[i]=view.homepageVM.paginationActivatedStyle;
			}
			else{
				view.notificationsVM.paginatorGuiStyle[i]=view.homepageVM.paginationStyle;
			}
		}
		view.newsVM.nbPages = Mathf.CeilToInt((view.newsVM.news.Count-1) / (10*view.newsVM.elementPerRow))+1;
		view.newsVM.pageDebut = 0 ;
		if (view.newsVM.nbPages>10){
			view.newsVM.pageFin = 9 ;
		}
		else{
			view.newsVM.pageFin = view.newsVM.nbPages;
		}
		view.newsVM.paginatorGuiStyle = new GUIStyle[view.newsVM.nbPages];
		for (int i = 0; i < view.newsVM.nbPages; i++) { 
			if (i==0){
				view.newsVM.paginatorGuiStyle[i]=view.homepageVM.paginationActivatedStyle;
			}
			else{
				view.newsVM.paginatorGuiStyle[i]=view.homepageVM.paginationStyle;
			}
		}
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
	}
	public void resize()
	{
		view.homepageScreenVM.resize ();
		view.homepageVM.resize (view.homepageScreenVM.heightScreen);
		view.notificationsVM.resize (view.homepageScreenVM.heightScreen);
		view.homepageVM.resize (view.homepageScreenVM.heightScreen);
		view.newsVM.resize (view.homepageScreenVM.heightScreen);

		view.notificationsVM.elementPerRow = 1;
		view.notificationsVM.blocksWidth = view.homepageScreenVM.blockLeftWidth / view.notificationsVM.elementPerRow;
		view.notificationsVM.blocksHeight = 0.9f*(view.homepageScreenVM.blockLeftHeight / 10f);
		view.notificationsVM.blocks=new Rect[10*view.notificationsVM.elementPerRow];
		for(int i=0;i<view.notificationsVM.blocks.Length;i++)
		{
			view.notificationsVM.blocks[i]=new Rect(view.homepageScreenVM.blockLeft.xMin+(i%view.notificationsVM.elementPerRow)*view.notificationsVM.blocksWidth,
			                                    view.homepageScreenVM.blockLeft.yMin+0.05f*view.homepageScreenVM.blockLeftHeight+Mathf.FloorToInt(i/view.notificationsVM.elementPerRow)*view.notificationsVM.blocksHeight,
			                                    view.notificationsVM.blocksWidth,
			                                    view.notificationsVM.blocksHeight);
		}
		view.newsVM.elementPerRow = 1;
		view.newsVM.blocksWidth = view.homepageScreenVM.blockRightWidth / view.newsVM.elementPerRow;
		view.newsVM.blocksHeight = 0.9f*(view.homepageScreenVM.blockRightHeight / 10f);
		view.newsVM.blocks=new Rect[10*view.newsVM.elementPerRow];
		for(int i=0;i<view.newsVM.blocks.Length;i++)
		{
			view.newsVM.blocks[i]=new Rect(view.homepageScreenVM.blockRight.xMin+(i%view.newsVM.elementPerRow)*view.newsVM.blocksWidth,
			                                        view.homepageScreenVM.blockRight.yMin+0.05f*view.homepageScreenVM.blockRightHeight+Mathf.FloorToInt(i/view.newsVM.elementPerRow)*view.newsVM.blocksHeight,
			                                        view.newsVM.blocksWidth,
			                                        view.newsVM.blocksHeight);
		}
		view.notificationsVM.chosenPage = 0;
		view.newsVM.chosenPage = 0;
		this.initPagination ();
		view.notificationsVM.displayPage ();
		view.newsVM.displayPage ();
	}
	private void updateReadNotifications()
	{
		IList<int> tempList = new List<int> ();
		for (int i=view.notificationsVM.start;i<view.notificationsVM.finish;i++)
		{
			if(view.notificationsVM.nonReadNotifications[i])
			{
				tempList.Add (i);
			}
		}
		view.notificationsVM.displayedNonReadNotifications = tempList.Count;
		if(tempList.Count>0)
		{
			StartCoroutine(model.updateReadNotifications (tempList));
		}
	}
	public void pagination(int section, int scenario, int chosenPage=0)
	{
		switch(section)
		{
		case 0:
			switch(scenario)
			{
			case 0:
				view.notificationsVM.pageDebut = view.notificationsVM.pageDebut-10;
				view.notificationsVM.pageFin = view.notificationsVM.pageDebut+10;
				break;
			case 1:
				view.notificationsVM.paginatorGuiStyle[view.notificationsVM.chosenPage]=view.homepageVM.paginationStyle;
				view.notificationsVM.chosenPage=chosenPage;
				view.notificationsVM.paginatorGuiStyle[chosenPage]=view.homepageVM.paginationActivatedStyle;
				view.notificationsVM.displayPage();
				break;
			case 2:
				view.notificationsVM.pageDebut = view.notificationsVM.pageDebut+10;
				view.notificationsVM.pageFin= Mathf.Min(view.notificationsVM.pageFin+10, view.notificationsVM.nbPages);
				break;
			}
			this.manageNonReadsNotifications ();
			break;
		case 1:
			switch(scenario)
			{
			case 0:
				view.newsVM.pageDebut = view.newsVM.pageDebut-10;
				view.newsVM.pageFin = view.newsVM.pageDebut+10;
				break;
			case 1:
				view.newsVM.paginatorGuiStyle[view.newsVM.chosenPage]=view.homepageVM.paginationStyle;
				view.newsVM.chosenPage=chosenPage;
				view.newsVM.paginatorGuiStyle[chosenPage]=view.homepageVM.paginationActivatedStyle;
				view.newsVM.displayPage();
				break;
			case 2:
				view.newsVM.pageDebut = view.newsVM.pageDebut+10;
				view.newsVM.pageFin= Mathf.Min(view.newsVM.pageFin+10, view.newsVM.nbPages);
				break;
			}
			break;
		}
	}
}

