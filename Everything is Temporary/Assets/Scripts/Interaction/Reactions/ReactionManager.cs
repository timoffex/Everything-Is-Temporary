using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour {
	
	public List<Reaction> reactions;
	
	public void PlayAllReactions()
	{
		foreach(Reaction reaction in reactions)
		{
			reaction.React();
		}
	}

}
