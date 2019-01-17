using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JsonConfiguration : MonoBehaviour {

	GameObject[] machines;

	string jsonString;
	string dataPath;
	string json;
	private Machine loadObject;

	public GameObject LoadButton;
	public GameObject loadPanel;


	// Use this for initialization
	void Start () {
		machines = GameObject.FindGameObjectsWithTag("Machine");
		dataPath = Application.dataPath + "/StreamingAssets/" + GetDate() + ".json";
		LoadConfig();
		
	}


	public void SaveData()
	{

		ListOfMachine myList = new ListOfMachine();
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

	public void LoadData(string myButton)
	{
		Debug.Log(myButton);

		ListOfMachine myListofMachine = new ListOfMachine();
		myListofMachine.machinelist = new List<Machine>();
		string dataAsJson = File.ReadAllText(myButton);
		myListofMachine = JsonUtility.FromJson<ListOfMachine>(dataAsJson);
		
		foreach(var element in myListofMachine.machinelist)
		{
			GameObject myTarget = GameObject.Find(element.Name);
			myTarget.transform.position = element.Position;
			myTarget.transform.rotation = element.Rotation;
		}

	}

	public void LoadConfig()
	{

		string[] filePaths = Directory.GetFiles(Application.dataPath + "/StreamingAssets/", "*.json");
		foreach (string filePath in filePaths)
		{
			GameObject newButton = Instantiate(LoadButton) as GameObject;
			newButton.transform.SetParent(loadPanel.transform, false);
			newButton.name = filePath;
			newButton.GetComponentInChildren<Text>().text = filePath;
			var button = newButton.GetComponent<UnityEngine.UI.Button>();
			button.onClick.AddListener(() => LoadData(newButton.name));

		}
	}


	public string GetDate()
	{
		DateTime localDate = DateTime.Now;
		string format = "yyyy MM dd – HH mm ss";

		String myDate = localDate.ToString(format);
		return myDate;
		
	}
	

}

[Serializable]
public class ListOfMachine
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