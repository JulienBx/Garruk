using UnityEngine;
using System.Collections;

public class SkillType
{
	public string Name;
	public int Id;
	public string Description;
	public int IdPicture;
	
	public SkillType()
	{
	}
    public int getPictureId()
    {
        return this.Id;
    }
}



