using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private string URLGetUserData = "http://54.77.118.214/GarrukServer/get_user_data.php";
	private Text name;
	private Text notifications;

	public GUIStyle menuBackgroundStyle;
	public GUIStyle buttonStyle;
	public GUIStyle logOutButtonStyle;
	public GUIStyle titleStyle;
	public GUIStyle welcomeStyle;
	public GUIStyle nonReadNotificationsButtonStyle;
	public GUIStyle nonReadNotificationsCounterStyle;
	public GUIStyle nonReadNotificationsCounterPoliceStyle;

	int widthScreen = Screen.width; 
	int heightScreen = Screen.height;
	float ratioScreen;

	float timer;
	public int refreshInterval;

	float flexibleSpaceSize;
	float distanceToNonReadNotificationsCounter;

	void Start(){ 
		StartCoroutine(loadUserData ());
		setStyles ();
	}

	void Update(){

		timer += Time.deltaTime;
		
		if (timer > refreshInterval) {
			timer=timer-refreshInterval;
			StartCoroutine(loadUserData ());
		}
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
		}
	}
	
	void OnGUI(){
		GUILayout.BeginArea (new Rect(0,0,widthScreen,0.1f*heightScreen),menuBackgroundStyle);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Garruk, le jeu",titleStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Accueil",buttonStyle)){
					homePageLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				if (ApplicationModel.nbNotificationsNonRead>0){
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("",nonReadNotificationsButtonStyle)){
							homePageLink();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				}
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Mes cartes",buttonStyle)){
						myGameLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("La boutique",buttonStyle)){
						shopLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Le bazar",buttonStyle)){
						marketLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Jouer",buttonStyle)){
						lobbyLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("Déconnexion",logOutButtonStyle)){
							logOutLink();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if(GUILayout.Button (ApplicationModel.username,welcomeStyle)){
							profileLink();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label (ApplicationModel.credits+" crédits",welcomeStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		if (ApplicationModel.nbNotificationsNonRead>0){
			GUILayout.BeginArea (new Rect(distanceToNonReadNotificationsCounter,0.01f*heightScreen,nonReadNotificationsButtonStyle.fixedHeight*0.5f,nonReadNotificationsButtonStyle.fixedHeight*0.5f),nonReadNotificationsCounterStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(ApplicationModel.nbNotificationsNonRead.ToString(),nonReadNotificationsCounterPoliceStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();	
			}
			GUILayout.EndArea();
		}
	}

	private void setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;

		this.titleStyle.fontSize = heightScreen*25/1000;
		this.titleStyle.fixedHeight = (int)heightScreen*6/100;
		this.titleStyle.fixedWidth = (int)widthScreen*15/100;

		this.nonReadNotificationsButtonStyle.fontSize = heightScreen*20/1000;
		this.nonReadNotificationsButtonStyle.fixedHeight = (int)heightScreen*6/100;
		this.nonReadNotificationsButtonStyle.fixedWidth = (int)heightScreen*6/100;

		this.buttonStyle.fontSize = heightScreen*20/1000;
		this.buttonStyle.fixedHeight = (int)heightScreen*6/100;
		this.buttonStyle.fixedWidth = (int)widthScreen*11/100;

		this.logOutButtonStyle.fontSize = heightScreen*20/1000;
		this.logOutButtonStyle.fixedHeight = (int)heightScreen*3/100;
		this.logOutButtonStyle.fixedWidth = (int)widthScreen*15/100;

		this.welcomeStyle.fontSize = heightScreen*20/1000;
		this.welcomeStyle.fixedHeight = (int)heightScreen*3/100;
		this.welcomeStyle.fixedWidth = (int)widthScreen*15/100;

		this.nonReadNotificationsCounterPoliceStyle.fontSize = heightScreen*2/100;

		flexibleSpaceSize = (widthScreen - this.titleStyle.fixedWidth - 5f * this.buttonStyle.fixedWidth - this.welcomeStyle.fixedWidth - this.nonReadNotificationsButtonStyle.fixedWidth) / 13;
		distanceToNonReadNotificationsCounter = 3f * flexibleSpaceSize + this.titleStyle.fixedWidth + this.buttonStyle.fixedWidth + 0.75f * this.nonReadNotificationsButtonStyle.fixedWidth;
	}
	
	private IEnumerator loadUserData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetUserData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			ApplicationModel.credits = System.Convert.ToInt32(data[0]);
			ApplicationModel.nbNotificationsNonRead = System.Convert.ToInt32(data[1]);
		}
	}

	public void homePageLink()
	{
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("HomePage");
	}
	public void myGameLink()
	{
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("MyGame");
	}
	public void shopLink()
	{
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("BuyCards");
	}
	public void marketLink()
	{
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("Market");
	}

	public void logOutLink() 
	{
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("ConnectionPage");
	}
	public void profileLink() 
	{
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("Profile");
	}
	public void lobbyLink()
	{
		if(Application.loadedLevelName=="LobbyPage"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("LobbyPage");
	}
}