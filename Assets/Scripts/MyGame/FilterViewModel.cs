using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FilterViewModel 
{
	public IList<string> matchValues;
	public IList<int> filtersCardType;

	public float minLifeVal;
	public float maxLifeVal;
	public float minAttackVal;
	public float maxAttackVal;
	public float minMoveVal;
	public float maxMoveVal;
	public float minQuicknessVal;
	public float maxQuicknessVal;
	public float minLifeLimit;
	public float maxLifeLimit;
	public float minAttackLimit;
	public float maxAttackLimit;
	public float minMoveLimit;
	public float maxMoveLimit;
	public float minQuicknessLimit;
	public float maxQuicknessLimit;
	public float oldMinLifeVal;
	public float oldMaxLifeVal;
	public float oldMinAttackVal;
	public float oldMaxAttackVal;
	public float oldMinMoveVal;
	public float oldMaxMoveVal;
	public float oldMinQuicknessVal;
	public float oldMaxQuicknessVal;
	public bool displayFilters;

	public GUIStyle[] styles;
	public GUIStyle filterTitleStyle;
	public GUIStyle toggleStyle;
	public GUIStyle filterTextFieldStyle;
	public GUIStyle myStyle;
	public GUIStyle smallPoliceStyle;

	public FilterViewModel()
	{
		minLifeVal            = 0;
		maxLifeVal            = 200;
		minAttackVal          = 0;
		maxAttackVal          = 100;
		minMoveVal            = 0;
		maxMoveVal            = 10;
		minQuicknessVal       = 0;
		maxQuicknessVal       = 100;
		minLifeLimit          = 0;
		maxLifeLimit          = 200;
		minAttackLimit        = 0;
		maxAttackLimit        = 100;
		minMoveLimit          = 0;
		maxMoveLimit          = 10;
		minQuicknessLimit     = 0;
		maxQuicknessLimit     = 100;
		oldMinLifeVal         = 0;
		oldMaxLifeVal         = 200;
		oldMinAttackVal       = 0;
		oldMaxAttackVal       = 100;
		oldMinMoveVal         = 0;
		oldMaxMoveVal         = 10;
		oldMinQuicknessVal    = 0;
		oldMaxQuicknessVal    = 100;
		displayFilters        = false;
	}
	public void initStyles()
	{
		filterTitleStyle          = styles[0];
		toggleStyle               = styles[1];
		filterTextFieldStyle      = styles[2];
		myStyle                   = styles[3];
		smallPoliceStyle          = styles[4];
	}
}
