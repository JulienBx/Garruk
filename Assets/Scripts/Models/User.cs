using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class User
{
	private string URLGetUserGameProfile    = ApplicationModel.host+"get_user_game_profile.php";
	private string ServerDirectory          = "img/profile/";
	private string URLDefaultProfilePicture = ApplicationModel.host+"img/profile/defautprofilepicture.png";
	private string URLGetDecks              = ApplicationModel.host + "get_decks_by_user.php";

	public string Username;
	public string Mail;
	public string FirstName;
	public string Surname;

	public string Picture;
	public int Money;
	public List<Connection> Connections;
	public int Division;
	public int Cup;
	public int Id;
	public int NbGamesCup;
	public int Ranking;
	public int RankingPoints;
	public int TotalNbWins;
	public int TotalNbLooses;
	public Texture2D texture;
	public int NbGamesDivision;

	public User()
	{
		this.Username = "";
		this.Picture = "";
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public User(int id)
	{
		this.Id = id;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public User(string username)
	{
		this.Username = username;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}

	public User(string username, string picture)
	{
		this.Username = username;
		this.Picture = picture;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}

	public User(string username, string mail, int money, string firstname, string surname, string picture)
	{
		this.Username = username;
		this.Mail = mail;
		this.Money = money;
		this.FirstName = firstname;
		this.Surname = surname;
		this.Picture = picture;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}

	public User(List<Connection> connections)
	{
		this.Connections = connections;
	}
	public User(int id, 
	            string username, 
	            string mail, 
	            int money, 
	            string firstname, 
	            string surname, 
	            string picture, 
	            int division, 
	            int rankingpoints, 
	            int ranking,  
	            int totalnbwins, 
	            int totalnblooses)
	{
		this.Id = id;
		this.Username = username;
		this.Mail = mail;
		this.Money = money;
		this.FirstName = firstname;
		this.Surname = surname;
		this.Picture = picture;
		this.Division = division;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public User(int id, 
	            string username, 
	            string picture, 
	            int division, 
	            int rankingpoints, 
	            int ranking,  
	            int totalnbwins, 
	            int totalnblooses)
	{
		this.Id = id;
		this.Username = username;
		this.Picture = picture;
		this.Division = division;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public User(int id, int money, int division, int nbgamesdivision, int cup, int nbgamescup, int rankingpoints,int ranking,  int totalnbwins, int totalnblooses)
	{
		this.Id = id;
		this.Money = money;
		this.Division = division;
		this.NbGamesDivision = nbgamesdivision;
		this.Cup = cup;
		this.NbGamesCup = nbgamescup;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public User(string username, string picture, int division, int rankingpoints,int ranking,  int totalnbwins, int totalnblooses)
	{
		this.Username = username;
		this.Picture = picture;
		this.Division = division;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
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
			//Debug.Log(w.text); 											// donne le retour
			this.Picture=w.text;
		}
	}

	public IEnumerator setProfilePicture(){
		
		if (this.Picture.StartsWith(ServerDirectory)){
			var www = new WWW(ApplicationModel.host+this.Picture);
			yield return www;
			www.LoadImageIntoTexture(this.texture);
		}
		else {
			var www = new WWW(URLDefaultProfilePicture);
			yield return www;
			www.LoadImageIntoTexture(this.texture);
		}
	}

	public IEnumerator getDecks(Action<string> callback)
	{	
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username); 	// Pseudo de l'utilisateur connecté	
		WWW w = new WWW(URLGetDecks, form); 						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null)
		{
			Debug.Log("erreur getDeck : " + w.error);
		}
		else{
			callback(w.text);
		}
	}
}



