using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileView : MonoBehaviour
{

	public FriendshipStatusViewModel friendshipStatusVM;
	public InvitationsReceivedViewModel invitationsReceivedVM;
	public InvitationsSentViewModel invitationsSentVM;
	public MyFriendsViewModel myFriendsVM;
	public ProfileViewModel profileVM;
	public StatsViewModel statsVM;
	public UserProfileViewModel userProfileVM;
	public ProfileScreenViewModel profileScreenVM;
	public MyTrophiesViewModel myTrophiesVM;

	private bool canDisplay=false;
	private bool isEditing=false;
	private bool checkPassword = false;

	private string tempOldPassword="";
	private string tempFirstName;
	private string tempSurname;
	private string tempMail;
	
	void Start()
	{
		friendshipStatusVM = new FriendshipStatusViewModel ();
		invitationsReceivedVM = new InvitationsReceivedViewModel ();
		invitationsSentVM = new InvitationsSentViewModel ();
		myFriendsVM = new MyFriendsViewModel ();
		profileVM = new ProfileViewModel ();
		statsVM = new StatsViewModel ();
		userProfileVM = new UserProfileViewModel ();
		profileScreenVM = new ProfileScreenViewModel ();
		myTrophiesVM = new MyTrophiesViewModel ();
	}

	public void setCanDisplay(bool value)
	{
		this.canDisplay = value;
	}
	public void setIsEditing(bool value)
	{
		this.isEditing= value;
	}
	void Update()
	{
		if (this.canDisplay)
		{
			if (Screen.width != profileScreenVM.widthScreen || Screen.height != profileScreenVM.heightScreen) {
				ProfileController.instance.resize();
			}
		}
	}
	void OnGUI()
	{
		if(checkPassword)
		{
			GUI.enabled=false;
		}
		if (canDisplay) 
		{
			//if (profileVM.isMyProfile){
			GUILayout.BeginArea(profileScreenVM.blockLeft,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label ("Mes informations",profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockLeftHeight*0.05f));
				GUILayout.Space (userProfileVM.profilePictureHeight); //editProfilePictureButtonStyle.fixedHeight
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space ((profileScreenVM.blockLeft.width-userProfileVM.profilePictureWidth)/2);
					GUILayout.BeginVertical();
					{
						GUILayout.Label ("Pseudo : " + userProfileVM.Profile.Username,userProfileVM.profileDataStyle);
						GUILayout.Label ("Argent : " + userProfileVM.Profile.Money + " credits",userProfileVM.profileDataStyle);
						if(!isEditing){
							GUILayout.Label ("Prenom : " + userProfileVM.Profile.FirstName,userProfileVM.profileDataStyle);
							GUILayout.Label ("Nom : " + userProfileVM.Profile.Surname,userProfileVM.profileDataStyle);
							GUILayout.Label (userProfileVM.Profile.Mail,userProfileVM.profileDataStyle);
							if (GUILayout.Button ("Modifier mes infos",userProfileVM.editProfileDataButtonStyle))
							{
								this.tempFirstName=userProfileVM.Profile.FirstName;
								this.tempSurname=userProfileVM.Profile.Surname;
								this.tempMail=userProfileVM.Profile.Mail;
								this.isEditing = true;
							}
							if (GUILayout.Button ("Changer le mot de passe",userProfileVM.editProfileDataButtonStyle))
							{
								this.checkPassword = true;
							}
						}
						if (isEditing){
							this.tempFirstName = GUILayout.TextField(this.tempFirstName, 15,userProfileVM.inputTextfieldStyle);
							this.tempSurname = GUILayout.TextField(this.tempSurname, 15,userProfileVM.inputTextfieldStyle);
							this.tempMail=GUILayout.TextField(this.tempMail, 30,userProfileVM.inputTextfieldStyle);
							if (GUILayout.Button ("Valider",userProfileVM.editProfileDataButtonStyle))
							{
								this.isEditing = false;
								//isDataLoaded=false;
								//StartCoroutine(updateProfile());
							}
							if (GUILayout.Button ("Annuler",userProfileVM.editProfileDataButtonStyle))
							{
								isEditing = false;
							}
						}
					}
					GUILayout.EndVertical();
					GUILayout.Space ((profileScreenVM.blockLeft.width-userProfileVM.profilePictureWidth)/2);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
			GUI.DrawTexture(new Rect(userProfileVM.profilePictureRect),userProfileVM.Profile.texture,ScaleMode.StretchToFill);
				
				//GUI.Box(new Rect(profilePictureStyle.fixedHeight+0.02f*widthScreen,
				                 //0.12f*heightScreen,
				                 //nbFriendsPerRow*friendLabelsAreaSizeX,
				                 //0.435f * heightScreen), "",borderBackgroundStyle);
				//GUI.Label (new Rect(profilePictureStyle.fixedHeight+0.02f*widthScreen,
				                    //0.12f*heightScreen,
				                    //nbFriendsPerRow*friendLabelsAreaSizeX,
				                    //heightScreen*0.03f), "Mes amis",titleStyle);
				//GUI.Label (new Rect(profilePictureStyle.fixedHeight+0.02f*widthScreen,
				                    //0.16f*heightScreen,
				                    //nbFriendsPerRow*friendLabelsAreaSizeX,
				                    //heightScreen*0.03f), labelNoFriends,labelNo);
				
			// BLOC HAUT CENTRE

			GUILayout.BeginArea(profileScreenVM.blockTopCenter,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label(myFriendsVM.title,profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockTopCenterHeight*0.1f));
				GUILayout.Space(profileScreenVM.blockTopCenterHeight*0.8f);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (myFriendsVM.pageDebut>0){
						if (GUILayout.Button("...",profileVM.paginationStyle,
					                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
					                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							myFriendsVM.pageDebut = myFriendsVM.pageDebut-10;
							myFriendsVM.pageFin = myFriendsVM.pageDebut+10;
						}
					}
					GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					for (int i = myFriendsVM.pageDebut ; i < myFriendsVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),myFriendsVM.paginatorGuiStyle[i],
					                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
					                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							myFriendsVM.paginatorGuiStyle[myFriendsVM.chosenPage]=profileVM.paginationStyle;
							myFriendsVM.chosenPage=i;
							myFriendsVM.paginatorGuiStyle[i]=profileVM.paginationActivatedStyle;
							myFriendsVM.displayPage();
						}
							GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					}
					if (myFriendsVM.nbPages>myFriendsVM.pageFin)
					{
						if (GUILayout.Button("...",profileVM.paginationStyle,
					                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
					                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
								myFriendsVM.pageDebut = myFriendsVM.pageDebut+10;
								myFriendsVM.pageFin= Mathf.Min(myFriendsVM.pageFin+10, myFriendsVM.nbPages);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();

			for (int i=myFriendsVM.start;i<myFriendsVM.finish;i++)
			{
				GUILayout.BeginArea(myFriendsVM.blocks[i-myFriendsVM.start]);
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
						GUILayout.BeginVertical();
						{
							GUILayout.Space (myFriendsVM.blocksHeight*0.05f);
							GUILayout.BeginHorizontal(profileVM.contactsBackgroundStyle,GUILayout.Height(myFriendsVM.blocksHeight*0.65f));
							{
								if(GUILayout.Button ("",profileVM.contactsPicturesButtonStyle[myFriendsVM.profilePictures[i]],
							                     GUILayout.Height (myFriendsVM.blocksHeight*0.65f),
							                     GUILayout.Width (myFriendsVM.blocksHeight*0.65f)))
								{
									ApplicationModel.profileChosen=myFriendsVM.contacts[i].Username;
									Application.LoadLevel("Profile");
								}
								GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
								GUILayout.BeginVertical();
								{
									GUILayout.Label (myFriendsVM.contacts[i].Username
									                 ,profileVM.contactsUsernameStyle);
									//GUILayout.Space (myFriendsVM.blocksHeight*0.02f);
									GUILayout.Label (myFriendsVM.contacts[i].TotalNbWins+" V "
								                 +myFriendsVM.contacts[i].TotalNbLooses+" D",
								                 profileVM.contactsInformationStyle);
									GUILayout.Label ("R : "+myFriendsVM.contacts[i].Ranking
									                 ,profileVM.contactsInformationStyle);
									GUILayout.Label ("Div : "+myFriendsVM.contacts[i].Division
									                 ,profileVM.contactsInformationStyle);
								}
								GUILayout.EndVertical();

							}
							GUILayout.EndHorizontal();
							GUILayout.Space (myFriendsVM.blocksHeight*0.05f);
							if(GUILayout.Button ("Retirer",profileVM.actionButtonStyle,GUILayout.Height(myFriendsVM.blocksHeight*0.2f)));
							{
//									isDataLoaded=false;
//									idConnection=userData.Connections[friendsToBeDisplayed[i]].Id;
//									StartCoroutine(removeConnection());
//									userData.Connections.RemoveAt(friendsToBeDisplayed[i]);
//									computeConnections();
							}
							GUILayout.Space (myFriendsVM.blocksHeight*0.05f);
						}
						GUILayout.EndVertical();
						GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();      
			}
			// BLOC MILIEU BAS GAUCHE
			if(profileVM.isMyProfile)
			{
				GUILayout.BeginArea(profileScreenVM.blockBottomCenterLeft,profileScreenVM.blockBorderStyle);
				{
					GUILayout.Label("Invitations envoyées",profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockBottomCenterLeftHeight*0.1f));
					GUILayout.Space(profileScreenVM.blockBottomCenterLeftHeight*0.8f);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (invitationsSentVM.pageDebut>0){
							if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								invitationsSentVM.pageDebut = invitationsSentVM.pageDebut-10;
								invitationsSentVM.pageFin = invitationsSentVM.pageDebut+10;
							}
						}
						GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						for (int i = invitationsSentVM.pageDebut ; i < invitationsSentVM.pageFin ; i++)
						{
							if (GUILayout.Button(""+(i+1),invitationsSentVM.paginatorGuiStyle[i],
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								invitationsSentVM.paginatorGuiStyle[invitationsSentVM.chosenPage]=profileVM.paginationStyle;
								invitationsSentVM.chosenPage=i;
								invitationsSentVM.paginatorGuiStyle[i]=profileVM.paginationActivatedStyle;
								invitationsSentVM.displayPage();
							}
								GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						}
						if (invitationsSentVM.nbPages>invitationsSentVM.pageFin)
						{
							if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
									invitationsSentVM.pageDebut = invitationsSentVM.pageDebut+10;
									invitationsSentVM.pageFin= Mathf.Min(invitationsSentVM.pageFin+10, invitationsSentVM.nbPages);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndArea();

				for (int i=invitationsSentVM.start;i<invitationsSentVM.finish;i++)
				{
					GUILayout.BeginArea(invitationsSentVM.blocks[i-invitationsSentVM.start]);
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space (invitationsSentVM.blocksWidth*0.05f);
							GUILayout.BeginVertical();
							{
								GUILayout.Space (invitationsSentVM.blocksHeight*0.05f);
								GUILayout.BeginHorizontal(profileVM.contactsBackgroundStyle,GUILayout.Height(invitationsSentVM.blocksHeight*0.65f));
								{
									if(GUILayout.Button ("",profileVM.contactsPicturesButtonStyle[invitationsSentVM.profilePictures[i]],
								                     GUILayout.Height (invitationsSentVM.blocksHeight*0.65f),
								                     GUILayout.Width (invitationsSentVM.blocksHeight*0.65f)))
									{
										ApplicationModel.profileChosen=invitationsSentVM.Contacts[i].Username;
										Application.LoadLevel("Profile");
									}
									GUILayout.Space (invitationsSentVM.blocksWidth*0.05f);
									GUILayout.BeginVertical();
									{
										GUILayout.Label (invitationsSentVM.Contacts[i].Username
										                 ,profileVM.contactsUsernameStyle);
										//GUILayout.Space (invitationsSentVM.blocksHeight*0.02f);
										GUILayout.Label (invitationsSentVM.Contacts[i].TotalNbWins+" V "
									                 +invitationsSentVM.Contacts[i].TotalNbLooses+" D",
									                 profileVM.contactsInformationStyle);
										GUILayout.Label ("R : "+invitationsSentVM.Contacts[i].Ranking
										                 ,profileVM.contactsInformationStyle);
										GUILayout.Label ("Div : "+invitationsSentVM.Contacts[i].Division
										                 ,profileVM.contactsInformationStyle);
									}
									GUILayout.EndVertical();

								}
								GUILayout.EndHorizontal();
								GUILayout.Space (invitationsSentVM.blocksHeight*0.05f);
								if(GUILayout.Button ("Retirer",profileVM.actionButtonStyle,GUILayout.Height(invitationsSentVM.blocksHeight*0.2f)));
								{
	//									isDataLoaded=false;
	//									idConnection=userData.Connections[friendsToBeDisplayed[i]].Id;
	//									StartCoroutine(removeConnection());
	//									userData.Connections.RemoveAt(friendsToBeDisplayed[i]);
	//									computeConnections();
								}
								GUILayout.Space (myFriendsVM.blocksHeight*0.05f);
							}
							GUILayout.EndVertical();
							GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();      
				}

				// BLOC MILIEU BAS DROIT

				GUILayout.BeginArea(profileScreenVM.blockBottomCenterRight,profileScreenVM.blockBorderStyle);
				{
					GUILayout.Label("Invitations reçues",profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockBottomCenterRightHeight*0.1f));
					GUILayout.Space(profileScreenVM.blockBottomCenterRightHeight*0.8f);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (invitationsReceivedVM.pageDebut>0){
							if (GUILayout.Button("...",profileVM.paginationStyle,
							                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
							                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								invitationsReceivedVM.pageDebut = invitationsReceivedVM.pageDebut-10;
								invitationsReceivedVM.pageFin = invitationsReceivedVM.pageDebut+10;
							}
						}
						GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						for (int i = invitationsReceivedVM.pageDebut ; i < invitationsReceivedVM.pageFin ; i++)
						{
							if (GUILayout.Button(""+(i+1),invitationsReceivedVM.paginatorGuiStyle[i],
							                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
							                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								invitationsReceivedVM.paginatorGuiStyle[invitationsReceivedVM.chosenPage]=profileVM.paginationStyle;
								invitationsReceivedVM.chosenPage=i;
								invitationsReceivedVM.paginatorGuiStyle[i]=profileVM.paginationActivatedStyle;
								invitationsReceivedVM.displayPage();
							}
							GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						}
						if (invitationsReceivedVM.nbPages>invitationsReceivedVM.pageFin)
						{
							if (GUILayout.Button("...",profileVM.paginationStyle,
							                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
							                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								invitationsReceivedVM.pageDebut = invitationsReceivedVM.pageDebut+10;
								invitationsReceivedVM.pageFin= Mathf.Min(invitationsReceivedVM.pageFin+10, invitationsReceivedVM.nbPages);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndArea();
				
				for (int i=invitationsReceivedVM.start;i<invitationsReceivedVM.finish;i++)
				{
					GUILayout.BeginArea(invitationsReceivedVM.blocks[i-invitationsReceivedVM.start]);
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space (invitationsReceivedVM.blocksWidth*0.05f);
							GUILayout.BeginVertical();
							{
								GUILayout.Space (invitationsReceivedVM.blocksHeight*0.05f);
								GUILayout.BeginHorizontal(profileVM.contactsBackgroundStyle,GUILayout.Height(invitationsReceivedVM.blocksHeight*0.65f));
								{
									if(GUILayout.Button ("",profileVM.contactsPicturesButtonStyle[invitationsReceivedVM.profilePictures[i]],
									                     GUILayout.Height (invitationsReceivedVM.blocksHeight*0.65f),
									                     GUILayout.Width (invitationsReceivedVM.blocksHeight*0.65f)))
									{
										ApplicationModel.profileChosen=invitationsReceivedVM.Contacts[i].Username;
										Application.LoadLevel("Profile");
									}
									GUILayout.Space (invitationsReceivedVM.blocksWidth*0.05f);
									GUILayout.BeginVertical();
									{
										GUILayout.Label (invitationsReceivedVM.Contacts[i].Username
										                 ,profileVM.contactsUsernameStyle);
										//GUILayout.Space (invitationsReceivedVM.blocksHeight*0.02f);
										GUILayout.Label (invitationsReceivedVM.Contacts[i].TotalNbWins+" V "
										                 +invitationsReceivedVM.Contacts[i].TotalNbLooses+" D",
										                 profileVM.contactsInformationStyle);
										GUILayout.Label ("R : "+invitationsReceivedVM.Contacts[i].Ranking
										                 ,profileVM.contactsInformationStyle);
										GUILayout.Label ("Div : "+invitationsReceivedVM.Contacts[i].Division
										                 ,profileVM.contactsInformationStyle);
									}
									GUILayout.EndVertical();
									
								}
								GUILayout.EndHorizontal();
								GUILayout.Space (invitationsReceivedVM.blocksHeight*0.05f);
								GUILayout.BeginHorizontal();
								{

									if(GUILayout.Button ("Ajouter",profileVM.actionButtonStyle,GUILayout.Height(invitationsReceivedVM.blocksHeight*0.2f)));
									{
										//									isDataLoaded=false;
										//									idConnection=userData.Connections[friendsToBeDisplayed[i]].Id;
										//									StartCoroutine(removeConnection());
										//									userData.Connections.RemoveAt(friendsToBeDisplayed[i]);
										//									computeConnections();
									}
									GUILayout.Space (invitationsReceivedVM.blocksWidth*0.05f);
									if(GUILayout.Button ("Retirer",profileVM.actionButtonStyle,GUILayout.Height(invitationsReceivedVM.blocksHeight*0.2f)));
									{
										//									isDataLoaded=false;
										//									idConnection=userData.Connections[friendsToBeDisplayed[i]].Id;
										//									StartCoroutine(removeConnection());
										//									userData.Connections.RemoveAt(friendsToBeDisplayed[i]);
										//									computeConnections();
									}
								}
								GUILayout.EndHorizontal();
								GUILayout.Space (myFriendsVM.blocksHeight*0.05f);
							}
							GUILayout.EndVertical();
							GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea(); 
				}
			}
			else
			{
				// BLOC SUPERIEUR DROIT
				GUILayout.BeginArea(profileScreenVM.blockTopRight,profileScreenVM.blockBorderStyle);
				{
					GUILayout.FlexibleSpace();
					switch (friendshipStatusVM.status)
					{
					case 0:
						GUILayout.Label(friendshipStatusVM.username+" souhaite faire partie de vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						GUILayout.BeginHorizontal();
						{
							if (GUILayout.Button("Accepter",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
							{
								//isDataLoaded=false;
								//idConnection=userData.Connections[friendshipConnection].Id;
								//StartCoroutine(confirmConnection());
								//userData.Connections[friendshipConnection].State =+1;
								//computeConnections();
							}
							if (GUILayout.Button("Ignorer",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
							{
								//isDataLoaded=false;
								//idConnection=userData.Connections[friendshipConnection].Id;
								//StartCoroutine(removeConnection());
								//userData.Connections.RemoveAt(friendshipConnection);
								//computeConnections();
							}
						}
						GUILayout.EndHorizontal();
						break;
					case 1:
						GUILayout.Label(friendshipStatusVM.username+" fait partie de vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						if (GUILayout.Button("Retirer",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
						{
							ProfileController.instance.removeConnection(friendshipStatusVM.indexConnection);
						}
						break;
					case 2:
						GUILayout.Label("vous souhaitez ajouter "+friendshipStatusVM.username+" à vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						if (GUILayout.Button("Retirer",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
						{
							ProfileController.instance.removeConnection(friendshipStatusVM.indexConnection);
						}
						break;
					case 3:
						GUILayout.Label(friendshipStatusVM.username+" ne fait pas partie de vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						if (GUILayout.Button("Ajouter",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
						{
							ProfileController.instance.addConnection();
						}
						break;
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndArea();
			}

			// BLOC MILIEU DROIT
			GUILayout.BeginArea(profileScreenVM.blockMiddleRight,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label("Statistiques",profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockMiddleRightHeight*0.1f));
				GUILayout.FlexibleSpace();
				GUILayout.Label("Victoires : " + statsVM.totalNbWins,statsVM.informationsStyle,GUILayout.Height(profileScreenVM.blockMiddleRightHeight*0.1f));
				GUILayout.Label("Défaites : " + statsVM.totalNbLooses,statsVM.informationsStyle,GUILayout.Height(profileScreenVM.blockMiddleRightHeight*0.1f));
				GUILayout.Label("Ranking : " + statsVM.ranking,statsVM.informationsStyle,GUILayout.Height(profileScreenVM.blockMiddleRightHeight*0.1f));
				GUILayout.Label("Ranking points : " + statsVM.rankingPoints,statsVM.informationsStyle,GUILayout.Height(profileScreenVM.blockMiddleRightHeight*0.1f));
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();

			// BLOC INFERIEUR DROIT
			GUILayout.BeginArea(profileScreenVM.blockBottomRight,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label("Trophées",profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockBottomRightHeight*0.1f));
				GUILayout.Space(profileScreenVM.blockBottomRightHeight*0.8f);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (myTrophiesVM.pageDebut>0){
						if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							myTrophiesVM.pageDebut = myTrophiesVM.pageDebut-10;
							myTrophiesVM.pageFin = myTrophiesVM.pageDebut+10;
						}
					}
					GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					for (int i = myTrophiesVM.pageDebut ; i < myTrophiesVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),myTrophiesVM.paginatorGuiStyle[i],
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							myTrophiesVM.paginatorGuiStyle[myTrophiesVM.chosenPage]=profileVM.paginationStyle;
							myTrophiesVM.chosenPage=i;
							myTrophiesVM.paginatorGuiStyle[i]=profileVM.paginationActivatedStyle;
							myTrophiesVM.displayPage();
						}
						GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					}
					if (myTrophiesVM.nbPages>myTrophiesVM.pageFin)
					{
						if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							myTrophiesVM.pageDebut = myTrophiesVM.pageDebut+10;
							myTrophiesVM.pageFin= Mathf.Min(myTrophiesVM.pageFin+10, myTrophiesVM.nbPages);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			for (int i=myTrophiesVM.start;i<myTrophiesVM.finish;i++)
			{
				GUILayout.BeginArea(myTrophiesVM.blocks[i-myTrophiesVM.start]);
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
						GUILayout.BeginVertical(profileVM.contactsBackgroundStyle,GUILayout.Height (myTrophiesVM.blocksHeight*0.80f));
						{
							GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
								if(GUILayout.Button ("",myTrophiesVM.trophiesPicturesButtonStyle[i],
								                     GUILayout.Height (myTrophiesVM.blocksHeight*0.65f),
								                     GUILayout.Width (myTrophiesVM.blocksHeight*0.65f)))
								{
								}
								GUILayout.Space (myTrophiesVM.blocksWidth*0.05f);
								GUILayout.BeginVertical();
								{
									GUILayout.Label (myTrophiesVM.trophies[i].CompetitionName
									                 ,myTrophiesVM.trophyNameStyle);
									GUILayout.Label (myTrophiesVM.trophies[i].Trophy.Date.ToString("dd/MM/yyyy"),
									                 myTrophiesVM.trophyDateStyle);
								}
								GUILayout.EndVertical();
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
						GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea(); 
			}
		}
		if (checkPassword) {
			GUI.enabled=true;
			GUILayout.BeginArea(profileScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(profileScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Saisissez votre mot de passe",profileScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space(profileScreenVM.widthScreen*0.05f);
						this.tempOldPassword = GUILayout.PasswordField(this.tempOldPassword,'*',profileScreenVM.centralWindowTextfieldStyle);
						GUILayout.Space(profileScreenVM.widthScreen*0.05f);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					if(userProfileVM.error!="")
					{
						GUILayout.Label (userProfileVM.error,profileScreenVM.centralWindowTitleStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",profileScreenVM.centralWindowButtonStyle,GUILayout.Width (profileScreenVM.centralWindow.width*0.3f))){
							//error="";
							//StartCoroutine(checkUserPassword());
						}
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Quitter",profileScreenVM.centralWindowButtonStyle,GUILayout.Width (profileScreenVM.centralWindow.width*0.3f))){
							//error="";
							//oldPassword="";
							checkPassword=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
		GUILayout.EndArea();
		}
	}

}

