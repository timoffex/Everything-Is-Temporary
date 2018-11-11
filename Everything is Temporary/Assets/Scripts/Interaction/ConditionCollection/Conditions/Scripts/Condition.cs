using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : ScriptableObject
{
	public virtual bool isMet()
	{
		return false;
	}
}
