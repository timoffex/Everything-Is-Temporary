using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour {

	public virtual bool IsMet()
	{
		return isMet;
	}
	
	protected bool isMet;

}
