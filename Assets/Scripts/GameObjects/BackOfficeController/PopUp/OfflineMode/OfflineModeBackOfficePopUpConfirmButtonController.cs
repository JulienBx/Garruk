
public class OfflineModeBackOfficePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<OfflineModeBackOfficePopUpController> ().reconnectHandler();
	}
}
