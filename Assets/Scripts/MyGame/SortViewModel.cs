using UnityEngine;
using System.Collections;

public class SortViewModel
{
	public GUIStyle[] sortButtonStyle;
	public int oldSortSelected;
	public int sortSelected;

	public GUIStyle[] styles;
	public GUIStyle sortDefaultButtonStyle;
	public GUIStyle sortActivatedButtonStyle;

	public SortViewModel()
	{
		oldSortSelected                         = 10;
		sortSelected                            = 10;
		sortButtonStyle                         = new GUIStyle[10];
	}

	public void initStyles()
	{
		sortDefaultButtonStyle                  = styles[0];
		sortActivatedButtonStyle                = styles[1];
	}
}
