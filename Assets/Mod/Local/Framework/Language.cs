using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Language : ScriptableObject {

	[Tooltip("Disabled languages are never chosen, eg. because they're still a work in progress.")]
	public bool Disabled = false;


	[Header("Language Information")]
	public string Iso639;
	[Space]

	public string Name;
	public string NativeName;
	[Space]

	[Tooltip("Useful for switching buttons around, etc.")]
	public bool RightToLeft = false;
	[Tooltip("The unity editor does not properly show right to left languages, showing them as left to right instead. If this happens on the actual text mesh too, the letters must be manually switched around 'siht ekil'. Tick this to make the mod do that for you. This will not change anything in the editor and it will maintain the incorrect left-to-right look. It is recommended to copy paste the texts into a text editor and edit them there. This does not do anything in the editor.")]
	public bool RTLFix = false;
	[Space]

	[Tooltip("Ticking this will swap the order of the Yes/No buttons. Used for right to left reading languages.")]
	public int Version = 1;
	public bool ManualAvailable = false;
	public string[] ManualLinks;
	[Space]
	[TextArea] public string TwitchHelpMessage = "!{0} twitch help message";

	[Header("Optional IETF BCP 47 Language Tag Information")]
	public string ExtLang = "";
	public Ietf.Scripts Script = Ietf.Scripts.Default;
	
	[Tooltip("UN M49 area codes.")]
	public Ietf.Regions LanguageRegion = Ietf.Regions.Default;
	[Tooltip("Two letter country code (ISO 3166-1). If set, overrides the Language Region set above.")]
	public string Iso3166 = "";
	public string Variant = "";
	public bool MachineTranslation = false;
	public string MachineUsed = "";
	public string[] AdditionalExtendedSubtags;
	public bool ShowVersionSubtag = false;
	public string[] AdditionalPrivateSubtags;
	[Space]
	public string IetfBcp47 = "";


	public abstract void Choose();

	public abstract string GetLabelFromEnglishName(string str);

	public abstract string GetLogFromEnglishName(string str);

	public abstract Sprite GetSpriteFromEnglishName(string str);

	public abstract int GetSizeFromEnglishName(string str);

	/// <summary>
	/// Reverses the characters of a string. Useful for when Unity fails to do so itself with RTL languages.
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	internal string Reverse(string str) {
		string[] splits = str.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
		for (int i = 0; i < splits.Length; i++) {
			IEnumerable<char> chars = splits[i].Reverse();
			splits[i] = new string(chars.ToArray());
		}
		return string.Join(Environment.NewLine, splits);
	}
}
