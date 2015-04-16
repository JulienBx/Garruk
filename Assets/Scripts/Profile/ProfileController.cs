using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileController : MonoBehaviour {

	public GameObject MenuObject;

	private ProfileView view;
	public static ProfileController instance;
	private ProfileModel model;

	//public GUIStyle[] myFriendsVMStyle;
	//public GUIStyle[] invitationsSentVMStyle;
	//public GUIStyle[] invitationsReceivedVMStyle;
	public GUIStyle[] friendshipStatusVMStyle;
	public GUIStyle[] statsVMStyle;
	public GUIStyle[] userProfileVMStyle;
	public GUIStyle[] profileScreenVMStyle;
	public GUIStyle[] profileVMStyle;
	public GUIStyle[] myTrophiesVMStyle;
	
	void Start () {
		
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <ProfileView>();
		this.model = new ProfileModel ();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization(){
		yield return StartCoroutine (model.getProfile ());
		ApplicationModel.profileChosen = "";
		this.picturesInitialization ();
		view.myTrophiesVM.trophies = model.Trophies;
		view.userProfileVM.Profile = model.Profile;
		view.statsVM = new StatsViewModel (model.Profile.TotalNbWins,
		                                   model.Profile.TotalNbLooses,
		                                   model.Profile.Division,
		                                   model.Profile.Ranking,
		                                   model.Profile.RankingPoints);
		if(model.Profile.Username!=ApplicationModel.username)
		{
			view.profileVM.isMyProfile=false;
			view.profileScreenVM.displayTopRightBlock=true;
			view.myFriendsVM.title="Les amis de "+model.Profile.Username;
		}
		this.initStyles ();
		this.loadData ();
		this.pictures ();
		view.setCanDisplay (true);
	}
	private void loadData()
	{
		this.computeConnections ();
		this.computeLabelsNo ();
		if(model.Profile.Username!=ApplicationModel.username)
		{
			this.computeFriendshipStatus();
		}
		this.resize ();
	}
	private void computeConnections(){

		view.myFriendsVM.contacts = new List<User> ();
		view.myFriendsVM.profilePictures = new List<int> ();
		view.invitationsReceivedVM.Contacts = new List<User> ();
		view.invitationsReceivedVM.profilePictures = new List<int> ();
		view.invitationsSentVM.Contacts = new List<User> ();
		view.invitationsSentVM.profilePictures = new List<int> ();

		for (int i =0; i<model.Connections.Count;i++)
		{
			if (model.Connections[i].IsAccepted)
			{
				view.myFriendsVM.contacts.Add (model.Contacts[i]);
				view.myFriendsVM.profilePictures.Add (i);
			}
			else if(model.Profile.Username==ApplicationModel.username)
			{
				if(model.Connections[i].IdUser1==model.Player.Id)
				{
					view.invitationsSentVM.Contacts.Add (model.Contacts[i]);
					view.invitationsSentVM.profilePictures.Add (i);
				}
				else if(model.Connections[i].IdUser2==model.Player.Id) 
				{
					view.invitationsReceivedVM.Contacts.Add (model.Contacts[i]);
					view.invitationsReceivedVM.profilePictures.Add (i);
				}
			}
		}
	}
	private void computeLabelsNo ()
	{
		if(view.myFriendsVM.contacts.Count==0)
		{
			view.myFriendsVM.labelNo="Vous n'avez pas encore d'amis";
		}
		else
		{
			view.myFriendsVM.labelNo="";
		}
		if(model.Profile.Username==ApplicationModel.username)
		{
			if(view.invitationsReceivedVM.Contacts.Count==0)
			{
				view.invitationsReceivedVM.labelNo="Vous n'avez pas d'invitations en attente";
			}
			else
			{
				view.invitationsReceivedVM.labelNo="";
			}
			if(view.invitationsSentVM.Contacts.Count==0)
			{
				view.invitationsSentVM.labelNo="Vous n'avez pas envoye d'invitations";
			}
			else
			{
				view.invitationsSentVM.labelNo="";
			}
		}
	}
	private void picturesInitialization(){

		view.userProfileVM.ProfilePicture = model.Profile.texture;
		view.profileVM.contactsPicturesButtonStyle=new List<GUIStyle>();
		view.myTrophiesVM.trophiesPicturesButtonStyle=new List<GUIStyle>();
		
		for(int i =0;i<model.Contacts.Count;i++)
		{
			view.profileVM.contactsPicturesButtonStyle.Add(new GUIStyle());
			view.profileVM.contactsPicturesButtonStyle[i].normal.background=model.Contacts[i].texture;
		}
		for(int i =0;i<model.Trophies.Count;i++)
		{
			view.myTrophiesVM.trophiesPicturesButtonStyle.Add(new GUIStyle());
			view.myTrophiesVM.trophiesPicturesButtonStyle[i].normal.background=model.Trophies[i].texture;
		}
	}
	public void pictures(){
		
		StartCoroutine(model.Profile.setProfilePicture());
		for(int i =0;i<model.Contacts.Count;i++)
		{
			StartCoroutine(model.Contacts[i].setProfilePicture());
		}
		for(int i =0;i<model.Trophies.Count;i++)
		{
			StartCoroutine(model.Trophies[i].setPicture());
		}
	}
	private void initializePagination()
	{
		view.myFriendsVM.nbPages = Mathf.CeilToInt((view.myFriendsVM.contacts.Count-1) / (3*view.myFriendsVM.elementPerRow))+1;
		view.myFriendsVM.pageDebut = 0 ;
		if (view.myFriendsVM.nbPages>10){
			view.myFriendsVM.pageFin = 9 ;
		}
		else{
			view.myFriendsVM.pageFin = view.myFriendsVM.nbPages;
		}
		view.myFriendsVM.paginatorGuiStyle = new GUIStyle[view.myFriendsVM.nbPages];
		for (int i = 0; i < view.myFriendsVM.nbPages; i++) { 
			if (i==0){
				view.myFriendsVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
			}
			else{
				view.myFriendsVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
			}
		}
		view.myTrophiesVM.nbPages = Mathf.CeilToInt((view.myTrophiesVM.trophies.Count-1) / (3*view.myTrophiesVM.elementPerRow))+1;
		view.myTrophiesVM.pageDebut = 0 ;
		if (view.myTrophiesVM.nbPages>10){
			view.myTrophiesVM.pageFin = 9 ;
		}
		else{
			view.myTrophiesVM.pageFin = view.myTrophiesVM.nbPages;
		}
		view.myTrophiesVM.paginatorGuiStyle = new GUIStyle[view.myTrophiesVM.nbPages];
		for (int i = 0; i < view.myTrophiesVM.nbPages; i++) { 
			if (i==0){
				view.myTrophiesVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
			}
			else{
				view.myTrophiesVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
			}
		}
		if(model.Profile.Username==ApplicationModel.username)
		{
			view.invitationsReceivedVM.nbPages = Mathf.CeilToInt((view.invitationsReceivedVM.Contacts.Count-1) / (3*view.invitationsReceivedVM.elementPerRow))+1;
			view.invitationsReceivedVM.pageDebut = 0 ;
			if (view.invitationsReceivedVM.nbPages>10){
				view.invitationsReceivedVM.pageFin = 9 ;
			}
			else{
				view.invitationsReceivedVM.pageFin = view.invitationsReceivedVM.nbPages;
			}
			view.invitationsReceivedVM.paginatorGuiStyle = new GUIStyle[view.invitationsReceivedVM.nbPages];
			for (int i = 0; i < view.invitationsReceivedVM.nbPages; i++) { 
				if (i==0){
					view.invitationsReceivedVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
				}
				else{
					view.invitationsReceivedVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
				}
			}
			view.invitationsSentVM.nbPages = Mathf.CeilToInt((view.invitationsSentVM.Contacts.Count-1) / (3*view.invitationsSentVM.elementPerRow))+1;
			view.invitationsSentVM.pageDebut = 0 ;
			if (view.invitationsSentVM.nbPages>10){
				view.invitationsSentVM.pageFin = 9 ;
			}
			else{
				view.invitationsSentVM.pageFin = view.invitationsSentVM.nbPages;
			}
			view.invitationsSentVM.paginatorGuiStyle = new GUIStyle[view.invitationsSentVM.nbPages];
			for (int i = 0; i < view.invitationsSentVM.nbPages; i++) { 
				if (i==0){
					view.invitationsSentVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
				}
				else{
					view.invitationsSentVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
				}
			}
		}
	}
	private void computeFriendshipStatus()
	{
		view.friendshipStatusVM.username = model.Profile.Username;
		for(int i=0;i<model.Connections.Count;i++)
		{
			if(model.Connections[i].IdUser1==model.Player.Id && model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=1;
				view.friendshipStatusVM.indexConnection=model.Connections[i].Id;
				break;
			}
			else if(model.Connections[i].IdUser1==model.Player.Id && !model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=2;
				view.friendshipStatusVM.indexConnection=model.Connections[i].Id;
				break;
			}
			else if(model.Connections[i].IdUser2==model.Player.Id && model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=1;
				view.friendshipStatusVM.indexConnection=model.Connections[i].Id;
				break;
			}
			else if(model.Connections[i].IdUser2==model.Player.Id && !model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=0;
				view.friendshipStatusVM.indexConnection=model.Connections[i].Id;
				break;
			}
			if (i==model.Connections.Count-1)
			{
				view.friendshipStatusVM.status=3;
			}
		}
	}
	private void initStyles()
	{
		//for(int i=0;i<this.myFriendsVMStyle.Length-1;i++)
		//{
		//view.myFriendsVM.styles[i]=this.myFriendsVMStyle[i];
		//}
		//for(int i=0;i<this.invitationsSentVMStyle.Length-1;i++)
		//{
		//	view.invitationsSentVM.styles[i]=this.invitationsSentVMStyle[i];
		//}
		//for(int i=0;i<this.invitationsReceivedVMStyle.Length-1;i++)
		//{
		//	view.invitationsReceivedVM.styles[i]=this.invitationsReceivedVMStyle[i];
		//}
		view.profileScreenVM.styles=new GUIStyle[this.profileScreenVMStyle.Length];
		for(int i=0;i<this.profileScreenVMStyle.Length;i++)
		{
			view.profileScreenVM.styles[i]=this.profileScreenVMStyle[i];
		}
		view.profileScreenVM.initStyles();
		view.userProfileVM.styles=new GUIStyle[this.userProfileVMStyle.Length];
		for(int i=0;i<this.userProfileVMStyle.Length;i++)
		{
			view.userProfileVM.styles[i]=this.userProfileVMStyle[i];
		}
		view.userProfileVM.initStyles();
		view.statsVM.styles=new GUIStyle[this.statsVMStyle.Length];
		for(int i=0;i<this.statsVMStyle.Length;i++)
		{
			view.statsVM.styles[i]=this.statsVMStyle[i];
		}
		view.statsVM.initStyles();
		view.profileVM.styles=new GUIStyle[this.profileVMStyle.Length];
		for(int i=0;i<this.profileVMStyle.Length;i++)
		{
			view.profileVM.styles[i]=this.profileVMStyle[i];
		}
		view.profileVM.initStyles();
		view.friendshipStatusVM.styles=new GUIStyle[this.friendshipStatusVMStyle.Length];
		for(int i=0;i<this.friendshipStatusVMStyle.Length;i++)
		{
			view.friendshipStatusVM.styles[i]=this.friendshipStatusVMStyle[i];
		}
		view.friendshipStatusVM.initStyles();
		view.myTrophiesVM.styles=new GUIStyle[this.myTrophiesVMStyle.Length];
		for(int i=0;i<this.myTrophiesVMStyle.Length;i++)
		{
			view.myTrophiesVM.styles[i]=this.myTrophiesVMStyle[i];
		}
		view.myTrophiesVM.initStyles();
	}
	public void resize()
	{
		view.profileScreenVM.resize ();
		view.friendshipStatusVM.resize (view.profileScreenVM.heightScreen);
		view.invitationsReceivedVM.resize (view.profileScreenVM.heightScreen);
		view.invitationsSentVM.resize (view.profileScreenVM.heightScreen);
		view.statsVM.resize (view.profileScreenVM.heightScreen);
		view.userProfileVM.resize (view.profileScreenVM.heightScreen);
		view.myFriendsVM.resize (view.profileScreenVM.heightScreen);
		view.profileVM.resize (view.profileScreenVM.heightScreen);

		view.userProfileVM.profilePictureWidth = (int)view.profileScreenVM.blockLeftWidth-2*(int)view.profileScreenVM.gapBetweenblocks;
		view.userProfileVM.profilePictureHeight = view.userProfileVM.profilePictureWidth;

		view.userProfileVM.profilePictureRect = new Rect (view.profileScreenVM.blockLeft.xMin + (view.profileScreenVM.blockLeftWidth-view.userProfileVM.profilePictureWidth)/2,
		                                                  view.profileScreenVM.blockLeft.yMin + 0.05f*view.profileScreenVM.blockLeftHeight,
		                                                  view.userProfileVM.profilePictureWidth,
		                                                  view.userProfileVM.profilePictureHeight);

		view.myFriendsVM.elementPerRow = 4 + 2*Mathf.FloorToInt(((float)view.profileScreenVM.blockTopCenterWidth/(float)view.profileScreenVM.blockTopCenterHeight - 1.5f));
		view.invitationsReceivedVM.elementPerRow=Mathf.FloorToInt(view.myFriendsVM.elementPerRow/2f);
		view.invitationsSentVM.elementPerRow=view.invitationsReceivedVM.elementPerRow;
		view.myTrophiesVM.elementPerRow = 1;

		view.myFriendsVM.blocksWidth = view.profileScreenVM.blockTopCenterWidth / view.myFriendsVM.elementPerRow;
		view.myFriendsVM.blocksHeight = 0.8f*(view.profileScreenVM.blockTopCenterHeight / 3);
		view.myFriendsVM.blocks=new Rect[3*view.myFriendsVM.elementPerRow];
		for(int i=0;i<view.myFriendsVM.blocks.Length;i++)
		{
			view.myFriendsVM.blocks[i]=new Rect(view.profileScreenVM.blockTopCenter.xMin+(i%view.myFriendsVM.elementPerRow)*view.myFriendsVM.blocksWidth,
			                                    view.profileScreenVM.blockTopCenter.yMin+0.1f*view.profileScreenVM.blockTopCenterHeight+Mathf.FloorToInt(i/view.myFriendsVM.elementPerRow)*view.myFriendsVM.blocksHeight,
			                                    view.myFriendsVM.blocksWidth,
			                                    view.myFriendsVM.blocksHeight);
		}

		view.myTrophiesVM.blocksWidth = view.profileScreenVM.blockBottomRightWidth / view.myTrophiesVM.elementPerRow;
		view.myTrophiesVM.blocksHeight = 0.8f*(view.profileScreenVM.blockBottomRightHeight / 3);
		view.myTrophiesVM.blocks=new Rect[3*view.myTrophiesVM.elementPerRow];
		for(int i=0;i<view.myTrophiesVM.blocks.Length;i++)
		{
			view.myTrophiesVM.blocks[i]=new Rect(view.profileScreenVM.blockBottomRight.xMin+(i%view.myTrophiesVM.elementPerRow)*view.myTrophiesVM.blocksWidth,
			                                     view.profileScreenVM.blockBottomRight.yMin+0.1f*view.profileScreenVM.blockBottomRightHeight+Mathf.FloorToInt(i/view.myTrophiesVM.elementPerRow)*view.myTrophiesVM.blocksHeight,
			                                     view.myTrophiesVM.blocksWidth,
			                                     view.myTrophiesVM.blocksHeight);
		}

		if(model.Profile.Username==ApplicationModel.username)
		{
			view.invitationsSentVM.blocksWidth = view.profileScreenVM.blockBottomCenterLeftWidth / view.invitationsSentVM.elementPerRow;
			view.invitationsSentVM.blocksHeight = 0.8f*(view.profileScreenVM.blockBottomCenterLeftHeight / 3);
			view.invitationsSentVM.blocks=new Rect[3*view.invitationsSentVM.elementPerRow];
			for(int i=0;i<view.invitationsSentVM.blocks.Length;i++)
			{
				view.invitationsSentVM.blocks[i]=new Rect(view.profileScreenVM.blockBottomCenterLeft.xMin+(i%view.invitationsSentVM.elementPerRow)*view.invitationsSentVM.blocksWidth,
				                                          view.profileScreenVM.blockBottomCenterLeft.yMin+0.1f*view.profileScreenVM.blockBottomCenterLeftHeight+Mathf.FloorToInt(i/view.invitationsSentVM.elementPerRow)*view.invitationsSentVM.blocksHeight,
				                                    view.invitationsSentVM.blocksWidth,
				                                    view.myFriendsVM.blocksHeight);
			}

			view.invitationsReceivedVM.blocksWidth = view.profileScreenVM.blockBottomCenterRightWidth / view.invitationsReceivedVM.elementPerRow;
			view.invitationsReceivedVM.blocksHeight = 0.8f*(view.profileScreenVM.blockBottomCenterRightHeight / 3);
			view.invitationsReceivedVM.blocks=new Rect[3*view.invitationsReceivedVM.elementPerRow];
			for(int i=0;i<view.invitationsReceivedVM.blocks.Length;i++)
			{
				view.invitationsReceivedVM.blocks[i]=new Rect(view.profileScreenVM.blockBottomCenterRight.xMin+(i%view.invitationsReceivedVM.elementPerRow)*view.invitationsReceivedVM.blocksWidth,
				                                          view.profileScreenVM.blockBottomCenterRight.yMin+0.1f*view.profileScreenVM.blockBottomCenterRightHeight+Mathf.FloorToInt(i/view.invitationsReceivedVM.elementPerRow)*view.invitationsReceivedVM.blocksHeight,
				                                          view.invitationsReceivedVM.blocksWidth,
				                                          view.myFriendsVM.blocksHeight);
			}
			view.invitationsReceivedVM.chosenPage = 0;
			view.invitationsSentVM.chosenPage = 0;
		}
		view.myFriendsVM.chosenPage = 0;
		view.myTrophiesVM.chosenPage = 0;
		this.initializePagination();
		view.myTrophiesVM.displayPage ();
		view.myFriendsVM.displayPage ();
		if(model.Profile.Username==ApplicationModel.username)
		{
			view.invitationsReceivedVM.displayPage ();
			view.invitationsSentVM.displayPage ();
		}
	}
	public IEnumerator addConnection()
	{
		model.Connections.Add (new Connection (model.Player.Id, model.Profile.Id, false));
		yield return StartCoroutine(model.Connections [model.Connections.Count - 1].add ());
		this.loadData ();
	}
	public void removeConnection(int indexConnection)
	{
		StartCoroutine(model.Connections [indexConnection].remove ());
		model.Connections.RemoveAt (indexConnection);
		this.loadData ();
	}
}

