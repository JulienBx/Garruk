using UnityEngine;
using TMPro;

public class NextLevelPopUpNextLevelIconController : SpriteButtonController 
{
	private int nbUpgrades;

	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingNextLevelPopUp.getReference(18),WordingNextLevelPopUp.getReference(19) + nbUpgrades.ToString() + WordingNextLevelPopUp.getReference(20));
	}
	public void setUpgrades(int upgrades)
	{
		this.nbUpgrades=upgrades;
	}
	public override void setHoveredState ()
	{
	}
	public override void setInitialState ()
	{
	}
}

