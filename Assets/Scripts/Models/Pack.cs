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
	public IList<Skill> NewSkills;
	public int CollectionPoints;

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
			if(w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.Error=errors[1];
			}
			else
			{
				this.Error="";
				string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				this.Cards=parseCard(data[0].Split(new string[] { "CARD" }, System.StringSplitOptions.None));
				this.CollectionPoints=System.Convert.ToInt32(data[1]);

				this.NewSkills=new List<Skill>();
				for(int i=0;i<this.Cards.Count;i++)
				{
					for(int j=0;j<this.Cards[i].Skills.Count;j++)
					{
						if(this.Cards[i].Skills[j].IsNew)
						{
							this.NewSkills.Add (this.Cards[i].Skills[j]);
						}
					}
				}
			}
		}
	}
	private IList<Card> parseCard(string[] array)
	{
		string[] cardData = null;
		string[] cardInformation = null;
		IList<Card> cards = new List<Card> ();

		for(int i=0;i<array.Length-1;i++)
		{
			cardData = array[i].Split(new string[] { "#S#" }, System.StringSplitOptions.None);
			for(int j = 0 ; j < cardData.Length-1 ; j++)
			{
				cardInformation = cardData[j].Split(new string[] { "//" }, System.StringSplitOptions.None); 
				if (j==0)
				{
					cards.Add(new Card ());
					cards[i].Id = System.Convert.ToInt32(cardInformation [0]);
					cards[i].Title = cardInformation [1];
					cards[i].Life = System.Convert.ToInt32(cardInformation [2]);
					cards[i].Attack = System.Convert.ToInt32(cardInformation [3]);
					cards[i].Speed = System.Convert.ToInt32(cardInformation [4]);
					cards[i].Move = System.Convert.ToInt32(cardInformation [5]);
					cards[i].ArtIndex = System.Convert.ToInt32(cardInformation [6]);
					cards[i].IdClass = System.Convert.ToInt32(cardInformation [7]);
					cards[i].TitleClass = cardInformation [8];
					cards[i].LifeLevel = System.Convert.ToInt32(cardInformation [9]);
					cards[i].MoveLevel = System.Convert.ToInt32(cardInformation [10]);
					cards[i].SpeedLevel = System.Convert.ToInt32(cardInformation [11]);
					cards[i].AttackLevel = System.Convert.ToInt32(cardInformation [12]);
					cards[i].NextLevelPrice = System.Convert.ToInt32(cardInformation [13]);
					cards[i].onSale = 0;
					cards[i].Experience = 0;
					cards[i].PercentageToNextLevel = 0;
					cards[i].ExperienceLevel = 0;
					cards[i].Skills = new List<Skill>();
				}
				else
				{       
					cards[i].Skills.Add(new Skill());
					cards[i].Skills[j-1].Name=cardInformation[0];
					cards[i].Skills[j-1].Id=System.Convert.ToInt32(cardInformation [1]);
					cards[i].Skills[j-1].IsActivated=System.Convert.ToInt32(cardInformation [2]);
					cards[i].Skills[j-1].Level=System.Convert.ToInt32(cardInformation [3]);
					cards[i].Skills[j-1].Power=System.Convert.ToInt32(cardInformation [4]);
					cards[i].Skills[j-1].ManaCost=System.Convert.ToInt32(cardInformation [5]);
					cards[i].Skills[j-1].Description=cardInformation [6];
					cards[i].Skills[j-1].IsNew=System.Convert.ToBoolean(System.Convert.ToInt32(cardInformation [7]));
				}
			}
		}
		return cards;
		
	}
}



