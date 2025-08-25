using System;
using UnityEngine;

namespace __MyGame.Code.Script.UI
{
    public class UIPausePanel : MonoBehaviour
    {
        private bool _isVisible;

        private void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void Toggle()
        {
            _isVisible = !_isVisible;
            this.gameObject.SetActive(_isVisible);
        }
    }
}