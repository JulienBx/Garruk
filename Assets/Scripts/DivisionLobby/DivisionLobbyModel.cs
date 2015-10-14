//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//public class DivisionLobbyModel
//{
//	private string URLGetDivisionLobbyData = ApplicationModel.host + "get_divisionlobby_data.php";
//	
//	public IList<PlayerResult> results;
//	public Division currentDivision;
//	public User player;
//
//	public DivisionLobbyModel ()
//	{
//		this.results = new List<PlayerResult> ();
//		this.currentDivision = new Division ();
//		this.player = new User ();
//	}
//	public IEnumerator getDivisionLobbyData()
//	{
//		WWWForm form = new WWWForm(); 											// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_nick", ApplicationModel.username);
//		
//		WWW w = new WWW(URLGetDivisionLobbyData, form); 				// On envoie le formulaire à l'url sur le serveur 
//		yield return w;
//		if (w.error != null) 
//		{
//			Debug.Log (w.error); 										// donne l'erreur eventuelle
//		} 
//		else 
//		{
//			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
//			this.results = this.parseResults(data[0].Split(new string[] { "#RESULT#" }, System.StringSplitOptions.None));
//			this.currentDivision = this.parseDivision(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
//			this.player.DivisionLobbyTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(data[2]));
//			ApplicationModel.currentDivision=this.currentDivision;
//		}
//	}
//	private List<PlayerResult> parseResults(string[] resultsData)
//	{
//		List<PlayerResult> results = new List<PlayerResult> ();
//		for(int i=0;i<resultsData.Length-1;i++)
//		{
//			string[] resultInformation = resultsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
//			results.Add (new PlayerResult());
//			results[i].HasWon=System.Convert.ToBoolean(System.Convert.ToInt32(resultInformation[0]));
//			results[i].Date=System.DateTime.ParseExact(resultInformation[1], "yyyy-MM-dd HH:mm:ss", null);
//			results[i].Opponent=new User();
//			results[i].Opponent.Username=resultInformation[2];
//			results[i].Opponent.Picture=resultInformation[3];
//			results[i].Opponent.Division=System.Convert.ToInt32(resultInformation[4]);
//			results[i].Opponent.RankingPoints=System.Convert.ToInt32(resultInformation[5]);
//			results[i].Opponent.Ranking=System.Convert.ToInt32(resultInformation[6]);
//			results[i].Opponent.TotalNbWins=System.Convert.ToInt32(resultInformation[7]);
//			results[i].Opponent.TotalNbLooses=System.Convert.ToInt32(resultInformation[8]);
//		}
//		return results;
//	}
//	private Division parseDivision(string[] divisionData)
//	{
//		Division division = new Division ();
//		division.Name = divisionData [0];
//		division.Picture = divisionData [1];
//		division.NbGames = System.Convert.ToInt32 (divisionData [2]);
//		division.NbWinsForRelegation = System.Convert.ToInt32 (divisionData [3]);
//		division.NbWinsForPromotion = System.Convert.ToInt32 (divisionData [4]);
//		division.NbWinsForTitle = System.Convert.ToInt32 (divisionData [5]);
//		division.TitlePrize = System.Convert.ToInt32 (divisionData [6]);
//		division.PromotionPrize = System.Convert.ToInt32 (divisionData [7]);
//		division.EarnXp_W= System.Convert.ToInt32 (divisionData [8]);
//		division.EarnXp_L= System.Convert.ToInt32 (divisionData [9]);
//		division.EarnCredits_W= System.Convert.ToInt32 (divisionData [10]);
//		division.EarnCredits_L= System.Convert.ToInt32 (divisionData [11]);
//		return division;
//	}
//}