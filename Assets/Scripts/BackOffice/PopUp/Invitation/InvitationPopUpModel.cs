using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class InvitationPopUpModel {
	
	public User player;
	public IList<Deck> decks;
	public Invitation invitation;
	public bool isInvitationStillExists;
	
	private string URLGetUserInvitationData = ApplicationModel.host+"get_user_invitation_data.php";
	
	public InvitationPopUpModel()
	{
	}
	
	public IEnumerator loadUserData()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());
		
		WWW w = new WWW(URLGetUserInvitationData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//Debug.Log(w.text);
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player = parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.decks = this.parseDecks(data[1].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
			if(data[2]!="-1")
			{
				this.isInvitationStillExists=true;
				this.invitation = parseInvitation(data[2].Split(new string[] { "//" }, System.StringSplitOptions.None));
			}
		}
	}
	private User parseUser(string[] array)
	{
		User player = new User ();
		player.SelectedDeckId = System.Convert.ToInt32(array [0]);
		return player;
	}
	private List<Deck> parseDecks(string[] decksData)
	{
		List<Deck> decks = new List<Deck> ();
		for(int i=0;i<decksData.Length-1;i++)
		{
			string[] deckInformation = decksData[i].Split(new string[] { "#DECKINFO#" }, System.StringSplitOptions.None);
			decks.Add (new Deck());
			decks[i].Id=System.Convert.ToInt32(deckInformation[0]);
			decks[i].Name=deckInformation[1];
			decks[i].NbCards=System.Convert.ToInt32(deckInformation[2]);
		}
		return decks;
	}
	private Invitation parseInvitation(string[] array)
	{
		Invitation invitation = new Invitation ();
		invitation.InvitedUser = this.player;
		invitation.SendingUser=new User();
		invitation.Id = System.Convert.ToInt32 (array [0]);
		invitation.SendingUser.Username = array [1];
		invitation.SendingUser.idProfilePicture = System.Convert.ToInt32(array [2]);
		invitation.SendingUser.CollectionRanking = System.Convert.ToInt32 (array [3]);
		invitation.SendingUser.RankingPoints = System.Convert.ToInt32 (array [4]);
		invitation.SendingUser.Ranking = System.Convert.ToInt32 (array [5]);
		invitation.SendingUser.TotalNbWins = System.Convert.ToInt32 (array [6]);
		invitation.SendingUser.TotalNbLooses = System.Convert.ToInt32 (array [7]);
		return invitation;
	}
}

