using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameScript : MonoBehaviour {



	public int gameType;
	public GameObject MenuObject;
	public int nbLimitFriendlyGameToDisplay;

	public GUIStyle blockBorderStyle;
	public GUIStyle rankingLabelStyle;
	public GUIStyle yourRankingStyle;
	public GUIStyle yourRankingPointsStyle;
	public GUIStyle lastResultsLabelStyle;
	public GUIStyle lastOpponentLabelStyle;
	public GUIStyle lastOponnentInformationsLabelStyle;
	public GUIStyle lastOponnentUsernameLabelStyle;
	public GUIStyle opponentsInformationsStyle;
	public GUIStyle winLabelResultsListStyle;
	public GUIStyle defeatLabelResultsListStyle;
	public GUIStyle winBackgroundResultsListStyle;
	public GUIStyle defeatBackgroundResultsListStyle;
	public GUIStyle lastOpponentBackgroundStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle paginationStyle;
	public GUIStyle mainLabelStyle;
	public GUIStyle subMainLabelStyle;
	public GUIStyle divisionLabelStyle;
	public GUIStyle divisionStrikeLabelStyle;
	public GUIStyle remainingGamesStyle;
	public GUIStyle gaugeBackgroundStyle;
	public GUIStyle startActiveGaugeBackgroundStyle;
	public GUIStyle activeGaugeBackgroundStyle;
	public GUIStyle relegationBarStyle;
	public GUIStyle promotionBarStyle;
	public GUIStyle titleBarStyle;

	private IList<GUIStyle> profilePictureButtonStyle=new List<GUIStyle>();
	private GUIStyle lastOpponentProfilePictureButtonStyle=new GUIStyle();
	private GUIStyle[] paginatorGuiStyle;
	
	private string URLGetUDivisionEndGameData="http://54.77.118.214/GarrukServer/get_division_end_game_data.php";
	private string URLGetCupEndGameData="http://54.77.118.214/GarrukServer/get_cup_end_game_data.php";
	private string URLGetFriendlyEndGameData="http://54.77.118.214/GarrukServer/get_friendly_end_game_data.php";
	private string ServerDirectory = "img/profile/";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";
	private User currentUser;
	private bool isDataLoaded=false;
	private bool toUpdateGauge=false;
	private int widthScreen; 
	private int heightScreen;
	private int start;
	private int finish;
	private int nbPages;
	private int pageDebut;
	private int pageFin;
	private int chosenPage=0;

	private int nbWinsDivision=0;
	private int nbLoosesDivision=0;
	private int remainingGames = 0;
	private int hasWon;

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
	private int lastOpponentProfilePictureSize;
	private Rect blockTopLeft;
	private Rect blockTopRight;
	private Rect blockBottomLeft;
	private Rect blockBottomRight;

	private string lastResultsLabel;

	private float gaugeSpace1=0f;
	private float gaugeSpace1Start=0f;
	private float gaugeSpace1Finish=0f;
	private float gaugeSpace2=0f;
	private float gaugeSpace2Start=0f;
	private float gaugeSpace2Finish=0f;
	private float gaugeSpace3=0f;
	private float gaugeSpace3Start=0f;
	private float gaugeSpace3Finish=0f;
	private float gaugeSpace4=0.1f;
	private float gaugeSpace1Width;
	private float gaugeSpace2Width;
	private float gaugeSpace3Width;
	private float gaugeSpace4Width;
	private float gaugeWidth;
	private float gaugeHeight;
	private float startActiveGaugeWidth=0.2f;
	private float activeGaugeWidth=0f;
	private float activeGaugeWidthStart=0f;
	private float activeGaugeWidthFinish=0f;
	private float relegationBarWidth=0f;
	private float relegationBarFinish=0f;
	private float promotionBarWidth=0f;
	private float promotionBarFinish=0f;
	private float titleBarWidth=0.005f; 
	private float titleBarFinish=0.005f;
	private float transformRatio=0f;
	private float transformSpeed=0.5f;
	
	// Use this for initialization
	void Start () {
		
		MenuObject = Instantiate(MenuObject) as GameObject;
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

		if (toUpdateGauge){
			transformRatio = transformRatio + transformSpeed * Time.deltaTime;
			computeGauge();
		}
	
	}
	void OnGUI () {
		if(isDataLoaded){
			// block SUP GAUCHE
			GUILayout.BeginArea(blockTopLeft,blockBorderStyle);
			{
				switch(gameType)
				{
				case 0:
					if(currentUser.ResultsHistory[0].HasWon)
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label ("BRAVO !",mainLabelStyle);
						GUILayout.Label ("Venez en match officiel vous mesurer aux meilleurs joueurs !",subMainLabelStyle);
						GUILayout.FlexibleSpace();
					}
					else
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label ("DOMMAGE !",mainLabelStyle);
						GUILayout.Label ("C'est en s'entrainant qu'on progresse ! Courage !",subMainLabelStyle);
						GUILayout.FlexibleSpace();
					}
					break;
				case 1:
					GUILayout.Label("Division "+currentUser.Division.Id.ToString(),divisionLabelStyle);
					GUILayout.Label("Série : "+nbWinsDivision+" V, "+nbLoosesDivision+" D",divisionStrikeLabelStyle);
					GUILayout.Label("Matchs restants : "+remainingGames.ToString(),remainingGamesStyle);
					GUILayout.BeginHorizontal(gaugeBackgroundStyle);
					{
						GUILayout.Label (nbWinsDivision+"V",startActiveGaugeBackgroundStyle);
						GUILayout.Label ("",activeGaugeBackgroundStyle);
						GUILayout.Space (gaugeSpace1Width);
						GUILayout.Label ("",relegationBarStyle);
						GUILayout.Space (gaugeSpace2Width);
						GUILayout.Label ("",promotionBarStyle);
						GUILayout.Space (gaugeSpace3Width);
						GUILayout.Label ("",titleBarStyle);
						GUILayout.Space (gaugeSpace4Width);
					}
					GUILayout.EndHorizontal();
					break;
				case 2:
					break;
				}
			}
			GUILayout.EndArea();

			// block INF GAUCHE
			GUILayout.BeginArea(blockBottomLeft,blockBorderStyle);
			{
				GUILayout.Label ("Votre dernier adversaire",lastOpponentLabelStyle);
				GUILayout.BeginHorizontal(lastOpponentBackgroundStyle);
				{
					GUILayout.Space(blockBottomLeftWidth*5/100);
					if(GUILayout.Button("",lastOpponentProfilePictureButtonStyle))
					{
						ApplicationModel.profileChosen=currentUser.ResultsHistory[0].Opponent.Username;
						Application.LoadLevel("profile");
					}
					GUILayout.Space(blockBottomLeftWidth*5/100);
					GUILayout.BeginVertical();
					{
						GUILayout.Label (currentUser.ResultsHistory[0].Opponent.Username
						                 ,lastOponnentUsernameLabelStyle);
						GUILayout.Space(profilePicturesSize*10/100);
						GUILayout.Label ("Victoires : "+currentUser.ResultsHistory[0].Opponent.TotalNbWins
						                 ,lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Défaites : "+currentUser.ResultsHistory[0].Opponent.TotalNbLooses
						                 ,lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Ranking : "+currentUser.ResultsHistory[0].Opponent.Ranking
						                 ,lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Ranking Points : "+currentUser.ResultsHistory[0].Opponent.RankingPoints
						                 ,lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Division : "+currentUser.ResultsHistory[0].Opponent.Division.Id
						                 ,lastOponnentInformationsLabelStyle);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();

			// block SUP DROIT
			GUILayout.BeginArea(blockTopRight,blockBorderStyle);
			{
				GUILayout.Label ("Vos statistiques",rankingLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Victoires : "+currentUser.TotalNbWins,yourRankingStyle);
				GUILayout.Label ("Défaites : "+currentUser.TotalNbLooses,yourRankingStyle);
				GUILayout.Label ("Ranking : "+currentUser.Ranking,yourRankingStyle);
				GUILayout.Label ("("+currentUser.Ranking+" pts)",yourRankingPointsStyle);
				GUILayout.FlexibleSpace();
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
		lastOpponentProfilePictureSize = (int)blockBottomLeftHeight * 85 / 100;

		this.lastResultsLabelStyle.fontSize = heightScreen * 2 / 100;
		this.lastResultsLabelStyle.fixedHeight = (int)heightScreen * 35 / 1000;

		this.lastOpponentLabelStyle.fontSize = heightScreen * 2 / 100;
		this.lastOpponentLabelStyle.fixedHeight = (int)heightScreen * 35 / 1000;

		this.rankingLabelStyle.fontSize = heightScreen * 2 / 100;
		this.rankingLabelStyle.fixedHeight = (int)heightScreen * 35 / 1000;

		this.yourRankingStyle.fontSize = heightScreen * 3 / 100;
		this.yourRankingStyle.fixedHeight = (int)heightScreen * 4 / 100;

		this.yourRankingPointsStyle.fontSize = heightScreen * 25 / 1000;
		this.yourRankingPointsStyle.fixedHeight = (int)heightScreen * 35 / 1000;

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

		this.lastOponnentInformationsLabelStyle.fontSize = (int)lastOpponentProfilePictureSize * 8 / 100;
		this.lastOponnentInformationsLabelStyle.fixedHeight = (int)lastOpponentProfilePictureSize*15/100;
		this.lastOponnentInformationsLabelStyle.fixedWidth = (int)lastOpponentProfilePictureSize*1.5f;

		this.lastOponnentUsernameLabelStyle.fontSize = (int)lastOpponentProfilePictureSize * 10 / 100;
		this.lastOponnentUsernameLabelStyle.fixedHeight = (int)lastOpponentProfilePictureSize*15/100;
		this.lastOponnentUsernameLabelStyle.fixedWidth = (int)lastOpponentProfilePictureSize*1.5f;

		this.lastOpponentProfilePictureButtonStyle.fixedWidth = (int)lastOpponentProfilePictureSize;
		this.lastOpponentProfilePictureButtonStyle.fixedHeight = (int)lastOpponentProfilePictureSize;

		this.paginationStyle.fontSize = (int)blockBottomRightHeight*3/100;
		this.paginationStyle.fixedWidth = (int)blockBottomRightWidth*10/100;
		this.paginationStyle.fixedHeight = (int)blockBottomRightHeight*4/100;
		this.paginationActivatedStyle.fontSize = (int)blockBottomRightHeight*3/100;
		this.paginationActivatedStyle.fixedWidth = (int)blockBottomRightWidth*10/100;
		this.paginationActivatedStyle.fixedHeight = (int)blockBottomRightHeight*4/100;

		this.mainLabelStyle.fontSize = (int)blockTopLeftHeight * 10 / 100;
		this.mainLabelStyle.fixedHeight = (int)blockTopLeftHeight * 15 / 100;

		this.subMainLabelStyle.fontSize = (int)blockTopLeftHeight * 7 / 100;
		this.subMainLabelStyle.fixedHeight = (int)blockTopLeftHeight * 15 / 100;

		if(gameType==1){
			this.divisionLabelStyle.fontSize= (int)blockTopLeftHeight * 4 / 100;
			this.divisionLabelStyle.fixedHeight = (int)blockTopLeftHeight * 5 / 100;
			
			this.divisionStrikeLabelStyle.fontSize= (int)blockTopLeftHeight * 4 / 100;
			this.divisionStrikeLabelStyle.fixedHeight = (int)blockTopLeftHeight * 5 / 100;
			
			this.remainingGamesStyle.fontSize= (int)blockTopLeftHeight * 4 / 100;
			this.remainingGamesStyle.fixedHeight = (int)blockTopLeftHeight * 5 / 100;
			drawGauge ();
		}
	}
	private void drawGauge(){
		gaugeWidth = blockTopLeftWidth;
		gaugeHeight = blockTopLeftHeight * 0.3f;

		this.gaugeBackgroundStyle.fixedWidth = gaugeWidth;
		this.gaugeBackgroundStyle.fixedHeight = gaugeHeight;
		
		this.startActiveGaugeBackgroundStyle.fixedWidth = startActiveGaugeWidth*gaugeWidth;
		this.startActiveGaugeBackgroundStyle.fixedHeight = gaugeHeight;
		
		this.activeGaugeBackgroundStyle.fixedWidth = activeGaugeWidth*gaugeWidth;
		this.activeGaugeBackgroundStyle.fixedHeight = gaugeHeight;
		
		this.relegationBarStyle.fixedWidth = relegationBarWidth*gaugeWidth;
		this.relegationBarStyle.fixedHeight = gaugeHeight;
		
		this.promotionBarStyle.fixedWidth = promotionBarWidth*gaugeWidth;
		this.promotionBarStyle.fixedHeight = gaugeHeight;
		
		this.titleBarStyle.fixedWidth = titleBarWidth*gaugeWidth;
		this.titleBarStyle.fixedHeight = gaugeHeight;
		
		this.gaugeSpace1Width=gaugeSpace1*gaugeWidth;
		this.gaugeSpace2Width=gaugeSpace2*gaugeWidth;
		this.gaugeSpace3Width=gaugeSpace3*gaugeWidth;
		this.gaugeSpace4Width=gaugeSpace4*gaugeWidth;
	}
	private void initializeGauge(){

		if(nbWinsDivision-hasWon>=currentUser.Division.NbWinsForPromotion)
		{
			if(nbWinsDivision==currentUser.Division.NbWinsForTitle)
			{
				titleBarFinish=0f;
			}
			float tempFloat = 1f-(startActiveGaugeWidth+gaugeSpace4);
			activeGaugeWidthStart=tempFloat*(((float)nbWinsDivision-(float)hasWon)/(float)currentUser.Division.NbWinsForTitle);
			activeGaugeWidth=activeGaugeWidthStart;
			activeGaugeWidthFinish=tempFloat*(((float)nbWinsDivision)/(float)currentUser.Division.NbWinsForTitle);

			gaugeSpace3Start=(1f-((float)nbWinsDivision-(float)hasWon)/(float)currentUser.Division.NbWinsForTitle)*tempFloat;
			gaugeSpace3=gaugeSpace3Start;
			gaugeSpace3Finish=(1f-((float)nbWinsDivision)/(float)currentUser.Division.NbWinsForTitle)*tempFloat;
		}
		else if(nbWinsDivision-hasWon>=currentUser.Division.NbWinsForRelegation)
		{
			promotionBarWidth=0.005f;
			gaugeSpace3=(1f-(gaugeSpace4+startActiveGaugeWidth+promotionBarWidth+titleBarWidth))*((float)currentUser.Division.NbWinsForTitle-(float)currentUser.Division.NbWinsForPromotion)/(float)currentUser.Division.NbWinsForTitle;
			if(nbWinsDivision==currentUser.Division.NbWinsForPromotion)
			{
				promotionBarFinish=0f;
			}
			else
			{	
				promotionBarFinish=promotionBarWidth;
			}
			float tempFloat = 1f-(startActiveGaugeWidth+gaugeSpace4+gaugeSpace3+titleBarWidth);
			activeGaugeWidthStart=tempFloat*(((float)nbWinsDivision-(float)hasWon)/(float)currentUser.Division.NbWinsForPromotion);
			activeGaugeWidth=activeGaugeWidthStart;
			activeGaugeWidthFinish=tempFloat*(((float)nbWinsDivision)/(float)currentUser.Division.NbWinsForPromotion);
			
			gaugeSpace2Start=(1f-((float)nbWinsDivision-(float)hasWon)/(float)currentUser.Division.NbWinsForPromotion)*tempFloat;
			gaugeSpace2=gaugeSpace2Start;
			gaugeSpace2Finish=(1f-((float)nbWinsDivision)/(float)currentUser.Division.NbWinsForPromotion)*tempFloat;
		}
		else
		{
			promotionBarWidth=0.005f;
			promotionBarFinish=promotionBarWidth;
			relegationBarWidth=0.005f;
			gaugeSpace3=(1f-(gaugeSpace4+startActiveGaugeWidth+promotionBarWidth+titleBarWidth))*((float)currentUser.Division.NbWinsForTitle-(float)currentUser.Division.NbWinsForPromotion)/(float)currentUser.Division.NbWinsForTitle;
			gaugeSpace2=(1f-(gaugeSpace4+gaugeSpace3+startActiveGaugeWidth+promotionBarWidth+titleBarWidth))*((float)currentUser.Division.NbWinsForPromotion-(float)currentUser.Division.NbWinsForRelegation)/(float)currentUser.Division.NbWinsForPromotion;
			if(nbWinsDivision==currentUser.Division.NbWinsForRelegation)
			{
				relegationBarFinish=0f;
			}
			else
			{
				relegationBarFinish=relegationBarWidth;
			}
			float tempFloat = 1f-(startActiveGaugeWidth+gaugeSpace4+gaugeSpace3+gaugeSpace2+titleBarWidth+promotionBarWidth);
			activeGaugeWidthStart=tempFloat*(((float)nbWinsDivision-(float)hasWon)/(float)currentUser.Division.NbWinsForRelegation);
			activeGaugeWidth=activeGaugeWidthStart;
			activeGaugeWidthFinish=tempFloat*(((float)nbWinsDivision)/(float)currentUser.Division.NbWinsForRelegation);
			
			gaugeSpace1Start=(1f-((float)nbWinsDivision-(float)hasWon)/(float)currentUser.Division.NbWinsForRelegation)*tempFloat;
			gaugeSpace1=gaugeSpace1Start;
			gaugeSpace1Finish=(1f-((float)nbWinsDivision)/(float)currentUser.Division.NbWinsForRelegation)*tempFloat;
		}
	}
	private void computeGauge(){

		if(transformRatio>=1f)
		{
			transformRatio=1f;
			toUpdateGauge=false;
		}
		if(activeGaugeWidthStart!=activeGaugeWidthFinish)
		{
			activeGaugeWidth=activeGaugeWidthStart+transformRatio*(activeGaugeWidthFinish-activeGaugeWidthStart);
		}
		if(gaugeSpace1Start!=gaugeSpace1Finish)
		{
			gaugeSpace1=gaugeSpace1Start+transformRatio*(gaugeSpace1Finish-gaugeSpace1Start);
		}
		if(gaugeSpace2Start!=gaugeSpace2Finish)
		{
			gaugeSpace2=gaugeSpace2Start+transformRatio*(gaugeSpace2Finish-gaugeSpace2Start);
		}
		if(gaugeSpace3Start!=gaugeSpace3Finish)
		{
			gaugeSpace3=gaugeSpace3Start+transformRatio*(gaugeSpace3Finish-gaugeSpace3Start);
		}
		if(relegationBarWidth!=relegationBarFinish && transformRatio==1f)
		{
			activeGaugeWidth=activeGaugeWidth+relegationBarWidth;
			relegationBarWidth=relegationBarFinish;
		}
		if(promotionBarWidth!=promotionBarFinish && transformRatio==1f)
		{
			activeGaugeWidth=activeGaugeWidth+promotionBarWidth;
			promotionBarWidth=promotionBarFinish;
		}
		if(titleBarWidth!=titleBarFinish && transformRatio==1f)
		{
			activeGaugeWidth=activeGaugeWidth+titleBarWidth;
			titleBarWidth=titleBarFinish;
		}
		drawGauge ();
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
				if (resultsHistory[i].HasWon)
				{
					nbWinsDivision++;
				}
				else
				{
					nbLoosesDivision++;
				}
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
			remainingGames=currentUser.Division.NbGames-nbWinsDivision-nbLoosesDivision;
			hasWon = System.Convert.ToInt32(currentUser.ResultsHistory[0].HasWon);
			initializeGauge();
			setStyles();
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
			setStyles();
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
			setStyles();
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
		lastOpponentProfilePictureButtonStyle.normal.background = profilePictures [0];
		nbPages = Mathf.CeilToInt(currentUser.ResultsHistory.Count/5f);
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
		toUpdateGauge = true;
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
