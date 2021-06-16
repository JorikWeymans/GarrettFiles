//Created by Jorik Weymans 2021

using UnityEngine;
using Garrett.UI;

namespace Garrett
{
    public sealed class EndHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.GetInstance().GameOver();
                ServiceLocator.UIService.OpenPanel(PanelNames.END_PANEL);
            }
        }
    }
}
