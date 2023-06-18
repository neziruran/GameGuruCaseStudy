using UnityEngine;
using UnityEngine.UI;
using _Project.Utilities;

namespace _Project.Main
{
    public class ButtonLoadSecondTask : MonoBehaviour
    {
        private Button _loadButton;
        private const int Index = 2;
        private void Awake()
        {
            _loadButton = GetComponent<Button>();
            ExtensionMethods.LoadSceneExtended(_loadButton,Index);   
        }

    }
}