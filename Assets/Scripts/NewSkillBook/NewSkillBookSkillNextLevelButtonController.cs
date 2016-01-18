public class NewSkillBookSkillNextLevelButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		gameObject.transform.parent.GetComponent<NewSkillBookSkillController> ().nextLevelHandler ();
	}
}