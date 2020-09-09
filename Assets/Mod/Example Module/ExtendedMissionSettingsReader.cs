using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ExtendedMissionSettingsReader : MonoBehaviour {

	public KMBombInfo _bombInfo;
	public Dictionary<string, string> FoundSettings;

	// Use this for initialization
	void Start () {
		StartCoroutine(CheckForMissions());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator CheckForMissions() {
			// todo clean up
			yield return new WaitForSeconds(2.0f);
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
			FoundSettings = dict;
	}
}
