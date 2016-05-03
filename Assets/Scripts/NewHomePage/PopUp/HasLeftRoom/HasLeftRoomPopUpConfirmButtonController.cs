
public class HasLeftRoomPopUpConfirmButtonController : SimpleButtonController
{
    public override void mainInstruction ()
    {
        gameObject.transform.parent.GetComponent<HasLeftRoomPopUpController> ().exitPopUp();    
    }
}
