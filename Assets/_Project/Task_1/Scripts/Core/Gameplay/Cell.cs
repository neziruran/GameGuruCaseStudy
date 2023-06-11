using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Project.Task_1
{
    public class Cell : MonoBehaviour
    {
        // required match count defined as const because it wont change runtime
        const int RequiredMatch = 3;
        
        //variables 
        
        [SerializeField] private SpriteRenderer crossSpriteRenderer;
        [SerializeField] private int row, col;
        
        private bool _isActive;
        private GridManager _gridManager;
        private UIManager _uiManager;
        private List<Cell> _neighbors;


        //getters and setters
        public int Row
        {
            get => row;
            set => row = value;
        }

        public int Col
        {
            get => col;
            set => col = value;
        }
        public bool IsActive => _isActive;

        public GridManager GridManager
        {
            set => _gridManager = value;
        }

        public UIManager UIManager
        {
            set => _uiManager = value;
        }

        private void Start()
        {
            ResetSpriteAlpha(); // reset cross sprite from cells before game starts
        }
        
        /// <summary>
        /// The process that the game is handled my mouse down event
        /// </summary>
        private void OnMouseDown()
        {
            SetCellState(); // updating cell state when players click it 
            UpdateNeighbors(); // update neighbors when player click a cell in the grid
            CheckMatch(); // lastly check if is there's any match on the grid
        }

        private void SetCellState()
        {
            if (_isActive) return;
            SetCell(true); // activate cell
            
        }

        private void UpdateNeighbors()
        {
            _neighbors = _gridManager.CheckNeighborhood(row,col); // update neighbors 

        }

        private void CheckMatch()
        {
            if (HasMatched())
            {
                foreach (var neighbor in _neighbors)
                {
                    //added a small delay to see all checkmarks
                    DOVirtual.DelayedCall(0.25f, () => neighbor.SetCell(false));
                }

                _uiManager.AddMatchCount();
            }
        }

        private bool HasMatched()
        {
            _neighbors= _gridManager.CheckNeighborhood(row,col);  
            return _neighbors.Count >= RequiredMatch;
        }
        
        private void ResetSpriteAlpha()
        {
            var color = crossSpriteRenderer.color;
            crossSpriteRenderer.color = new Color(color.r, color.g, color.b, 0);
        }
        
        private IEnumerator SpriteFade(SpriteRenderer sr, float endValue, float duration)
        {
            float elapsedTime = 0;
            float startValue = sr.color.a;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
                yield return null;
            }
        }
        private void SetCell(bool active)
        {
            _isActive = active;
           
            StartCoroutine(!_isActive
                ? SpriteFade(crossSpriteRenderer, 0, .25f)
                : SpriteFade(crossSpriteRenderer, 1, .25f));
        }
    }
}