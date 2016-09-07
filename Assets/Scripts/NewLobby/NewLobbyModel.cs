using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewLobbyModel 
{
	
	private string URLGetLobbyData = ApplicationModel.host+"lobby_getUserData.php";

	public IList<PlayerResult> lastResults;
	
	public NewLobbyModel()
	{
	}
	public IEnumerator getLobbyData(bool isEndGameLobby)
	{
		this.lastResults = new List<PlayerResult> ();
	
		int isEndGameLobbyInt = 0;
		if(isEndGameLobby)
		{
			isEndGameLobbyInt = 1;
		}

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_isendgamelobby", isEndGameLobbyInt.ToString());

		ServerController.instance.setRequest(URLGetLobbyData, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");

		if(ServerController.instance.getError()=="")
		{
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			ApplicationModel.player.MyDivision=parseDivision(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.lastResults=parseResults(data[2].Split(new string[] {"RESULT"},System.StringSplitOptions.None));
		}
		else
		{
			Debug.Log(ServerController.instance.getError());
			BackOfficeController.instance.displayDetectOfflinePopUp ();
		}
	}
	private Division parseDivision(string[] array)
	{
		Division division = new Division ();
		division.GamesPlayed= System.Convert.ToInt32(array [0]);
		division.NbWins= System.Convert.ToInt32(array [1]);
		division.NbLooses= System.Convert.ToInt32(array [2]);
		division.Status= System.Convert.ToInt32(array [3]);
		division.Id=System.Convert.ToInt32(array[4]);
		//division.IdPicture= System.Convert.ToInt32(array[5]);
		division.TitlePrize = System.Convert.ToInt32(array [6]);
		division.PromotionPrize = System.Convert.ToInt32(array [7]);
		division.NbWinsForRelegation = System.Convert.ToInt32(array [8]);
		division.NbWinsForPromotion = System.Convert.ToInt32(array [9]);
		division.NbWinsForTitle = System.Convert.ToInt32(array [10]);
		division.NbGames = System.Convert.ToInt32(array [11]);
		return division;
	}
	private void parseUser(string[] array)
	{
		ApplicationModel.player.Ranking = System.Convert.ToInt32 (array [0]);
		ApplicationModel.player.RankingPoints = System.Convert.ToInt32 (array [1]);
		ApplicationModel.player.CollectionRanking = System.Convert.ToInt32 (array [2]);
		ApplicationModel.player.CollectionPoints = System.Convert.ToInt32 (array [3]);
		ApplicationModel.player.TotalNbWins = System.Convert.ToInt32 (array [4]);
		ApplicationModel.player.TotalNbLooses = System.Convert.ToInt32 (array [5]);
		//ApplicationModel.player.SelectedDeckId=System.Convert.ToInt32 (array [6]);
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
			results[i].Opponent.IdProfilePicture=System.Convert.ToInt32(resultData[3]);
			results[i].Opponent.Ranking=System.Convert.ToInt32(resultData[4]);
			results[i].Opponent.CollectionRanking=System.Convert.ToInt32(resultData[5]);
			results[i].Opponent.TotalNbWins=System.Convert.ToInt32(resultData[6]);
			results[i].Opponent.TotalNbLooses=System.Convert.ToInt32(resultData[7]);
			results[i].Opponent.Division=ApplicationModel.player.MyDivision.Id;
			results[i].Opponent.TrainingStatus=System.Convert.ToInt32(resultData[9]);
			results[i].Opponent.isPublic=System.Convert.ToBoolean(System.Convert.ToInt32(resultData[10]));
		}
		return results;
	}
	
}



