using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameScript : MonoBehaviour {



	public int gameType;
	public bool hasWon;
	public GameObject MenuObject;
	public int nbLimitFriendlyGameToDisplay;

	public GUIStyle blockBorderStyle;
	public GUIStyle lastResultsLabelStyle;
	public GUIStyle profilePictureButtonDefaultStyle;
	public GUIStyle opponentsInformationsStyle;
	public GUIStyle winLabelResultsListStyle;
	public GUIStyle defeatLabelResultsListStyle;
	public GUIStyle winBackgroundResultsListStyle;
	public GUIStyle defeatBackgroundResultsListStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle paginationStyle;

	private IList<GUIStyle> profilePictureButtonStyle=new List<GUIStyle>();
	private GUIStyle[] paginatorGuiStyle;
	
	private string URLGetUDivisionEndGameData="http://54.77.118.214/GarrukServer/get_division_end_game_data.php";
	private string URLGetCupEndGameData="http://54.77.118.214/GarrukServer/get_cup_end_game_data.php";
	private string URLGetFriendlyEndGameData="http://54.77.118.214/GarrukServer/get_friendly_end_game_data.php";
	private string ServerDirectory = "img/profile/";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";
	private User currentUser;
	private bool isDataLoaded=false;
	private int widthScreen; 
	private int heightScreen;
	private int start;
	private int finish;
	private int nbPages;
	private int pageDebut;
	private int pageFin;
	private int chosenPage=0;

	private Texture2D[] profilePictures;

	private float blockTopLeftWidth;
	private float blockTopLeftHeight;
	private float blockTopRightWidth;
	private float blockTopRightHeight;
	private float blockBottomLeftWidth;
	private float blockBottomLeftHeight;
	private float blockBottomRightWidth;
	private float blockBottomRightHeight;
	private float gapBetweenblocks;
	private int profilePicturesSize;
	private Rect blockTopLeft;
	private Rect blockTopRight;
	private Rect blockBottomLeft;
	private Rect blockBottomRight;

	private string lastResultsLabel;



	// Use this for initialization
	void Start () {
		
		MenuObject = Instantiate(MenuObject) as GameObject;
		setStyles ();
		switch(gameType)
		{
		case 0:
			StartCoroutine (getFriendlyEndGameData ());
			lastResultsLabel="Vos derniers résultats";
			break;
		case 1:
			StartCoroutine (getDivisionEndGameData ());
			lastResultsLabel="Vos résultats de division";
			break;
		case 2:
			StartCoroutine (getCupEndGameData ());
			lastResultsLabel="Vos résultats de coupe";
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
		}
	
	}

	void OnGUI () {

		if(isDataLoaded){

			// block SUP GAUCHE
			GUILayout.BeginArea(blockTopLeft,blockBorderStyle);
			{
			}
			GUILayout.EndArea();

			// block INF GAUCHE
			GUILayout.BeginArea(blockBottomLeft,blockBorderStyle);
			{
			}
			GUILayout.EndArea();

			// block SUP DROIT
			GUILayout.BeginArea(blockTopRight,blockBorderStyle);
			{
			}
			GUILayout.EndArea();

			// block INF DROIT
			GUILayout.BeginArea(blockBottomRight,blockBorderStyle);
			{
				GUILayout.Label (lastResultsLabel,lastResultsLabelStyle);
				for (int i=start;i<finish;i++){
					if(currentUser.ResultsHistory[i].HasWon)
					{
						GUILayout.BeginHorizontal(winBackgroundResultsListStyle);
					}
					else
					{
						GUILayout.BeginHorizontal(defeatBackgroundResultsListStyle);
					}
					{

						GUILayout.Space(blockBottomRightWidth*5/100);
						if(GUILayout.Button("",profilePictureButtonStyle[i]))
						{
							ApplicationModel.profileChosen=currentUser.ResultsHistory[i].Opponent.Username;
							Application.LoadLevel("profile");
						}
						GUILayout.Space(blockBottomRightWidth*5/100);
						GUILayout.BeginVertical();
						{
							GUILayout.Space(profilePicturesSize*5/100);
							if(currentUser.ResultsHistory[i].HasWon)
							{
								GUILayout.Label ("Victoire",winLabelResultsListStyle);
							}
							else
							{
								GUILayout.Label ("Défaite",defeatLabelResultsListStyle);
							}
							GUILayout.Space(profilePicturesSize*5/100);
							GUILayout.Label (currentUser.ResultsHistory[i].Opponent.Username
							                 ,opponentsInformationsStyle);
							GUILayout.Label ("V : "+currentUser.ResultsHistory[i].Opponent.TotalNbWins
							                 +" D : " +currentUser.ResultsHistory[i].Opponent.TotalNbLooses
							                 ,opponentsInformationsStyle);
							GUILayout.Label ("Ranking : "+currentUser.ResultsHistory[i].Opponent.Ranking
							                 ,opponentsInformationsStyle);
							GUILayout.Label ("Division : "+currentUser.ResultsHistory[i].Opponent.Division.Id
							                 ,opponentsInformationsStyle);
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(blockBottomRightHeight*10/1000);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (pageDebut>0){
						if (GUILayout.Button("...",paginationStyle)){
							pageDebut = pageDebut-5;
							pageFin = pageDebut+5;
						}
					}
					GUILayout.Space(widthScreen*0.01f);
					for (int i = pageDebut ; i < pageFin ; i++){
						if (GUILayout.Button(""+(i+1),paginatorGuiStyle[i])){
							paginatorGuiStyle[chosenPage]=this.paginationStyle;
							chosenPage=i;
							paginatorGuiStyle[i]=this.paginationActivatedStyle;
							displayPage();
						}
						GUILayout.Space(widthScreen*0.01f);
					}
					if (nbPages>pageFin){
						if (GUILayout.Button("...",paginationStyle)){
							pageDebut = pageDebut+5;
							pageFin = Mathf.Min(pageFin+5, nbPages);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(blockBottomRightHeight*10/1000);
			}
			GUILayout.EndArea();
		
		}
	}

	private void setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;

		gapBetweenblocks = 0.005f * widthScreen;
		blockTopLeftHeight = 0.66f*(0.9f*heightScreen-3*gapBetweenblocks);
		blockTopLeftWidth = 0.75f*(widthScreen-3*gapBetweenblocks);
		blockBottomLeftHeight = 0.34f*(0.9f*heightScreen-3*gapBetweenblocks);
		blockBottomLeftWidth = blockTopLeftWidth;
		blockTopRightWidth = 0.25f*(widthScreen-3*gapBetweenblocks);
		blockTopRightHeight = 0.30f*(0.9f*heightScreen-3*gapBetweenblocks);
		blockBottomRightWidth = blockTopRightWidth;
		blockBottomRightHeight = 0.70f*(0.9f*heightScreen-3*gapBetweenblocks);

		blockTopLeft = new Rect (gapBetweenblocks, 0.1f * heightScreen + gapBetweenblocks, blockTopLeftWidth, blockTopLeftHeight);
		blockBottomLeft = new Rect (gapBetweenblocks, 0.1f * heightScreen + 2 * gapBetweenblocks + blockTopLeftHeight, blockBottomLeftWidth, blockBottomLeftHeight);
		blockTopRight = new Rect (blockTopLeftWidth+2*gapBetweenblocks, 0.1f * heightScreen + gapBetweenblocks, blockTopRightWidth, blockTopRightHeight);
		blockBottomRight = new Rect (blockTopLeftWidth+2*gapBetweenblocks, 0.1f * heightScreen + 2 * gapBetweenblocks + blockTopRightHeight, blockBottomRightWidth, blockBottomRightHeight);

		profilePicturesSize=(int)blockBottomRightHeight* 17 / 100;

		this.lastResultsLabelStyle.fontSize = heightScreen * 2 / 100;
		this.lastResultsLabelStyle.fixedHeight = (int)heightScreen * 35 / 1000;

		this.winLabelResultsListStyle.fontSize = (int)profilePicturesSize * 18 / 100;
		this.winLabelResultsListStyle.fixedHeight = (int)profilePicturesSize*20/100;
		this.winLabelResultsListStyle.fixedWidth = 2*(int)profilePicturesSize;

		this.defeatLabelResultsListStyle.fontSize = (int)profilePicturesSize * 18 / 100;
		this.defeatLabelResultsListStyle.fixedHeight = (int)profilePicturesSize*20/100;
		this.defeatLabelResultsListStyle.fixedWidth = 2*(int)profilePicturesSize;

		this.opponentsInformationsStyle.fontSize = (int)profilePicturesSize * 15 / 100;
		this.opponentsInformationsStyle.fixedHeight = (int)profilePicturesSize * 17 / 100;
		this.opponentsInformationsStyle.fixedWidth = 2*(int)profilePicturesSize;

		for(int i=0;i<profilePictureButtonStyle.Count;i++){
		this.profilePictureButtonStyle[i].fixedWidth = profilePicturesSize;
		this.profilePictureButtonStyle[i].fixedHeight = profilePicturesSize;
		}

		this.paginationStyle.fontSize = (int)blockBottomRightHeight*3/100;
		this.paginationStyle.fixedWidth = (int)blockBottomRightWidth*10/100;
		this.paginationStyle.fixedHeight = (int)blockBottomRightHeight*4/100;
		this.paginationActivatedStyle.fontSize = (int)blockBottomRightHeight*3/100;
		this.paginationActivatedStyle.fixedWidth = (int)blockBottomRightWidth*10/100;
		this.paginationActivatedStyle.fixedHeight = (int)blockBottomRightHeight*4/100;
	}
	private IEnumerator getDivisionEndGameData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetUDivisionEndGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			IList<Result> resultsHistory=new List<Result>();
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] userData=data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] divisionData=data[1].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] resultsData=data[2].Split(new string[] { "RESULT" }, System.StringSplitOptions.None);

			for (int i =0;i<resultsData.Length-1;i++)
			{
				string[] resultData=resultsData[i].Split (new string[] {"//"}, System.StringSplitOptions.None);
				resultsHistory.Add (new Result(new User(resultData[2],
				                                        resultData[3],
				                                        new Division(System.Convert.ToInt32(resultData[4])),
				                                        System.Convert.ToInt32(resultData[5]),
				                                        System.Convert.ToInt32(resultData[6]),
				                                        System.Convert.ToInt32(resultData[7]),
				                                        System.Convert.ToInt32(resultData[8])),
				                               System.Convert.ToBoolean(System.Convert.ToInt32(resultData[0])),
				                               DateTime.ParseExact(resultData[1], "yyyy-MM-dd HH:mm:ss", null)));
			}
			currentUser = new User(new Division(System.Convert.ToInt32(divisionData[0]),
												System.Convert.ToInt32(divisionData[1]),
			                                    System.Convert.ToInt32(divisionData[2]),
			                                    System.Convert.ToInt32(divisionData[3]),
			                                    System.Convert.ToInt32(divisionData[4]),
			                                    System.Convert.ToInt32(divisionData[5]),
			                                    System.Convert.ToInt32(divisionData[6])),
			                       resultsHistory,
			                       System.Convert.ToInt32(userData[0]),
			                       System.Convert.ToInt32(userData[1]),
			                       System.Convert.ToInt32(userData[2]),
			                       System.Convert.ToInt32(userData[3]));
			picturesInitialization();
		}
	}
	private IEnumerator getCupEndGameData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetCupEndGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			IList<Result> resultsHistory=new List<Result>();
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] userData=data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] cupData=data[1].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] resultsData=data[2].Split(new string[] { "RESULT" }, System.StringSplitOptions.None);
			
			for (int i =0;i<resultsData.Length-1;i++)
			{
				string[] resultData=resultsData[i].Split (new string[] {"//"}, System.StringSplitOptions.None);
				resultsHistory.Add (new Result(new User(resultData[2],
				                                        resultData[3],
				                                        new Division(System.Convert.ToInt32(resultData[4])),
				                                        System.Convert.ToInt32(resultData[5]),
				                                        System.Convert.ToInt32(resultData[6]),
				                                        System.Convert.ToInt32(resultData[7]),
				                                        System.Convert.ToInt32(resultData[8])),
				                               System.Convert.ToBoolean(System.Convert.ToInt32(resultData[0])),
				                               DateTime.ParseExact(resultData[1], "yyyy-MM-dd HH:mm:ss", null)));
			}
			currentUser = new User(new Cup(System.Convert.ToInt32(cupData[0]),
			                               System.Convert.ToInt32(cupData[1]),
			                               System.Convert.ToInt32(cupData[2])),
			                       resultsHistory,
			                       System.Convert.ToInt32(userData[0]),
			                       System.Convert.ToInt32(userData[1]),
			                       System.Convert.ToInt32(userData[2]),
			                       System.Convert.ToInt32(userData[3]));
			picturesInitialization();
		}
	}
	private IEnumerator getFriendlyEndGameData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_limit", nbLimitFriendlyGameToDisplay);
		
		WWW w = new WWW(URLGetFriendlyEndGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			IList<Result> resultsHistory=new List<Result>();
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] userData=data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] resultsData=data[1].Split(new string[] { "RESULT" }, System.StringSplitOptions.None);
			
			for (int i =0;i<resultsData.Length-1;i++)
			{
				string[] resultData=resultsData[i].Split (new string[] {"//"}, System.StringSplitOptions.None);
				resultsHistory.Add (new Result(new User(resultData[2],
				                                        resultData[3],
				                                        new Division(System.Convert.ToInt32(resultData[4])),
				                                        System.Convert.ToInt32(resultData[5]),
				                                        System.Convert.ToInt32(resultData[6]),
				                                        System.Convert.ToInt32(resultData[7]),
				                                        System.Convert.ToInt32(resultData[8])),
				                               System.Convert.ToBoolean(System.Convert.ToInt32(resultData[0])),
				                               DateTime.ParseExact(resultData[1], "yyyy-MM-dd HH:mm:ss", null)));
			}
			currentUser = new User(resultsHistory,
			                       System.Convert.ToInt32(userData[0]),
			                       System.Convert.ToInt32(userData[1]),
			                       System.Convert.ToInt32(userData[2]),
			                       System.Convert.ToInt32(userData[3]));
			picturesInitialization();
		}
	}
	private void picturesInitialization(){
		profilePictures =new Texture2D[currentUser.ResultsHistory.Count];
		for(int i =0;i<currentUser.ResultsHistory.Count;i++)
		{
			profilePictures[i]=new Texture2D(profilePicturesSize, profilePicturesSize, TextureFormat.ARGB32, false);
			profilePictureButtonStyle.Add(new GUIStyle());
			profilePictureButtonStyle[i].normal.background=profilePictures[i];
			profilePictureButtonStyle[i].fixedWidth = profilePicturesSize;
			profilePictureButtonStyle[i].fixedHeight = profilePicturesSize;
		}
		nbPages = Mathf.CeilToInt(currentUser.ResultsHistory.Count/5)+1;
		pageDebut = 0 ;
		if (nbPages>5){
			pageFin = 4 ;
		}
		else{
			pageFin = nbPages ;
		}
		paginatorGuiStyle = new GUIStyle[nbPages];
		for (int i = 0; i < nbPages; i++) { 
			if (i==0){
				paginatorGuiStyle[i]=paginationActivatedStyle;
			}
			else{
				paginatorGuiStyle[i]=paginationStyle;
			}
		}
		displayPage ();
		isDataLoaded=true;
		setProfilePictures ();
	}
	private void displayPage(){
		start = chosenPage*5;
		if (currentUser.ResultsHistory.Count < (5*(chosenPage+1))){
			finish = currentUser.ResultsHistory.Count;
		}
		else{
			finish = (chosenPage+1)*5;
		}
	}
	private void setProfilePictures(){

		for(int i =0;i<currentUser.ResultsHistory.Count;i++)
		{
			if (currentUser.ResultsHistory[i].Opponent.Picture.StartsWith(ServerDirectory))
			{
				StartCoroutine(loadProfilePicture(i));
			}
			else
			{
				StartCoroutine(loadDefaultProfilePicture(i));
			}
		}
	}
	private IEnumerator loadProfilePicture(int i){

		var www = new WWW(ApplicationModel.host + currentUser.ResultsHistory[i].Opponent.Picture);
		yield return www;
		www.LoadImageIntoTexture(profilePictures[i]);
		profilePictureButtonStyle[i].normal.background=profilePictures[i];
	}
	private IEnumerator loadDefaultProfilePicture(int i){

		var www = new WWW(URLDefaultProfilePicture);
		yield return www;
		www.LoadImageIntoTexture(profilePictures[i]);
		profilePictureButtonStyle[i].normal.background=profilePictures[i];
	}
}
