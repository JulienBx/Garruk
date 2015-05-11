using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Cup 
{
	public int NbRounds;
	public int CupPrize;
	public int Id;
	public string Name;
	public string Picture;
	public Texture2D texture;
	
	public Cup()
	{
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public Cup(int id, int nbrounds, int cupprize, string name)
	{
		this.Id = id;
		this.NbRounds = nbrounds;
		this.CupPrize = cupprize;
		this.Name = name;
	}
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host+this.Picture);
		yield return www;
		www.LoadImageIntoTexture(this.texture);
	}
}



