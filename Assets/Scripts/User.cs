using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : MonoBehaviour
{
	private string URLGetUserGameProfile = "http://54.77.118.214/GarrukServer/get_user_game_profile.php";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";

	public string Username;
	public string Mail;
	public string FirstName;
	public string Surname;
	public string Picture;
	public int Money;
	public List<Connection> Connections;
	public Division Division;
	public Cup Cup;
	public int NbGamesCup;
	public int Ranking;
	public int RankingPoints;
	public int TotalNbWins;
	public int TotalNbLooses;
	public IList<Result> ResultsHistory;
	public Texture2D texture ; 

	public User()
	{
		this.Username = "";
		this.Picture = "";
	}

	public User(string username)
	{
		this.Username = username;
	}

	public IEnumerator setPicture(){
		yield return StartCoroutine(retrievePicture());
	}

	public User(string username, string picture)
	{
		this.Username = username;
		this.Picture = picture;
	}

	public User(string username, string mail, int money, string firstname, string surname, string picture)
	{
		this.Username = username;
		this.Mail = mail;
		this.Money = money;
		this.FirstName = firstname;
		this.Surname = surname;
		this.Picture = picture;
	}

	public User(List<Connection> connections)
	{
		this.Connections = connections;
	}

	public User(Division division, IList<Result> resultshistory,  int rankingpoints,int ranking, int totalnbwins, int totalnblooses)
	{
		this.Division = division;
		this.ResultsHistory = resultshistory;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}

	public User(Cup cup, IList<Result> resultshistory,  int rankingpoints,int ranking, int totalnbwins, int totalnblooses)
	{
		this.Cup = cup;
		this.ResultsHistory = resultshistory;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}

	public User(IList<Result> resultshistory,  int rankingpoints,int ranking, int totalnbwins, int totalnblooses)
	{
		this.ResultsHistory = resultshistory;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}

	public User(string username, string picture, Division division, int rankingpoints,int ranking,  int totalnbwins, int totalnblooses)
	{
		this.Username = username;
		this.Picture = picture;
		this.Division = division;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}

	public IEnumerator retrievePicture(){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username);
			
		WWW w = new WWW(URLGetUserGameProfile, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null){ 
			Debug.Log (w.error); 
		}
		else{
			//print(w.text); 											// donne le retour
			this.Picture=w.text;
		}		
	}

	private IEnumerator setProfilePicture(){
		
		this.texture = new Texture2D (4, 4, TextureFormat.DXT1, false);
		
		if (this.Picture.StartsWith("http")){
			var www = new WWW(this.Picture);
			yield return www;
			www.LoadImageIntoTexture(this.texture);
		}
		else {
			var www = new WWW(URLDefaultProfilePicture);
			yield return www;
			www.LoadImageIntoTexture(this.texture);
		}
	}
}



