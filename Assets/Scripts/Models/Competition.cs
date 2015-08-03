using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Competition
{
	public int Id;
	public string Name;
	public string Picture;
	public Sprite texture;
	public bool isTextureLoaded;
	public int EarnXp_W;
	public int EarnXp_L;
	public int EarnCredits_W;
	public int EarnCredits_L;
	
	public Competition()
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
}



