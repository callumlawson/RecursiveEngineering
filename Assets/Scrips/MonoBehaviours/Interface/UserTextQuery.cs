using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scrips.MonoBehaviours.Interface
{
    public class UserTextQuery : MonoBehaviour
    {
        [UsedImplicitly] public GameObject TextQueryWindow;
        [UsedImplicitly] public InputField InputField;
        [UsedImplicitly] public Text PlaceholderText;

        public static UserTextQuery Instance;

        [UsedImplicitly]
        public void Start()
        {
            Instance = this;
            TextQueryWindow.SetActive(false);
        }

        public void GetTextResponse(string prompt, UnityAction<string> response)
        {
            InputField.onEndEdit.RemoveAllListeners();
            PlaceholderText.text = prompt;

            TextQueryWindow.SetActive(true);
            InputField.ActivateInputField();
            InputField.onEndEdit.AddListener(result =>
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    response(result);
                    TextQueryWindow.SetActive(false);
                }
            });
        }
    }
}
