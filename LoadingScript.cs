using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class LoadingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("LoadAdequateScene");
	}
	
	IEnumerator LoadAdequateScene()
	{
		yield return new WaitForSeconds(1);
		switch(GlobalVariables.SceneToLoad)
		{
		case 0:
			Application.LoadLevel("MainScene");
			break;
		case 1:
			Application.LoadLevel("GamePlay");
			break;
		case 2:
			Application.LoadLevel("GamePlay");
			break;
		case 3:
			Application.LoadLevel("GamePlay");
			break;
		}
		//0 - MainScene, 1 - GamePlay, 2 - TimeAttack, 3 - Championship,
	}

}
