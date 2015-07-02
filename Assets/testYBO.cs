using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class testYBO : MonoBehaviour
{
	public static testYBO instance;
	public GameObject spriteNude;
	public Texture2D[] spritesTextures;

	private GameObject[] sprites;

	private Texture2D atlas;
	private Rect[] atlasRects;
	private int textureSize;

	void Start()
	{
		instance = this;
		this.textureSize = 8192;
		this.sprites=new GameObject[10];
		this.atlas = new Texture2D(this.textureSize, this.textureSize);
		this.atlasRects = atlas.PackTextures (spritesTextures, 2, this.textureSize);

		for(int i=0;i<this.sprites.Length;i++)
		{
			this.sprites[i] = Instantiate(this.spriteNude) as GameObject;
			this.sprites [i].GetComponent<SpriteRenderer> ().sprite = Sprite.Create(this.atlas,new Rect(this.atlasRects[i].x*atlas.width,this.atlasRects[i].y*atlas.height,this.atlasRects[i].width*atlas.width,this.atlasRects[i].height*atlas.height),new Vector2(0.5f,0.5f));;
			this.sprites[i].transform.position=new Vector3(-5+i,0,0);
		}

		Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");


		this.sprites[0].transform.FindChild("text").GetComponent<TextMesh>().color=Color.red;
		this.sprites[1].transform.FindChild("text").GetComponent<TextMesh>().color=Color.blue;
		TextMesh textMesh = this.sprites [0].transform.FindChild ("text").GetComponent<TextMesh> (); 
		textMesh.font = ArialFont;
		textMesh.GetComponent<Renderer>().material = ArialFont.material;
		textMesh.fontSize = 15;
		textMesh.fontStyle = FontStyle.Bold;
	}
}