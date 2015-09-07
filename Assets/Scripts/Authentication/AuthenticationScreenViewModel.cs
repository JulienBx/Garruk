using System;
using UnityEngine;

public class AuthenticationScreenViewModel
{
	public Rect mainBlock;
	public Rect upperBlock;
	
	public AuthenticationScreenViewModel ()
	{
	}
	public void initStyles()
	{
	}
	public void setMainBlock(Rect mainBlock)
	{
		this.mainBlock = mainBlock;
	}
	public void setUpperBlock(Rect upperBlock)
	{
		this.upperBlock = upperBlock;
	}
}

