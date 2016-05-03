using UnityEngine;
using TMPro;

public class HasLeftRoomPopUpCloseButtonController : SpriteButtonController
{
    public override void mainInstruction ()
    {
        gameObject.transform.parent.GetComponent<HasLeftRoomPopUpController> ().exitPopUp ();   
    }
}

