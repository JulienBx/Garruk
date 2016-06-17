
public class InscriptionFacebookPopUpTouTextController : TextButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionFacebookPopUpController> ().touLinkHandler();
	}
	public override void setHoveredState ()
	{
		
	}
	public override void setInitialState ()
	{
		
	}
}
