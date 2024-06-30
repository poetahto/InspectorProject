using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public static class WindowsCursorUtil
    {
        public enum WindowsCursor
        {
            StandardArrowAndSmallHourglass = 32650,
            StandardArrow = 32512,
            Crosshair = 32515,
            Hand = 32649,
            ArrowAndQuestionMark = 32651,
            IBeam = 32513,
            SlashedCircle = 32648,
            FourPointedArrowPointingNorthSouthEastAndWest = 32646,
            DoublePointedArrowPointingNortheastAndSouthwest = 32643,
            DoublePointedArrowPointingNorthAndSouth = 32645,
            DoublePointedArrowPointingNorthwestAndSoutheast = 32642,
            DoublePointedArrowPointingWestAndEast = 32644,
            VerticalArrow = 32516,
            Hourglass = 32514,
        }

        public static void ChangeCursor(WindowsCursor cursor)
        {
            SetCursor(LoadCursor(IntPtr.Zero , (int)cursor));
        }

        [DllImport("user32.dll", EntryPoint = "SetCursor")]
        private static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("user32.dll", EntryPoint = "LoadCursor")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
    }
    
    public class InputController : MonoBehaviour
    {
        public enum State
        {
            Idle, Orbiting, Interacting,
        }
        
        public CameraOrbitController orbitController;
        public Camera mainCamera;

        private readonly RaycastHit[] _results = new RaycastHit[100];
        private Interactable _currentInteractable;
        private Interactable _hoveredInteractable;
        private bool _hasHoveredInteractable;
        private State _currentState;

        private void OnGUI()
        {
            GUILayout.Label($"{_currentState.ToString()}");
        }

        public void StopInteracting()
        {
            ChangeState(State.Idle);
        }

        private void ChangeState(State newState)
        {
            switch (_currentState)
            {
                case State.Interacting:
                {
                    _currentInteractable.InteractEnd();
                    break;
                }
            }
            
            _currentState = newState;

            switch (newState)
            {
                case State.Interacting:
                {
                    _currentInteractable.InteractStart();
                    break;
                }
            }
        }
        
        private void Update()
        {
            // check to see if we are hovering over an interactable
            {
                Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                int hits = Physics.RaycastNonAlloc(mouseRay, _results);
                _hasHoveredInteractable = false;

                for (int i = 0; i < hits; i++)
                {
                    if (_results[i].collider.TryGetComponent(out Interactable interactable) && interactable.IsInteractable)
                    {
                        _hoveredInteractable = interactable;
                        _hasHoveredInteractable = true;
                        break;
                    }
                }
            }

            // todo: buggy
            // WindowsCursorUtil.ChangeCursor(_hasHoveredInteractable 
            //     ? WindowsCursorUtil.WindowsCursor.Hand 
            //     : WindowsCursorUtil.WindowsCursor.StandardArrow
            // );

            switch (_currentState)
            {
                case State.Idle:
                {
                    // check to see if we are clicking on an interactable
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (_hasHoveredInteractable)
                        {
                            _currentInteractable = _hoveredInteractable;
                            ChangeState(State.Interacting);
                        }
                        
                        // if we didnt hit an interactable, we want to start orbiting
                        if (_currentState != State.Interacting)
                            ChangeState(State.Orbiting);
                    }
                    break;
                }
                case State.Orbiting:
                {
                    // orbit the camera, if we release click go to idle
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                        ChangeState(State.Idle);
                    
                    orbitController.ProcessMouseInput();
                    break;
                }
                case State.Interacting:
                {
                    _currentInteractable.InteractUpdate();
                    
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                        ChangeState(State.Idle);
                    break;
                }
            }
        }
    }
}
