using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.World.UnityGUI
{
    public class GUI_World_BoardView : MonoBehaviour, IPanelDestroyRestriction
    {
        [SerializeField]
        private Board _prefab = default;
        private Board _instanceBoard = null;

        private void InstantiateBoard()
        {
            if (_instanceBoard == null)
            {
                _instanceBoard = GameManager.Instance.RegisterBoard(_prefab);
            }
        }

        private void OnEnable()
        {
            if (_instanceBoard == null)
            {
                InstantiateBoard();
            }
            GameManager.Instance.SetCurrentActiveBoard(_instanceBoard);
            GameManager.Instance.GetSelectionManager().ResetSelection();
            GameManager.Instance.GetValidation().StartCalculator();
        }

        private void OnDestroy()
        {
            GameManager.Instance.RemoveBoard(_instanceBoard);
        }

        public event Action PanelViewDestroyedEvent;

        public void Destroy()
        {
            if (_instanceBoard.HasUnsavedChanges)
            {
                var message = GameManager.Instance.CreateMessageBox();
                var name = Path.GetFileNameWithoutExtension(_instanceBoard.SavePath);
                if (string.IsNullOrEmpty(name))
                {
                    name = "Unsaved World";
                }
                message.Init($"Do you want to save the changes you made in \"{name}\"?");
                message.OnSaveButtonClickedEvent.AddListener(SaveButtonClickedEventListener);
                message.OnDontSaveButtonClickedEvent.AddListener(DontSaveClickedEventListener);
            }
            else
            {
                PanelViewDestroyedEvent?.Invoke();
            }
        }

        private void DontSaveClickedEventListener()
        {
            PanelViewDestroyedEvent?.Invoke();
        }

        private void SaveButtonClickedEventListener()
        {
            if (GameManager.Instance.SaveCurrentWorld(_instanceBoard))
            {
                PanelViewDestroyedEvent?.Invoke();
            }
        }
    }

    public interface IPanelDestroyRestriction
    {
        event Action PanelViewDestroyedEvent;
        void Destroy();
    }
}
