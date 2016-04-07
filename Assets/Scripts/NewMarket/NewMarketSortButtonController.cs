using UnityEngine;
using TMPro;

public class NewMarketSortButtonController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		NewMarketController.instance.sortButtonHandler (base.getId());	
	}
	public override void showToolTip ()
	{
		string string1="";
		string string2="";
		if(base.getId()%2==0)
		{
			string1=WordingFilters.getReference(25);
			string2=WordingFilters.getReference(26);
		}
		else
		{
			string1=WordingFilters.getReference(27);
			string2=WordingFilters.getReference(28);
		}
		BackOfficeController.instance.displayToolTip(string1,string2);
	}
}