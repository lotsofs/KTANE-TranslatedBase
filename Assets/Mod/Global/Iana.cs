using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;

public class Iana : MonoBehaviour {

	public class IanaSubtag {
		public string Type;
		public string Subtag;
		public List<string> Anglonym = new List<string>();
		public List<string> Autonym = new List<string>();
		public string SuppressScript;
		public bool RightToLeft;
		public string Scope;    // macrolanguage, special, collection
		public bool Deprecated;
		public string PreferredValue;
		public string Macrolanguage;
		public string Comments;
		public List<string> Prefix;
	}

	public TextAsset file;
	public List<IanaSubtag> languages = new List<IanaSubtag>();
	public List<IanaSubtag> extlangs = new List<IanaSubtag>();
	public List<IanaSubtag> scripts = new List<IanaSubtag>();
	public List<IanaSubtag> regions = new List<IanaSubtag>();
	public List<IanaSubtag> variants = new List<IanaSubtag>();

	public void Start() {
		ParseFile();
	}

	public void ParseFile() {
		//StreamReader reader = File.OpenText()
		DateTime start = DateTime.Now;
		Debug.Log("Starting!");
		string[] items = file.text.Split(new string[] { "%%" }, StringSplitOptions.RemoveEmptyEntries);
		//StartCoroutine(Dump(items));
		//return;
		foreach (string item in items) {
			string[] subItems = item.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			IanaSubtag subtag = new IanaSubtag();
			foreach (string si in subItems) {
				int charLoc = si.IndexOf(": ", StringComparison.Ordinal);
				if (charLoc == -1) continue;

				switch (si.Substring(0, charLoc)) {
					case "Type":
						subtag.Type = si.Substring(charLoc + 2);
						//if (subtag.Type.StartsWith("language")) {
						//	type = -1;
						//	continue;
						//}
						break;
					case "Subtag":
						subtag.Subtag = si.Substring(charLoc + 2);
						break;
					case "Description":
						subtag.Anglonym.Add(si.Substring(charLoc + 2));
						break;
					case "Suppress-Script":
						subtag.SuppressScript = si.Substring(charLoc + 2);
						break;
					case "Scope":
						subtag.Scope = si.Substring(charLoc + 2);
						break;
					case "Added":
					case "Tag":
					case "File-Date":
						// do nothing
						break;
					case "Deprecated":
						subtag.Deprecated = true;
						break;
					case "Preferred-Value":
						subtag.PreferredValue = si.Substring(charLoc + 2);
						break;
					case "Macrolanguage":
						subtag.Macrolanguage = si.Substring(charLoc + 2);
						break;
					case "Prefix":
						subtag.Prefix.Add(si.Substring(charLoc + 2));
						break;
					case "Comments":
						subtag.Comments = si.Substring(charLoc + 2);
						break;
					default:
						Debug.Log(si);
						Debug.Log(item);
						continue;
				}
			}
			if (!string.IsNullOrEmpty(subtag.Subtag)) {
				if (subtag.Type.StartsWith("language")) {
					languages.Add(subtag);
				}
				else if (subtag.Type.StartsWith("extlang")) {
					extlangs.Add(subtag);
				}
				else if (subtag.Type.StartsWith("script")) {
					scripts.Add(subtag);
				}
				else if (subtag.Type.StartsWith("region")) {
					regions.Add(subtag);
				}
				else if (subtag.Type.StartsWith("variant")) {
					variants.Add(subtag);
				}
				else {
					Debug.Log(subtag.Type);
				}
			}
		}
		//DoneReadingLanguages:
		Debug.LogFormat("[Translation Service] Processed {0} languages in {1} seconds", languages.Count, (DateTime.Now - start).TotalSeconds);
		//StartCoroutine(Dump(null));
	}

	public IEnumerator Dump(string[] items) {
		foreach (IanaSubtag l in languages) {
			Debug.LogFormat("{0}, {1}, {2}, {3}", l.Subtag, l.Anglonym[0], l.SuppressScript, l.Scope);
			yield return null;
		}
	}
}
