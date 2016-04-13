using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class InvitationPopUpModel {
	
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
		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());

		ServerController.instance.setRequest(URLGetUserInvitationData, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		if(ServerController.instance.getError()=="")
		{
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.decks = this.parseDecks(data[1].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
			if(data[2]!="-1")
			{
				this.isInvitationStillExists=true;
				this.invitation = parseInvitation(data[2].Split(new string[] { "//" }, System.StringSplitOptions.None));
			}
		}
		else
		{
			Debug.Log(ServerController.instance.getError());
		}
	}
	private void parseUser(string[] array)
	{
		ApplicationModel.player.SelectedDeckId = System.Convert.ToInt32(array [0]);
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
		invitation.InvitedUser = ApplicationModel.player;
		invitation.SendingUser=new User();
		invitation.Id = System.Convert.ToInt32 (array [0]);
		invitation.SendingUser.Username = array [1];
		invitation.SendingUser.IdProfilePicture = System.Convert.ToInt32(array [2]);
		invitation.SendingUser.CollectionRanking = System.Convert.ToInt32 (array [3]);
		invitation.SendingUser.RankingPoints = System.Convert.ToInt32 (array [4]);
		invitation.SendingUser.Ranking = System.Convert.ToInt32 (array [5]);
		invitation.SendingUser.TotalNbWins = System.Convert.ToInt32 (array [6]);
		invitation.SendingUser.TotalNbLooses = System.Convert.ToInt32 (array [7]);
		return invitation;
	}
}

