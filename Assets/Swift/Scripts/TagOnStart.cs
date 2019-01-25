using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Give the given tag to the gameObject on Start()
 */
public class TagOnStart : MonoBehaviour {

    public string Tag;

	void Start () {
        gameObject.tag = Tag;
	}

}
