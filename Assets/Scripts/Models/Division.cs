using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Division : Competition
{
	public int NbGames;
	public int NbWinsForRelegation;
	public int NbWinsForPromotion;
	public int NbWinsForTitle;
	public int TitlePrize;
	public int PromotionPrize;
	
	public Division()
	{
		this.texture = Sprite.Create (new Texture2D (1, 1, TextureFormat.ARGB32, false), new Rect (0, 0, 1, 1), new Vector2 (0.5f, 0.5f));
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
}



