using System;
using UnityEngine;

public class SkillBookScreenViewModel
{

	public int heightScreen;
	public int widthScreen;
	public float gapBetweenblocks;
	public float blockBookWidth;
	public float blockBookHeight;
	public float blockSkillsWidth;
	public float blockSkillsHeight;
	public float blockCTypeSelectedWidth;
	public float blockCTypeSelectedHeight;
	public float blockCTypesWidth;
	public float blockCTypesHeight;
	public float blockStatsWidth;
	public float blockStatsHeight;
	public float blockNextButtonWidth;
	public float blockNextButtonHeight;
	public float blockBackButtonWidth;
	public float blockBackButtonHeight;
	public Rect blockBook;
	public Rect blockSkills;
	public Rect blockCTypeSelected;
	public Rect blockCTypes;
	public Rect blockStats;
	public Rect blockNextButton;
	public Rect blockBackButton;
	
	public SkillBookScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		
	}
	public void resize()
	{
		heightScreen=Screen.height;
		widthScreen=Screen.width;
		
		this.gapBetweenblocks = 5;

		this.blockBookWidth = 0.80f * (this.widthScreen);
		this.blockBookHeight = 0.75f * (this.heightScreen - 3 * this.gapBetweenblocks);

		this.blockCTypeSelectedHeight = 0.15f * (this.blockBookHeight);

		this.blockSkillsWidth = 0.85f*this.blockBookWidth;
		this.blockSkillsHeight = 0.75f * (this.blockBookHeight);

		this.blockCTypesWidth = 0.1f * (this.widthScreen);
		this.blockCTypesHeight = 0.93f * this.blockBookHeight;

		this.blockStatsWidth = 1f*(this.widthScreen-2*this.gapBetweenblocks);
		this.blockStatsHeight = 0.12f * (this.heightScreen - 3 * this.gapBetweenblocks);

		this.blockNextButtonWidth = this.blockSkillsWidth * 0.03f;
		this.blockNextButtonHeight = this.blockCTypesHeight;

		this.blockBackButtonWidth = this.blockNextButtonWidth;
		this.blockBackButtonHeight = this.blockCTypesHeight;
		
		this.blockBook = new Rect (1f*(this.widthScreen-this.blockBookWidth)/2f,
		                           0.1f * this.heightScreen + this.gapBetweenblocks+0.03f*this.heightScreen,
		                           this.blockBookWidth,
		                           this.blockBookHeight);

		this.blockSkills = new Rect (this.blockBook.xMin+1.05f*(this.blockBookWidth-this.blockSkillsWidth)/2f,
		                             this.blockBook.yMin+this.blockCTypeSelectedHeight,
		                             this.blockSkillsWidth,
		                             this.blockSkillsHeight);

		this.blockCTypes = new Rect (this.blockBook.xMax-0.04f*this.blockBookWidth,
		                             this.blockBook.yMin+(this.blockBookHeight-this.blockCTypesHeight)/2f,
		                             this.blockCTypesWidth,
		                             this.blockCTypesHeight);

		this.blockStats = new Rect (this.gapBetweenblocks,
		                            this.blockBook.yMax+this.gapBetweenblocks,
		                            this.blockStatsWidth,
		                            this.blockStatsHeight);

		this.blockNextButton = new Rect (this.blockSkills.xMax,
		                                 this.blockCTypes.yMin,
		                                 this.blockNextButtonWidth,
		                                 this.blockNextButtonHeight);

	}
}

