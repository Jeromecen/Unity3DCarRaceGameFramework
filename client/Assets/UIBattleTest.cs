using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIBattleTest : MonoBehaviour {

	// Use this for initialization
	int mTestSceneID = -1;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChgScene()
	{
		mTestSceneID = mTestSceneID % 3;
		GameObject mapObj = GameObject.Find ("/Scene/map");
		if (mapObj != null) {
			mTestSceneID = mTestSceneID + 1;
            MapLogic mapScript = mapObj.GetComponent<MapLogic> ();
			mapScript.ChgMap (mTestSceneID);
		}
	}
}
