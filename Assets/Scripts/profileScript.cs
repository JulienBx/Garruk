using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class profileScript : MonoBehaviour {

	int sizeMax = 3145728;
	List<string> availableExtension = new List<string>(){".jpg", ".png"};

	bool isDataLoaded = false;
	bool myProfile = false;
	bool isEditing = false;
	bool displayPopUp = false;
	bool changePassword = false;
	bool checkPassword = false;
	bool profileInitialized =false;
	bool displayNewPicture = false;
	bool isPictureLoaded = false;

	string firstName;
	string surname;
	string mail;
	string error;
	string oldPassword ="";
	string newPassword1="";
	string newPassword2="";

	string profileChosen=ApplicationModel.profileChosen;

	int idConnection;
	int friendshipConnection;
	User userData;
	int friendshipState = -1;

	public GUISkin fileBrowserSkin ;
	
	Texture2D profilePicture;

	public Texture2D white;
	public Texture2D backButton ;
	public Texture2D backActivatedButton ;

	public GameObject MenuObject;

	public GUIStyle profilePictureStyle;
	public GUIStyle editProfilePictureButtonStyle;
	public GUIStyle profileData;
	public GUIStyle editProfileDataButtonStyle;
	public GUIStyle friendshipStateButtonStyle;
	public GUIStyle friendButtonStyle;
	public GUIStyle actionButtonStyle;
	public GUIStyle inputTextfieldStyle;
	public GUIStyle labelNo;
	public GUIStyle titleStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle paginationStyle;
	public GUIStyle centralWindowStyle ;
	public GUIStyle centralWindowTitleStyle ;
	public GUIStyle centralWindowButtonStyle ;
	public GUIStyle centralWindowTextfieldStyle ;

	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;

	int friendsStart;
	int friendsFinish;

	int invitationsSentStart;
	int invitationsSentFinish;

	int invitationsReceivedStart;
	int invitationsReceivedFinish;

	int nbFriendsPerRow;
	int nbInvitationsSentPerRow;
	int nbInvitationsReceivedPerRow;

	int nbFriendsPages;
	int nbInvitationsSentPages;
	int nbInvitationsReceivedPages;
	
	int chosenPageFriends=0;
	int pageDebutFriends;
	int pageFinFriends;
	int chosenPageInvitationsSent=0;
	int pageDebutInvitationsSent;
	int pageFinInvitationsSent;
	int chosenPageInvitationsReceived=0;
	int pageDebutInvitationsReceived;
	int pageFinInvitationsReceived;

	int nbFriendsToDisplay;
	int nbInvitationsSentToDisplay;
	int nbInvitationsReceivedToDisplay;

	int friendLabelsAreaSizeX;
	int friendLabelsAreaSizeY;

	GUIStyle[] paginatorFriendsGuiStyle;
	GUIStyle[] paginatorInvitationsReceivedGuiStyle;
	GUIStyle[] paginatorInvitationsSentGuiStyle;

	private string URLGetUserProfile = "http://54.77.118.214/GarrukServer/get_user_profile.php";
	private string URLGetMyProfile = "http://54.77.118.214/GarrukServer/get_myprofile.php";
	private string URLConfirmConnection = "http://54.77.118.214/GarrukServer/confirm_connection.php";
	private string URLRemoveConnection = "http://54.77.118.214/GarrukServer/remove_connection.php";
	private string URLCreateConnection = "http://54.77.118.214/GarrukServer/create_connection.php";
	private string URLUpdateProfile = "http://54.77.118.214/GarrukServer/update_profile.php";
	private string URLUpdateProfilePicture = "http://54.77.118.214/GarrukServer/update_profilepicture.php";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";
	private string URLCheckPassword = "http://54.77.118.214/GarrukServer/check_password.php";
	private string URLEditPassword = "http://54.77.118.214/GarrukServer/edit_password.php";
	private string ServerDirectory = "img/profile/";

	private IList<int> friendsToBeDisplayed ;
	private IList<int> invitationsSentToBeDisplayed ;
	private IList<int> invitationsReceivedToBeDisplayed ;

	string labelNoFriends;
	string labelNoInvitationsReceived;
	string labelNoInvitationsSent;

	string m_textPath;
	
	FileBrowser m_fileBrowser;

	Rect centralWindow;
	Rect fileBrowserWindow;
	Rect errorWindow ;
	Rect changePasswordWindow ;

	public Texture2D m_directoryImage;
	public Texture2D m_fileImage;
	
	// Use this for initialization
	void Start () {
		MenuObject = Instantiate(MenuObject) as GameObject;
		displayNewPicture = true;
		setStyles();
		if (profileChosen != "" && profileChosen != ApplicationModel.username){
			ApplicationModel.profileChosen="";
			StartCoroutine(getUserProfile());
		}
		else {
			myProfile=true;
			StartCoroutine(getMyProfile());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			m_fileBrowser=null;
			this.setStyles();
			chosenPageFriends=0;
			chosenPageInvitationsReceived=0;
			chosenPageInvitationsSent=0;
			this.setDisplayParameters();
		}

		if (profileInitialized && displayNewPicture){
			displayNewPicture=false;
			StartCoroutine(setProfilePicture());
		}

		if(displayPopUp){
			if(Input.GetKeyDown(KeyCode.Return)) {
				displayPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Escape)) {
				displayPopUp=false;
			}
		}
		if(m_fileBrowser!=null){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				m_fileBrowser=null;
			}
		}
		if(checkPassword){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				checkPassword=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				error="";
				StartCoroutine(checkUserPassword());
			}
		}
		if(changePassword){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				changePassword=false;
			}
			else if (Input.GetKeyDown(KeyCode.Return)){
				if(newPassword1==newPassword2 && newPassword1!="" && newPassword2!=""){
					error="";
					StartCoroutine(editPassword());
				}
				else if(newPassword1!=newPassword2 && newPassword1!="" && newPassword2!=""){
					error="les saisies ne correspondent pas";
				}
			}
		}
		if(isEditing){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				isEditing=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				isEditing = false;
				isDataLoaded=false;
				StartCoroutine(updateProfile());
			}
		}
	}

	void OnGUI () {

		if(checkPassword || changePassword || displayPopUp || m_fileBrowser!=null){
			GUI.enabled=false;
		}

		if (isDataLoaded && isPictureLoaded) {
			GUILayout.BeginArea(new Rect(0.86f*widthScreen,0.13f*heightScreen,widthScreen*0.12f,heightScreen*15f));
			{
				switch (friendshipState)
				{
				case 0:
					if (GUILayout.Button("Confirmer la demande",friendshipStateButtonStyle)){
						isDataLoaded=false;
						idConnection=userData.Connections[friendshipConnection].Id;
						StartCoroutine(confirmConnection());
						userData.Connections[friendshipConnection].State =+1;
						computeConnections();
					}
					if (GUILayout.Button("Rejeter la demande",friendshipStateButtonStyle)){
						isDataLoaded=false;
						idConnection=userData.Connections[friendshipConnection].Id;
						StartCoroutine(removeConnection());
						userData.Connections.RemoveAt(friendshipConnection);
						computeConnections();
					}
					break;
				case 1:
					if (GUILayout.Button("Retirer de vos amis",friendshipStateButtonStyle)){
						isDataLoaded=false;
						idConnection=userData.Connections[friendshipConnection].Id;
						StartCoroutine(removeConnection());
						userData.Connections.RemoveAt(friendshipConnection);
						computeConnections();
					}
					break;
				case 2:
					if (GUILayout.Button("Retirer votre demande",friendshipStateButtonStyle)){
						isDataLoaded=false;
						idConnection=userData.Connections[friendshipConnection].Id;
						StartCoroutine(removeConnection());
						userData.Connections.RemoveAt(friendshipConnection);
						computeConnections();
					}
					break;
				case 4:
					if (GUILayout.Button("Envoyer une demande",friendshipStateButtonStyle)){
						StartCoroutine(createConnection());
					}
					break;
				}
			}
			GUILayout.EndArea();
			if (myProfile){
				GUI.DrawTexture(new Rect(0.01f*widthScreen,0.12f*heightScreen,profilePictureStyle.fixedHeight,profilePictureStyle.fixedHeight),profilePicture,ScaleMode.StretchToFill);
				GUILayout.BeginArea(new Rect(0.01f*widthScreen,0.10f*heightScreen+profilePictureStyle.fixedHeight,profilePictureStyle.fixedWidth,0.88f*heightScreen));
				{
					GUILayout.Space (editProfilePictureButtonStyle.fixedHeight);
					GUILayout.Label ("Pseudo : " + userData.Username,profileData);
					GUILayout.Label ("Argent : " + userData.Money + " credits",profileData);
					if(!isEditing){
						GUILayout.Label ("Prenom : " + userData.FirstName,profileData);
						GUILayout.Label ("Nom : " + userData.Surname,profileData);
						GUILayout.Label (userData.Mail,profileData);
						if (GUILayout.Button ("Modifier mes infos",editProfileDataButtonStyle))
						{
							isEditing = true;
						}
						if (GUILayout.Button ("Changer le mot de passe",editProfileDataButtonStyle))
						{
							checkPassword = true;
						}
					}
					if (isEditing){
						firstName = GUILayout.TextField(firstName, 15,inputTextfieldStyle);
						surname = GUILayout.TextField(surname, 15,inputTextfieldStyle);
						mail=GUILayout.TextField(mail, 30,inputTextfieldStyle);
						if (GUILayout.Button ("Valider",editProfileDataButtonStyle))
						{
							isEditing = false;
							isDataLoaded=false;
							StartCoroutine(updateProfile());
						}
						if (GUILayout.Button ("Annuler",editProfileDataButtonStyle))
						{
							isEditing = false;
						}
					}
				}
				GUILayout.EndArea();

				GUI.Label (new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen,0.12f*heightScreen,widthScreen*0.5f,heightScreen*0.03f), "Mes amis",titleStyle);
				GUI.Label (new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen,0.16f*heightScreen,widthScreen*0.5f,heightScreen*0.03f), labelNoFriends,labelNo);

				for (int i = friendsStart;i<friendsFinish;i++){
					GUILayout.BeginArea(new Rect(profilePictureStyle.fixedHeight+0.02f*widthScreen + ((i-friendsStart)%nbFriendsPerRow)*friendLabelsAreaSizeX,
					                    0.15f*heightScreen + (Mathf.FloorToInt((i-friendsStart)/nbFriendsPerRow)*friendLabelsAreaSizeY),
					                    friendLabelsAreaSizeX,
					                    friendLabelsAreaSizeY));
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							GUILayout.BeginVertical();
							{
								if(GUILayout.Button (userData.Connections[friendsToBeDisplayed[i]].User,friendButtonStyle))
								{
									ApplicationModel.profileChosen=userData.Connections[friendsToBeDisplayed[i]].User;
									Application.LoadLevel("Profile");
								}
								if(GUILayout.Button ("Retirer",actionButtonStyle))
								{
									isDataLoaded=false;
									idConnection=userData.Connections[friendsToBeDisplayed[i]].Id;
									StartCoroutine(removeConnection());
									userData.Connections.RemoveAt(friendsToBeDisplayed[i]);
									computeConnections();
								}
							}
							GUILayout.EndVertical();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();
				}
				GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen,0.51f*heightScreen,0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen,0.03f*heightScreen));
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (pageDebutFriends>0){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutFriends = pageDebutFriends-10;
								pageFinFriends = pageDebutFriends+10;
							}
						}
						GUILayout.Space(widthScreen*0.01f);
						for (int i = pageDebutFriends ; i < pageFinFriends ; i++){
							if (GUILayout.Button(""+(i+1),paginatorFriendsGuiStyle[i])){
								paginatorFriendsGuiStyle[chosenPageFriends]=this.paginationStyle;
								chosenPageFriends=i;
								paginatorFriendsGuiStyle[i]=this.paginationActivatedStyle;
								displayPageFriends();
							}
							GUILayout.Space(widthScreen*0.01f);
						}
						if (nbFriendsPages>pageFinFriends){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutFriends = pageDebutFriends+10;
								pageFinFriends = Mathf.Min(pageFinFriends+10, nbFriendsPages);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();

				GUI.Label (new Rect (profilePictureStyle.fixedWidth+0.02f*widthScreen,0.56f*heightScreen,widthScreen * 0.25f,heightScreen*0.03f), "Invitations recues",titleStyle);
				GUI.Label (new Rect (profilePictureStyle.fixedWidth+0.02f*widthScreen,0.60f*heightScreen,widthScreen * 0.25f,heightScreen*0.03f), labelNoInvitationsReceived,labelNo);

				for (int i = invitationsReceivedStart;i<invitationsReceivedFinish;i++){
					GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen + ((i-invitationsReceivedStart)%nbInvitationsReceivedPerRow)*friendLabelsAreaSizeX,
					                             0.59f*heightScreen + (Mathf.FloorToInt((i-invitationsReceivedStart)/nbInvitationsReceivedPerRow)*friendLabelsAreaSizeY),
					                             friendLabelsAreaSizeX,
					                             friendLabelsAreaSizeY));
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							GUILayout.BeginVertical();
							{
								if(GUILayout.Button (userData.Connections[invitationsReceivedToBeDisplayed[i]].User,friendButtonStyle))
								{
									ApplicationModel.profileChosen=userData.Connections[invitationsReceivedToBeDisplayed[i]].User;
									Application.LoadLevel("Profile");
								}
								if(GUILayout.Button ("Confirmer",actionButtonStyle))
								{
									isDataLoaded=false;
									idConnection=userData.Connections[invitationsReceivedToBeDisplayed[i]].Id;
									StartCoroutine(confirmConnection());
									userData.Connections[invitationsReceivedToBeDisplayed[i]].State =+1;
									computeConnections();
								}
								if(GUILayout.Button ("Rejeter",actionButtonStyle))
								{
									isDataLoaded=false;
									idConnection=userData.Connections[invitationsReceivedToBeDisplayed[i]].Id;
									StartCoroutine(removeConnection());
									userData.Connections.RemoveAt(invitationsReceivedToBeDisplayed[i]);
									computeConnections();	
								}
							}
							GUILayout.EndVertical();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();
				}
				GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen,
				                             0.96f*heightScreen,
				                             nbInvitationsReceivedPerRow*friendLabelsAreaSizeX,
				                             0.03f*heightScreen));
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (pageDebutInvitationsReceived>0){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutInvitationsReceived = pageDebutInvitationsReceived-5;
								pageFinInvitationsReceived = pageDebutInvitationsReceived+5;
							}
						}
						GUILayout.Space(widthScreen*0.01f);
						for (int i = pageDebutInvitationsReceived ; i < pageFinInvitationsReceived ; i++){
							if (GUILayout.Button(""+(i+1),paginatorInvitationsReceivedGuiStyle[i])){
								paginatorInvitationsReceivedGuiStyle[chosenPageInvitationsReceived]=this.paginationStyle;
								chosenPageInvitationsReceived=i;
								paginatorInvitationsReceivedGuiStyle[i]=this.paginationActivatedStyle;
								displayPageInvitationsReceived();
							}
							GUILayout.Space(widthScreen*0.01f);
						}
						if (nbInvitationsReceivedPages>pageFinInvitationsReceived){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutInvitationsReceived = pageDebutInvitationsReceived+5;
								pageFinInvitationsReceived = Mathf.Min(pageFinInvitationsReceived+5, nbInvitationsReceivedPages);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();

				GUI.Label (new Rect (profilePictureStyle.fixedWidth+0.02f*widthScreen+(0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen)/2,0.56f*heightScreen,widthScreen * 0.25f,heightScreen*0.03f), "Invitations envoyees",titleStyle);
				GUI.Label (new Rect (profilePictureStyle.fixedWidth+0.02f*widthScreen+(0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen)/2,0.60f*heightScreen,widthScreen * 0.25f,heightScreen*0.03f), labelNoInvitationsSent,labelNo);

				for (int i = invitationsSentStart;i<invitationsSentFinish;i++){
					GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen+(0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen)/2 + ((i-invitationsSentStart)%nbInvitationsSentPerRow)*friendLabelsAreaSizeX,
					                             0.59f*heightScreen + (Mathf.FloorToInt((i-invitationsSentStart)/nbInvitationsSentPerRow)*friendLabelsAreaSizeY),
					                             friendLabelsAreaSizeX,
					                             friendLabelsAreaSizeY));
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							GUILayout.BeginVertical();
							{
								if(GUILayout.Button (userData.Connections[invitationsSentToBeDisplayed[i]].User,friendButtonStyle))
								{
									ApplicationModel.profileChosen=userData.Connections[invitationsSentToBeDisplayed[i]].User;
									Application.LoadLevel("Profile");
								}
								if(GUILayout.Button ("Retirer",actionButtonStyle))
								{
									isDataLoaded=false;
									idConnection=userData.Connections[invitationsSentToBeDisplayed[i]].Id;
									StartCoroutine(removeConnection());
									userData.Connections.RemoveAt(invitationsSentToBeDisplayed[i]);
									computeConnections();
								}
							}
							GUILayout.EndVertical();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();
				}
				GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen+(0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen)/2,
				                             0.96f*heightScreen,
				                             nbInvitationsSentPerRow*friendLabelsAreaSizeX,
				                             0.03f*heightScreen));
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (pageDebutInvitationsSent>0){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutInvitationsSent = pageDebutInvitationsSent-5;
								pageFinInvitationsSent = pageDebutInvitationsSent+5;
							}
						}
						GUILayout.Space(widthScreen*0.01f);
						for (int i = pageDebutInvitationsSent ; i < pageFinInvitationsSent ; i++){
							if (GUILayout.Button(""+(i+1),paginatorInvitationsSentGuiStyle[i])){
								paginatorInvitationsSentGuiStyle[chosenPageInvitationsSent]=this.paginationStyle;
								chosenPageInvitationsSent=i;
								paginatorInvitationsSentGuiStyle[i]=this.paginationActivatedStyle;
								displayPageInvitationsSent();
							}
							GUILayout.Space(widthScreen*0.01f);
						}
						if (nbInvitationsSentPages>pageFinInvitationsSent){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutInvitationsSent = pageDebutInvitationsSent+5;
								pageFinInvitationsSent = Mathf.Min(pageFinInvitationsSent+5, nbInvitationsSentPages);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();
				GUI.skin = fileBrowserSkin;
				if (m_fileBrowser != null) {
					m_fileBrowser.OnGUI();
					
				} else {
					if (GUI.Button(new Rect(0.01f*widthScreen, 0.10f*heightScreen + profilePictureStyle.fixedHeight,  editProfilePictureButtonStyle.fixedWidth,editProfilePictureButtonStyle.fixedHeight), "Modifier l'image",editProfilePictureButtonStyle)) {
						m_fileBrowser = new FileBrowser(
							fileBrowserWindow,
							"Sélectionnez une image",
							FileSelectedCallback
							);
						m_fileBrowser.DirectoryImage = m_directoryImage;
						m_fileBrowser.FileImage = m_fileImage;
					}
				}
			}
			else {

				GUI.DrawTexture(new Rect(0.01f*widthScreen,0.12f*heightScreen,profilePictureStyle.fixedWidth,profilePictureStyle.fixedHeight),profilePicture,ScaleMode.StretchToFill);
				GUILayout.BeginArea(new Rect(0.01f*widthScreen,0.10f*heightScreen + profilePictureStyle.fixedHeight,profilePictureStyle.fixedWidth,0.88f*heightScreen));
				{
					GUILayout.Space (editProfilePictureButtonStyle.fixedHeight);
					GUILayout.Label ("Pseudo : " + userData.Username,profileData);
				}
				GUILayout.EndArea();


				GUI.Label (new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen,0.12f*heightScreen,nbFriendsPerRow,heightScreen*0.03f), "Les amis de "+userData.Username,titleStyle);
				for (int i = friendsStart;i<friendsFinish;i++){
					GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen + ((i-friendsStart)%nbFriendsPerRow)*friendLabelsAreaSizeX,
					                             0.15f*heightScreen + (Mathf.FloorToInt((i-friendsStart)/nbFriendsPerRow)*friendLabelsAreaSizeY),
					                             friendLabelsAreaSizeX,
					                             friendLabelsAreaSizeY));
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							GUILayout.BeginVertical();
							{
								if(GUILayout.Button (userData.Connections[friendsToBeDisplayed[i]].User,friendButtonStyle))
								{
									ApplicationModel.profileChosen=userData.Connections[friendsToBeDisplayed[i]].User;
									Application.LoadLevel("Profile");
								}
							}
							GUILayout.EndVertical();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();
				}
				GUILayout.BeginArea(new Rect(profilePictureStyle.fixedWidth+0.02f*widthScreen,0.51f*heightScreen,0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen,0.03f*heightScreen));
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (pageDebutFriends>0){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutFriends = pageDebutFriends-10;
								pageFinFriends = pageDebutFriends+10;
							}
						}
						GUILayout.Space(widthScreen*0.01f);
						for (int i = pageDebutFriends ; i < pageFinFriends ; i++){
							if (GUILayout.Button(""+(i+1),paginatorFriendsGuiStyle[i])){
								paginatorFriendsGuiStyle[chosenPageFriends]=this.paginationStyle;
								chosenPageFriends=i;
								paginatorFriendsGuiStyle[i]=this.paginationActivatedStyle;
								displayPageFriends();
							}
							GUILayout.Space(widthScreen*0.01f);
						}
						if (nbFriendsPages>pageFinFriends){
							if (GUILayout.Button("...",paginationStyle)){
								pageDebutFriends = pageDebutFriends+10;
								pageFinFriends = Mathf.Min(pageFinFriends+10, nbFriendsPages);
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();
			}
		}
		if (checkPassword) {
			GUI.enabled=true;
			GUILayout.BeginArea(centralWindow);
			{
				GUILayout.BeginVertical(centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Saisissez votre mot de passe",centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space(widthScreen*0.05f);
						oldPassword = GUILayout.PasswordField(oldPassword,'*',centralWindowTextfieldStyle);
						GUILayout.Space(widthScreen*0.05f);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label (error,centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",centralWindowButtonStyle)){
							error="";
							StartCoroutine(checkUserPassword());
						}
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Quitter",centralWindowButtonStyle)){
							error="";
							oldPassword="";
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
		if (changePassword) {
			GUI.enabled=true;
			GUILayout.BeginArea(centralWindow);
			{
				GUILayout.BeginVertical(centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Entrez votre nouveau mot de passe",centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (widthScreen*0.05f);
						newPassword1 = GUILayout.PasswordField(newPassword1,'*',centralWindowTextfieldStyle);
						GUILayout.Space (widthScreen*0.05f);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Confirmer la saisie",centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (widthScreen*0.05f);
						newPassword2 = GUILayout.PasswordField(newPassword2,'*',centralWindowTextfieldStyle);
						GUILayout.Space (widthScreen*0.05f);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label (error,centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",centralWindowButtonStyle)){
							if(newPassword1==newPassword2 && newPassword1!="" && newPassword2!=""){
								error="";
								StartCoroutine(editPassword());
							}
							else if(newPassword1!=newPassword2 && newPassword1!="" && newPassword2!=""){
								error="les saisies ne correspondent pas";
							}
						}
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Quitter",centralWindowButtonStyle)){
							changePassword=false;
							checkPassword=false;
							newPassword1="";
							newPassword2="";
							oldPassword="";	
							error="";
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
		if (displayPopUp)
		{
			GUI.enabled=true;
			GUILayout.BeginArea(centralWindow);
			{
				GUILayout.BeginVertical(centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label (error,centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Quitter",centralWindowButtonStyle)){
							error="";
							displayPopUp=false;
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

	private void setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;

		nbFriendsPerRow = 4 + Mathf.FloorToInt(((float)widthScreen/(float)heightScreen - 1f) * 4f);
		nbInvitationsReceivedPerRow=Mathf.FloorToInt(nbFriendsPerRow/2);
		nbInvitationsSentPerRow=nbInvitationsReceivedPerRow;

		this.profilePictureStyle.fixedHeight = (int)heightScreen * 27 / 100;
		this.profilePictureStyle.fixedWidth = profilePictureStyle.fixedHeight;

		this.friendLabelsAreaSizeX=Mathf.FloorToInt((0.75f * widthScreen-profilePictureStyle.fixedWidth-0.02f*widthScreen) / nbFriendsPerRow);
		this.friendLabelsAreaSizeY = Mathf.FloorToInt(heightScreen*0.12f);

		this.profileData.fontSize = heightScreen * 2 / 100;
		this.profileData.fixedHeight = (int)heightScreen * 25 / 1000;
		this.profileData.fixedWidth = (int)widthScreen * 20 / 100;

		this.labelNo.fontSize = heightScreen * 2 / 100;
		this.labelNo.fixedHeight = (int)heightScreen * 25 / 1000;

		this.editProfilePictureButtonStyle.fontSize = heightScreen * 2 / 100;
		this.editProfilePictureButtonStyle.fixedHeight = (int)heightScreen * 25 / 1000;
		this.editProfilePictureButtonStyle.fixedWidth = profilePictureStyle.fixedHeight;

		this.editProfileDataButtonStyle.fontSize = heightScreen * 2 / 100;
		this.editProfileDataButtonStyle.fixedHeight = (int)heightScreen * 25 / 1000;
		this.editProfileDataButtonStyle.fixedWidth = profilePictureStyle.fixedHeight;

		this.friendshipStateButtonStyle.fontSize = heightScreen * 2 / 100;
		this.friendshipStateButtonStyle.fixedHeight = (int)heightScreen * 5 / 100;
		this.friendshipStateButtonStyle.fixedWidth = (int)widthScreen * 12 / 100;

		this.friendButtonStyle.fixedHeight = friendLabelsAreaSizeY*0.5f;
		this.friendButtonStyle.fixedWidth = friendLabelsAreaSizeX *0.9f;
		this.friendButtonStyle.fontSize = (int)friendButtonStyle.fixedHeight*35/100;

		this.actionButtonStyle.fixedHeight = (int)friendLabelsAreaSizeY*(18/100);
		this.actionButtonStyle.fixedWidth = friendButtonStyle.fixedWidth;
		this.actionButtonStyle.fontSize = friendButtonStyle.fontSize;

		this.inputTextfieldStyle.fontSize = heightScreen * 2 / 100;
		this.inputTextfieldStyle.fixedHeight = (int)heightScreen * 25 / 1000;
		this.inputTextfieldStyle.fixedWidth = profilePictureStyle.fixedWidth;

		this.titleStyle.fontSize = heightScreen * 25 / 1000;
		this.titleStyle.fixedHeight = (int)heightScreen * 3 / 100;

		this.paginationStyle.fontSize = heightScreen*2/100;
		this.paginationStyle.fixedWidth = widthScreen*3/100;
		this.paginationStyle.fixedHeight = heightScreen*3/100;
		this.paginationActivatedStyle.fontSize = heightScreen*2/100;
		this.paginationActivatedStyle.fixedWidth = widthScreen*3/100;
		this.paginationActivatedStyle.fixedHeight = heightScreen*3/100;

		this.centralWindow = new Rect (widthScreen * 0.25f, 0.12f * heightScreen, widthScreen * 0.50f, 0.25f * heightScreen);
		this.fileBrowserWindow = new Rect (widthScreen * 0.25f, 0.125f * heightScreen, widthScreen * 0.50f, 0.75f * heightScreen);

		this.centralWindowStyle.fixedWidth = widthScreen*0.5f-5;
		
		this.centralWindowTitleStyle.fontSize = heightScreen*2/100;
		this.centralWindowTitleStyle.fixedHeight = (int)heightScreen*3/100;
		this.centralWindowTitleStyle.fixedWidth = (int)widthScreen*5/10;
		
		this.centralWindowButtonStyle.fontSize = heightScreen*2/100;
		this.centralWindowButtonStyle.fixedHeight = (int)heightScreen*3/100;
		this.centralWindowButtonStyle.fixedWidth = (int)widthScreen*20/100;

		this.centralWindowTextfieldStyle.fontSize = heightScreen*2/100;
		this.centralWindowTextfieldStyle.fixedHeight = (int)heightScreen*3/100;
		this.centralWindowTextfieldStyle.fixedWidth = (int)widthScreen*4/10;

	}
	
	private IEnumerator getUserProfile() {
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", profileChosen);
		WWW w = new WWW(URLGetUserProfile, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] userInformations = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			string[] usersConnections1 = data[1].Split(new char[] { '\n' }, System.StringSplitOptions.None);
			string[] usersConnections2 = data[2].Split(new char[] { '\n' }, System.StringSplitOptions.None);

			this.userData = new User(profileChosen,
			                         userInformations[0]); // picture
			
			this.userData.Connections = new List<Connection>();
			
			if (usersConnections1.Length>0){
				for (int i=0; i<usersConnections1.Length-1;i++){
					
					string[] connectionInfo = usersConnections1[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					this.userData.Connections.Add (new Connection (System.Convert.ToInt32(connectionInfo[0]),connectionInfo[1],
					                                               System.Convert.ToInt32(connectionInfo[2])));
				}
			}
			if (usersConnections2.Length>0){
				for (int i=0; i<usersConnections2.Length-1;i++){		
					string[] connectionInfo = usersConnections2[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					this.userData.Connections.Add (new Connection (System.Convert.ToInt32(connectionInfo[0]),connectionInfo[1],
					                                               System.Convert.ToInt32(connectionInfo[2])+2));
				}
			}
			profileInitialized=true;
			computeConnections();
		}
	}
	
	private IEnumerator getMyProfile() {

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);

		WWW w = new WWW(URLGetMyProfile, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] userInformations = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			string[] usersConnections1 = data[1].Split(new char[] { '\n' }, System.StringSplitOptions.None);
			string[] usersConnections2 = data[2].Split(new char[] { '\n' }, System.StringSplitOptions.None);

			this.userData = new User(ApplicationModel.username,
			                         userInformations[0], // mail
			                         System.Convert.ToInt32(userInformations[1]), // money
			                         userInformations[2], // firstname
			                         userInformations[3], // surname
			                         userInformations[4]); // picture

			this.userData.Connections = new List<Connection>();

			if (usersConnections1.Length>0){
				for (int i=0; i<usersConnections1.Length-1;i++){

					string[] connectionInfo = usersConnections1[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					this.userData.Connections.Add (new Connection (System.Convert.ToInt32(connectionInfo[0]),connectionInfo[1],
					                                               System.Convert.ToInt32(connectionInfo[2])));
				}
			}

			if (usersConnections2.Length>0){
				for (int i=0; i<usersConnections2.Length-1;i++){
					
					string[] connectionInfo = usersConnections2[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);

					this.userData.Connections.Add (new Connection (System.Convert.ToInt32(connectionInfo[0]),connectionInfo[1],
					                                               System.Convert.ToInt32(connectionInfo[2])+2));
				}
			}

			firstName=userData.FirstName;
			surname=userData.Surname;
			mail=userData.Mail;

			profileInitialized=true;
			computeConnections();
		}
	}

	private IEnumerator setProfilePicture(){

		profilePicture = new Texture2D (4, 4, TextureFormat.DXT1, false);

		if (userData.Picture.StartsWith(ServerDirectory)){
			var www = new WWW(ApplicationModel.host + userData.Picture);
			yield return www;
			www.LoadImageIntoTexture(profilePicture);
			isPictureLoaded=true;
		}
		else {
			var www = new WWW(URLDefaultProfilePicture);
			yield return www;
			www.LoadImageIntoTexture(profilePicture);
			isPictureLoaded = true;
		}
	}
	
	public void computeConnections(){

		this.friendsToBeDisplayed = new List<int> ();
		this.invitationsSentToBeDisplayed = new List<int> ();
		this.invitationsReceivedToBeDisplayed = new List<int> ();

		for (int i =0; i<userData.Connections.Count;i++){
			if (userData.Connections[i].State % 2 !=0){
				this.friendsToBeDisplayed.Add(i);
			}
			else if(userData.Connections[i].State ==2){
				this.invitationsReceivedToBeDisplayed.Add (i);
			}
			else if(userData.Connections[i].State ==0){
				this.invitationsSentToBeDisplayed.Add (i);
			}
		}

		if(!myProfile){

			friendshipState=4;
			for (int i = 0; i<userData.Connections.Count;i++){
				if (userData.Connections[i].User== ApplicationModel.username) {

					friendshipState = userData.Connections[i].State;
					friendshipConnection = i;
				
					if (friendshipState==3)
						friendshipState=1;
				}

			}
		}
		setDisplayParameters ();
	}
	
	public void setDisplayParameters(){

		nbFriendsToDisplay = friendsToBeDisplayed.Count;
		nbInvitationsSentToDisplay = invitationsSentToBeDisplayed.Count;
		nbInvitationsReceivedToDisplay = invitationsReceivedToBeDisplayed.Count;


		nbFriendsPages = Mathf.CeilToInt((nbFriendsToDisplay-1) / (3*nbFriendsPerRow))+1;

		if (nbFriendsToDisplay==0){
			nbFriendsPages =0;
			labelNoFriends="Vous n'avez pas encore d'amis";
		} else {
			labelNoFriends="";
		}

		nbInvitationsSentPages = Mathf.CeilToInt((nbInvitationsSentToDisplay-1) / (3*nbInvitationsReceivedPerRow))+1;

		if (nbInvitationsSentToDisplay==0){
			nbInvitationsSentPages =0;
			labelNoInvitationsSent="Vous n'avez pas envoye d'invitations";
		} else {
			labelNoInvitationsSent="";
		}

		nbInvitationsReceivedPages = Mathf.CeilToInt((nbInvitationsReceivedToDisplay-1) / (3*nbInvitationsSentPerRow)+1);

		if (nbInvitationsReceivedToDisplay==0){
			nbInvitationsReceivedPages =0;
			labelNoInvitationsReceived="Vous n'avez pas d'invitations en attente";
		} else {
			labelNoInvitationsReceived="";
		}
		pageDebutFriends = 0 ;
		if (nbFriendsPages>10){
			pageFinFriends = 9 ;
		}
		else{
			pageFinFriends = nbFriendsPages ;
		}
		paginatorFriendsGuiStyle = new GUIStyle[nbFriendsPages];
		for (int i = 0; i < nbFriendsPages; i++) { 
			if (i==0){
				paginatorFriendsGuiStyle[i]=paginationActivatedStyle;
			}
			else{
				paginatorFriendsGuiStyle[i]=paginationStyle;
			}
		}
		
		pageDebutInvitationsReceived = 0 ;
		if (nbInvitationsReceivedPages>5){
			pageFinInvitationsReceived = 4 ;
		}
		else{
			pageFinInvitationsReceived = nbInvitationsReceivedPages ;
		}
		paginatorInvitationsReceivedGuiStyle = new GUIStyle[nbInvitationsReceivedPages];
		for (int i = 0; i < nbInvitationsReceivedPages; i++) { 
			if (i==0){
				paginatorInvitationsReceivedGuiStyle[i]=paginationActivatedStyle;
			}
			else{
				paginatorInvitationsReceivedGuiStyle[i]=paginationStyle;
			}
		}
		
		pageDebutInvitationsSent = 0 ;
		if (nbInvitationsSentPages>5){
			pageFinInvitationsSent = 4 ;
		}
		else{
			pageFinInvitationsSent = nbInvitationsSentPages ;
		}
		paginatorInvitationsSentGuiStyle = new GUIStyle[nbInvitationsSentPages];
		for (int i = 0; i < nbInvitationsSentPages; i++) { 
			if (i==0){
				paginatorInvitationsSentGuiStyle[i]=paginationActivatedStyle;
			}
			else{
				paginatorInvitationsSentGuiStyle[i]=paginationStyle;
			}
		}
		
		displayPageFriends ();
		displayPageInvitationsSent ();
		displayPageInvitationsReceived ();
		isDataLoaded =true;
	}

	public void displayPageFriends(){
		
		friendsStart = chosenPageFriends*(nbFriendsPerRow*3);
		if (nbFriendsToDisplay < (3*nbFriendsPerRow*(chosenPageFriends+1))){
			friendsFinish = nbFriendsToDisplay;
		}
		else{
			friendsFinish = (chosenPageFriends+1)*(3 * nbFriendsPerRow);
		}
	}

	public void displayPageInvitationsSent(){

		invitationsSentStart = chosenPageInvitationsSent*(nbInvitationsSentPerRow*3);
		if (nbInvitationsSentToDisplay < (3*nbInvitationsSentPerRow*(chosenPageInvitationsSent+1))){
			invitationsSentFinish = nbInvitationsSentToDisplay;
		}
		else{
			invitationsSentFinish = (chosenPageInvitationsSent+1)*(nbInvitationsSentPerRow*3);
		}
	}

	public void displayPageInvitationsReceived(){

		invitationsReceivedStart = chosenPageInvitationsReceived*(nbInvitationsReceivedPerRow*3);
		if (nbInvitationsReceivedToDisplay < (3*nbInvitationsReceivedPerRow*(chosenPageInvitationsReceived+1))){
			invitationsReceivedFinish = nbInvitationsReceivedToDisplay;
		}
		else{
			invitationsReceivedFinish = (chosenPageInvitationsReceived+1)*(nbInvitationsReceivedPerRow*3);
		}
	}

	private IEnumerator confirmConnection(){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idconnection", idConnection.ToString());
		form.AddField("myform_date",  System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLConfirmConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); 											// donne le retour
			if (w.text != "1"){
				if (myProfile)
					StartCoroutine(getMyProfile());
				else
					StartCoroutine(getUserProfile());
			}
		}
	}


	private IEnumerator removeConnection(){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idconnection", idConnection.ToString());
		
		WWW w = new WWW(URLRemoveConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			//print(w.text); 											// donne le retour
			if (w.text != "1"){
				if (myProfile)
					StartCoroutine(getMyProfile());
				else
					StartCoroutine(getUserProfile());
			}
		}
	}


	private IEnumerator createConnection(){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_target", profileChosen);
		form.AddField("myform_date",  System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		
		WWW w = new WWW(URLCreateConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			//print(w.text); 											// donne le retour
			string[] data = w.text.Split(new string[] { "\\" }, System.StringSplitOptions.None);
			string lastId=data[0];

			if (data[1]=="1"){
				isDataLoaded=false;
				if (myProfile)
					StartCoroutine(getMyProfile());
				else
					StartCoroutine(getUserProfile());
			}
			else{
				isDataLoaded=false;
				userData.Connections.Add (new Connection(System.Convert.ToInt32(lastId),ApplicationModel.username,2));
				computeConnections();
			}
		}
	}

	private IEnumerator updateProfile(){
			
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_firstname", firstName);
		form.AddField("myform_surname", surname);
		form.AddField("myform_mail", mail);
		
		WWW w = new WWW(URLUpdateProfile, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			//print(w.text); 											// donne le retour
			StartCoroutine(getMyProfile());
		}
	}


	public void FileSelectedCallback(string path) {
		m_fileBrowser = null;
		m_textPath = path;
		if (m_textPath!=null)
		StartCoroutine (updateProfilePicture ());

	}


	IEnumerator updateProfilePicture()
	{

		error = "";
		var fileInfo = new System.IO.FileInfo(m_textPath);
		long fileSize = fileInfo.Length;
		string fileExtension = fileInfo.Extension;

		if (!availableExtension.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)) {
			displayPopUp=true;
			error ="Chargement annule, l'image doit etre au format .png ou .jpg";
			yield break;
		}

		if (fileSize > sizeMax) 
		{
			displayPopUp = true;
			error = "Chargement annule, l'image ne doit pas depasser " + (sizeMax / 1024) + "Mo";
			yield break;
		}
		//limit = 3145728

		//byte[] localFile = System.IO.File.ReadAllBytes(m_textPath);

		WWW localFile = new WWW("file:///" + m_textPath);
		yield return localFile;
		if (localFile.error == null){
			//Debug.Log("Loaded file successfully");
		}
		else
		{
			Debug.Log("Open file error: "+localFile.error);
			displayPopUp=true;
			error ="Le chargement de l'image a echoue, veuilez recommencer";
			yield break; // stop the coroutine here
		}

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddBinaryData("myform_file",localFile.bytes, ApplicationModel.username+fileExtension);
		
		WWW w = new WWW(URLUpdateProfilePicture, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text);
			if(System.Convert.ToInt32(w.text)==1){
				userData.Picture=ApplicationModel.host+ServerDirectory + ApplicationModel.username + fileExtension;
				//print(userData.Picture);

				var www = new WWW(userData.Picture);
				yield return www;
				
				// assign the downloaded image to the main texture of the object
				www.LoadImageIntoTexture(profilePicture);
			}
			else	
			{
					displayPopUp=true;
					error ="Le chargement de l'image a echoue, veuilez recommencer";
					yield break;
			}
		}
	}


	IEnumerator checkUserPassword()
	{

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 
		form.AddField("myform_pass", oldPassword);
		
		WWW w = new WWW(URLCheckPassword, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text);
			if (w.text.Equals("YES")) 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				checkPassword=false;
				changePassword=true;
				error="";
				oldPassword="";
			}
			else 
			{
				error = "mot de passe incorect";		
			}
		}

	}


	IEnumerator editPassword()
	{

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 
		form.AddField("myform_pass", newPassword1);
		
		WWW w = new WWW(URLEditPassword, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			newPassword1="";
			newPassword2="";
			oldPassword="";
			checkPassword=false;
			changePassword=false;
		}
	}

}
