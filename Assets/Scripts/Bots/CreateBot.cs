using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Bots
{
    public class CreateBot : MonoBehaviour
    {
        [SerializeField] private GameObject[] _BodyList;
        [SerializeField] private GameObject[] _WheelsList;
        [SerializeField] private GameObject[] _WeaponsList;
        [SerializeField] private GameObject _WeaponButtonsPlace;
        [SerializeField] private GameObject _weaponButton;
        private _Body _body = 0;
        private _Weapon[] _weapons;
        private _Wheels _wheels = 0;
        public void InitBody(int _option)
        {
            _body = (_Body) _option;
        }
        public void InitWheels(int _option)
        {
            _wheels = (_Wheels) _option;
        }
        public void InitWeapons(Dropdown dropdown)
        {
            ButtonTag buttonTag = dropdown.gameObject.GetComponent<ButtonTag>();
            if(buttonTag != null)
                _weapons[buttonTag.Tag] = (_Weapon)dropdown.value;
        }

        public void CreateWeaponsButtons()
        {
            Dropdown[] _dropdowns = _WeaponButtonsPlace.GetComponentsInChildren<Dropdown>();
            if(_dropdowns != null)
                foreach (var VARIABLE in _dropdowns)
                {
                    Destroy(VARIABLE.gameObject);
                }
            Body body = _BodyList[(int)_body].GetComponent<Body>();
            for (byte i = 0; i < body._numberOfWeapons; i++)
            {
                GameObject g = Instantiate(_weaponButton, _WeaponButtonsPlace.transform);
                g.GetComponent<ButtonTag>().Tag = i;
                Dropdown _dropdown =g.GetComponent<Dropdown>();
                _dropdown.onValueChanged.AddListener(delegate { InitWeapons(_dropdown);});
            }

            _weapons = new _Weapon[body._numberOfWeapons];
        }
        GameObject CreateBody(_Body body)
        {
            return _BodyList[(int) body];
        }
        GameObject CreateWheels(_Wheels wheels)
        {
            return _WheelsList[(int) wheels];
        }
        GameObject CreateWeapons(_Weapon weapon)
        {
            return _WeaponsList[(int) weapon];
        }
        public void CreateBotParts()
        {
            GameObject body = CreateBody(_body);
            body.GetComponent<Bot>()._body = body;
            body.GetComponent<Bot>()._weapons = new GameObject[body.GetComponent<Body>()._numberOfWeapons];
            body.GetComponent<Bot>()._wheels = CreateWheels(_wheels);
            for (int o = 0; o<body.GetComponent<Body>()._numberOfWeapons; o++)
            {
                body.GetComponent<Bot>()._weapons[o] = CreateWeapons(_weapons[o]);
            }
        
        }
    
    }
}

