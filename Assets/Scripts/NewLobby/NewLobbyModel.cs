using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewLobbyModel 
{
	
	private string URLGetLobbyData = ApplicationModel.host+"get_lobby_data.php";

	public IList<PlayerResult> lastResults;
	public Cup currentCup;
	public Division currentDivision;
	public User player;
	
	public NewLobbyModel()
	{
	}
	public IEnumerator getLobbyData(bool isDivisionLobby, bool isEndGameLobby)
	{

		this.lastResults = new List<PlayerResult> ();

		int isDivisionLobbyInt = 0;
		if(isDivisionLobby)
		{
			isDivisionLobbyInt = 1;
		}
	
		int isEndGameLobbyInt = 0;
		if(isEndGameLobby)
		{
			isEndGameLobbyInt = 1;
		}

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_isdivisionlobby", isDivisionLobbyInt.ToString());
		form.AddField("myform_isendgamelobby", isEndGameLobbyInt.ToString());
		
		WWW w = new WWW(URLGetLobbyData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player = parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));

			if(isDivisionLobby)
			{
				this.currentDivision=parseDivision(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
				ApplicationModel.currentDivision=this.currentDivision;
			}
			else
			{
				this.currentCup=parseCup(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
				ApplicationModel.currentCup=this.currentCup;
			}
			this.lastResults=parseResults(data[2].Split(new string[] {"RESULT"},System.StringSplitOptions.None));
		}
	}

	private Cup parseCup(string[] array)
	{
		Cup cup = new Cup ();
		cup.GamesPlayed= System.Convert.ToInt32(array [0]);
		cup.NbWins= System.Convert.ToInt32(array [1]);
		cup.NbLooses= System.Convert.ToInt32(array [2]);
		cup.Status= System.Convert.ToInt32(array [3]);
		cup.Name= array[4];
		cup.Picture= array[5];
		cup.CupPrize = System.Convert.ToInt32(array [6]);
		cup.NbRounds = System.Convert.ToInt32(array [7]);
		cup.EarnXp_W= System.Convert.ToInt32(array [8]);
		cup.EarnXp_L= System.Convert.ToInt32(array [9]);
		cup.EarnCredits_W= System.Convert.ToInt32(array [10]);
		cup.EarnCredits_L= System.Convert.ToInt32(array [11]);
		return cup;
	}
	private Division parseDivision(string[] array)
	{
		Division division = new Division ();
		division.GamesPlayed= System.Convert.ToInt32(array [0]);
		division.NbWins= System.Convert.ToInt32(array [1]);
		division.NbLooses= System.Convert.ToInt32(array [2]);
		division.Status= System.Convert.ToInt32(array [3]);
		division.Name= array[4];
		division.Picture= array[5];
		division.TitlePrize = System.Convert.ToInt32(array [6]);
		division.PromotionPrize = System.Convert.ToInt32(array [7]);
		division.NbWinsForRelegation = System.Convert.ToInt32(array [8]);
		division.NbWinsForPromotion = System.Convert.ToInt32(array [9]);
		division.NbWinsForTitle = System.Convert.ToInt32(array [10]);
		division.NbGames = System.Convert.ToInt32(array [11]);
		division.EarnXp_W= System.Convert.ToInt32(array [12]);
		division.EarnXp_L= System.Convert.ToInt32(array [13]);
		division.EarnCredits_W= System.Convert.ToInt32(array [14]);
		division.EarnCredits_L= System.Convert.ToInt32(array [15]);
		return division;
	}
	private User parseUser(string[] array)
	{
		User player = new User ();
		player.Ranking = System.Convert.ToInt32 (array [0]);
		player.RankingPoints = System.Convert.ToInt32 (array [1]);
		player.CollectionPoints = System.Convert.ToInt32 (array [2]);
		player.CollectionRanking = System.Convert.ToInt32 (array [3]);
		player.TotalNbWins = System.Convert.ToInt32 (array [4]);
		player.TotalNbLooses = System.Convert.ToInt32 (array [5]);
		return player;
	}
	public IList<PlayerResult> parseResults(string[] array)
	{
		IList<PlayerResult> results = new List<PlayerResult> ();
		
		for (int i=0;i<array.Length-1;i++)
		{
			
			string[] resultData=array[i].Split (new string[] {"//"}, System.StringSplitOptions.None);
			results.Add(new PlayerResult());
			results[i].HasWon=System.Convert.ToBoolean(System.Convert.ToInt32(resultData[0]));
			results[i].Date = System.DateTime.ParseExact(resultData[1], "yyyy-MM-dd HH:mm:ss", null);
			results[i].Opponent=new User();
			results[i].Opponent.Username=resultData[2];
			results[i].Opponent.idProfilePicture=System.Convert.ToInt32(resultData[3]);
			results[i].Opponent.Ranking=System.Convert.ToInt32(resultData[4]);
			results[i].Opponent.CollectionRanking=System.Convert.ToInt32(resultData[5]);
			results[i].Opponent.TotalNbWins=System.Convert.ToInt32(resultData[6]);
			results[i].Opponent.TotalNbLooses=System.Convert.ToInt32(resultData[7]);

		}
		
		return results;
	}
	
}



