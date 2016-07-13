using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Idioma  {
/*
The Lang Class adds easy to use multiple language support to any Unity project by parsing an XML file
containing numerous strings translated into any languages of your choice.  Refer to UMLS_Help.html and lang.xml
for more information.
*/
    private Hashtable strings;
    static string currentLang = "English";
    static Idioma lman;
    /*
	Initialize Lang class
	path = path to XML resource example:  Path.Combine(Application.dataPath, "lang.xml")
	language = language to use example:  "English"
	web = boolean indicating if resource is local or on-line example:  true if on-line, false if local
	
	NOTE:
	If XML resource is on-line rather than local do not supply the path to the path variable as stated above
	instead use the WWW class to download the resource and then supply the resource.text to this initializer
	
	Web Example:
	var wwwXML : WWW = new WWW("http://www.exampleURL.com/lang.xml");
	yield wwwXML;
		
	var LangClass : Lang = new Lang(wwwXML.text, currentLang, true)
	*/
    public Idioma(TextAsset reference, string language, bool web)
    {
        currentLang = language;
        lman = this;

        setLanguageReference(reference, language);
           
     
    }

    public Idioma(string path, string language,bool web)
	{
            currentLang = language;
            lman = this;
            if (!web)
            {
            setLanguageResource("lang", language);
            //setLanguage(path, language);
            }
            else {
                setLanguageWeb(path, language);
            
            }
    }

    public Idioma(string path, bool web)
    {
        lman = this;
        string language = currentLang;
        if (!web)
        {
            setLanguageResource("lang", language);
            //setLanguage(path, language);
        }
        else {
            setLanguageWeb(path, language);

        }
    }

    /*
    Use the setLanguage function to swap languages after the Lang class has been initialized.
    This function is called automatically when the Lang class is initialized.
    path = path to XML resource example:  Path.Combine(Application.dataPath, "lang.xml")
    language = language to use example:  "English"

    NOTE:
    If the XML resource is stored on the web rather than on the local system use the
    setLanguageWeb function
    */
    public void setLanguage(string path , string language)
        {
            XmlDocument xml= new XmlDocument();
            xml.Load(path);

            strings = new Hashtable();
            XmlElement element= xml.DocumentElement[language];
            if (element!=null)
            {
            IEnumerator elemEnum = element.GetEnumerator();
                while (elemEnum.MoveNext())
                {
                XmlElement xmlItem =(XmlElement) elemEnum.Current;
                    strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
                }
            }
            else {
                Debug.LogError("The specified language does not exist: " + language);
            }
        }

    public void setLanguageReference(TextAsset textAsset, string language)
    {
        
        if (textAsset == null)
        {
            Debug.LogError("erro nao achei o arquivo");
            return;
        }
        XmlDocument xml = new XmlDocument();
		xml.LoadXml(textAsset.text);

        strings = new Hashtable();
        XmlElement element = xml.DocumentElement[language];
        if (element != null)
        {
            IEnumerator elemEnum = element.GetEnumerator();
            while (elemEnum.MoveNext())
			{
				// Adjust made to ignore Comment Elements on the document.
				if (((XmlNode)elemEnum.Current).NodeType == XmlNodeType.Comment)
					continue;

                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }
        }
        else {
            Debug.LogError("The specified language does not exist: " + language);
        }
    }

    public void setLanguageResource(string namefile, string language)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(namefile);
        if (textAsset == null)
        {
            Debug.LogError("erro nao achei o arquivo"+ namefile);
            return;
        }
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);

        strings = new Hashtable();
        XmlElement element = xml.DocumentElement[language];
        if (element != null)
        {
            IEnumerator elemEnum = element.GetEnumerator();
            while (elemEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }
        }
        else {
            Debug.LogError("The specified language does not exist: " + language);
        }
    }

    /*
    Use the setLanguageWeb function to swap languages after the Lang class has been initialized
    and the XML resource is stored on the web rather than locally.  This function is called automatically
    when the Lang class is initialized.
    xmlText = String containing all XML nodes
    language = language to use example:  "English"

    Example:
    var wwwXML : WWW = new WWW("http://www.exampleURL.com/lang.xml");
    yield wwwXML;

    var LangClass : Lang = new Lang(wwwXML.text, currentLang)
    */
    public void setLanguageWeb(string xmlText , string language )
        {
            XmlDocument xml  = new XmlDocument();
            xml.Load(new StringReader(xmlText));

            strings = new Hashtable();
            XmlElement element = xml.DocumentElement[language];
            if (element!=null)
            {
                IEnumerator elemEnum = element.GetEnumerator();
                while (elemEnum.MoveNext())
                {
                    XmlElement xmlItem = (XmlElement)elemEnum.Current;
                    strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
                }
            }
            else {
                Debug.LogError("The specified language does not exist: " + language);
            }
        }

        /*
        Access strings in the currently selected language by supplying this getString function with
        the name identifier for the string used in the XML resource.

        Example:
        XML file:
        <languages>
            <English>
                <string name="app_name">Unity Multiple Language Support</string>
                <string name="description">This script provides convenient multiple language support.</string>
            </English>
            <French>
                <string name="app_name">Unité Langue Soutien Multiple</string>
                <string name="description">Ce script fournit un soutien multilingue pratique.</string>
            </French>
        </languages>

        JavaScript:
        var appName : String = langClass.getString("app_name");
        */
	public string getString(string name){
        if (strings == null)
        {
            return name;
        }

        if (!strings.ContainsKey(name)) {
			//Debug.Log("The specified string does not exist: " + name);
			return name;
		}
	
		return (string) strings[name];
	}

	public string getStringWithOneArgument(string name, int argument){
		if (strings == null)
		{
			return name;
		}

		if (!strings.ContainsKey(name)) {
			//Debug.Log("The specified string does not exist: " + name);
			return name;
		}
		string aux = (string)strings [name];
		aux = aux.Replace("{0}", argument.ToString());

		return aux;
	}

    public static Idioma GetInstance()
    {

        if (lman == null)
        {
            Debug.LogWarning("Carregando lingua ingles Padrao; Rode O jogo do inicio para carregar a lingua adequada");
			lman= new Idioma(Path.Combine(Application.dataPath, "lang.xml"), currentLang, false);
        }
        return lman;
    }


}
