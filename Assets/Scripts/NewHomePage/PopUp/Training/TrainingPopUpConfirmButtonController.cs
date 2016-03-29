
public class TrainingPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<TrainingPopUpController> ().exitPopUp();	
	}
}
