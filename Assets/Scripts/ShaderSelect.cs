using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSelect : MonoBehaviour
{
	public int a = 0;
	public int b = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		GameObject objA = GameObject.FindGameObjectWithTag("A");
		GameObject objB = GameObject.FindGameObjectWithTag("B");
		if (Input.GetKeyDown(KeyCode.N))
		{
			a = 1;
			SelectTag("A", a);
		}
		if(Input.GetKeyDown(KeyCode.M))
		{
			b = 1;
			SelectTag("B", b);
		}
		Shader.SetGlobalInt("_selected", 0);
    }

	void SelectTag(string tag, int selected)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		int numOfObjs = array.Length;
		for(int i = 0; i < numOfObjs; i++)
		{
			//selected = 1;
			//Shader.SetGlobalInt("_selected", selected);
		}
	}
}
