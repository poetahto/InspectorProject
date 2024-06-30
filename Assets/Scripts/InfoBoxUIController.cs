using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class InfoBoxUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Vector2 hoveredSize;
        public Vector2 normalSize;
        public float animationSpeed = 15;
        public float hiddenTextThreshold = 0.01f;
        public CanvasGroup shownText;
        public CanvasGroup hiddenText;

        public enum State
        {
            Shown, RevealingShownText, ChangingSize, RevealingHiddenText, Hidden,
        }

        private State _currentState;
        private bool _isHovered;

        private void Update()
        {
            RectTransform rt = (RectTransform) transform;
            
            switch (_currentState)
            {
                case State.Shown:
                {
                    if (!_isHovered)
                        ChangeState(State.RevealingShownText);
                    
                    break;
                }
                case State.RevealingShownText:
                {
                    float targetAlpha = _isHovered ? 1 : 0;
                    shownText.alpha = Mathf.Lerp(shownText.alpha, targetAlpha, animationSpeed * Time.deltaTime);

                    if (Mathf.Abs(shownText.alpha - targetAlpha) < hiddenTextThreshold)
                    {
                        shownText.alpha = targetAlpha;
                        ChangeState(_isHovered ? State.Shown : State.ChangingSize);
                    }
                    
                    break;
                }
                case State.ChangingSize:
                {
                    Vector2 targetSize = _isHovered ? hoveredSize : normalSize;
                    rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, targetSize, animationSpeed * Time.deltaTime);
                    
                    // check if we reached the target, and change state if so
                    if ((rt.sizeDelta - targetSize).sqrMagnitude < 0.1f)
                        ChangeState(_isHovered ? State.RevealingShownText : State.RevealingHiddenText);
                    
                    break;
                }
                case State.RevealingHiddenText:
                {
                    float targetAlpha = _isHovered ? 0 : 1;
                    hiddenText.alpha = Mathf.Lerp(hiddenText.alpha, targetAlpha, animationSpeed * Time.deltaTime);

                    if (Mathf.Abs(hiddenText.alpha - targetAlpha) < hiddenTextThreshold)
                    {
                        hiddenText.alpha = targetAlpha;
                        ChangeState(_isHovered ? State.ChangingSize : State.Hidden);
                    }
                    
                    break;
                }
                case State.Hidden:
                {
                    if (_isHovered)
                        ChangeState(State.RevealingHiddenText);
                    
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeState(State newState)
        {
            _currentState = newState;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
        }
    }
}
