﻿
public class PermuteCardPopUpPermuteButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PermuteCardPopUpController> ().permuteCardHandler ();	
	}
}
