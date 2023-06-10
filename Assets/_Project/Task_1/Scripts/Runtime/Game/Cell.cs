using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Task_1.Runtime.Game
{
    public class Cell : MonoBehaviour
    {

        const int RequiredMatch = 3;
        
        //variables 
        
        [SerializeField] private SpriteRenderer crossSpriteRenderer;
        [SerializeField] private int row, col;
        [SerializeField] private bool isActive;
        
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private UIManager _uiManager;
        
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
        public bool IsActive => isActive;

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
            ResetSpriteAlpha();
        }


        private void OnMouseDown()
        {
            OnCellClick();

        }

        private void OnCellClick()
        {
            if (isActive) return;
            SetCell(true);
            var neighbors= _gridManager.CheckNeighborhood(row,col);
            if (neighbors.Count < RequiredMatch) return;
            foreach (var neighbor in neighbors)
            {
                //added a small delay to see all checkmarks
                DOVirtual.DelayedCall(0.25f, () => neighbor.SetCell(false));
            }
            _uiManager.AddMatchCount();
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
            isActive = active;
           
            StartCoroutine(!isActive
                ? SpriteFade(crossSpriteRenderer, 0, .25f)
                : SpriteFade(crossSpriteRenderer, 1, .25f));
        }
    }
}