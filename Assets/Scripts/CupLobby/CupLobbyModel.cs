using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
public class CupLobbyModel
{
	private string URLGetCupLobbyData = ApplicationModel.host + "get_cuplobby_data.php";
	
	public IList<PlayerResult> results;
	public Cup currentCup;
	public User player;

	public CupLobbyModel ()
	{
		this.results = new List<PlayerResult> ();
		this.currentCup = new Cup ();
		this.player = new User ();
	}
	public IEnumerator getCupLobbyData()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetCupLobbyData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.results = this.parseResults(data[0].Split(new string[] { "#RESULT#" }, System.StringSplitOptions.None));
			this.currentCup = this.parseCup(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.player.CupLobbyTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(data[2]));
			ApplicationModel.currentCup=this.currentCup;
		}
	}
	private List<PlayerResult> parseResults(string[] resultsData)
	{
		List<PlayerResult> results = new List<PlayerResult> ();
		for(int i=0;i<resultsData.Length-1;i++)
		{
			string[] resultInformation = resultsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			results.Add (new PlayerResult());
			results[i].HasWon=System.Convert.ToBoolean(System.Convert.ToInt32(resultInformation[0]));
			results[i].Date=System.DateTime.ParseExact(resultInformation[1], "yyyy-MM-dd HH:mm:ss", null);
			results[i].Opponent=new User();
			results[i].Opponent.Username=resultInformation[2];
			results[i].Opponent.Picture=resultInformation[3];
			results[i].Opponent.Division=System.Convert.ToInt32(resultInformation[4]);
			results[i].Opponent.RankingPoints=System.Convert.ToInt32(resultInformation[5]);
			results[i].Opponent.Ranking=System.Convert.ToInt32(resultInformation[6]);
			results[i].Opponent.TotalNbWins=System.Convert.ToInt32(resultInformation[7]);
			results[i].Opponent.TotalNbLooses=System.Convert.ToInt32(resultInformation[8]);
		}
		return results;
	}
	private Cup parseCup(string[] cupData)
	{
		Cup cup = new Cup ();
		cup.Name = cupData [0];
		cup.Picture = cupData [1];
		cup.NbRounds = System.Convert.ToInt32 (cupData [2]);
		cup.CupPrize = System.Convert.ToInt32 (cupData [3]);
		cup.EarnXp_W= System.Convert.ToInt32 (cupData [4]);
		cup.EarnXp_L= System.Convert.ToInt32 (cupData [5]);
		cup.EarnCredits_W= System.Convert.ToInt32 (cupData [6]);
		cup.EarnCredits_L= System.Convert.ToInt32 (cupData [7]);
		return cup;
	}
}