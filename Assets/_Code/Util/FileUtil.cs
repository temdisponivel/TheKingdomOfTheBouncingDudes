using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Code.Game;
using UnityEngine;

namespace BounceDudes
{
    public static class FileUtil
    {

        public static void WriteToPlayerPrefs<T>()
        {
            
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            Debug.Log(filePath);

            ES3.Save<T>(filePath, objectToWrite);
            
            //var objString = JsonUtility.ToJson(objectToWrite);
           // File.WriteAllText(filePath, objString, Encoding.UTF8);
            
            /*
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                Debug.Log(filePath);
                var objString = JsonUtility.ToJson(objectToWrite);
                File.WriteAllText(filePath, objString);
            }
            */
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromJsonFile<T>(string filePath)
        {
            return ES3.Load<T>(filePath);
            
           // string fileContent = File.ReadAllText(filePath, Encoding.UTF8);
          //  var jsonToReturn = JsonUtility.FromJson<T>(fileContent);
           // return jsonToReturn;
            
            /*
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                string fileContent = File.ReadAllText(filePath);
                var jsonToReturn = JsonUtility.FromJson<T>(fileContent);
                return jsonToReturn;
            }
            */
        }
    }
}
