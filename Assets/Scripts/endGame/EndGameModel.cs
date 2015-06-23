using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGameModel
{
	public IList<PlayerResult> lastResults;
	public User currentUser;
	public Division currentDivision;
	public Cup currentCup;
	public int competitionState;
	public int nbWinsDivision;
	public int nbLoosesDivision;
	
	private string URLGetEndGameData="http://54.77.118.214/GarrukServer/get_end_game_data.php";
	
	public IEnumerator getEndGameData()
	{
		this.currentUser = new User ();

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 
		
		WWW w = new WWW(URLGetEndGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 
		// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.lastResults=new List<PlayerResult>();
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] userData=data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] resultsData=data[1].Split(new string[] { "RESULT" }, System.StringSplitOptions.None);
			string[] gameTypeData=data[2].Split(new string[] { "//" }, System.StringSplitOptions.None);

			currentUser=new User();
			this.currentUser.Id=System.Convert.ToInt32(userData[0]);
			this.currentUser.Division=System.Convert.ToInt32(userData[1]);
			this.currentUser.NbGamesDivision=System.Convert.ToInt32(userData[2]);
			this.currentUser.Cup=System.Convert.ToInt32(userData[3]);
			this.currentUser.NbGamesCup=System.Convert.ToInt32(userData[4]);
			this.currentUser.RankingPoints=System.Convert.ToInt32(userData[5]);
			this.currentUser.Ranking=System.Convert.ToInt32(userData[6]);
			this.currentUser.TotalNbWins=System.Convert.ToInt32(userData[7]);
			this.currentUser.TotalNbLooses=System.Convert.ToInt32(userData[8]);
			this.currentUser.TutorialStep=System.Convert.ToInt32(userData[9]);
			
			for (int i =0;i<resultsData.Length-1;i++)
			{
				string[] resultData=resultsData[i].Split (new string[] {"//"}, System.StringSplitOptions.None);
				this.lastResults.Add (new PlayerResult(System.Convert.ToBoolean(System.Convert.ToInt32(resultData[0])),
				                                  System.DateTime.ParseExact(resultData[1], "yyyy-MM-dd HH:mm:ss", null),
				                                  System.Convert.ToInt32(resultData[2]),
				                                  new User(resultData[3],
				         resultData[4],
				         System.Convert.ToInt32(resultData[5]),
				         System.Convert.ToInt32(resultData[6]),
				         System.Convert.ToInt32(resultData[7]),
				         System.Convert.ToInt32(resultData[8]),
				         System.Convert.ToInt32(resultData[9])))); //total Nblooses
			}
			if(this.lastResults[0].GameType==1)
			{
				this.currentDivision=new Division();
				this.currentDivision.Id=System.Convert.ToInt32(gameTypeData[0]);
				this.currentDivision.NbGames=System.Convert.ToInt32(gameTypeData[1]);
				this.currentDivision.NbWinsForRelegation=System.Convert.ToInt32(gameTypeData[2]);
				this.currentDivision.NbWinsForPromotion=System.Convert.ToInt32(gameTypeData[3]);
				this.currentDivision.NbWinsForTitle=System.Convert.ToInt32(gameTypeData[4]);
				this.currentDivision.TitlePrize=System.Convert.ToInt32(gameTypeData[5]);
				this.currentDivision.PromotionPrize=System.Convert.ToInt32(gameTypeData[6]);
				this.currentDivision.Name=gameTypeData[7];
				this.currentDivision.Picture=gameTypeData[8];
				this.nbWinsDivision=System.Convert.ToInt32(gameTypeData[9]);
				this.nbLoosesDivision=System.Convert.ToInt32(gameTypeData[10]);
				this.competitionState=System.Convert.ToInt32(gameTypeData[11]);
			}
			else if(this.lastResults[0].GameType==2)
			{
				this.currentCup = new Cup();
				this.currentCup.Id=System.Convert.ToInt32(gameTypeData[0]);
				this.currentCup.NbRounds=System.Convert.ToInt32(gameTypeData[1]);
				this.currentCup.CupPrize=System.Convert.ToInt32(gameTypeData[2]);
				this.currentCup.Name=gameTypeData[3];
				this.currentCup.Picture=gameTypeData[4];
				this.competitionState=System.Convert.ToInt32(gameTypeData[5]);
			}
		}
	}
}