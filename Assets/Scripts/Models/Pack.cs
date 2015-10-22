using UnityEngine;
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
	public Sprite texture;
	public bool isTextureLoaded;
	public string Name;
	public Cards Cards;
	public string Error;
	public IList<Skill> NewSkills;
	public int CollectionPoints;
	public int CollectionPointsRanking;

	private string URLBuyPack = ApplicationModel.host + "buyPack.php";

	
	public Pack()
	{
		this.texture = Sprite.Create (new Texture2D (1, 1, TextureFormat.ARGB32, false), new Rect (0, 0, 1, 1), new Vector2 (0.5f, 0.5f));
	}
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host+this.Picture);
		yield return www;
		Texture2D tempTexture=new Texture2D (1, 1, TextureFormat.ARGB32, false);
		www.LoadImageIntoTexture(tempTexture);
		this.texture=Sprite.Create (tempTexture, new Rect (0, 0, tempTexture.width, tempTexture.height), new Vector2 (0.5f, 0.5f));
		this.isTextureLoaded = true;
	}
	public IEnumerator buyPack(int cardType=-1)
	{
		this.Cards = new Cards ();
		this.NewSkills = new List<Skill> ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_Id", this.Id.ToString());	
		form.AddField("myform_cardtype", cardType.ToString());	
		
		WWW w = new WWW(URLBuyPack, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		Debug.Log (w.text);
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
				if(data[0]!="")
				{
					this.NewSkills=parseNewSkills(data[0].Split(new string[] { "#S#" }, System.StringSplitOptions.None));
				}
				this.Cards.parseCards(data[1]);
				this.CollectionPoints=System.Convert.ToInt32(data[2]);
				this.CollectionPointsRanking=System.Convert.ToInt32(data[3]);
			}
		}
	}
	public List<Skill> parseNewSkills(string[] array)
	{
		List<Skill> newSkills = new List<Skill>();
		for(int i = 0 ; i < array.Length-1 ; i++)
		{
			newSkills.Add(new Skill());
			newSkills[i].Name=array[i];
		}
		return newSkills;
	}
}



