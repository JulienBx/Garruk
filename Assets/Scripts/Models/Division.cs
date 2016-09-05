using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Division
{
	public int Id;
	public string Name;
	public int GamesPlayed;
	public int NbWins;
	public int NbLooses;
	public int Status;
	public int GameType;
	public int NbGames;
	public int NbWinsForRelegation;
	public int NbWinsForPromotion;
	public int NbWinsForTitle;
	public int TitlePrize;
	public int PromotionPrize;
	public int earnXp_W;
	public int earnXp_L;
	public int earnCredits_W;
	public int earnCredits_L;

	// Status pour division
	// - 1 -> Relégation (fin de saison)
	// 0 -> En cours
	// 1 -> Maintien (fin de saison)
	// 11 -> Maintien (vient juste d'être obtenu)
	// 12 -> Maintien (obtenu précédemment)
	// 10 -> Maintien (vient juste d'être obtenu + fin de saison)
	// 2 -> Promotion (fin de saison)
	// 21 -> Promotion (vient juste d'être obtenue)
	// 22 -> Promotion (obtenu précédemment)
	// 20 -> Promotion (vient juste d'être obtenu + fin de saison)
	// 3 -> Titre (fin de saison) + Promotion
	// 31 -> Titre (fin de saison)

	public Division()
	{
		this.GameType=11;
	}
	public int getPictureId()
	{
		if(this.GameType>10 && this.GameType<21)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
}



