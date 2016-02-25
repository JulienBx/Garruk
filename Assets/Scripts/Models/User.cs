using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class User
{
	public string Username;
	public int Division;
	public int Id;
	public int Ranking;
	public int RankingPoints;
	public int TotalNbWins;
	public int TotalNbLooses;
	public int OnlineStatus; // 0 -> Offline, 1 -> Online, 2-> PlayingGame
	public int CollectionPoints;
	public int CollectionRanking;
	public int IdProfilePicture;
	public bool IsConnectedToPlayer;
	public Connection ConnectionWithPlayer;

	public User()
	{
		this.Username = "";
	}
}



