using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class EndGameModel
{
	public IList<PlayerResult> lastResults;
	public User currentUser=new User();
	public Division currentDivision;
	public Cup currentCup;
	public Trophy trophyWon=new Trophy(0,0,0);
	
	private string URLGetEndGameData="http://54.77.118.214/GarrukServer/get_end_game_data.php";
	private string URLUpdateUserResults="http://54.77.118.214/GarrukServer/update_user_results.php";
	private int nbLimitFriendlyGameToDisplay=30;

	public IEnumerator getEndGameData()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 
		form.AddField("myform_limit", nbLimitFriendlyGameToDisplay.ToString()); // Pseudo de l'utilisateur connecté
		
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
			string[] gameTypeData=data[1].Split(new string[] { "//" }, System.StringSplitOptions.None);
			string[] resultsData=data[2].Split(new string[] { "RESULT" }, System.StringSplitOptions.None);
			
			currentUser = new User(System.Convert.ToInt32(userData[0]), //id
			                       System.Convert.ToInt32(userData[1]), //money
			                       System.Convert.ToInt32(userData[2]), //current division
			                       System.Convert.ToInt32(userData[3]), //nbgames division
			                       System.Convert.ToInt32(userData[4]), //current cup
			                       System.Convert.ToInt32(userData[5]), //nbgames cup
			                       System.Convert.ToInt32(userData[6]), //ranking Points
			                       System.Convert.ToInt32(userData[7]), //ranking
			                       System.Convert.ToInt32(userData[8]),  //totalNbWins
			                       System.Convert.ToInt32(userData[9])); //totalNbLooses
			
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
			}
			else if(this.lastResults[0].GameType==2)
			{
				this.currentCup = new Cup();
				this.currentCup.Id=System.Convert.ToInt32(gameTypeData[0]);
				this.currentCup.NbRounds=System.Convert.ToInt32(gameTypeData[1]);
				this.currentCup.CupPrize=System.Convert.ToInt32(gameTypeData[2]);
				this.currentCup.Name=gameTypeData[3];
				this.currentCup.Picture=gameTypeData[4];
			}
		}
	}
	public IEnumerator updateUserResults()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_userid", this.currentUser.Id); 
		form.AddField("myform_money", this.currentUser.Money);
		form.AddField("myform_currentdivision", this.currentUser.Division);
		form.AddField("myform_currentcup", this.currentUser.Cup);
		form.AddField("myform_nbgamesdivision", this.currentUser.NbGamesDivision);
		form.AddField("myform_nbgamescup", this.currentUser.NbGamesCup);
		form.AddField("myform_trophytype", this.trophyWon.TrophyType);
		form.AddField("myform_trophynumber", this.trophyWon.TrophyNumber);
		
		
		WWW w = new WWW(URLUpdateUserResults, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 													// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
	}

}