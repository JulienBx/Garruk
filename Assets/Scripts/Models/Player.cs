using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : User
{
	private string URLGetUserGameProfile ;
	private string URLDefaultProfilePicture;
	private string URLDefaultThumbProfilePicture;
	private string URLGetDecks;
	private string URLGetMyCardsPage;
	private string URLUpdateUserInformations;
	private string URLUpdateProfilePicture;
	private string URLGetNonReadNotifications;
	private string URLGetMoney;
	private string URLAddMoney;
	private string URLSelectedDeck;
	private string URLCleanCards;
	private string URLSetTutorialStep;
	private string URLSetMarketTutorial;
	private string URLSetProfileTutorial;
	private string URLSetSkillBookTutorial;
	private string URLSetLobbyTutorial;
	private string URLUpdateEndGameData;
	private string URLInvitePlayer;
	private string URLSetProfilePicture;
	private string URLSetDisplayTutorial;
	private string URLCheckPassword;
	private string URLEditPassword;
	private string URLChooseLanguage;
	private string URLCheckAuthentification; 
	private string URLCheckPermanentConnexion;
	private string URLCreateAccount;
	private string URLRefreshUserData;
	private int TotalNbResultLimit;
	public string Error;
	public string Mail;
	public string FirstName;
	public string Surname;
	public int Money;
	public List<Connection> Connections;
	public bool Readnotificationsystem;
	public int SelectedDeckId;
	public List<int> CardTypesAllowed;
	public bool IsAdmin;
	public int TutorialStep;
	public bool MarketTutorial;
	public bool ProfileTutorial;
	public bool SkillBookTutorial;
	public bool LobbyTutorial;
	public int ConnectionBonus;
	public bool DisplayTutorial;
	public int NbNotificationsNonRead;
	public FriendlyGame CurrentFriendlyGame;
	public Cup CurrentCup;
	public Division CurrentDivision;
	public string ProfileChosen;
	public int IdLanguage;
	public bool ToDeconnect;
	public bool GoToNotifications;
	public bool HasDeck;
	public bool HasWonLastGame;
	public bool IsFirstPlayer;
	public bool ToLaunchGameTutorial;
	public bool ToLaunchEndGameSequence;
	public int PackToBuy;
	public int ChosenGameType;
	public string MacAdress;
	public bool IsInviting;
	public bool IsInvited;
	public bool IsBusy;
	public bool IsAccountActivated;

	public Player()
	{
		this.Username = "";
		this.ProfileChosen="";
		this.NbNotificationsNonRead=0;
		this.IsFirstPlayer=false;
		this.ToLaunchGameTutorial=false;
		this.ToLaunchEndGameSequence=false;
		this.HasWonLastGame=false;
		this.PackToBuy=-1;
		this.ChosenGameType=0;
		this.MacAdress=SystemInfo.deviceUniqueIdentifier;
		this.URLGetUserGameProfile = ApplicationModel.host + "get_user_game_profile.php";
		this.URLDefaultProfilePicture = ApplicationModel.host + "img/profile/defaultprofilepicture.png";
		this.URLDefaultThumbProfilePicture = ApplicationModel.host + "img/profile/defaultthumbprofilepicture.png";
		this.URLGetDecks = ApplicationModel.host + "get_decks_by_user.php";
		this.URLGetMyCardsPage = ApplicationModel.host + "get_mycardspage_data.php";
		this.URLUpdateUserInformations= ApplicationModel.host + "update_user_informations.php";
		this.URLUpdateProfilePicture  = ApplicationModel.host + "update_profilepicture.php";
		this.URLGetNonReadNotifications = ApplicationModel.host +"get_non_read_notifications_by_user.php";
		this.URLGetMoney = ApplicationModel.host + "get_money_by_user.php";
		this.URLAddMoney = ApplicationModel.host + "add_money.php";
		this.URLSelectedDeck = ApplicationModel.host + "set_selected_deck.php";
		this.URLCleanCards = ApplicationModel.host + "clean_cards.php";
		this.URLSetTutorialStep = ApplicationModel.host + "set_tutorialstep.php";
		this.URLSetMarketTutorial = ApplicationModel.host + "set_marketTutorial.php";
		this.URLSetProfileTutorial = ApplicationModel.host + "set_profileTutorial.php";
		this.URLSetSkillBookTutorial = ApplicationModel.host + "set_skillBookTutorial.php";
		this.URLSetLobbyTutorial = ApplicationModel.host + "set_lobbyTutorial.php";
		this.URLUpdateEndGameData = ApplicationModel.host + "get_end_game_data.php";
		this.URLInvitePlayer = ApplicationModel.host + "invite_player.php";
		this.URLSetProfilePicture = ApplicationModel.host + "set_profile_picture.php";
		this.URLSetDisplayTutorial = ApplicationModel.host + "set_displaytutorial.php";
		this.URLCheckPassword = ApplicationModel.host + "check_password.php";
		this.URLEditPassword = ApplicationModel.host + "edit_password.php";
		this.URLChooseLanguage = ApplicationModel.host + "choose_language.php";
		this.URLCheckAuthentification = ApplicationModel.host + "check_authentication.php";
		this.URLCheckPermanentConnexion = ApplicationModel.host + "check_permanent_connexion.php";
		this.URLCreateAccount = ApplicationModel.host + "create_account.php";
		this.URLRefreshUserData = ApplicationModel.host+"refresh_user_data.php";
		this.TotalNbResultLimit=1000;
		this.Error="";
		this.Connections=new List<Connection>();
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
	public IEnumerator setProfilePicture(int idprofilepicture)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_idprofilepicture", idprofilepicture.ToString()); 
		
		WWW w = new WWW (URLSetProfilePicture, form); 								// On envoie le formulaire à l'url sur le serveur 
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
				this.IdProfilePicture=idprofilepicture;
			}
		}
	}
	public IEnumerator countNonReadsNotifications(int totalNbResultLimit)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLGetNonReadNotifications, form); 				
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}
		else
		{
			this.NbNotificationsNonRead=System.Convert.ToInt32(w.text);
		}
	}
	public IEnumerator addMoney(int money)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username);
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
		form.AddField("myform_nick", this.Username);
		
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté     
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck);
		
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
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
	public IEnumerator setDisplayTutorial(bool displayTutorial)
	{
		string displayTutorialString = "0";
		if(displayTutorial)
		{
			displayTutorialString="1";
		}

		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_displaytutorial",displayTutorialString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetDisplayTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
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
				this.DisplayTutorial=displayTutorial;
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
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
	public IEnumerator setLobbyTutorial(bool step)
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
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", tempString);                 // Deck sélectionné
		
		WWW w = new WWW (URLSetLobbyTutorial, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			this.LobbyTutorial=step;
		}
	}
	public IEnumerator updateEndGameData()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		
		WWW w = new WWW(URLUpdateEndGameData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 
		// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
	}

	public IEnumerator checkPassword(string password)
	{
		Error = "";
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		form.AddField("myform_pass", password);
		
		WWW w = new WWW(URLCheckPassword, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else
		{
			if (w.text != "") 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				Error = w.text;
			}
		}
	}
	public IEnumerator editPassword(string password)
	{
		WWWForm form = new WWWForm(); 								 //Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		 				//hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		form.AddField("myform_pass", password);
		
		WWW w = new WWW(URLEditPassword, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
	}
	public IEnumerator chooseLanguage(int id)
	{
		WWWForm form = new WWWForm(); 								 //Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		 				//hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		form.AddField("myform_idlanguage", id.ToString());
		
		WWW w = new WWW(URLChooseLanguage, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				Error = errors [1];
			} 
			else
			{
				Error = "";
				this.IdLanguage=id;
			}		
		}
	}
	public IEnumerator refreshUserData()
	{
		string isInvitingString = "0";
		if(this.IsInviting)
		{
			isInvitingString="1";
		}

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_limit", TotalNbResultLimit); 
		form.AddField("myform_isinviting", isInvitingString);
		
		WWW w = new WWW(URLRefreshUserData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] playerData =  data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.Money	= System.Convert.ToInt32(playerData[0]);
			this.NbNotificationsNonRead=System.Convert.ToInt32(playerData[1]);
			this.IdProfilePicture=System.Convert.ToInt32(playerData[2]);
			if(this.IsInviting)
			{
				this.Error=data[1];
			}
			if(data[2]!="-1")
			{
				this.IsInvited=true;
			}
			else
			{
				this.IsInvited=false;
			}
		}
	}

	#region AUTHENTICATION

	public IEnumerator permanentConnexion()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_macadress", this.MacAdress); 	
		
		WWW w = new WWW(URLCheckPermanentConnexion, form);
		yield return w;
		
		if (w.error != null)
		{
			Error = w.error;
		} 
		else
		{
			
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				Error = errors [1];
			} 
			else if(w.text.Contains("#SUCESS#"))
			{
				Error = "";
				string[] data = w.text.Split(new string[] { "#SUCESS#" }, System.StringSplitOptions.None);
				string[] profileData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				this.Username = profileData [0];
				this.TutorialStep = System.Convert.ToInt32(profileData [1]);
				this.IsAdmin = System.Convert.ToBoolean(System.Convert.ToInt32(profileData [2]));
				this.Money = System.Convert.ToInt32(profileData [3]);
				this.DisplayTutorial = System.Convert.ToBoolean(System.Convert.ToInt32(profileData [4]));
				this.IdLanguage=System.Convert.ToInt32(profileData[5]);
				this.IdProfilePicture=System.Convert.ToInt32(profileData[6]);
				this.Id=System.Convert.ToInt32(profileData[7]);
			}
			else
			{
				Error="";
				this.Id=-1;
			}		
		}
	}
	public IEnumerator Login(string nick, string password, bool toMemorize)
	{	
		string toMemorizeString;
		if (toMemorize)
		{
			toMemorizeString = "1";
		} else
		{
			toMemorizeString = "0";
		}
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", nick);
		form.AddField("myform_pass", password);
		form.AddField("myform_memorize", toMemorizeString);
		form.AddField("myform_macadress", this.MacAdress);
		
		WWW w = new WWW(URLCheckAuthentification, form);
		yield return w;
		
		if (w.error != null)
		{
			Error = w.error;
		} 
		else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				Error = errors [1];
			} 
			else if(w.text.Contains("#SUCESS#"))
			{
				Error = "";
				string[] data = w.text.Split(new string[] { "#SUCESS#" }, System.StringSplitOptions.None);
				string[] profileData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				this.Username = profileData [0];
				this.TutorialStep = System.Convert.ToInt32(profileData [1]);
				this.IsAdmin = System.Convert.ToBoolean(System.Convert.ToInt32(profileData [2]));
				this.Money = System.Convert.ToInt32(profileData [3]);
				this.DisplayTutorial = System.Convert.ToBoolean(System.Convert.ToInt32(profileData [4]));
				this.IdLanguage=System.Convert.ToInt32(profileData[5]);
				this.IdProfilePicture=System.Convert.ToInt32(profileData[6]);
				this.Id=System.Convert.ToInt32(profileData[7]);
				this.IsAccountActivated=true;
			}
			else if(w.text.Contains("#NONACTIVE#"))
			{
				Error="";
				this.Id=-1;
				this.IsAccountActivated=false;
			}
			else
			{
				Error="";
				this.Id=-1;
			}												
		}
	}
	public IEnumerator createAccount(string nick, string email, string password)
	{	
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", nick);
		form.AddField("myform_email", email);
		form.AddField("myform_pass", password);
		form.AddField("myform_macadress", this.MacAdress);
		
		WWW w = new WWW(URLCreateAccount, form);
		yield return w;
		
		if (w.error != null)
		{
			Error = w.error;
		} else
		{
			if (w.text == "")
			{	 				
				this.Username = nick;
				this.ToDeconnect = false;
				this.Mail=email;
			} 
			else
			{
				Error = w.text;
			}										
		}
	}

	#endregion
}



