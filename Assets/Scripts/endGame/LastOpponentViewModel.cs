using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LastOpponentViewModel {
	
	public string Username;
	public string Picture;
	public int Division;
	public int RankingPoints;
	public int Ranking;
	public int TotalNbWins;
	public int TotalNbLooses;
	public Texture2D profilePicture;
	public int profilePictureSize;

	public GUIStyle lastOpponentProfilePictureButtonStyle;
	public GUIStyle lastOpponentLabelStyle;
	public GUIStyle lastOponnentInformationsLabelStyle;
	public GUIStyle lastOponnentUsernameLabelStyle;
	public GUIStyle lastOpponentBackgroundStyle;

	
	public LastOpponentViewModel (User lastopponent){
		
		this.Username = lastopponent.Username;
		this.Division = lastopponent.Division;
		this.RankingPoints = lastopponent.RankingPoints;
		this.Ranking = lastopponent.Ranking;
		this.TotalNbWins = lastopponent.TotalNbWins;
		this.TotalNbLooses = lastopponent.TotalNbLooses;
		this.Picture = lastopponent.Picture;
		
	}
}
