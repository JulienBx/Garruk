﻿using UnityEngine;
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
	public bool hastLostConnection;
  
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
		this.URLCheckAuthentification = ApplicationModel.host + "check_authentication.php";
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
            PlayerPrefs.DeleteAll();
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
				this.Error=WordingServerError.getReference(data[1],true);
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

	public IEnumerator permanentConnexion()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_macadress", this.MacAdress);
        form.AddField("myform_product", ApplicationModel.Decrypt(PlayerPrefs.GetString("Product","")));
        form.AddField("myform_productowner", ApplicationModel.Decrypt(PlayerPrefs.GetString("ProductOwner","")));

		ServerController.instance.setRequest(URLCheckPermanentConnexion, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();
		
		if(this.Error=="")
		{
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
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
			}
			else
			{
				this.Id=-1;
			}		
		}
		else
		{
			Debug.Log(this.Error);
			this.Id=-1;
		}
	}
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

		ServerController.instance.setRequest(URLCheckAuthentification, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            string result = ServerController.instance.getResult();
			if(result.Contains("#SUCESS#"))
			{
				string[] data =result.Split(new string[] { "#SUCESS#" }, System.StringSplitOptions.None);
				string[] profileData = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				this.Username = profileData [0];
				this.TutorialStep = System.Convert.ToInt32(profileData [1]);
				this.IsAdmin = System.Convert.ToBoolean(System.Convert.ToInt32(profileData [2]));
				this.Money = System.Convert.ToInt32(profileData [3]);
				this.IdLanguage=System.Convert.ToInt32(profileData[4]);
				this.IdProfilePicture=System.Convert.ToInt32(profileData[5]);
				this.Id=System.Convert.ToInt32(profileData[6]);
				this.ToChangePassword=System.Convert.ToBoolean(System.Convert.ToInt32(profileData[7]));
				this.TrainingStatus=System.Convert.ToInt32(profileData[8]);
				this.Mail=profileData[9];
				this.CurrentDivision=new Division();
				this.CurrentDivision.Id=System.Convert.ToInt32(profileData[10]);
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
			this.TrainingAllowedCardType=9;
			this.TrainingPreviousAllowedCardType=9;
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
			this.TrainingAllowedCardType=4;
			this.TrainingPreviousAllowedCardType=0;
		}
		else if(TrainingStatus<59)
		{
			this.TrainingAllowedCardType=5;
			this.TrainingPreviousAllowedCardType=4;
		}
		else if(TrainingStatus<69)
		{
			this.TrainingAllowedCardType=6;
			this.TrainingPreviousAllowedCardType=5;
		}
		else if(TrainingStatus<79)
		{
			this.TrainingAllowedCardType=7;
			this.TrainingPreviousAllowedCardType=6;
		}
		else if(TrainingStatus<89)
		{
			this.TrainingAllowedCardType=8;
			this.TrainingPreviousAllowedCardType=7;
		}
		else if(TrainingStatus<99)
		{
			this.TrainingAllowedCardType=9;
			this.TrainingPreviousAllowedCardType=8;
		}
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
}



