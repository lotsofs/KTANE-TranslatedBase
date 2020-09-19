using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StickerGlobal : MonoBehaviour {

	[SerializeField] TextMesh _bigLabel;
	[SerializeField] TextMesh _blackBar;

	const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	/// <summary>
	/// Adds some aesthetics to the sticker
	/// </summary>
	public void GenerateText(string iso, int langVersion, string periodicTableSymbol) {
		_bigLabel.text = iso.ToUpperInvariant();

		string hexPTC = "";
		foreach (char c in periodicTableSymbol) {
			hexPTC += Convert.ToInt32(c).ToString("X");
		}

		string randomChars = "";
		for (int i = 0; i < 5; i++) {
			randomChars += chars[UnityEngine.Random.Range(0, chars.Length)];
		}

		// {1} is a random 5-digit alphanumeric code. Local version will only have 4 digits, to tell them apart.
		_blackBar.text = string.Format("TLM{0}.{1}.{2}{3}{4}", 
			hexPTC, 
			randomChars, 
			iso.ToUpperInvariant(), 
			UnityEngine.Random.Range(0, 1000).ToString("000"),
			langVersion.ToString()
		);
	}
}
