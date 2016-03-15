using UnityEngine;
using TMPro;

public class NewSkillBookCardTypeFilterController : SpriteButtonController
{	
	new void OnMouseUp()
	{
		NewSkillBookController.instance.cardTypeFilterHandler (base.getId());	
	}
}

