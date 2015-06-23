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
	private string URLUpdateUserInformations= ApplicationModel.host + "update_user_informations.php";
	private string URLUpdateProfilePicture  = ApplicationModel.host + "update_profilepicture.php";
	private string URLGetNonReadNotifications = ApplicationModel.host +"get_non_read_notifications_by_user.php";
	private string URLGetMoney = ApplicationModel.host + "get_money_by_user.php";
	private string URLAddMoney = ApplicationModel.host + "add_money.php";
	private string URLSelectedDeck = ApplicationModel.host + "set_selected_deck.php";
	private string URLCleanCards = ApplicationModel.host + "clean_cards.php";
	private string URLSetTutorialStep = ApplicationModel.host + "set_tutorialstep.php";
	private string URLSetMarketTutorial = ApplicationModel.host + "set_marketTutorial.php";
	private string URLSetMyProfileTutorial = ApplicationModel.host + "set_myProfileTutorial.php";
	private string URLSetProfileTutorial = ApplicationModel.host + "set_profileTutorial.php";
	private string URLSetSkillBookTutorial = ApplicationModel.host + "set_skillBookTutorial.php";
	private string URLSetStoreTutorial = ApplicationModel.host + "set_storeTutorial.php";
	private string URLSetDivisionLobbyTutorial = ApplicationModel.host + "set_divisionLobbyTutorial.php";
	private string URLSetCupLobbyTutorial = ApplicationModel.host + "set_cupLobbyTutorial.php";
	private string URLUpdateEndGameData = ApplicationModel.host + "get_end_game_data.php";

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
	public bool readnotificationsystem;
	public int nonReadNotifications;
	public int SelectedDeckId;
	public List<int> CardTypesAllowed;
	public bool IsAdmin;
	public int CollectionPoints;
	public int CollectionRanking;
	public int TutorialStep;
	public bool MarketTutorial;
	public bool MyProfileTutorial;
	public bool ProfileTutorial;
	public bool SkillBookTutorial;
	public bool StoreTutorial;
	public bool DivisionLobbyTutorial;
	public bool CupLobbyTutorial;
	public int ConnectionBonus;

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
	public IEnumerator countNonReadsNotifications(int totalNbResultLimit)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLGetNonReadNotifications, form); 				
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}
		else
		{
			this.nonReadNotifications=System.Convert.ToInt32(w.text);
		}
	}
	public IEnumerator addMoney(int money)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_money", money.ToString());
		
		WWW w = new WWW(URLAddMoney, form); 				
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}
		else
		{
			this.Money=System.Convert.ToInt32(w.text);
		}
	}
	public IEnumerator getMoney()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetMoney, form); 				
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}
		else
		{
			this.Money=System.Convert.ToInt32(w.text);
		}
	}
	public IEnumerator SetSelectedDeck(int selectedDeckId)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", selectedDeckId.ToString());                 // Deck sélectionné
		
		WWW w = new WWW (URLSelectedDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else {
		}
	}
	public IEnumerator cleanCards()
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté          
		
		WWW w = new WWW (URLCleanCards, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			if(w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				Debug.Log (errors[1]);
			}
			else
			{
				string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				this.CollectionPoints=System.Convert.ToInt32(data[1]);
				this.CollectionRanking=System.Convert.ToInt32(data[2]);
			}
		}
	}
	public IEnumerator setTutorialStep(int step)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", step.ToString());                 // Deck sélectionné
		
		WWW w = new WWW (URLSetTutorialStep, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			if(w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				Debug.Log (errors[1]);
			}
			else
			{
				this.TutorialStep=step;
			}
		}
	}
	public IEnumerator setMarketTutorial(bool step)
	{
		string tempString;
		if(step)
		{
			tempString="1";
		}
		else
		{
			tempString="0";
		}
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetMarketTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.MarketTutorial=step;
		}
	}
	public IEnumerator setMyProfileTutorial(bool step)
	{
		string tempString;
		if(step)
		{
			tempString="1";
		}
		else
		{
			tempString="0";
		}
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetMyProfileTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.MyProfileTutorial=step;
		}
	}
	public IEnumerator setProfileTutorial(bool step)
	{
		string tempString;
		if(step)
		{
			tempString="1";
		}
		else
		{
			tempString="0";
		}
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetProfileTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.ProfileTutorial=step;
		}
	}
	public IEnumerator setSkillBookTutorial(bool step)
	{
		string tempString;
		if(step)
		{
			tempString="1";
		}
		else
		{
			tempString="0";
		}
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetSkillBookTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.SkillBookTutorial=step;
		}
	}
	public IEnumerator setDivisionLobbyTutorial(bool step)
	{
		string tempString;
		if(step)
		{
			tempString="1";
		}
		else
		{
			tempString="0";
		}
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetDivisionLobbyTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.DivisionLobbyTutorial=step;
		}
	}
	public IEnumerator setCupLobbyTutorial(bool step)
	{
		string tempString;
		if(step)
		{
			tempString="1";
		}
		else
		{
			tempString="0";
		}
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetCupLobbyTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.CupLobbyTutorial=step;
		}
	}
	public IEnumerator updateEndGameData()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 
		
		WWW w = new WWW(URLUpdateEndGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 
		// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
	}
}



