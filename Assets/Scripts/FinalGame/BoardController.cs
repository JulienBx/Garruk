using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController
{
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;

	public BoardController (int x, int y)
	{
		this.verticalBorders = new GameObject[x+1];
		this.horizontalBorders = new GameObject[y+1];
	}

	public void addVerticalBorder(int i, GameObject g){
		this.verticalBorders[i] = g;
		this.verticalBorders[i].name = "VB"+i;
	}

	public void addHorizontalBorder(int i, GameObject g){
		this.horizontalBorders[i] = g;
		this.horizontalBorders[i].name = "HB"+i;
	}

	public void resize(float realWidth, int width, int height){
		float scale = Mathf.Min(1f,realWidth/6.05f);
		for (int i = 0; i < height+1; i++){
			this.horizontalBorders[i].transform.localPosition = new Vector3(0f,(-height/2f)*scale+scale*i,0f);
		}
		
		for (int i = 0; i < width+1; i++){
			this.verticalBorders[i].transform.localPosition = new Vector3((-width/2f)*scale+scale*i, 0f, 0f);
			this.verticalBorders[i].transform.localScale = new Vector3(0.5f, scale, 1f);
		}
	}
}