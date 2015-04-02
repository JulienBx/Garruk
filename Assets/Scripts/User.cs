using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class User 
{
	public string Username;
	public string Mail;
	public string FirstName;
	public string Surname;
	public string Picture;
	public int Money;
	public List<Connection> Connections;
	public Division Division;
	public Cup Cup;
	public int NbGamesCup;
	public int Ranking;
	public int RankingPoints;
	public int TotalNbWins;
	public int TotalNbLooses;
	public IList<Result> ResultsHistory;

	public User()
	{
		this.Username = "";
		this.Picture = "";
	}

	public User(string username, string picture)
	{
		this.Username = username;
		this.Picture = picture;
	}
	public User(string username, string mail, int money, string firstname, string surname, string picture)
	{
		this.Username = username;
		this.Mail = mail;
		this.Money = money;
		this.FirstName = firstname;
		this.Surname = surname;
		this.Picture = picture;
	}
	public User(List<Connection> connections)
	{
		this.Connections = connections;
	}
	public User(Division division, IList<Result> resultshistory,  int rankingpoints,int ranking, int totalnbwins, int totalnblooses)
	{
		this.Division = division;
		this.ResultsHistory = resultshistory;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}
	public User(Cup cup, IList<Result> resultshistory,  int rankingpoints,int ranking, int totalnbwins, int totalnblooses)
	{
		this.Cup = cup;
		this.ResultsHistory = resultshistory;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}
	public User(IList<Result> resultshistory,  int rankingpoints,int ranking, int totalnbwins, int totalnblooses)
	{
		this.ResultsHistory = resultshistory;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}
	public User(string username, string picture, Division division, int rankingpoints,int ranking,  int totalnbwins, int totalnblooses)
	{
		this.Username = username;
		this.Picture = picture;
		this.Division = division;
		this.RankingPoints = rankingpoints;
		this.Ranking = ranking;
		this.TotalNbWins = totalnbwins;
		this.TotalNbLooses = totalnblooses;
	}

}



