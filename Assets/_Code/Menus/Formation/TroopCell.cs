using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BounceDudes
{
    public class TroopCell : MonoBehaviour
    {
        public Image SelectedImage = null;
        public Soldier Soldier = null;

        private bool _selected = false;

        private GameObject _representation;

        public void Start()
        {
            this.Soldier.transform.SetParent(this.transform);
            this.SelectedImage.enabled = false;
        }

        public void Select()
        {
            if (!Formation.Instance.HasSpace)
            {
                _selected = false;
                return;
            }

            _representation = Instantiate(this.Soldier.gameObject);
            _representation.GetComponent<Soldier>()._soldierName = Soldier._soldierName;
            Formation.Instance.AddToFormation(this.Soldier, _representation);
            this.SelectedImage.enabled = true;
            Formation.Instance.ShowName(Soldier._soldierName);
        }

        public void Unselect()
        {
            Formation.Instance.RemoveFromFormation(this.Soldier, _representation);
            _representation = null;
            this.SelectedImage.enabled = false;
            Formation.Instance.ShowName(string.Empty);
        }

        public void OnClick()
        {
			AudioManager.Instance.PlayInterfaceSound (0);
            _selected = !_selected;

            if (_selected)
                this.Select();
            else
                this.Unselect();
        }
    }
}
