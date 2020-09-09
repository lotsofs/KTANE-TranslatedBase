using System.Collections;
using UnityEngine;

public class ExampleModule : MonoBehaviour
{
    public KMSelectable[] buttons;

    // todo cleanup
    public ExtendedMissionSettingsReader emsr;
    public Renderer moduleBackdrop;

    int correctIndex;
    bool isActivated = false;

    void Start()
    {
        Init();
        moduleBackdrop.material.color = Color.yellow;

        StartCoroutine(SomeSettingsThing());
        GetComponent<KMBombModule>().OnActivate += ActivateModule;
    }

    IEnumerator SomeSettingsThing() {
        // todo: cleanup
        yield return new WaitForSeconds(5f);
        Debug.Log("AAAA");
        if (emsr.FoundSettings.ContainsKey("Color")) {
            Debug.Log("BBBB");
            Color col;
            Debug.Log("CCCC");
            bool yeah = ColorUtility.TryParseHtmlString(emsr.FoundSettings["Color"], out col);
            Debug.Log("DDDD");
            moduleBackdrop.material.color = col;
        }
        Debug.Log("EEEEE");
    }

    void Init()
    {
        correctIndex = Random.Range(0, 4);

        for(int i = 0; i < buttons.Length; i++)
        {
            string label = i == correctIndex ? "O" : "X";

            TextMesh buttonText = buttons[i].GetComponentInChildren<TextMesh>();
            buttonText.text = label;
            int j = i;
            buttons[i].OnInteract += delegate () { OnPress(j == correctIndex); return false; };
        }
    }

    void ActivateModule()
    {
        isActivated = true;
    }

    void OnPress(bool correctButton)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();

        if (!isActivated)
        {
            Debug.Log("Pressed button before module has been activated!");
            GetComponent<KMBombModule>().HandleStrike();
        }
        else
        {
            Debug.Log("Pressed " + correctButton + " button");
            if (correctButton)
            {
                GetComponent<KMBombModule>().HandlePass();
            }
            else
            {
                GetComponent<KMBombModule>().HandleStrike();
            }
        }
    }
}
