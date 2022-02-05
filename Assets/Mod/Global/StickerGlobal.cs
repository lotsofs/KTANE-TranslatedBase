using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StickerGlobal : MonoBehaviour {


	[SerializeField] TextMesh _blackBarTop;
	[SerializeField] TextMesh _blackBarBottom;

	[SerializeField] StickerLayout[] _stickerLayouts;

	const string CHARS_ALPHANUMERIC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	const string CHARS_NUMERIC = "0123456789";
	const string CHARS_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	const string CHARS_ALPHABETLOWER = "abcdefghijklmnopqrstuvwxyz";
	const string CHARS_NOTHEXADECIMAL = "GHIJKLMNOPQRSTUVWXYZ";

	// todo: with a valid ietftag provided, none of these parameters should be needed other than the periodic table symbol
	void GenerateText(string ietfTag, string periodicTableSymbol) {
		if (ietfTag == "-") {
			// fallback language
			_stickerLayouts[0].Language.text = "-";
			_stickerLayouts[0].gameObject.SetActive(true);
			return; 
		}

		// dissect the tag https://tools.ietf.org/rfc/bcp/bcp47.txt
		var subtags = ietfTag.ToLowerInvariant().Split(new[] { '-' });
		foreach (string subtag in subtags) {
			if (subtag.Length > 8) {
				Debug.LogErrorFormat("[Translation Service] Module {0} provided langtag {1} which has subtag {2} of length {3}. Subtags may not have a length greater than 8",
					periodicTableSymbol, ietfTag, subtag, subtag.Length);
			}
		}
		int index = 0;

		string language = "";
		List<string> extlang = new List<string>();
		string script = "";
		string region = "";
		List<string> variant = new List<string>();
		string machineTranslation = "";
		string privateVariant = "";
		string version = "";
		bool untranslatedContent = false;

		// language, non-optional
		language = subtags[index];
		index++; if (index > subtags.Length) goto Sticker;
		// extlang, which will always be 3 alpha
		for (int i = 0; i < 3; i++) {
			if (subtags[index].Length == 3 && CHARS_ALPHABETLOWER.Contains(subtags[index][0])) {
				extlang.Add(subtags[index]);
				index++; if (index > subtags.Length) goto Sticker;
			}
			else {
				break;
			}
		}
		// script, 4 alpha
		if (subtags[index].Length == 4 && CHARS_ALPHABETLOWER.Contains(subtags[index][0])) {
			script = subtags[index];
			index++; if (index > subtags.Length) goto Sticker;
		}
		// region, 2 alpha or 3 digit
		bool twoAlpha = subtags[index].Length == 2 && CHARS_ALPHABETLOWER.Contains(subtags[index][0]);
		bool threeDigit = subtags[index].Length == 3 && CHARS_NUMERIC.Contains(subtags[index][0]);
		if (twoAlpha || threeDigit) {
			region = subtags[index];
			index++; if (index > subtags.Length) goto Sticker;
		}
		// variant, alphanumeric, max 8. first character a digit = minimum of 4, first char alpha = min of 5
		for (int fallback = 100; fallback > 0; fallback--) {
			bool fiveAlpha = subtags[index].Length >= 5 && CHARS_ALPHABETLOWER.Contains(subtags[index][0]);
			bool fourDigit = subtags[index].Length >= 4 && CHARS_NUMERIC.Contains(subtags[index][0]);
			if (fiveAlpha || fourDigit) {
				variant.Add(subtags[index]);
				index++; if (index > subtags.Length) goto Sticker;
			}
			else {
				break;
			}
		}
		// extension, starting with a singleton
		if (subtags[index].Length > 1) {
			Debug.LogErrorFormat("[Translation Service] Module {0} provided langtag {1} which has subtag {2} of length {3}. However, expected a singleton at this position",
				periodicTableSymbol, ietfTag, subtags[index], subtags[index].Length);
			index++; if (index > subtags.Length) goto Sticker;
		}
		else {
			Singleton:
			switch (subtags[index]) {
				case "t":
					// transformed content
					index++; if (index > subtags.Length) goto Sticker;
					TransformedContent:
					for (int fallback = 100; fallback > 0; fallback--) {
						if (subtags[index].Length == 1) goto Singleton;
						switch (subtags[index]) {
							case "t0":
								// machine translated content
								machineTranslation = "und";
								index++; if (index > subtags.Length) goto Sticker;
								if (subtags[index].Length == 1) goto Singleton;
								else {
									machineTranslation = subtags[index];
									index++; if (index > subtags.Length) goto Sticker;
								}
								if (subtags[index].Length == 1) goto Singleton;
								else {
									// TODO: we didn't expect more subtags related to machine translation. This also isn't a singleton, check as a key.
									goto TransformedContent;
								}
							case "m0":
							case "s0":
							case "d0":
							case "i0":
							case "k0":
							case "h0":
							case "x0":
								// TODO: not supported subtag
								index++; if (index > subtags.Length) goto Sticker;
								break;
							default:
								// TODO: unexpected subtag
								index++; if (index > subtags.Length) goto Sticker;
								break;
						}
					}
					break;
				case "u":
					// TODO: not supported extension
					index++; if (index > subtags.Length) goto Sticker;
					break;
				case "x":
					// private use
					index++; if (index > subtags.Length) goto Sticker;
					goto PrivateUse;
				default:
					// TODO: unexpected extension
					index++; if (index > subtags.Length) goto Sticker;
					break;
			}
		}
		// private use
		PrivateUse:
		for (int fallback = 100; fallback > 0; fallback--) {
			if (subtags[index].Length > 1 && subtags[index][0] == 'v' && CHARS_NUMERIC.Contains(subtags[index][1])) {
				version = subtags[index];
				index++; if (index > subtags.Length) goto Sticker;
				continue;
			}
			switch (subtags[index]) {
				case "offc":
				case "nvml":
				case "orig":
					privateVariant = subtags[index];
					break;
				case "untrc":
					untranslatedContent = true;
					break;
				default:
					// todo: unknown private use subtag
					break;
			}
			index++; if (index > subtags.Length) goto Sticker;
		}


	Sticker:
		// plop unto the sticker
		List<StickerLayout> layouts = _stickerLayouts.ToList();

		if (!string.IsNullOrEmpty(script)) {
			layouts.RemoveAll(item => item.Script == null);
		}
		
		if (!string.IsNullOrEmpty(region)) {
			layouts.RemoveAll(item => item.Region == null);
			if (region.Length == 3) {
				layouts.RemoveAll(item => item.RegionLength == 2);
			}
		}

		if (!string.IsNullOrEmpty(version)) {
			layouts.RemoveAll(item => item.Version == null);
		}

		if (language.Length == 3) {
			layouts.RemoveAll(item => item.LanguageLength == 2);
		}

		StickerLayout usedLayout;
		if (layouts.Count > 0) {
			usedLayout = layouts[0];
		}
		else {
			usedLayout = _stickerLayouts[0];
		}

		usedLayout.Language.text = language;
		if (usedLayout.Script) usedLayout.Script.text = script;
		if (usedLayout.Region) usedLayout.Region.text = region;
		if (usedLayout.Version) usedLayout.Version.text = "v" + version.ToString();
		usedLayout.gameObject.SetActive(true);

		TopBlackBar(periodicTableSymbol);
		BottomBlackBar(ietfTag);
	}

	public void TopBlackBar(string periodicTableSymbol) {
		string hexPTC = "";
		foreach (char c in periodicTableSymbol) {
			hexPTC += Convert.ToInt32(c).ToString("X");
		}

		string randomChars = "";
		randomChars += CHARS_ALPHANUMERIC[UnityEngine.Random.Range(0, CHARS_ALPHANUMERIC.Length)];
		for (int i = 0; i < 13 - hexPTC.Length; i++) {
			randomChars += CHARS_ALPHANUMERIC[UnityEngine.Random.Range(0, CHARS_ALPHANUMERIC.Length)];
		}

		_blackBarTop.text = string.Format("TLM:{0}.{1}.{2}{3}",
			hexPTC,
			randomChars,
			periodicTableSymbol[0].ToString().ToUpperInvariant(),
			UnityEngine.Random.Range(0, 10000).ToString("0000")
		);
	}

	public void BottomBlackBar(string fullIetf) {
		string randomChars = "";
		for (int i = fullIetf.Length; i < 32; i++) {
			randomChars += CHARS_ALPHANUMERIC[UnityEngine.Random.Range(0, CHARS_ALPHANUMERIC.Length)];
		}

		_blackBarBottom.text = string.Format("{0} ● {1}",
			fullIetf,
			randomChars
		).Substring(0, 34);
	}
}
