using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class User
{
	private string URLGetUserGameProfile    = ApplicationModel.host + "get_user_game_profile.php";
	private string URLDefaultProfilePicture = ApplicationModel.host + "img/profile/defautprofilepicture.png";
	private string URLGetDecks              = ApplicationModel.host + "get_decks_by_user.php";
	private string URLGetMyCardsPage        = ApplicationModel.host + "get_mycardspage_data.php";
	private string URLSellCard              = ApplicationModel.host + "sellRandomCard.php";
	private string URLPutOnMarket           = ApplicationModel.host + "putonmarket.php";
	private string URLRemoveFromMarket      = ApplicationModel.host + "removeFromMarket.php";
	private string URLChangeMarketPrice     = ApplicationModel.host + "changeMarketPrice.php";
	private string URLRenameCard            = ApplicationModel.host + "renameCard.php";
	private string URLUpdateUserInformations= ApplicationModel.host + "update_user_informations.php";
	private string URLUpdateProfilePicture  = ApplicationModel.host + "update_profilepicture.php";

	private string ServerDirectory          = "img/profile/";
	
	public string Error = "";

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

	public IEnumerator updateInformations(){

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_id",this.Id);
		form.AddField("myform_firstname", this.FirstName); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_surname", this.Surname);
		form.AddField("myform_mail", this.Mail);
		
		WWW w = new WWW(URLUpdateUserInformations, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null){ 
			Debug.Log (w.error); 
		}
	}
	public IEnumerator updateProfilePicture(File file)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddBinaryData("myform_file",file.LocalFile.bytes, ApplicationModel.username+file.FileExtension);
		
		WWW w = new WWW(URLUpdateProfilePicture, form); 				 // On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			if(w.text==""){
				this.Picture=this.ServerDirectory + this.Username + file.FileExtension;
			}
			else	
			{
				this.Error =w.text;
				yield break;
			}
		}
	}
	public IEnumerator retrievePicture(){
		
		WWWForm form = new WWWForm(); 											 //Création de la connexion
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

	public IEnumerator getCards(Action<string> callback)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username);		
		WWW w = new WWW(URLGetMyCardsPage, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null) 
		{
			Debug.Log("erreur getCards : " + w.error);
		} 
		else 
		{
			callback(w.text);
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
			Debug.Log("erreur getDecks : " + w.error);
		}
		else
		{
			callback(w.text);
		}
	}

	public IEnumerator sellCard(int idCard, int cost)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username);
		form.AddField("myform_idcard", idCard);
		form.AddField("myform_cost", cost);		
		WWW w = new WWW(URLSellCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			Debug.Log("erreur sellCard : " + w.error);
		}
	}

	public IEnumerator toSell(int cardId, int price)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username);
		form.AddField("myform_idcard", cardId);
		form.AddField("myform_price", price);
		form.AddField("myform_date",  System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString());	
		WWW w = new WWW(URLPutOnMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			Debug.Log("erreur toSell : " + w.error);
		}
	}

	public IEnumerator notToSell(int cardId)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username);
		form.AddField("myform_idcard", cardId);
		WWW w = new WWW(URLRemoveFromMarket, form);             				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			Debug.Log("erreur notTosell : " + w.error);
		}
	}

	public IEnumerator changePriceCard(int cardId, int price)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username);
		form.AddField("myform_idcard", cardId);
		form.AddField("myform_price", price);
		WWW w = new WWW(URLChangeMarketPrice, form); 				            // On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			Debug.Log("erreur changePriceCard : " + w.error);
		}
	}

	public IEnumerator renameCard(int idCard, string newName, int renameCost)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", Username);
		form.AddField("myform_idcard", idCard);
		form.AddField("myform_title", newName);
		form.AddField("myform_cost", renameCost);
		
		WWW w = new WWW(URLRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			Debug.Log("erreur renameCard : " + w.error);
		}
	}
}



