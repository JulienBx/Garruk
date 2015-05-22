using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class DisplayedTrophy 
{
	public Trophy Trophy;
	public string CompetitionName;
	public Texture2D texture;
	public string Picture;
	
	public DisplayedTrophy(Trophy trophy, string competitionname, string picture)
	{
		this.Trophy = trophy;
		this.CompetitionName = competitionname;
		this.Picture = picture;
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host+this.Picture);
		yield return www;
		www.LoadImageIntoTexture(this.texture);
	}
}