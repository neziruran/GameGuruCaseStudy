using UnityEngine;
using UnityEngine.UI;
using _Project.Utilities;

namespace _Project.Main
{
    public class ButtonLoadMainMenu : MonoBehaviour
    {
        private Button _loadButton;
        private const int Index = 0;
        private void Awake()
        {
            _loadButton = GetComponent<Button>();
            ExtensionMethods.LoadSceneExtended(_loadButton,Index);   
        }
    }

}