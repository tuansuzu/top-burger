using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
/// <summary>
/// Name by Nemanja
/// </summary>
public class PowerFitter : MonoBehaviour {

	public GameObject canvas;
	public float defScaleValue=10000;

	void OnGUI () 
	{
		float scaleW= Screen.width ; 
		float scaleH= Screen.height ; 
		transform.localScale = (scaleW/scaleH)*
				defScaleValue*Vector3.one*1/canvas.GetComponent<RectTransform>().rect.height*1/canvas.GetComponent<RectTransform>().rect.width*1/canvas.transform.localScale.x;
	}
}
