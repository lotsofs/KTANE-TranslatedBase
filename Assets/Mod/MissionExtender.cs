using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionExtender : MonoBehaviour {

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
		StartCoroutine(CheckForMissions());
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
		Debug.Log("QQQQQ123412341234123412341234QQQasdfasdfasdfasdf");
		Debug.Log(fieldSettings.ToString());
		Dictionary<string, Dictionary<string, string>> settings = (Dictionary<string, Dictionary<string, string>>)fieldSettings.GetValue(ems);
		Debug.Log("QQQQQ One");
		Dictionary<string, string> extraSettings = new Dictionary<string, string>();
		Debug.Log("QQQQQ Two");
		extraSettings.Add("Alfa", "Bravo");
		extraSettings.Add("Charlie", "Delta");
		Debug.Log("QQQQQ Three");
		settings.Add("freeplay", extraSettings);
		Debug.Log("QQQQQQQQasdfasdfasdfasdf");
		fieldSettings.SetValue(ems, settings);
		Debug.Log("QQQQQQQDid it work");
	}

	IEnumerator CheckForMissions() {
		while (true) {
			while (!_bombInfo.IsBombPresent()) {
				yield return null;
			}
			GameObject gameObject = GameObject.Find("ExtendedMissionSettings(Clone)");
			if (gameObject == null) {
				Debug.Log("TODO: Something went wrong");
				yield break;
			}
			Component ems = gameObject.GetComponent("ExtendedMissionSettings");
			if (ems == null) {
				Debug.Log("TODO: Something went wrong");
				yield break;
			}
			Type type = ems.GetType();

			FieldInfo fieldReady;
			try {
				fieldReady = type.GetField("BombStarted", BindingFlags.Public | BindingFlags.Static);
			}
			catch (Exception e) {
				Debug.Log("TODO: Something went wrong");
				Debug.Log(e.Message);
				yield break;
			}
			bool ready = false;
			while (!ready) {
				ready = (bool)fieldReady.GetValue(ems);
				yield return null;
			}
			FieldInfo fieldSettings;
			try {
				fieldSettings = type.GetField("CurrentMissionSettings");
			}
			catch (Exception e) {
				Debug.Log("TODO: Something went wrong");
				Debug.Log(e.Message);
				yield break;
			}

			Dictionary<string, string> dict = (Dictionary<string, string>)fieldSettings.GetValue(ems);
			foreach (string d in dict.Keys) {
				Debug.LogFormat("QWEQWEQWE: {0} {1}", d, dict[d]);
			}
			while (ready) {
				ready = (bool)fieldReady.GetValue(ems);
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
