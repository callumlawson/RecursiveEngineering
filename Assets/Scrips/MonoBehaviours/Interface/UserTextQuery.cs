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

        public void Start()
        {
            Instance = this;
            TextQueryWindow.SetActive(false);
        }

        public void GetTextResponse(string prompt, UnityAction<string> response)
        {
            PlaceholderText.text = prompt;
            TextQueryWindow.SetActive(true);
            InputField.onEndEdit.RemoveAllListeners();
            InputField.onEndEdit.AddListener(response);
            InputField.onEndEdit.AddListener(result =>
                TextQueryWindow.SetActive(false)
            );
        }
    }
}
