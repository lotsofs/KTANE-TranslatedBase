using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionSettingExtender : MonoBehaviour {

	[SerializeField] KMBombInfo _bombInfo;
	bool _bombStarted = false;

	string GetMissionExtenderSettings () {
		Component ems = GameObject.Find("ExtendedMissionSettings(Clone)").GetComponent("ExtendedMissionSettings");
		Type type = ems.GetType();
		FieldInfo fieldMissions = type.GetField("Missions", BindingFlags.Public | BindingFlags.Static);
		string missionsTest = fieldMissions.GetValue(ems).ToString();

		return missionsTest;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(RegisterMissions());
	}

	//// Update is called once per frame
	//void Update () {
	//	if (_bombInfo.IsBombPresent() && !_bombStarted) {
	//		_bombStarted = true;
	//		Debug.LogFormat("Logging something: {0}", GetMissionExtenderSettings());
	//	}
	//	else if (!_bombInfo.IsBombPresent() && _bombStarted) {
	//		_bombStarted = false;
	//	}
	//}

	IEnumerator RegisterMissions() {
		yield return new WaitForSeconds(2f);
		// todo: Use the ems/s ready type here and wait for that.
		Component ems = GameObject.Find("ExtendedMissionSettings(Clone)").GetComponent("ExtendedMissionSettings");
		Type type = ems.GetType();
		FieldInfo fieldSettings = type.GetField("RegisteredMissions", BindingFlags.Public | BindingFlags.Static);
		Dictionary<string, Dictionary<string, string>> settings = (Dictionary<string, Dictionary<string, string>>)fieldSettings.GetValue(ems);
		Dictionary<string, string> extraSettings = new Dictionary<string, string>();
		extraSettings.Add("Color", "#0000FF");
		settings.Add("mod_ExtendedMissionSettings_ExampleMission", extraSettings);
		fieldSettings.SetValue(ems, settings);
	}


}
