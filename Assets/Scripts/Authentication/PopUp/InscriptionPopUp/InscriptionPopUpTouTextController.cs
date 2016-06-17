
public class InscriptionPopUpTouTextController : TextButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionPopUpController> ().touLinkHandler();
	}
	public override void setHoveredState ()
	{
		
	}
	public override void setInitialState ()
	{
		
	}
}
