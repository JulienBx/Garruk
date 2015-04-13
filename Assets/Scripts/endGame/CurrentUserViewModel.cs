using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurrentUserViewModel {

	public int Division;
	public int RankingPoints;
	public int Ranking;
	public int TotalNbWins;
	public int TotalNbLooses;

	public GUIStyle rankingLabelStyle;
	public GUIStyle yourRankingStyle;
	public GUIStyle yourRankingPointsStyle;
	
	public CurrentUserViewModel (User currentuser){

		this.Division = currentuser.Division;
		this.RankingPoints = currentuser.RankingPoints;
		this.Ranking = currentuser.Ranking;
		this.TotalNbWins = currentuser.TotalNbWins;
		this.TotalNbLooses = currentuser.TotalNbLooses;
		
	}
}
