using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StickerGlobal : MonoBehaviour {

	[SerializeField] TextMesh _labelLang;
	[SerializeField] TextMesh _labelLang3;
	[SerializeField] TextMesh _labelScript;
	[SerializeField] TextMesh _labelRegion;
	[SerializeField] TextMesh _blackBarTop;
	[SerializeField] TextMesh _blackBarBottom;

	const string CHARS_ALPHANUMERIC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	const string CHARS_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	const string CHARS_NOTHEXADECIMAL = "GHIJKLMNOPQRSTUVWXYZ";

	public void GenerateText(string language, int langVersion, string periodicTableSymbol) {
		_labelLang.text = language;
		if (language.Length == 3) _labelLang.fontSize = 222;
		_labelScript.text = string.Empty;
		_labelRegion.text = string.Empty;

		TopBlackBar(language, string.Empty, periodicTableSymbol);
		BottomBlackBar(langVersion, string.Empty);
	}

	void GenerateText(string language, string region, string script, int langVersion, string periodicTableSymbol) {
		if (language.Length == 3) {
			_labelLang.gameObject.SetActive(false);
			_labelLang3.gameObject.SetActive(true);
			_labelLang = _labelLang3;
		}
		
		_labelLang.text = language;
		_labelScript.text = script;
		_labelRegion.text = region;

		TopBlackBar(language, region, periodicTableSymbol);
		BottomBlackBar(langVersion, script);
	}

	public void TopBlackBar(string language, string region, string periodicTableSymbol) {
		string hexPTC = "";
		foreach (char c in periodicTableSymbol) {
			hexPTC += Convert.ToInt32(c).ToString("X");
		}

		string randomChars = "";
		for (int i = 0; i < 10 - language.Length; i++) {
			randomChars += CHARS_ALPHANUMERIC[UnityEngine.Random.Range(0, CHARS_ALPHANUMERIC.Length)];
		}

		_blackBarTop.text = string.Format("{0}.{1}.{2}{3}{4}",
			hexPTC,
			randomChars,
			language.ToUpperInvariant(),
			region.ToUpperInvariant(),
			UnityEngine.Random.Range(0, 1000).ToString("000")
		);
	}

	public void BottomBlackBar(int langVersion, string script) {
		string randomChars = "";
		for (int i = script.Length; i < 5; i++) {
			randomChars += CHARS_ALPHABET[UnityEngine.Random.Range(0, CHARS_ALPHABET.Length)];
		}

		_blackBarBottom.text = string.Format("TLM:{0}{1}371{2}{3}{4}",
			randomChars,
			script.ToUpperInvariant(),
			UnityEngine.Random.Range(0, 0x1000).ToString("X3"),
			CHARS_NOTHEXADECIMAL[UnityEngine.Random.Range(0, CHARS_NOTHEXADECIMAL.Length)],
			langVersion
		);
	}
}
