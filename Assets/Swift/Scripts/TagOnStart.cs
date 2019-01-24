using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagOnStart : MonoBehaviour {

    public string Tag;

	void Start () {
        gameObject.tag = Tag;
	}

}
