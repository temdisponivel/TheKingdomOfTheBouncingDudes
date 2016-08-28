using UnityEngine;
using System.Collections;
using System.IO;

#pragma warning disable 0414

public class SetLanguage : MonoBehaviour {

	static protected SetLanguage _instance = null;
	static public SetLanguage Instance { get { return SetLanguage._instance; } }

    public string Language = "English";
    public bool useSystemLanguage;
	private Idioma _languageReference;
	public Idioma LanguageReference { get { return _languageReference; } }
    public TextAsset textFile;

	protected string[] _supportedLanguages = new string[] {"English", "Portuguese"};

    // Use this for initialization
    void Awake () {

		if (SetLanguage.Instance == null)
		{
			SetLanguage._instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			GameObject.Destroy(this.gameObject);
			return;
		}


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

		_languageReference = new Idioma(textFile, Language, false);

	}

}
