using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : User
{
	private string URLUpdateUserInformations;
	private string URLGetNonReadNotifications;
	private string URLAddMoney;
	private string URLSelectedDeck;
	private string URLCleanCards;
	private string URLSetTutorialStep;
	private string URLSetMarketTutorial;
	private string URLSetProfileTutorial;
	private string URLSetSkillBookTutorial;
	private string URLSetNextLevelTutorial;
	private string URLSetLobbyTutorial;
	private string URLUpdateEndGameData;
	private string URLSetProfilePicture;
	private string URLCheckPassword;
	private string URLEditPassword;
	private string URLChooseLanguage;
	private string URLCheckAuthentification; 
	private string URLCheckPermanentConnexion;
	private string URLCreateAccount;
	private string URLRefreshUserData;
	private string URLLostLogin;
	private string URLSentNewEmail; 
	private string URLLinkAccount;
	private string URLGetPurchasingToken;
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
	public bool NextLevelTutorial;
	public bool LobbyHelp;
	public int ConnectionBonus;
	public int NbNotificationsNonRead;
	public Division CurrentDivision;
	public string ProfileChosen;
	public int IdLanguage;
	public bool ToDeconnect;
	public bool GoToNotifications;
	public bool HasDeck;
	public bool HasWonLastGame;
	public bool IsFirstPlayer;
	public bool ToLaunchGameTutorial;
	public bool ToLaunchGameIA;
	public bool ToLaunchEndGameSequence;
    public bool ToLaunchChallengeGame;
	public int PackToBuy;
	public int ChosenGameType;
	public string MacAdress;
	public bool IsInviting;
	public bool IsInvited;
	public bool IsBusy;
	public bool IsAccountActivated;
	public bool ToRememberLogins;
	public string Password;
	public string FacebookId;
	public bool IsAccountCreated;
	public bool ToChangePassword;
	public Deck MyDeck;
	public int PercentageLooser;
	public string DesktopPurchasingToken;
	public int TrainingAllowedCardType;
	public int TrainingPreviousAllowedCardType;
	public bool HasToBuyTrainingPack;
	public bool HasLostConnection;
    public bool HasLostConnectionDuringGame;
    public bool ShouldQuitGame;
    public bool AutomaticConnection;
    public bool IsOnline;
    public Cards MyCards;
	public Decks MyDecks;
	public Users Users;
	public Notifications MyNotifications;
	public NewsList MyNews;
	public List<int> MyFriends;
	public Connections MyConnections;
	public Trophies MyTrophies;
	public ChallengesRecords MyChallengesRecords;
	public Division MyDivision;
    public Skills MySkills;
  
	public Player()
	{
		this.Username = "";
		this.Password="";
		this.FacebookId="";
		this.Mail="";
		this.Id=-1;
		this.ProfileChosen="";
		this.NbNotificationsNonRead=0;
		this.IsFirstPlayer=false;
		this.ToLaunchGameTutorial=false;
		this.ToLaunchEndGameSequence=false;
		this.HasWonLastGame=false;
		this.PackToBuy=-1;
		this.ChosenGameType=0;
		this.URLUpdateUserInformations= ApplicationModel.host + "update_user_informations.php";
		this.URLAddMoney = ApplicationModel.host + "add_money.php";
		this.URLSelectedDeck = ApplicationModel.host + "set_selected_deck.php";
		this.URLCleanCards = ApplicationModel.host + "clean_cards.php";
		this.URLSetTutorialStep = ApplicationModel.host + "set_tutorialstep.php";
		this.URLSetMarketTutorial = ApplicationModel.host + "set_marketTutorial.php";
		this.URLSetProfileTutorial = ApplicationModel.host + "set_profileTutorial.php";
		this.URLSetSkillBookTutorial = ApplicationModel.host + "set_skillBookTutorial.php";
		this.URLSetLobbyTutorial = ApplicationModel.host + "set_lobbyTutorial.php";
		this.URLSetNextLevelTutorial = ApplicationModel.host + "set_nextLevelTutorial.php";
		this.URLSetProfilePicture = ApplicationModel.host + "set_profile_picture.php";
		this.URLCheckPassword = ApplicationModel.host + "check_password.php";
		this.URLEditPassword = ApplicationModel.host + "edit_password.php";
		this.URLChooseLanguage = ApplicationModel.host + "choose_language.php";
		this.URLCheckAuthentification = ApplicationModel.host + "authentication_check.php";
		this.URLCheckPermanentConnexion = ApplicationModel.host + "check_permanent_connexion.php";
		this.URLCreateAccount = ApplicationModel.host + "create_account.php";
		this.URLRefreshUserData = ApplicationModel.host+"refresh_user_data.php";
		this.URLLostLogin=ApplicationModel.host+"lost_login.php";
		this.URLSentNewEmail = ApplicationModel.host+"sent_newemail.php";
		this.URLLinkAccount = ApplicationModel.host+"link_account.php";
		this.URLGetPurchasingToken = ApplicationModel.host+"/payment/getToken.php";
		this.TotalNbResultLimit=1000;
		this.Error="";
		this.Connections=new List<Connection>();
		this.CurrentDivision=new Division();
		this.IsOnline=true;
	}
	public IEnumerator updateInformations(string firstname, string surname, string mail, bool isNewEmail, bool isPublic)
	{
		string isNewEmailString="0";
		if(isNewEmail)
		{
			isNewEmailString="1";
		}
		string isPublicString="0";
		if(isPublic)
		{
			isPublicString="1";
		}

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_id",this.Id);
		form.AddField("myform_firstname", firstname); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_surname", surname);
		form.AddField("myform_mail", mail);
		form.AddField("myform_isnewemail", isNewEmailString);
		form.AddField("myform_ispublic", isPublicString);

		ServerController.instance.setRequest(URLUpdateUserInformations, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			this.FirstName=firstname;
			this.Surname=surname;
			this.Mail=mail;
			this.isPublic=isPublic;
		}
	}
	public IEnumerator setProfilePicture(int idprofilepicture)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_idprofilepicture", idprofilepicture.ToString()); 

		ServerController.instance.setRequest(URLSetProfilePicture, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			this.IdProfilePicture=idprofilepicture;
		}
	}
	public IEnumerator addMoney()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
        form.AddField("myform_product", ApplicationModel.Decrypt(PlayerPrefs.GetString("Product","")));
        form.AddField("myform_productowner", ApplicationModel.Decrypt(PlayerPrefs.GetString("ProductOwner","")));

		ServerController.instance.setRequest(URLAddMoney, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
        if(ServerController.instance.getError()=="")
        {
            PlayerPrefs.DeleteKey("Product");
            PlayerPrefs.DeleteKey("ProductOwner");
            PlayerPrefs.Save();
        }
        else
        {
            ServerController.instance.lostConnection();
        }
	}
	public IEnumerator SetSelectedDeck(int selectedDeckId)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", selectedDeckId.ToString());                 // Deck sélectionné
		
		WWW w = new WWW (URLSelectedDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 

		this.SelectedDeckId=selectedDeckId;
													// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		}
	}
	public IEnumerator cleanCards()
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté     
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck);

		ServerController.instance.setRequest(URLCleanCards, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");

		if(ServerController.instance.getError()!="")
		{
			Debug.Log(ServerController.instance.getError());
		}
	}
	public IEnumerator setTutorialStep(int step)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", this.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_step", step.ToString());                 // Deck sélectionné

		ServerController.instance.setRequest(URLSetTutorialStep, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		
		if (ServerController.instance.getError()!="") 
		{
			Debug.Log (ServerController.instance.getError()); 										
		}
		this.TutorialStep=step;
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

		ServerController.instance.setRequest(URLSetMarketTutorial, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		
		if (ServerController.instance.getError()!="") 
		{
			Debug.Log (ServerController.instance.getError()); 										
		}
		this.MarketTutorial=step;
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

		ServerController.instance.setRequest(URLSetProfileTutorial, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		
		if (ServerController.instance.getError()!="") 
		{
			Debug.Log (ServerController.instance.getError()); 										
		}
		this.ProfileTutorial=step;
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

		ServerController.instance.setRequest(URLSetSkillBookTutorial, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		
		if (ServerController.instance.getError()!="") 
		{
			Debug.Log (ServerController.instance.getError()); 										
		}
		this.SkillBookTutorial=step;
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

		ServerController.instance.setRequest(URLSetLobbyTutorial, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		
		if (ServerController.instance.getError()!="") 
		{
			Debug.Log (ServerController.instance.getError()); 										
		}
		this.LobbyHelp=step;
	}
	public IEnumerator setNextLevelTutorial(bool step)
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
		form.AddField("myform_step", tempString);  

		ServerController.instance.setRequest(URLSetNextLevelTutorial, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		
		if (ServerController.instance.getError()!="") 
		{
			Debug.Log (ServerController.instance.getError()); 										
		}
		this.NextLevelTutorial=step;
	}
	public IEnumerator checkPassword(string password)
	{
		Error = "";
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		form.AddField("myform_pass", password);

		ServerController.instance.setRequest(URLCheckPassword, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
	}
	public IEnumerator editPassword()
	{
		WWWForm form = new WWWForm(); 								 //Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		 				//hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		form.AddField("myform_pass",this.Password);

		ServerController.instance.setRequest(URLEditPassword, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
	}
	public IEnumerator chooseLanguage(int id)
	{
		WWWForm form = new WWWForm(); 								 //Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		 				//hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", this.Username); 
		form.AddField("myform_idlanguage", id.ToString());

		ServerController.instance.setRequest(URLChooseLanguage, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			this.IdLanguage=id;
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
			if(this.IsInviting && data[1]!="")
			{
                if(data[1]=="ok")
                {
                    this.ToLaunchChallengeGame=true;
                }
                else
                {
                    this.Error=WordingServerError.getReference(data[1],true);
                }
			}
			else if(data[2]!="-1")
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

	public IEnumerator Login()
	{	
		string toMemorizeString;
		if (this.ToRememberLogins)
		{
			toMemorizeString = "1";
		} else
		{
			toMemorizeString = "0";
		}
		string autoConnectString;
		if(this.AutomaticConnection)
		{
			autoConnectString="1";
		}
		else
		{
			autoConnectString="0";
		}
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", this.Username);
		form.AddField("myform_pass", this.Password);
		form.AddField("myform_memorize", toMemorizeString);
		form.AddField("myform_macadress", this.MacAdress);
		form.AddField("myform_facebookid", this.FacebookId);
		form.AddField("myform_mail", this.Mail);
        form.AddField("myform_product", ApplicationModel.Decrypt(PlayerPrefs.GetString("Product","")));
        form.AddField("myform_productowner", ApplicationModel.Decrypt(PlayerPrefs.GetString("ProductOwner","")));
		form.AddField("myform_autoconnect", autoConnectString);

		ServerController.instance.setRequest(URLCheckAuthentification, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
            PlayerPrefs.DeleteKey("Product");
            PlayerPrefs.DeleteKey("ProductOwner");
            PlayerPrefs.Save();
            string result = ServerController.instance.getResult();

			if(result.Contains("#PROVISIONNINGOK#"))
			{
				PlayerPrefs.DeleteKey("Product");
            	PlayerPrefs.DeleteKey("ProductOwner");
            	PlayerPrefs.Save();
			}
			if(result.Contains("#SUCESS#"))
			{
				this.IsAccountActivated=true;
				this.IsAccountCreated=true;
				string[] data =result.Split(new string[] { "#DATASEPARATOR#" }, System.StringSplitOptions.None);
				string[] gameData =data[1].Split(new string[] { "#END#" }, System.StringSplitOptions.None);

				this.MyCards=new Cards();
				this.MyDecks=new Decks();
				this.MyNotifications=new Notifications();
				this.MyConnections=new Connections();
				this.MyNews=new NewsList();
				this.MyTrophies=new Trophies();
				this.MyChallengesRecords=new ChallengesRecords();
				this.MyDivision=new Division();
				this.Users=new Users();
                this.MySkills=new Skills();
				ApplicationModel.packs=new Packs();
				ApplicationModel.products=new DisplayedProducts();
                ApplicationModel.cardTypes=new CardTypes();
                ApplicationModel.skillTypes=new SkillTypes();
                ApplicationModel.skills=new Skills();

				this.parsePlayerInformations(gameData[0]);
				if(gameData[10]!="")
				{
					this.Users.parseUsers(gameData[10]);
				}
				if(gameData[1]!="")
				{
					this.MyCards.parseCards(gameData[1]);
				}
				if(gameData[2]!="")
				{
					this.MyDecks.parseDecks(gameData[2]);
					this.retrieveCardsDeck();
				}
				if(gameData[3]!="")
				{
					this.MyNotifications.parseNotifications(gameData[3]);
					this.MyNotifications.lookForNonReadNotification();
				}
				if(gameData[4]!="")
				{
					this.MyFriends=this.parseFriends(gameData[4]);
				}
				if(gameData[5]!="")
				{
					this.MyConnections.parseConnections(gameData[5]);
				}
				if(gameData[6]!="")
				{
					this.MyNews.parseNews(gameData[6]);
					this.MyNews.filterNews(this.Id);
				}
				if(gameData[7]!="")
				{
					this.MyTrophies.parseTrophies(gameData[7]);
				}
				if(gameData[8]!="")
				{
					this.MyChallengesRecords.parseChallengesRecords(gameData[8]);
				}
				if(gameData[9]!="")
				{
					this.MyDivision=this.parseDivision(gameData[9]);
				}
				if(gameData[11]!="")
				{
					ApplicationModel.packs.parsePacks(gameData[11]);
				}
				if(gameData[12]!="")
				{
					ApplicationModel.products.parseProducts(gameData[12]);
				}
                if(gameData[13]!="")
                {
                    ApplicationModel.skillTypes.parseSkillTypes(gameData[13]);
                }
                if(gameData[14]!="")
                {
                    ApplicationModel.cardTypes.parseCardTypes(gameData[14]);
                }
                if(gameData[15]!="")
                {
                    ApplicationModel.skills.parseSkills(gameData[15]);
                    this.retrieveMySkills();
                }
				if(System.Convert.ToInt32(data[2])!=-1)
                {
					string[] resultsHistoryData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
                    this.HasLostConnectionDuringGame=true;
					this.HasWonLastGame=System.Convert.ToBoolean(System.Convert.ToInt32(resultsHistoryData[0]));
					this.ChosenGameType=System.Convert.ToInt32(resultsHistoryData[1]);
                    ApplicationModel.player.MyDeck=new Deck();
                    string[] myDeckData =data[1].Split(new string[] { "#MYDECK#" }, System.StringSplitOptions.None);
                    string[] myDeckCards = myDeckData[1].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
                    for(int i = 0 ; i < myDeckCards.Length ; i++)
                    {
                        ApplicationModel.player.MyDeck.cards.Add(new Card());
                        ApplicationModel.player.MyDeck.cards[i].parseCard(myDeckCards[i]);
                        ApplicationModel.player.MyDeck.cards[i].deckOrder=i;
                    }
                }
			}
			else if(result.Contains("#NONACTIVE#"))
			{
				string[] data = result.Split(new string[] { "#NONACTIVE#" }, System.StringSplitOptions.None);
				string[] profileData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				this.Mail = profileData [0];
				this.Id=-1;
				this.IsAccountActivated=false;
				this.IsAccountCreated=true;
			}
			else if(result.Contains("#FIRSTCONNECTION"))
			{
				this.Id=-1;
				this.IsAccountCreated=false;
				this.IsAccountActivated=false;
			}
			else
			{
				Error=WordingAuthentication.getReference(13);
				this.Id=-1;
			}					
		}
	}
	public IEnumerator createAccount()
	{	
		string isAccountActivatedString;
		if (this.IsAccountActivated)
		{
			isAccountActivatedString = "1";
		} 
		else
		{
			isAccountActivatedString = "0";
		}

		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", Username);
		form.AddField("myform_email", Mail);
		form.AddField("myform_pass", Password);
		form.AddField("myform_macadress", MacAdress);
		form.AddField("myform_facebookid",FacebookId);
		form.AddField("myform_ismailactivated",isAccountActivatedString);
		form.AddField("myform_idlanguage",IdLanguage.ToString());

		ServerController.instance.setRequest(URLCreateAccount, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			if(result.Contains("#SUCESS#"))
			{
				string[] data = result.Split(new string[] { "#SUCESS#" }, System.StringSplitOptions.None);
				string[] profileData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				this.Username = profileData [0];
				this.TutorialStep = System.Convert.ToInt32(profileData [1]);
				this.IsAdmin = System.Convert.ToBoolean(System.Convert.ToInt32(profileData [2]));
				this.Money = System.Convert.ToInt32(profileData [3]);
				this.IdLanguage=System.Convert.ToInt32(profileData[4]);
				this.IdProfilePicture=System.Convert.ToInt32(profileData[5]);
				this.Id=System.Convert.ToInt32(profileData[6]);
				this.TrainingStatus=System.Convert.ToInt32(profileData[7]);
				this.Mail=profileData[8];
				this.CurrentDivision=new Division();
				this.CurrentDivision.Id=System.Convert.ToInt32(profileData[9]);
                
				this.IsAccountActivated=true;
				this.IsAccountCreated=true;
			}
			else if(result.Contains("#NONACTIVE#"))
			{
				string[] data = result.Split(new string[] { "#NONACTIVE#" }, System.StringSplitOptions.None);
				string[] profileData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				this.Mail = profileData [0];
				this.Id=-1;
				this.IsAccountActivated=false;
				this.IsAccountCreated=true;
			}		
		}
	}
	public IEnumerator lostLogin()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", this.Username); 	
		form.AddField("myform_macadress", this.MacAdress); 	

		ServerController.instance.setRequest(URLLostLogin, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
	}
	public IEnumerator sentNewEmail()
	{	
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", Username);
		form.AddField("myform_email", Mail);

		ServerController.instance.setRequest(URLSentNewEmail, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
	}
	public IEnumerator linkAccount()
	{	
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", Username);
		form.AddField("myform_pass", Password);
		form.AddField("myform_facebookid", FacebookId);

		ServerController.instance.setRequest(URLLinkAccount, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
	}
	public IEnumerator getPurchasingToken()
	{	
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_id", Id.ToString());
		form.AddField("myform_nick", Username);
		form.AddField("myform_email", Mail);

		ServerController.instance.setRequest(URLGetPurchasingToken, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(ServerController.instance.getError()!="")
		{
			Debug.Log(ServerController.instance.getError());
			ServerController.instance.lostConnection();
		}
		else
		{
			ApplicationModel.player.DesktopPurchasingToken=ServerController.instance.getResult();
		}
	}
	#endregion

	public void getTrainingAllowedCardType()
	{
		if(TrainingStatus==-1)
		{
			this.TrainingAllowedCardType=7;
			this.TrainingPreviousAllowedCardType=7;
		}
		else if(TrainingStatus<9)
		{
			this.TrainingAllowedCardType=2;
			this.TrainingPreviousAllowedCardType=2;
		}
		else if(TrainingStatus<19)
		{
			this.TrainingAllowedCardType=3;
			this.TrainingPreviousAllowedCardType=2;
		}
		else if(TrainingStatus<29)
		{
			this.TrainingAllowedCardType=1;
			this.TrainingPreviousAllowedCardType=3;
		}
		else if(TrainingStatus<39)
		{
			this.TrainingAllowedCardType=0;
			this.TrainingPreviousAllowedCardType=1;
		}
		else if(TrainingStatus<49)
		{
			this.TrainingAllowedCardType=6;
			this.TrainingPreviousAllowedCardType=0;
		}
		else if(TrainingStatus<59)
		{
			this.TrainingAllowedCardType=7;
			this.TrainingPreviousAllowedCardType=6;
		}
//		else if(TrainingStatus<69)
//		{
//			this.TrainingAllowedCardType=4;
//			this.TrainingPreviousAllowedCardType=7;
//		}
//		else if(TrainingStatus<79)
//		{
//			this.TrainingAllowedCardType=5;
//			this.TrainingPreviousAllowedCardType=4;
//		}
//		else if(TrainingStatus<89)
//		{
//			this.TrainingAllowedCardType=8;
//			this.TrainingPreviousAllowedCardType=5;
//		}
//		else if(TrainingStatus<99)
//		{
//			this.TrainingAllowedCardType=9;
//			this.TrainingPreviousAllowedCardType=8;
//		}
	}
	public bool canAccessTrainingMode()
	{
		for(int i=0;i<this.MyDeck.cards.Count;i++)
		{
			if(this.MyDeck.cards[i].CardType.Id!=this.TrainingAllowedCardType)
			{
				return false;
			}
		}
		return true;
	}
	private void parsePlayerInformations(string s)
	{
		string[] array=s.Split(new string[] { "//" }, System.StringSplitOptions.None);

		this.Id= System.Convert.ToInt32(array[0]);
		this.Mail= array[1];
		this.Money= System.Convert.ToInt32(array[2]);
		this.FirstName	= array[3];
		this.Surname= array[4];
		this.IdProfilePicture= System.Convert.ToInt32(array[5]);
		this.AutomaticConnection	= System.Convert.ToBoolean(System.Convert.ToInt32(array[6]));
		this.SelectedDeckId= System.Convert.ToInt32(array[7]);
		this.RankingPoints = System.Convert.ToInt32 (array [8]);
		this.Ranking = System.Convert.ToInt32 (array [9]);
		this.CollectionPoints = System.Convert.ToInt32 (array [10]);
		this.CollectionRanking = System.Convert.ToInt32 (array [11]);
		this.TotalNbWins = System.Convert.ToInt32 (array [12]);
		this.TotalNbLooses = System.Convert.ToInt32 (array [13]);
		this.Readnotificationsystem=System.Convert.ToBoolean(System.Convert.ToInt32(array[14]));
		this.IsAdmin=System.Convert.ToBoolean(System.Convert.ToInt32(array[15]));
		this.TutorialStep = System.Convert.ToInt32 (array [16]);
		this.MarketTutorial = System.Convert.ToBoolean(System.Convert.ToInt32 (array [17]));
		this.ProfileTutorial = System.Convert.ToBoolean(System.Convert.ToInt32 (array [18]));
		this.LobbyHelp = System.Convert.ToBoolean(System.Convert.ToInt32 (array [19]));
		this.SkillBookTutorial = System.Convert.ToBoolean(System.Convert.ToInt32 (array [20]));
		this.NextLevelTutorial = System.Convert.ToBoolean(System.Convert.ToInt32 (array [21]));
		this.IdLanguage	 = System.Convert.ToInt32 (array [22]);
		this.CurrentDivision	 = new Division();
		this.CurrentDivision.Id=System.Convert.ToInt32 (array [23]);
		this.TrainingStatus=System.Convert.ToInt32 (array [24]);
		this.HasToBuyTrainingPack=System.Convert.ToBoolean(System.Convert.ToInt32 (array [25]));
		this.isPublic=System.Convert.ToBoolean(System.Convert.ToInt32 (array [26]));
		this.getTrainingAllowedCardType();
	}

	private List<int> parseFriends(string s)
	{
		string[] friendsData=s.Split(new string[] { "//" }, System.StringSplitOptions.None);

		List<int> friends = new List<int> ();
		for(int i=0;i<friendsData.Length-1;i++)
		{
			friends.Add (this.Users.returnUsersIndex(System.Convert.ToInt32(friendsData[i])));
		}
		return friends;
	}

	private Division parseDivision(string s)
	{

		string[] array=s.Split(new string[] { "//" }, System.StringSplitOptions.None);

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
	private void retrieveCardsDeck()
	{
		for(int i=0;i<this.MyCards.getCount();i++)
		{
			this.MyCards.getCard(i).Decks=new List<int>();
		}
		for (int i=0;i<this.MyDecks.getCount();i++)
		{
			for(int j=0;j<this.MyDecks.getDeck(i).cards.Count;j++)
			{
				for(int k=0;k<this.MyCards.getCount();k++)
				{
					if(this.MyCards.getCard(k).Id==MyDecks.getDeck(i).cards[j].Id)
					{
						this.MyCards.getCard(k).Decks.Add (MyDecks.getDeck(i).Id);
					}
				}
			}
		}
	}
	public void moveToFriend(int id)
	{
		this.MyFriends.Add (this.MyNotifications.getNotification(id).SendingUser);
		this.MyNotifications.remove(id);
	}
    public bool hasSkills(int id)
    {
        for(int i=0;i<this.MySkills.getCount();i++)
        {
            if(this.MySkills.getSkill(i).Id==id && this.MySkills.getSkill(i).Power>0)
            {
                return true;
            }
        }
        return false;
    }
    public void retrieveMySkills()
    {
        for(int i=0;i<ApplicationModel.skills.getCount();i++)
        {
            this.MySkills.add();
            this.MySkills.getSkill(this.MySkills.getCount()-1).Id=ApplicationModel.skills.getSkill(i).Id;
            this.MySkills.getSkill(this.MySkills.getCount()-1).IdCardType=ApplicationModel.skills.getSkill(i).IdCardType;
            this.MySkills.getSkill(this.MySkills.getCount()-1).IdSkillType=ApplicationModel.skills.getSkill(i).IdSkillType;
            this.MySkills.getSkill(this.MySkills.getCount()-1).Power=0;
            this.MySkills.getSkill(this.MySkills.getCount()-1).Level=0;
        }
        for(int i=0;i<this.MyCards.getCount();i++)
        {
            for(int j=0;j<this.MyCards.getCard(i).Skills.Count();j++)
            {
                if(this.MyCards.getCard(i).Skills[j].IsActivated==1)
                {
                    for(int k=0;k<this.MySkills.getCount();k++)
                    {
                        if(this.MySkills.getSkill(k).Id==this.MyCards.getCard(i).Skills[j].Id && this.MySkills.getSkill(k).Power<this.MyCards.getCard(i).Skills[j].Power)
                        {
                            this.MySkills.getSkill(k).Power=this.MyCards.getCard(i).Skills[j].Power;
                            this.MySkills.getSkill(k).Level=this.MyCards.getCard(i).Skills[j].Level;
                        }
                    }
                }
            }
        }
    }
}



