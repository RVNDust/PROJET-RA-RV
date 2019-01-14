using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class JsonConfiguration : MonoBehaviour {

	GameObject[] machines;

	string jsonString;
	string dataPath;
	string json;
	private Machine loadObject;


	// Use this for initialization
	void Start () {
		machines = GameObject.FindGameObjectsWithTag("Machine");
		dataPath = Application.dataPath + "/StreamingAssets/" + GetFileName();
		
	}


	public void SaveData()
	{
		
		MyList myList = new MyList();
		myList.machinelist = new List<Machine>();
		
		foreach (var machine in machines)
		{
			Machine machineData = new Machine();
			machineData.Name = machine.name;
			Debug.Log(machineData.Name);
			machineData.Position = machine.transform.position;
			machineData.Rotation = machine.transform.rotation;
			myList.machinelist.Add(machineData);
		
		}
		
		json = JsonUtility.ToJson(myList);
		Debug.Log(json);
		File.WriteAllText(dataPath, json);

	}

	public void LoadData()
	{

	}

	public string GetFileName()
	{
		DateTime localDate = DateTime.Now;
		string format = "yyyy MM dd – HH mm ss";

		String myDate = localDate.ToString(format);
		return myDate;
		
	}
	

}

[Serializable]
public class MyList
{
	public List<Machine> machinelist;
}

[Serializable]
public class Machine
{
	public string Name;
	public Vector3 Position;
	public Quaternion Rotation;
	
}