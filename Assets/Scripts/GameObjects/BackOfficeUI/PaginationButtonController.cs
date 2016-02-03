using UnityEngine;
using TMPro;

public class PaginationButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		gameObject.transform.parent.GetComponent<PaginationController> ().paginationHandler (base.getId ());
	}
}

