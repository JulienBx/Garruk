using UnityEngine;
using System.Collections;

public class PaginationViewModel
{
	public GUIStyle[] paginatorGuiStyle;
	public int chosenPage;
	public int nbPages;
	public int pageDebut;
	public int pageFin;

	public GUIStyle[] styles;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;

	public PaginationViewModel()
	{
	}

	public void initStyles()
	{
		paginationStyle = styles [0];
		paginationActivatedStyle = styles [1];
	}
}
