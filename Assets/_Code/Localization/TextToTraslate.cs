using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class TextToTraslate : MonoBehaviour {
	
    Text texto;
	string originalTagText;

	// Use this for initialization
	void Awake() {
		
        if (texto == null)
        {
            texto = gameObject.GetComponent<Text>();
        }

		originalTagText = texto.text;
		this.Translate ();

    }

	public void Translate(bool usingOriginalText = false){
		
		if (texto != null) {
			if (!usingOriginalText)
				texto.text = SetLanguage.Instance.LanguageReference.getString (texto.text);
			else
				texto.text = SetLanguage.Instance.LanguageReference.getString (originalTagText);
		}
		else
			Debug.Log ("Text to be translated is NULL");
	}

	public void TranslateWithOneArgument(int value, bool usingOriginalText = false){
		if (texto != null){
			if (!usingOriginalText)
				texto.text = SetLanguage.Instance.LanguageReference.getStringWithOneArgument (texto.text, value);
			else
				texto.text = SetLanguage.Instance.LanguageReference.getStringWithOneArgument (originalTagText, value);
		}
		else
			Debug.Log ("Text to be translated is NULL");
	}
}
