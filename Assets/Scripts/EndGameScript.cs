using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameScript : MonoBehaviour {


	public int gameType;
	public bool hasWon;
	public GameObject MenuObject;

	private string URLGetUDivisionEndGameData="http://54.77.118.214/GarrukServer/get_division_end_game_data.php";
	private string URLGetCupEndGameData="http://54.77.118.214/GarrukServer/get_cup_end_game_data.php";
	private string URLGetFriendlyEndGameData="http://54.77.118.214/GarrukServer/get_friendly_end_game_data.php";
	
	private User currentUser;
	private bool isDataLoaded=false;

	// Use this for initialization
	void Start () {
	
		MenuObject = Instantiate(MenuObject) as GameObject;
		switch(gameType)
		{
		case 0:
			StartCoroutine (getFriendlyEndGameData ());
			break;
		case 1:
			StartCoroutine (getDivisionEndGameData ());
			break;
		case 2:
			StartCoroutine (getCupEndGameData ());
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
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
			isDataLoaded=true;
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
			isDataLoaded=true;
		}
	}
	private IEnumerator getFriendlyEndGameData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
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
			isDataLoaded=true;
		}
	}
}
