using UnityEngine;
using TMPro;

public class NewFocusedCardCardTypeController : SpriteButtonController 
{

	public CardType c;
	
	public virtual void show()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto(this.c.getPictureId());
	}
	public virtual void setCardType(CardType c)
	{
		this.c = c;
		this.show ();
	}
	public override void setHoveredState()
	{
	}
	public override void setInitialState()
	{
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(this.c.Id),WordingCardTypes.getDescription(this.c.Id));
	}
}

