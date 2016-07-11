using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class TextToTraslate : MonoBehaviour {
	
    Text texto;

	// Use this for initialization
	void Awake () {
		
        if (texto == null)
        {
            texto = gameObject.GetComponent<Text>();
        }
			
		this.Translate ();

    }

	public void Translate(){
		if (texto != null)
			texto.text = Idioma.GetInstance().getString(texto.text);
	}

	public void TranslateWithOneArgument(int value){
		if (texto != null)
			texto.text = Idioma.GetInstance().getStringWithOneArgument(texto.text, value);
	}
}
