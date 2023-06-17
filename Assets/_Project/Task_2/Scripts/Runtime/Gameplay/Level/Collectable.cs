using System.Collections;
using System.Collections.Generic;
using _Project.Task2.Gameplay.Player;
using DG.Tweening;
using UnityEngine;

namespace _Project.Task2.Gameplay.Level
{
    public class Collectable : MonoBehaviour
    {
        private AudioSource _audioSource;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            transform.DOMoveY(transform.position.y + .5f, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            transform.DORotate(new Vector3(0, 90, 0), 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }
        private void OnTriggerEnter(Collider other)
        {
            bool overlapPlayer = other.TryGetComponent(out PlayerController player);
            if (!overlapPlayer) return;
            
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            _audioSource.Stop();
            _audioSource.Play();
            DOTween.Kill(transform);

            Destroy(gameObject, 2);
        }
    }

}
