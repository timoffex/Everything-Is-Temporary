using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : Reaction {
	
	public GameObject audioGameObject;
	
	public override void React()
	{
		audioGameObject.GetComponent<AudioSource>().Play();
	}
	
}
