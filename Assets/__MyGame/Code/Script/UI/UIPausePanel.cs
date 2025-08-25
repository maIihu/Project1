using System;
using UnityEngine;

namespace __MyGame.Code.Script.UI
{
    public class UIPausePanel : MonoBehaviour
    {
        private bool _isPaused;

        private void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void ControlPausePanel()
        {
            _isPaused = !_isPaused;
            this.gameObject.SetActive(_isPaused);
        }
    }
}