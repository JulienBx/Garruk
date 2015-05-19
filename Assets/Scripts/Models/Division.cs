using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Division 
{
	public int NbGames;
	public int NbWinsForRelegation;
	public int NbWinsForPromotion;
	public int NbWinsForTitle;
	public int TitlePrize;
	public int PromotionPrize;
	public int Id;
	public string Name;
	public string Picture;
	public Texture2D texture;
	public int EarnXp_W;
	public int EarnXp_L;
	public int EarnCredits_W;
	public int EarnCredits_L;

	public Division()
	{
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public Division(int id, int nbgames, int nbwinsforrelegation, int nbwinsforpromotion, int nbwinsfortitle, int titleprize, int promotionprize)
	{
		this.Id = id;
		this.NbGames = nbgames;
		this.NbWinsForRelegation = nbwinsforrelegation;
		this.NbWinsForPromotion = nbwinsforpromotion;
		this.NbWinsForTitle = nbwinsfortitle;
		this.TitlePrize = titleprize;
		this.PromotionPrize = promotionprize;
	}
	public Division(int id)
	{
		this.Id = id;
	}
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host+this.Picture);
		yield return www;
		www.LoadImageIntoTexture(this.texture);
	}
}



