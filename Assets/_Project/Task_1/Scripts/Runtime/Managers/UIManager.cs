using System;
using Project.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Task_1.Runtime.Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI matchCountText;
        [SerializeField] private Button buttonGenerateGrid;
        [SerializeField] private TMP_InputField inputGridSize;
        
        private int _matchCount;
        private GridManager _gridManager;
        
        
        [Inject]
        private void OnInstaller(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        private void Start()
        {
            buttonGenerateGrid.onClick.AddListener(() => {
                _gridManager.GridSize = Convert.ToInt32(inputGridSize.text);
                _gridManager.GenerateGrid();
                _matchCount = 0;
                matchCountText.text = _matchCount.ToString();
            });
        }
        public void AddMatchCount()
        {
            _matchCount++;
            matchCountText.text = _matchCount.ToString();
        }
    }
}