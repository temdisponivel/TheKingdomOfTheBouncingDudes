using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace BounceDudes
{
    /// <summary>
    /// Class that handles a input field.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public int _id;
        public Troop _troop = null;

        public void OnEndEdit()
        {
            this._troop.EndEditName(this._id, this.GetComponent<InputField>().text);
            Debug.Log(this.GetComponent<InputField>().text);
            this.GetComponent<InputField>().text = "";
        }
    }
}