using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Task_1
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
            
            // initialize button
            buttonGenerateGrid.onClick.AddListener(() => {
                _gridManager.GridSize = Convert.ToInt32(inputGridSize.text);
                _gridManager.GenerateGrid();
                _matchCount = 0;
                matchCountText.text = _matchCount.ToString();
            });
        }
        
        // update match when a match found
        public void AddMatchCount()
        {
            _matchCount++;
            matchCountText.text = _matchCount.ToString();
        }
    }
}