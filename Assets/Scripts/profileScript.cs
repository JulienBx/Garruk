using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class profileScript : MonoBehaviour {
	
	bool isDataLoaded = false;
	bool myProfile = false;
	bool isEditing = false;
	bool displayPopUp = false;
	bool changePassword = false;
	bool correctPassword = false;

	string firstName;
	string surname;
	string mail;
	string error;
	string oldPassword ="";
	string newPassword1="";
	string newPassword2="";

	int idConnection;
	int friendshipConnection;
	User userData;
	int friendshipState = -1;

	public GUISkin fileBrowserSkin ;
	
	Texture2D profilePicture;

	public Texture2D white;
	public Texture2D backButton ;
	public Texture2D backActivatedButton ;


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
	int chosenPageInvitationsSent=0;
	int chosenPageInvitationsReceived=0;

	int nbFriendsToDisplay;
	int nbInvitationsSentToDisplay;
	int nbInvitationsReceivedToDisplay;

	GUIStyle profileData;
	GUIStyle profilePictureStyle;
	GUIStyle labelNo;

	GUIStyle[] paginatorFriendsGuiStyle;
	GUIStyle[] paginatorInvitationsReceivedGuiStyle;
	GUIStyle[] paginatorInvitationsSentGuiStyle;

	private string URLGetUserProfile = "http://54.77.118.214/GarrukServer/get_user_profile.php";
	private string URLGetMyProfile = "http://54.77.118.214/GarrukServer/get_myprofile.php";
	private string URLConfirmConnection = "http://54.77.118.214/GarrukServer/confirm_connection.php";
	private string URLRemoveConnection = "http://54.77.118.214/GarrukServer/remove_connection.php";
	private string URLCreateConnection = "http://54.77.118.214/GarrukServer/create_connection.php";
	private string URLUpdateProfile = "http://54.77.118.214/GarrukServer/update_profile.php";
	private string URLUpdateProfilePicture = "hhttp://54.77.118.214/GarrukServer/update_profilepicture.php";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautProfilePicture.png";
	private string URLCheckPassword = "http://54.77.118.214/GarrukServer/check_password.php";
	private string URLEditPassword = "http://54.77.118.214/GarrukServer/edit_password.php";

	private IList<int> friendsToBeDisplayed ;
	private IList<int> invitationsSentToBeDisplayed ;
	private IList<int> invitationsReceivedToBeDisplayed ;

	string labelNoFriends;
	string labelNoInvitationsReceived;
	string labelNoInvitationsSent;


	string m_textPath;
	
	FileBrowser m_fileBrowser;

	Rect errorWindow ;
	Rect changePasswordWindow ;

	public Texture2D m_directoryImage;
	public Texture2D m_fileImage;


	// Use this for initialization
	void Start () {


	
		StartCoroutine(setStyles());

		if (ApplicationModel.profileChosen != "" && ApplicationModel.profileChosen != ApplicationModel.username){
			StartCoroutine(getUserProfile());
		}
		else {
			myProfile=true;
			StartCoroutine(getMyProfile());
		}


	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnGUI () {

		if (displayPopUp)
			errorWindow = GUI.Window(0, new Rect(Screen.width/2-100, Screen.height/2-50, 300, 110), DoErrorWindow, "Erreur");

		if (changePassword)
			changePasswordWindow = GUI.Window(0, new Rect(Screen.width/2-150, Screen.height/2-110, 300, 220), DoChangePasswordWindow, "Modification du mot de passe");



		if (isDataLoaded) {


			switch (friendshipState)
			{
			case 0:
				if (GUI.Button(new Rect(0.86f*widthScreen,
				                        0.03f*heightScreen,
				                        widthScreen*0.12f,
				                        heightScreen*0.05f),
				               "Confirmer la demande")){
					
					
					isDataLoaded=false;
					idConnection=userData.Connections[friendshipConnection].Id;
					StartCoroutine(confirmConnection());
					userData.Connections[friendshipConnection].State =+1;
					computeConnections();
				}
				if (GUI.Button(new Rect(0.86f*widthScreen,
				                        0.085f*heightScreen,
				                        widthScreen*0.12f,
				                        heightScreen*0.05f),
				               "Rejeter la demande")){
					
					
					isDataLoaded=false;
					idConnection=userData.Connections[friendshipConnection].Id;
					StartCoroutine(removeConnection());
					userData.Connections.RemoveAt(friendshipConnection);
					computeConnections();
				}
				break;
			case 1:
				if (GUI.Button(new Rect(0.86f*widthScreen,
				                        0.03f*heightScreen,
				                        widthScreen*0.12f,
				                        heightScreen*0.05f),
				               "Retirer de vos amis")){
					
					
					isDataLoaded=false;
					idConnection=userData.Connections[friendshipConnection].Id;
					StartCoroutine(removeConnection());
					userData.Connections.RemoveAt(friendshipConnection);
					computeConnections();
				}
				break;
			
			case 2:
				if (GUI.Button(new Rect(0.86f*widthScreen,
				                        0.03f*heightScreen,
				                        widthScreen*0.12f,
				                        heightScreen*0.05f),
				               "Retirer votre demande")){
					
					
					isDataLoaded=false;
					idConnection=userData.Connections[friendshipConnection].Id;
					StartCoroutine(removeConnection());
					userData.Connections.RemoveAt(friendshipConnection);
					computeConnections();
					
				}
				break;
			case 4:
				if (GUI.Button(new Rect(0.86f*widthScreen,
				                        0.03f*heightScreen,
				                        widthScreen*0.12f,
				                        heightScreen*0.05f),
				               "Envoyer une demande")){
					
					
					isDataLoaded=false;
					StartCoroutine(createConnection());

					
				}
				break;
			}


			if (myProfile){


				GUILayout.BeginArea(new Rect(0.05f*widthScreen,0.05f*heightScreen,widthScreen * 0.20f,heightScreen));
				{

	
					GUILayout.Box(profilePicture,profilePictureStyle);

					GUILayout.Space (5);
					GUILayout.Label ("Pseudo : " + userData.Username,profileData);
					GUILayout.Label ("Argent : " + userData.Money + " credits",profileData);

					if(!isEditing){

						GUILayout.Label ("Prenom : " + userData.FirstName,profileData);
						GUILayout.Label ("Nom : " + userData.Surname,profileData);
						GUILayout.Label ("Email : " + userData.Mail,profileData);


						if (GUILayout.Button ("Modifier mes infos",GUILayout.Width(profilePicture.width),GUILayout.Height(0.03f*heightScreen)))
						{
							isEditing = true;
						}

						if (GUILayout.Button ("Changer le mot de passe",GUILayout.Width(profilePicture.width),GUILayout.Height(0.03f*heightScreen)))
						{
							changePassword = true;
						}

					}

					if (isEditing){

				
						firstName = GUILayout.TextField(firstName, 15, GUILayout.Width(profilePicture.width));
						surname = GUILayout.TextField(surname, 15,GUILayout.Width(profilePicture.width));
						mail=GUILayout.TextField(mail, 30,GUILayout.Width(profilePicture.width));

						if (GUILayout.Button ("Valider",GUILayout.Width(profilePicture.width),GUILayout.Height(0.03f*heightScreen)))
						{
							isEditing = false;
							isDataLoaded=false;
							StartCoroutine(updateProfile());


						}

						if (GUILayout.Button ("Annuler",GUILayout.Width(profilePicture.width),GUILayout.Height(0.03f*heightScreen)))
						{
							isEditing = false;
						}

					}

						
				}
				GUILayout.EndArea();


				GUI.Label (new Rect(0.25f*widthScreen,0.03f*heightScreen,nbFriendsPerRow,heightScreen*0.05f), "Mes amis",profileData);
				GUI.Label (new Rect(0.25f*widthScreen,0.08f*heightScreen,nbFriendsPerRow,heightScreen*0.05f), labelNoFriends,labelNo);

				for (int i = friendsStart;i<friendsFinish;i++){

					if (GUI.Button(new Rect(0.25f*widthScreen + ((i-friendsStart)%nbFriendsPerRow)*(widthScreen* 0.50f/(nbFriendsPerRow)),
					                        0.10f*heightScreen + (Mathf.FloorToInt((i-friendsStart)/nbFriendsPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.05f),
					               			userData.Connections[friendsToBeDisplayed[i]].User)){


						ApplicationModel.profileChosen=userData.Connections[friendsToBeDisplayed[i]].User;
						Application.LoadLevel("Profile");
					}


					if (GUI.Button(new Rect(0.25f*widthScreen + ((i-friendsStart)%nbFriendsPerRow)*(widthScreen* 0.50f/(nbFriendsPerRow)),
					                        0.15f*heightScreen + (Mathf.FloorToInt((i-friendsStart)/nbFriendsPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.025f),
					               			"Retirer")){


						isDataLoaded=false;
						idConnection=userData.Connections[friendsToBeDisplayed[i]].Id;
						StartCoroutine(removeConnection());
						userData.Connections.RemoveAt(friendsToBeDisplayed[i]);
						computeConnections();

					}


				}


				for (int i = 0 ; i < nbFriendsPages ; i++){
					if (GUI.Button(new Rect(widthScreen*0.25f+i*widthScreen*0.03f,
					                        0.45f*heightScreen,
					                        0.02f*widthScreen,
					                        0.03f*heightScreen),""+(i+1),paginatorFriendsGuiStyle[i])){

						paginatorFriendsGuiStyle[chosenPageFriends].normal.background=backButton;
						paginatorFriendsGuiStyle[chosenPageFriends].normal.textColor=Color.black;
						chosenPageFriends=i;
						paginatorFriendsGuiStyle[i].normal.background=backActivatedButton;
						paginatorFriendsGuiStyle[i].normal.textColor=Color.white;
						displayPageFriends ();
					}
				}




				GUI.Label (new Rect (0.25f*widthScreen,0.50f*heightScreen,widthScreen * 0.20f,heightScreen*0.05f), "Invitations recues",profileData);
				GUI.Label (new Rect (0.25f*widthScreen,0.55f*heightScreen,widthScreen * 0.20f,heightScreen*0.05f), labelNoInvitationsReceived,labelNo);


				for (int i = invitationsReceivedStart;i<invitationsReceivedFinish;i++){
					
					if (GUI.Button(new Rect(0.25f*widthScreen + ((i-invitationsReceivedStart)%nbInvitationsReceivedPerRow)*(widthScreen* 0.20f/(nbInvitationsReceivedPerRow)),
					                        0.57f*heightScreen + (Mathf.FloorToInt((i-invitationsReceivedStart)/nbInvitationsReceivedPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.05f),
					               			userData.Connections[invitationsReceivedToBeDisplayed[i]].User)){
						
						
						ApplicationModel.profileChosen=userData.Connections[invitationsReceivedToBeDisplayed[i]].User;
						Application.LoadLevel("Profile");
					}
					
					
					if (GUI.Button(new Rect(0.25f*widthScreen + ((i-invitationsReceivedStart)%nbInvitationsReceivedPerRow)*(widthScreen* 0.20f/(nbInvitationsReceivedPerRow)),
					                        0.62f*heightScreen + (Mathf.FloorToInt((i-invitationsReceivedStart)/nbInvitationsReceivedPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.025f),
					               "Confirmer")){

						isDataLoaded=false;
						idConnection=userData.Connections[invitationsReceivedToBeDisplayed[i]].Id;
						StartCoroutine(confirmConnection());
						userData.Connections[invitationsReceivedToBeDisplayed[i]].State =+1;
						computeConnections();


					}

					if (GUI.Button(new Rect(0.25f*widthScreen + ((i-invitationsReceivedStart)%nbInvitationsReceivedPerRow)*(widthScreen* 0.20f/(nbInvitationsReceivedPerRow)),
					                        0.645f*heightScreen + (Mathf.FloorToInt((i-invitationsReceivedStart)/nbInvitationsReceivedPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.025f),
					               "Rejeter")){
						
						isDataLoaded=false;
						idConnection=userData.Connections[invitationsReceivedToBeDisplayed[i]].Id;
						StartCoroutine(removeConnection());
						userData.Connections.RemoveAt(invitationsReceivedToBeDisplayed[i]);
						computeConnections();
						
					}
					
					
				}

				for (int i = 0 ; i < nbInvitationsReceivedPages ; i++){
					if (GUI.Button(new Rect(widthScreen*0.25f+i*widthScreen*0.03f,
					                        0.92f*heightScreen,
					                        0.02f*widthScreen,
					                        0.03f*heightScreen),""+(i+1),paginatorInvitationsReceivedGuiStyle[i])){
						
						paginatorInvitationsReceivedGuiStyle[chosenPageInvitationsReceived].normal.background=backButton;
						paginatorInvitationsReceivedGuiStyle[chosenPageInvitationsReceived].normal.textColor=Color.black;
						chosenPageInvitationsReceived=i;
						paginatorInvitationsReceivedGuiStyle[i].normal.background=backActivatedButton;
						paginatorInvitationsReceivedGuiStyle[i].normal.textColor=Color.white;
						displayPageInvitationsReceived ();
					}
				}





				GUI.Label (new Rect (0.50f*widthScreen,0.50f*heightScreen,widthScreen * 0.20f,heightScreen*0.05f), "Invitations envoyees",profileData);
				GUI.Label (new Rect (0.50f*widthScreen,0.55f*heightScreen,widthScreen * 0.20f,heightScreen*0.05f), labelNoInvitationsSent,labelNo);


				for (int i = invitationsSentStart;i<invitationsSentFinish;i++){
					
					if (GUI.Button(new Rect(0.50f*widthScreen + ((i-invitationsSentStart)%nbInvitationsSentPerRow)*(widthScreen* 0.20f/(nbInvitationsSentPerRow)),
					                        0.57f*heightScreen + (Mathf.FloorToInt((i-invitationsSentStart)/nbInvitationsSentPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.05f),
					               			userData.Connections[invitationsSentToBeDisplayed[i]].User)){
						
						
						ApplicationModel.profileChosen=userData.Connections[invitationsSentToBeDisplayed[i]].User;
						Application.LoadLevel("Profile");
					}
					
					
					if (GUI.Button(new Rect(0.50f*widthScreen + ((i-invitationsSentStart)%nbInvitationsSentPerRow)*(widthScreen* 0.20f/(nbInvitationsSentPerRow)),
					                        0.62f*heightScreen + (Mathf.FloorToInt((i-invitationsSentStart)/nbInvitationsSentPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.025f),
					               "Retirer")){

						isDataLoaded=false;
						idConnection=userData.Connections[invitationsSentToBeDisplayed[i]].Id;
						StartCoroutine(removeConnection());
						userData.Connections.RemoveAt(invitationsSentToBeDisplayed[i]);
						computeConnections();

					}
					
					
				}
				
				for (int i = 0 ; i < nbInvitationsSentPages ; i++){
					if (GUI.Button(new Rect(widthScreen*0.50f+i*widthScreen*0.03f,
					                        0.92f*heightScreen,
					                        0.02f*widthScreen,
					                        0.03f*heightScreen),""+(i+1),paginatorInvitationsSentGuiStyle[i])){
						
						paginatorInvitationsSentGuiStyle[chosenPageInvitationsSent].normal.background=backButton;
						paginatorInvitationsSentGuiStyle[chosenPageInvitationsSent].normal.textColor=Color.black;
						chosenPageInvitationsSent=i;
						paginatorInvitationsSentGuiStyle[i].normal.background=backActivatedButton;
						paginatorInvitationsSentGuiStyle[i].normal.textColor=Color.white;
						displayPageInvitationsSent ();
					}
				}


				GUI.skin = fileBrowserSkin;

				if (m_fileBrowser != null) {
					m_fileBrowser.OnGUI();
				} else {
					//GUILayout.BeginHorizontal();
					//GUILayout.Label("Text File", GUILayout.Width(100));
					//GUILayout.FlexibleSpace();
					//GUILayout.Label(m_textPath ?? "none selected");
					if (GUI.Button(new Rect(0.05f*widthScreen,profilePicture.height+0.02f*heightScreen,profilePicture.width,0.03f*heightScreen),"Modifier l'image")) {
						m_fileBrowser = new FileBrowser(
							new Rect(100, 100, 600, 500),
							"Sélectionnez une image",
							FileSelectedCallback
							);
						//m_fileBrowser.SelectionPattern = "*.png";
						m_fileBrowser.DirectoryImage = m_directoryImage;
						m_fileBrowser.FileImage = m_fileImage;
					}
					//GUILayout.EndHorizontal();;
				}


			}

			else {





				GUILayout.BeginArea(new Rect(0.05f*widthScreen,0.05f*heightScreen,widthScreen * 0.20f,heightScreen));
				{
					GUILayout.BeginVertical(); // also can put width in here
					{
						GUILayout.Box(profilePicture,profilePictureStyle);
						GUILayout.Space (5);
						GUILayout.Label ("Pseudo : "+ userData.Username,profileData);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();


				GUI.Label (new Rect(0.25f*widthScreen,0.03f*heightScreen,nbFriendsPerRow,heightScreen*0.05f), "Les amis de "+userData.Username,profileData);
				
				
				for (int i = friendsStart;i<friendsFinish;i++){
					
					if (GUI.Button(new Rect(0.25f*widthScreen + ((i-friendsStart)%nbFriendsPerRow)*(widthScreen* 0.50f/(nbFriendsPerRow)),
					                        0.10f*heightScreen + (Mathf.FloorToInt((i-friendsStart)/nbFriendsPerRow)*heightScreen*0.12f),
					                        widthScreen*0.50f/(nbFriendsPerRow+1),
					                        heightScreen*0.05f),
					               userData.Connections[friendsToBeDisplayed[i]].User)){
						
						
						ApplicationModel.profileChosen=userData.Connections[friendsToBeDisplayed[i]].User;
						Application.LoadLevel("Profile");
					}
				}


			}

		}
	}






	private IEnumerator setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		
		this.profileData = new GUIStyle ();
		profileData.normal.textColor = Color.black;
		profileData.fontSize = heightScreen/40;
		profileData.alignment = TextAnchor.MiddleLeft;

		this.labelNo = new GUIStyle ();
		labelNo.normal.textColor = Color.black;
		labelNo.fontSize = heightScreen/60;
		labelNo.alignment = TextAnchor.MiddleLeft;
		
		this.profilePictureStyle = new GUIStyle ();
		profilePictureStyle.normal.background = white;
		profilePictureStyle.alignment = TextAnchor.MiddleLeft;
		
		
		yield break;
	}


	private IEnumerator getUserProfile() {

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.profileChosen);
		
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
			
			this.userData = new User(ApplicationModel.profileChosen,
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

			//computeConnections();
			StartCoroutine(setProfilePicture());


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

			//computeConnections();
			StartCoroutine(setProfilePicture());


		}
	}




	private IEnumerator setProfilePicture(){

		profilePicture = new Texture2D (4, 4, TextureFormat.DXT1, false);

		if (userData.Picture.StartsWith("http")){
			var www = new WWW(userData.Picture);
			yield return www;
			www.LoadImageIntoTexture(profilePicture);

		}
		else {
			var www = new WWW(URLDefaultProfilePicture);
			yield return www;
			www.LoadImageIntoTexture(profilePicture);
		}
			
			
			computeConnections();

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

		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.6f;

		nbFriendsPerRow = Mathf.FloorToInt(width/2f);
		nbInvitationsReceivedPerRow=Mathf.FloorToInt(width/4f);
		nbInvitationsSentPerRow=Mathf.FloorToInt(width/4f);

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


		setPaginationParameters ();

	}

	public void setPaginationParameters(){
		
		paginatorFriendsGuiStyle = new GUIStyle[nbFriendsPages];
		for (int i = 0; i < nbFriendsPages; i++) { 
			paginatorFriendsGuiStyle [i] = new GUIStyle ();
			paginatorFriendsGuiStyle [i].alignment = TextAnchor.MiddleCenter;
			paginatorFriendsGuiStyle [i].fontSize = 12;
			if (i==chosenPageFriends){
				paginatorFriendsGuiStyle[i].normal.background=backActivatedButton;
				paginatorFriendsGuiStyle[i].normal.textColor=Color.white;
			}
			else{
				paginatorFriendsGuiStyle[i].normal.background=backButton;
				paginatorFriendsGuiStyle[i].normal.textColor=Color.black;
			}
			
		}
		
		paginatorInvitationsReceivedGuiStyle = new GUIStyle[nbInvitationsReceivedPages];
		for (int i = 0; i < nbInvitationsReceivedPages; i++) { 
			paginatorInvitationsReceivedGuiStyle [i] = new GUIStyle ();
			paginatorInvitationsReceivedGuiStyle [i].alignment = TextAnchor.MiddleCenter;
			paginatorInvitationsReceivedGuiStyle [i].fontSize = 12;
			if (i==chosenPageInvitationsReceived){
				paginatorInvitationsReceivedGuiStyle[i].normal.background=backActivatedButton;
				paginatorInvitationsReceivedGuiStyle[i].normal.textColor=Color.white;
			}
			else{
				paginatorInvitationsReceivedGuiStyle[i].normal.background=backButton;
				paginatorInvitationsReceivedGuiStyle[i].normal.textColor=Color.black;
			}
			
		}
		
		paginatorInvitationsSentGuiStyle = new GUIStyle[nbInvitationsSentPages];
		for (int i = 0; i < nbInvitationsSentPages; i++) { 
			paginatorInvitationsSentGuiStyle [i] = new GUIStyle ();
			paginatorInvitationsSentGuiStyle [i].alignment = TextAnchor.MiddleCenter;
			paginatorInvitationsSentGuiStyle [i].fontSize = 12;
			if (i==chosenPageInvitationsSent){
				paginatorInvitationsSentGuiStyle[i].normal.background=backActivatedButton;
				paginatorInvitationsSentGuiStyle[i].normal.textColor=Color.white;
			}
			else{
				paginatorInvitationsSentGuiStyle[i].normal.background=backButton;
				paginatorInvitationsSentGuiStyle[i].normal.textColor=Color.black;
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
		
		WWW w = new WWW(URLConfirmConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
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
		form.AddField("myform_target", ApplicationModel.profileChosen);
		
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
				if (myProfile)
					StartCoroutine(getMyProfile());
				else
					StartCoroutine(getUserProfile());
			}
			else{
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

		if (fileExtension != ".jpg" && fileExtension != ".png") {
			displayPopUp=true;
			error ="Chargement annule,\n l'image doit etre au format .png ou .jpg";
			yield break;
		}

		if (fileSize > 3145728) {
			displayPopUp=true;
			error ="Chargement annule,\n l'image ne doit pas depasser 3 Mo";
			yield break;
		}
		//limit = 3145728

		//byte[] localFile = System.IO.File.ReadAllBytes(m_textPath);

		WWW localFile = new WWW("file:///" + m_textPath);
		yield return localFile;
		if (localFile.error == null)
			Debug.Log("Loaded file successfully");
		else
		{
			Debug.Log("Open file error: "+localFile.error);
			displayPopUp=true;
			error ="Le chargement de l'image a echoue,\n veuilez recommencer";
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
			if(System.Convert.ToInt32(w.text)==1){
				userData.Picture="http://localhost/GarrukServer/img/profile/" + ApplicationModel.username + fileExtension;
			
				//print(userData.Picture);

				var www = new WWW(userData.Picture);
				yield return www;
				
				// assign the downloaded image to the main texture of the object
				www.LoadImageIntoTexture(profilePicture);
			}
			else	
			{
					displayPopUp=true;
					error ="Le chargement de l'image a echoue,\n veuilez recommencer";
					yield break;
			}

			
		}

	}


	IEnumerator checkPassword()
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
				correctPassword=true;
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
			correctPassword=false;
			changePassword=false;
		}
	}

	void DoErrorWindow(int windowID) {
		
		GUI.Label (new Rect(10,20,280,50),error);
		if (GUI.Button(new Rect(110,80,80,20), "Quitter")){
			displayPopUp=false;

		}
		
	}

	void DoChangePasswordWindow(int windowID) {
		
		if (correctPassword) {

			GUI.Label (new Rect(10,30,280,20),"Entrez votre nouveau mot de passe");
			newPassword1 = GUI.PasswordField(new Rect(10,60,280,20), newPassword1,'*');
			GUI.Label (new Rect(10,90,280,20),"Confirmer la saisie");
			newPassword2 = GUI.PasswordField(new Rect(10,120,280,20), newPassword2,'*');
			GUI.Label (new Rect(10,150,280,20),error);
			if (GUI.Button(new Rect(210,180,80,20), "OK")){

				if(newPassword1==newPassword2 && newPassword1!="" && newPassword2!=""){
				error="";
				StartCoroutine(editPassword());
				}
				else if(newPassword1!=newPassword2 && newPassword1!="" && newPassword2!=""){
				error="les saisies ne correspondent pas";
				}
				
			}
			if (GUI.Button(new Rect(120,180,80,20), "Quitter")){
				changePassword=false;
				correctPassword=false;
				newPassword1="";
				newPassword2="";
				oldPassword="";
				
			}
		
		}

		else {

			GUI.Label (new Rect(10,30,280,20),"Saisissez votre mot de passe");
			oldPassword = GUI.PasswordField(new Rect(10,60,280,20), oldPassword,'*');
			GUI.Label (new Rect(10,90,280,20),error);
			if (GUI.Button(new Rect(210,180,80,20), "OK")){
				error="";
				StartCoroutine(checkPassword());
				
			}
			if (GUI.Button(new Rect(120,180,80,20), "Quitter")){
				error="";
				oldPassword="";
				changePassword=false;
				
			}
		

		}
		
	}


		
		
		
		
		
	
	
}
