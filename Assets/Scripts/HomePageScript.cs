using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageScript : MonoBehaviour {

	private string URLRetrieveNotifications = "http://54.77.118.214/GarrukServer/get_notifications_by_user.php";
	private string URLUpdateNotificationsRead = "http://54.77.118.214/GarrukServer/update_notifications_read.php";

	int totalNbResultLimit = 1000;
	private IList<Notification> notifications ;
	bool notificationsLoaded =false;
	int objectCount=0;

	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;
	
	int chosenPage=0;
	int oldChosenPage=-1;
	int nbPages;
	int pageDebut;
	int pageFin;
	int nbNotificationToDisplay;
	int start;
	int finish;

	string notificationsRead;

	public GUIStyle notificationValueStyle;
	public GUIStyle notificationCardStyle;
	public GUIStyle notificationUserStyle;
	public GUIStyle notificationDateStyle;
	public GUIStyle notificationStandardStyle;
	public GUIStyle notificationTitleStyle;
	public GUIStyle notificationNonReadStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle paginationStyle;
	public GUIStyle newNotificationStyle;
	
	GUIStyle[] paginatorGuiStyle;


	// Use this for initialization
	void Start () {

		this.setStyles();
		StartCoroutine (retrieveNotifications ());
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
			displayPage();
		}
	
	}

	void OnGUI () {
		
		if(notificationsLoaded){

			GUILayout.BeginArea(new Rect(widthScreen * 0.01f,0.1f*heightScreen,widthScreen * 0.98f,0.865f*heightScreen));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Mes actualités",notificationTitleStyle);
					if(ApplicationModel.nbNotificationsNonRead>0){
						GUILayout.Label(" - "+ApplicationModel.nbNotificationsNonRead+" nouvelles",notificationNonReadStyle);
						GUILayout.FlexibleSpace();
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.Space (heightScreen*3/100);
				for (int i = start ; i<finish;i++){
					GUILayout.BeginHorizontal();
					{
						if(notifications[i].IsRead==false){
							GUILayout.Label("Nouveau ! ",newNotificationStyle);
						}
						GUILayout.Label(notifications[i].Date.ToString(),notificationDateStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						objectCount=0;
						for(int j=0;j<notifications[i].Content.Length;j++){
							switch (notifications[i].Content[j])
							{
							case "*user*":
								if (GUILayout.Button(notifications[i].NotificationObjects[objectCount].Param1,notificationUserStyle)){
									ApplicationModel.profileChosen=notifications[i].NotificationObjects[objectCount].Param1;
									Application.LoadLevel("Profile");
								}
								objectCount++;
								break;
							case "*card*":
								GUILayout.Label(notifications[i].NotificationObjects[objectCount].Param1,notificationCardStyle);
								objectCount++;
								break;
							case "*value*":
								GUILayout.Label(notifications[i].NotificationObjects[objectCount].Param1,notificationValueStyle);
								objectCount++;
								break;
							default:
								GUILayout.Label(notifications[i].Content[j],notificationStandardStyle);
								break;
							}
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space (heightScreen*3/100);
				}
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(widthScreen * 0.01f,0.965f*heightScreen,widthScreen * 0.98f,0.03f*heightScreen));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (pageDebut>0){
						if (GUILayout.Button("...",paginationStyle)){
							pageDebut = pageDebut-15;
							pageFin = pageDebut+15;
						}
					}
					GUILayout.Space(widthScreen*0.01f);
					for (int i = pageDebut ; i < pageFin ; i++){
						if (GUILayout.Button(""+(i+1),paginatorGuiStyle[i])){
							paginatorGuiStyle[chosenPage]=this.paginationStyle;
							oldChosenPage=chosenPage;
							chosenPage=i;
							paginatorGuiStyle[i]=this.paginationActivatedStyle;
							displayPage();
						}
						GUILayout.Space(widthScreen*0.01f);
					}
					if (nbPages>pageFin){
						if (GUILayout.Button("...",paginationStyle)){
							pageDebut = pageDebut+15;
							pageFin = Mathf.Min(pageFin+15, nbPages);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		
		}
	}
	
	private void setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		
		this.notificationValueStyle.fontSize = heightScreen*25/1000;
		this.notificationValueStyle.fixedHeight = (int)heightScreen*3/100;

		this.notificationCardStyle.fontSize = heightScreen*25/1000;
		this.notificationCardStyle.fixedHeight = (int)heightScreen*3/100;

		this.notificationUserStyle.fontSize = heightScreen*25/1000;
		this.notificationUserStyle.fixedHeight = (int)heightScreen*3/100;

		this.notificationDateStyle.fontSize = heightScreen*2/100;
		this.notificationDateStyle.fixedHeight = (int)heightScreen*25/1000;

		this.newNotificationStyle.fontSize = heightScreen*2/100;
		this.newNotificationStyle.fixedHeight = (int)heightScreen*25/1000;

		this.notificationStandardStyle.fontSize = heightScreen*25/1000;
		this.notificationStandardStyle.fixedHeight = (int)heightScreen*3/100;

		this.notificationTitleStyle.fontSize = heightScreen*3/100;
		this.notificationTitleStyle.fixedHeight = (int)heightScreen*4/100;

		this.notificationNonReadStyle.fontSize = heightScreen*3/100;
		this.notificationNonReadStyle.fixedHeight = (int)heightScreen*4/100;

		this.paginationStyle.fontSize = heightScreen*2/100;
		this.paginationStyle.fixedWidth = widthScreen*3/100;
		this.paginationStyle.fixedHeight = heightScreen*3/100;
		this.paginationActivatedStyle.fontSize = heightScreen*2/100;
		this.paginationActivatedStyle.fixedWidth = widthScreen*3/100;
		this.paginationActivatedStyle.fixedHeight = heightScreen*3/100;

	}

	private void displayPage(){

		if (oldChosenPage!=-1){
			for (int i=start; i<finish; i++) {
				if (!notifications[i].IsRead && notifications[i].IdNotificationType!=1){
					notifications[i].IsRead=true;
					ApplicationModel.nbNotificationsNonRead--;
				}
			}
		}

		pageDebut = 0 ;
		if (nbPages>15){
			pageFin = 14 ;
		}
		else{
			pageFin = nbPages ;
		}

		start = 10 * chosenPage;
		if (10*(chosenPage+1)>nbNotificationToDisplay){
			finish=nbNotificationToDisplay;
		}
		else{
			finish=start+10;
		}

		if(ApplicationModel.nbNotificationsNonRead>0){

			notificationsRead=" AND (";
			for (int i=start; i<finish; i++) {
				if (!notifications[i].IsRead){
					notificationsRead=notificationsRead + " id="+notifications[i].Id+" OR"; // A COMPLETER 
				}
			}
			notificationsRead = notificationsRead.Remove(notificationsRead.Length - 2) + ")";
			//print (notificationsRead);
			StartCoroutine(updateNotificationsRead());
		}

	}

	private IEnumerator retrieveNotifications(){
		
		this.notifications = new List<Notification>();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLRetrieveNotifications, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			//print (w.text);
			string[] data=w.text.Split(new string[] { "#N#" }, System.StringSplitOptions.None);
			int nbNonRead =0;

			for (int i=0;i<data.Length-1;i++){

				string[] notificationData =data[i].Split(new string[] { "//" }, System.StringSplitOptions.None);

				bool isRead=true;

				if (notificationData[3]=="0"){
					isRead=false;
					nbNonRead++;
				}

				this.notifications.Add(new Notification(System.Convert.ToInt32(notificationData[0]),
														notificationData[1].Split(new string[] { "#" }, System.StringSplitOptions.None), // content
				                                        DateTime.ParseExact(notificationData[2], "yyyy-MM-dd hh:mm:ss", null), // date
				                                        isRead,
				                                        System.Convert.ToInt32(notificationData[4]))); // isRead

				this.notifications[i].NotificationObjects = new List<NotificationObject>();
				for(int j=5;j<notificationData.Length-1;j++){

					string[] notificationObjectData = notificationData[j].Split (new char[] {':'},System.StringSplitOptions.None);

					switch (notificationObjectData[0])
					{
					case "user":
						this.notifications[i].NotificationObjects.Add (new NotificationObject(notificationObjectData[1]));
						break;
					case "card":
						this.notifications[i].NotificationObjects.Add (new NotificationObject(notificationObjectData[1]));
						break;
					case "value":
						this.notifications[i].NotificationObjects.Add (new NotificationObject(notificationObjectData[1]));
						break;
					}
				}

			}
			ApplicationModel.nbNotificationsNonRead=nbNonRead;

			nbNotificationToDisplay=notifications.Count;
			nbPages = Mathf.CeilToInt((nbNotificationToDisplay-1) / (10))+1;
			paginatorGuiStyle = new GUIStyle[nbPages];
			for (int i = 0; i < nbPages; i++) { 
				if (i==0){
					paginatorGuiStyle[i]=paginationActivatedStyle;
				}
				else{
					paginatorGuiStyle[i]=paginationStyle;
				}
			}
			displayPage();
			notificationsLoaded=true;
		}
	}
	
	private IEnumerator updateNotificationsRead(){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_notificationsRead", notificationsRead);
		
		WWW w = new WWW(URLUpdateNotificationsRead, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
	}
	
}
