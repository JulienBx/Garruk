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
	}

	public void addHorizontalBorder(int i, GameObject g){
		this.horizontalBorders[i] = g;
	}

}