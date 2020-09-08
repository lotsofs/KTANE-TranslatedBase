using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using System.Reflection;

public class ExtendedMissionSettings : MonoBehaviour {

	public static Dictionary<string, Dictionary<string, string>> RegisteredMissions = new Dictionary<string, Dictionary<string, string>>();
	
	[SerializeField] KMBombInfo _bombInfo;
	[HideInInspector] public static bool Ready = false;
	[HideInInspector] public static bool BombStarted = false;

	[HideInInspector] public Dictionary<string, string> CurrentMissionSettings = null;

	void Start() {
		Ready = true;
	}

	void Update() {
		// todo make this a courotine
		if (!BombStarted && _bombInfo.IsBombPresent()) {
			string missionName = GetMissionName();
			if (RegisteredMissions.ContainsKey(missionName)) {
				Debug.LogFormat("[Extended Mission Settings] Found {1} settings for mission {0}. ", missionName, RegisteredMissions[missionName].Count);
				CurrentMissionSettings = RegisteredMissions[missionName];
			}
			else {
				Debug.LogFormat("[Extended Mission Settings] No settings found for mission {0}", missionName);
				CurrentMissionSettings = new Dictionary<string, string>();
			}
			BombStarted = true;
		}
		else if (!_bombInfo.IsBombPresent() && BombStarted) {
			BombStarted = false;
			CurrentMissionSettings = null;
		}
	}

	string GetMissionName() {
		try {
			Component gameplayState = GameObject.Find("GameplayState(Clone)").GetComponent("GameplayState");
			Type type = gameplayState.GetType();
			FieldInfo fieldMission = type.GetField("MissionToLoad", BindingFlags.Public | BindingFlags.Static);
			string currentMission = fieldMission.GetValue(gameplayState).ToString();
			return currentMission;
		}
		catch (Exception e) {
			Debug.Log("[Extended Mission Settings] Ran into an issue when fetching the mission name.");
			Debug.Log(e.Message);
			return "unknown";
		}
	}
}
