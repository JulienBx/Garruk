using UnityEngine;
using TMPro;

public class NewSkillBookCardTypeFilterController : SpriteButtonController
{	
	void OnMouseUp()
	{
		NewSkillBookController.instance.cardTypeFilterHandler (base.getId());	
	}
}

