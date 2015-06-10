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

	public FileBrowser m_fileBrowser;
	private string m_textPath;
	
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
	private void FileSelectedCallback(string path) {
		this.m_fileBrowser = null;
		this.m_textPath = path;
		if (this.m_textPath!=null)
		{
			ProfileController.instance.updateProfilePictureHandler(this.m_textPath);
		}
	}
	void Update()
	{
		if (this.canDisplay)
		{
			if (Screen.width != profileScreenVM.widthScreen || Screen.height != profileScreenVM.heightScreen) {
				this.m_fileBrowser=null;
				ProfileController.instance.resize();
			}
			if(Input.GetKeyDown(KeyCode.Return)) 
			{
				ProfileController.instance.returnPressed();
			}
			if(Input.GetKeyDown(KeyCode.Escape)) 
			{
				ProfileController.instance.escapePressed();
			}
		}
	}
	void OnGUI()
	{
		GUI.enabled = profileVM.guiEnabled;
		if (canDisplay) 
		{
			GUILayout.BeginArea(profileScreenVM.blockLeft,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label (userProfileVM.title,profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockLeftHeight*0.05f));
				GUILayout.Space (userProfileVM.profilePictureHeight+
				                 userProfileVM.updateProfilePictureButtonHeight); //editProfilePictureButtonStyle.fixedHeight
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space ((profileScreenVM.blockLeft.width-userProfileVM.profilePictureWidth)/2);
					GUILayout.BeginVertical();
					{
						GUILayout.Label ("Pseudo : " + userProfileVM.Profile.Username,userProfileVM.profileDataStyle);
						if(profileVM.isMyProfile)
						{
							GUILayout.Label ("Argent : " + userProfileVM.Profile.Money + " credits",userProfileVM.profileDataStyle);
						}
						if(!profileVM.isEditing){
							GUILayout.Label ("Prenom : " + userProfileVM.Profile.FirstName,userProfileVM.profileDataStyle);
							GUILayout.Label ("Nom : " + userProfileVM.Profile.Surname,userProfileVM.profileDataStyle);
							GUILayout.Label (userProfileVM.Profile.Mail,userProfileVM.profileDataStyle);
							if(profileVM.isMyProfile)
							{
								GUI.enabled=profileVM.buttonsEnabled;
								if (GUILayout.Button ("Modifier mes infos",userProfileVM.editProfileDataButtonStyle))
								{
									ProfileController.instance.editProfileInformations(true);
								}
								if (GUILayout.Button ("Changer le mot de passe",userProfileVM.editProfileDataButtonStyle))
								{
									ProfileController.instance.displayCheckPasswordPopUp();
								}
								GUI.enabled = profileVM.guiEnabled;
							}
						}
						if (profileVM.isEditing)
						{
							profileVM.tempFirstName = GUILayout.TextField(profileVM.tempFirstName, 15,userProfileVM.inputTextfieldStyle);
							profileVM.tempSurname = GUILayout.TextField(profileVM.tempSurname, 15,userProfileVM.inputTextfieldStyle);
							profileVM.tempMail=GUILayout.TextField(profileVM.tempMail, 30,userProfileVM.inputTextfieldStyle);
							if (GUILayout.Button ("Valider",userProfileVM.editProfileDataButtonStyle))
							{
								ProfileController.instance.updateUserInformationsHandler();
							}
							if (GUILayout.Button ("Annuler",userProfileVM.editProfileDataButtonStyle))
							{
								ProfileController.instance.editProfileInformations(false);
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
				
			// BLOC HAUT CENTRE

			GUILayout.BeginArea(profileScreenVM.blockTopCenter,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label(myFriendsVM.title,profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockTopCenterHeight*0.1f));
				GUILayout.Label(myFriendsVM.labelNo,profileVM.labelNoStyle,GUILayout.Height(profileScreenVM.blockTopCenterHeight*0.1f));
				GUILayout.Space(profileScreenVM.blockTopCenterHeight*0.7f);
				GUILayout.FlexibleSpace();
				GUI.enabled=profileVM.buttonsEnabled;
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (myFriendsVM.pageDebut>0){
						if (GUILayout.Button("...",profileVM.paginationStyle,
					                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
					                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							ProfileController.instance.pagination(0,0);
						}
					}
					GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					for (int i = myFriendsVM.pageDebut ; i < myFriendsVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),myFriendsVM.paginatorGuiStyle[i],
					                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
					                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							ProfileController.instance.pagination(0,1,i);
						}
							GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					}
					if (myFriendsVM.nbPages>myFriendsVM.pageFin)
					{
						if (GUILayout.Button("...",profileVM.paginationStyle,
					                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
					                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							ProfileController.instance.pagination(0,2);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUI.enabled = profileVM.guiEnabled;
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
								GUI.enabled=profileVM.buttonsEnabled;
								if(GUILayout.Button ("",profileVM.contactsPicturesButtonStyle[myFriendsVM.contactsDisplayed[i]],
							                     GUILayout.Height (myFriendsVM.blocksHeight*0.65f),
							                     GUILayout.Width (myFriendsVM.blocksHeight*0.65f)))
								{
									ApplicationModel.profileChosen=myFriendsVM.contacts[i].Username;
									Application.LoadLevel("Profile");
								}
								GUI.enabled = profileVM.guiEnabled;
								GUILayout.Space (myFriendsVM.blocksWidth*0.05f);
								GUILayout.BeginVertical();
								{
									GUILayout.Label (myFriendsVM.contacts[i].Username
									                 ,profileVM.contactsUsernameStyle);
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
							if(profileVM.isMyProfile)
							{
								GUILayout.Space (myFriendsVM.blocksHeight*0.05f);
								GUI.enabled=profileVM.buttonsEnabled;
								if(GUILayout.Button ("Retirer",profileVM.actionButtonStyle,GUILayout.Height(myFriendsVM.blocksHeight*0.2f)))
								{
									ProfileController.instance.removeConnection(myFriendsVM.contactsDisplayed[i]);
								}
								GUI.enabled = profileVM.guiEnabled;
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
					GUILayout.Label(invitationsSentVM.labelNo,profileVM.labelNoStyle,GUILayout.Height(profileScreenVM.blockBottomCenterLeftHeight*0.1f));
					GUILayout.Space(profileScreenVM.blockBottomCenterLeftHeight*0.7f);
					GUILayout.FlexibleSpace();
					GUI.enabled=profileVM.buttonsEnabled;
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (invitationsSentVM.pageDebut>0){
							if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								ProfileController.instance.pagination(1,0);
							}
						}
						GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						for (int i = invitationsSentVM.pageDebut ; i < invitationsSentVM.pageFin ; i++)
						{
							if (GUILayout.Button(""+(i+1),invitationsSentVM.paginatorGuiStyle[i],
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								ProfileController.instance.pagination(1,1,i);
							}
								GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						}
						if (invitationsSentVM.nbPages>invitationsSentVM.pageFin)
						{
							if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								ProfileController.instance.pagination(1,2);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUI.enabled = profileVM.guiEnabled;
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
									GUI.enabled=profileVM.buttonsEnabled;
									if(GUILayout.Button ("",profileVM.contactsPicturesButtonStyle[invitationsSentVM.contactsDisplayed[i]],
								                     GUILayout.Height (invitationsSentVM.blocksHeight*0.65f),
								                     GUILayout.Width (invitationsSentVM.blocksHeight*0.65f)))
									{
										ApplicationModel.profileChosen=invitationsSentVM.contacts[i].Username;
										Application.LoadLevel("Profile");
									}
									GUI.enabled = profileVM.guiEnabled;
									GUILayout.Space (invitationsSentVM.blocksWidth*0.05f);
									GUILayout.BeginVertical();
									{
										GUILayout.Label (invitationsSentVM.contacts[i].Username
										                 ,profileVM.contactsUsernameStyle);
										GUILayout.Label (invitationsSentVM.contacts[i].TotalNbWins+" V "
									                 +invitationsSentVM.contacts[i].TotalNbLooses+" D",
									                 profileVM.contactsInformationStyle);
										GUILayout.Label ("R : "+invitationsSentVM.contacts[i].Ranking
										                 ,profileVM.contactsInformationStyle);
										GUILayout.Label ("Div : "+invitationsSentVM.contacts[i].Division
										                 ,profileVM.contactsInformationStyle);
									}
									GUILayout.EndVertical();

								}
								GUILayout.EndHorizontal();
								GUILayout.Space (invitationsSentVM.blocksHeight*0.05f);
								GUI.enabled=profileVM.buttonsEnabled;
								if(GUILayout.Button ("Retirer",profileVM.actionButtonStyle,GUILayout.Height(invitationsSentVM.blocksHeight*0.2f)))
								{
									ProfileController.instance.removeConnection(invitationsSentVM.contactsDisplayed[i]);
								}
								GUI.enabled = profileVM.guiEnabled;
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
					GUILayout.Label(invitationsReceivedVM.labelNo,profileVM.labelNoStyle,GUILayout.Height(profileScreenVM.blockBottomCenterRightHeight*0.1f));
					GUILayout.Space(profileScreenVM.blockBottomCenterRightHeight*0.7f);
					GUILayout.FlexibleSpace();
					GUI.enabled=profileVM.buttonsEnabled;
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (invitationsReceivedVM.pageDebut>0)
						{
							if (GUILayout.Button("...",profileVM.paginationStyle,
							                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
							                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								ProfileController.instance.pagination(2,0);
							}
						}
						GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						for (int i = invitationsReceivedVM.pageDebut ; i < invitationsReceivedVM.pageFin ; i++)
						{
							if (GUILayout.Button(""+(i+1),invitationsReceivedVM.paginatorGuiStyle[i],
							                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
							                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								ProfileController.instance.pagination(2,1,i);
							}
							GUILayout.Space(profileScreenVM.widthScreen*0.01f);
						}
						if (invitationsReceivedVM.nbPages>invitationsReceivedVM.pageFin)
						{
							if (GUILayout.Button("...",profileVM.paginationStyle,
							                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
							                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
							{
								ProfileController.instance.pagination(2,2);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUI.enabled = profileVM.guiEnabled;
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
									GUI.enabled=profileVM.buttonsEnabled;
									if(GUILayout.Button ("",profileVM.contactsPicturesButtonStyle[invitationsReceivedVM.contactsDisplayed[i]],
									                     GUILayout.Height (invitationsReceivedVM.blocksHeight*0.65f),
									                     GUILayout.Width (invitationsReceivedVM.blocksHeight*0.65f)))
									{
										ApplicationModel.profileChosen=invitationsReceivedVM.contacts[i].Username;
										Application.LoadLevel("Profile");
									}
									GUI.enabled = profileVM.guiEnabled;
									GUILayout.Space (invitationsReceivedVM.blocksWidth*0.05f);
									GUILayout.BeginVertical();
									{
										GUILayout.Label (invitationsReceivedVM.contacts[i].Username
										                 ,profileVM.contactsUsernameStyle);
										GUILayout.Label (invitationsReceivedVM.contacts[i].TotalNbWins+" V "
										                 +invitationsReceivedVM.contacts[i].TotalNbLooses+" D",
										                 profileVM.contactsInformationStyle);
										GUILayout.Label ("R : "+invitationsReceivedVM.contacts[i].Ranking
										                 ,profileVM.contactsInformationStyle);
										GUILayout.Label ("Div : "+invitationsReceivedVM.contacts[i].Division
										                 ,profileVM.contactsInformationStyle);
									}
									GUILayout.EndVertical();
									
								}
								GUILayout.EndHorizontal();
								GUILayout.Space (invitationsReceivedVM.blocksHeight*0.05f);
								GUILayout.BeginHorizontal();
								{

									GUI.enabled=profileVM.buttonsEnabled;
									if(GUILayout.Button ("Ajouter",profileVM.actionButtonStyle,GUILayout.Height(invitationsReceivedVM.blocksHeight*0.2f)))
									{
										StartCoroutine(ProfileController.instance.confirmConnection(invitationsReceivedVM.contactsDisplayed[i]));
									}
									GUILayout.Space (invitationsReceivedVM.blocksWidth*0.05f);
									if(GUILayout.Button ("Retirer",profileVM.actionButtonStyle,GUILayout.Height(invitationsReceivedVM.blocksHeight*0.2f)))
									{
										ProfileController.instance.removeConnection(invitationsReceivedVM.contactsDisplayed[i]);
									}
									GUI.enabled = profileVM.guiEnabled;
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
						GUI.enabled=profileVM.buttonsEnabled;
						GUILayout.BeginHorizontal();
						{
							if (GUILayout.Button("Accepter",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
							{
								StartCoroutine(ProfileController.instance.confirmConnection(friendshipStatusVM.indexConnection));
							}
							if (GUILayout.Button("Ignorer",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
							{
								ProfileController.instance.removeConnection(friendshipStatusVM.indexConnection);
							}
						}
						GUILayout.EndHorizontal();
						GUI.enabled = profileVM.guiEnabled;
						break;
					case 1:
						GUILayout.Label(friendshipStatusVM.username+" fait partie de vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						GUI.enabled=profileVM.buttonsEnabled;
						if (GUILayout.Button("Retirer",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
						{
							ProfileController.instance.removeConnection(friendshipStatusVM.indexConnection);
						}
						GUI.enabled = profileVM.guiEnabled;
						break;
					case 2:
						GUILayout.Label("vous avez invité "+friendshipStatusVM.username+" à faire partie de vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						GUI.enabled=profileVM.buttonsEnabled;
						if (GUILayout.Button("Retirer",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
						{
							ProfileController.instance.removeConnection(friendshipStatusVM.indexConnection);
						}
						GUI.enabled = profileVM.guiEnabled;
						break;
					case 3:
						GUILayout.Label(friendshipStatusVM.username+" ne fait pas partie de vos amis",
						                friendshipStatusVM.situationStyle,GUILayout.Height(0.5f*profileScreenVM.blockTopRightHeight));
						GUI.enabled=profileVM.buttonsEnabled;
						if (GUILayout.Button("Ajouter",friendshipStatusVM.buttonStyle,GUILayout.Height(0.4f*profileScreenVM.blockTopRightHeight)))
						{
							StartCoroutine(ProfileController.instance.addConnection());
						}
						GUI.enabled = profileVM.guiEnabled;
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
				GUILayout.Label("Victoires : " + statsVM.totalNbWins,statsVM.informationsStyle);
				GUILayout.Label("Défaites : " + statsVM.totalNbLooses,statsVM.informationsStyle);
				if(statsVM.ranking!="")
				{
					GUILayout.Label(statsVM.ranking,statsVM.informationsStyle);
					GUILayout.Label(statsVM.rankingPoints,statsVM.subInformationsStyle);
				}
				if(statsVM.collectionRanking!="")
				{
					GUILayout.Label(statsVM.collectionRanking,statsVM.informationsStyle);
					GUILayout.Label(statsVM.collectionPoints,statsVM.subInformationsStyle);
				}
				GUILayout.FlexibleSpace();
				if(profileVM.isMyProfile)
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUI.enabled=profileVM.buttonsEnabled;
						if(GUILayout.Button("Ma collection",statsVM.buttonStyle,GUILayout.Height(profileScreenVM.blockMiddleRightHeight*0.1f),GUILayout.Width(profileScreenVM.blockMiddleRightWidth*0.8f)))
						{
							Application.LoadLevel("SkillBook");
						}
						GUI.enabled = profileVM.guiEnabled;
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();

			// BLOC INFERIEUR DROIT
			GUILayout.BeginArea(profileScreenVM.blockBottomRight,profileScreenVM.blockBorderStyle);
			{
				GUILayout.Label("Trophées",profileVM.titleStyle,GUILayout.Height(profileScreenVM.blockBottomRightHeight*0.1f));
				GUILayout.Label(myTrophiesVM.labelNo,profileVM.labelNoStyle,GUILayout.Height(profileScreenVM.blockBottomRightHeight*0.1f));
				GUILayout.Space(profileScreenVM.blockBottomRightHeight*0.7f);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUI.enabled=profileVM.buttonsEnabled;
					GUILayout.FlexibleSpace();
					if (myTrophiesVM.pageDebut>0){
						if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							ProfileController.instance.pagination (3,0);
						}
					}
					GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					for (int i = myTrophiesVM.pageDebut ; i < myTrophiesVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),myTrophiesVM.paginatorGuiStyle[i],
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							ProfileController.instance.pagination (3,1,i);
						}
						GUILayout.Space(profileScreenVM.widthScreen*0.01f);
					}
					if (myTrophiesVM.nbPages>myTrophiesVM.pageFin)
					{
						if (GUILayout.Button("...",profileVM.paginationStyle,
						                     GUILayout.Height(profileScreenVM.heightScreen*3/100),
						                     GUILayout.Width (profileScreenVM.widthScreen*2/100)))
						{
							ProfileController.instance.pagination (3,2);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUI.enabled = profileVM.guiEnabled;
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
			if(profileVM.isMyProfile)
			{
				GUI.skin = profileScreenVM.fileBrowserSkin;
				if (m_fileBrowser != null)
				{
					this.m_fileBrowser.OnGUI();
				} 
				else 
				{
					GUI.enabled=profileVM.buttonsEnabled;
					if (GUI.Button (userProfileVM.updateProfilePictureButtonRect,"Modifier ma photo",userProfileVM.editProfileDataButtonStyle))
					{
						this.m_fileBrowser = new FileBrowser(profileScreenVM.fileBrowserWindow,
						                                     "Sélectionnez une image",
						                                     this.FileSelectedCallback);
						this.m_fileBrowser.DirectoryImage = profileScreenVM.m_directoryImage;
						this.m_fileBrowser.FileImage = profileScreenVM.m_fileImage;
					}
					GUI.enabled = profileVM.guiEnabled;
				}
			}
		}
	}	
}

