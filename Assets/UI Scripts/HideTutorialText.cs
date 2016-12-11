using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTutorialText : MonoBehaviour
{
	void Start ()
    {
        if (PersistentData.CurrentLevel == 1)
        {
            MeshRenderer renderer = this.GetComponent<MeshRenderer>();
            renderer.enabled = true;
        }	
	}
}
