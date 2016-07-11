using UnityEngine;
using System.Collections;
using System.IO;

#pragma warning disable 0414

public class SetLanguage : MonoBehaviour {
	
    public string Language="English";
    public bool useSystemLanguage;
	private Idioma LMan;
    public TextAsset tt;

	protected string[] _supportedLanguages = new string[] {"English", "Portuguese"};

    // Use this for initialization
    void Awake () {

		bool langFoundInSupported = false;

        if (useSystemLanguage)
        {
            Language = Application.systemLanguage.ToString();
			for (var i = 0; i <= _supportedLanguages.Length-1; i++) 
			{
				if (Language != _supportedLanguages [i])
					continue;

				langFoundInSupported = true;
			}

			if (!langFoundInSupported)
				Language = _supportedLanguages [0];

        }

		LMan = new Idioma(tt, Language, false);

	}

}
