using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Pack 
{
	public int Id;
	public int NbCards;
	public int CardType;
	public int Price;
	public bool OnHomePage;
	public bool New;
	public string Picture;
	public Texture2D texture;
	public string Name;
	public IList<Card> Cards;
	public string Error;

	private string URLBuyPack = ApplicationModel.host + "buyPack.php";

	
	public Pack()
	{
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host+this.Picture);
		yield return www;
		www.LoadImageIntoTexture(this.texture);
	}
	public IEnumerator buyPack(int cardType=-1)
	{
		this.Cards = new List<Card> ();
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_Id", this.Id.ToString());	
		form.AddField("myform_cardtype", cardType.ToString());	
		
		WWW w = new WWW(URLBuyPack, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.Error = w.error;
		} 
		else
		{
			string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.Error = data [0];
			if (this.Error == "")
			{
				for(int i=1;i<data.Length-1;i++)
				{
					this.Cards.Add(new Card());
					this.Cards[i-1]=parseCard(data [i].Split(new string[] { "\n" }, System.StringSplitOptions.None));
				}
			}
		}
	}
	private Card parseCard(string[] array)
	{
		Card card = new Card ();
		
		string[] cardInformation = array[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
		card.Id = System.Convert.ToInt32(cardInformation [0]);
		card.Title = cardInformation [1];
		card.Life = System.Convert.ToInt32(cardInformation [2]);
		card.Attack = System.Convert.ToInt32(cardInformation [3]);
		card.Speed = System.Convert.ToInt32(cardInformation [4]);
		card.Move = System.Convert.ToInt32(cardInformation [5]);
		card.ArtIndex = System.Convert.ToInt32(cardInformation [6]);
		card.IdClass = System.Convert.ToInt32(cardInformation [7]);
		card.TitleClass = cardInformation [8];
		card.LifeLevel = System.Convert.ToInt32(cardInformation [9]);
		card.MoveLevel = System.Convert.ToInt32(cardInformation [10]);
		card.SpeedLevel = System.Convert.ToInt32(cardInformation [11]);
		card.AttackLevel = System.Convert.ToInt32(cardInformation [12]);
		card.onSale = 0;
		card.Experience = 0;
		
		card.Skills = new List<Skill>();
		for (int i = 1; i < 5; i++)
		{         
			cardInformation = array [i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			card.Skills.Add(new Skill(cardInformation [0], //skillName
			                          System.Convert.ToInt32(cardInformation [1]), // idskill
			                          System.Convert.ToInt32(cardInformation [2]), // isactivated
			                          System.Convert.ToInt32(cardInformation [3]), // level
			                          System.Convert.ToInt32(cardInformation [4]), // power
			                          System.Convert.ToInt32(cardInformation [5]), // manaCost
			                          cardInformation [6])); // description
		}
		return card;
	}
}



