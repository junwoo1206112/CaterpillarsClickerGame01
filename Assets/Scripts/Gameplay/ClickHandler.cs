using UnityEngine;
using UnityEngine.Events;

namespace ClickerGame.Gameplay
{
    public class ClickHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string characterName = "Character";

        [Header("Events")]
        public UnityEvent OnClick;
        public UnityEvent<Vector3> OnClickWithPosition;

        private SpriteRenderer _spriteRenderer;
        private Vector3 _originalScale;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalScale = transform.localScale;

            if (OnClick == null)
                OnClick = new UnityEvent();

            if (OnClickWithPosition == null)
                OnClickWithPosition = new UnityEvent<Vector3>();
        }

        private void OnMouseDown()
        {
            HandleClick();
        }

        public void HandleClick()
        {
            OnClick?.Invoke();
            OnClickWithPosition?.Invoke(transform.position);

            PlayClickEffect();

            Debug.Log($"[{characterName}] Clicked!");
        }

        private void PlayClickEffect()
        {
            StopAllCoroutines();
            StartCoroutine(ClickAnimation());
        }

        private System.Collections.IEnumerator ClickAnimation()
        {
            float duration = 0.1f;
            float elapsed = 0f;

            Vector3 targetScale = _originalScale * 0.9f;

            while (elapsed < duration)
            {
                transform.localScale = Vector3.Lerp(_originalScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = _originalScale;
        }

        public void SetCharacterName(string name)
        {
            characterName = name;
        }
    }
}
