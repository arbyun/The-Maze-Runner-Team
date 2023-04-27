using System;
using System.Collections.Generic;
using Dialogue.Data;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Dialogue.Models
{
    public static class NodeElementsUtilities
    {
        public static Button CreateButton(string title, Action onClick = null)
        {
            var button = new Button(onClick)
            {
                text = title
            };
            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            var foldout = new Foldout
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(
            this DialogueNode node,
            string portName = "",
            Orientation orientation = Orientation.Horizontal,
            Direction direction = Direction.Output,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            var port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;

            return port;
        }

        public static TextElement CreateTextElement(string value = null)
        {
            var textElement = new TextElement
            {
                text = value
            };

            return textElement;
        }

        public static TextField CreateTextField(
            string value = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null
        )
        {
            var textTextField = new TextField
            {
                value = value
            };

            if (onValueChanged != null) textTextField.RegisterValueChangedCallback(onValueChanged);

            return textTextField;
        }

        public static TextField CreateTextField(
            string value = null,
            string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null
        )
        {
            var textTextField = new TextField
            {
                label = label,
                value = value
            };

            if (onValueChanged != null) textTextField.RegisterValueChangedCallback(onValueChanged);

            return textTextField;
        }

        public static TextField CreateTextArea(
            string value = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null
        )
        {
            var textArea = CreateTextField(value, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }

        public static ToolbarMenu CreateDropDownMenu(string title = "")
        {
            return new ToolbarMenu { text = title };
        }

        public static void AppendCharacterAction(
            this ToolbarMenu toolbarMenu,
            List<CharacterData> characters,
            string savedCharacterId = null,
            Action<DropdownMenuAction> action = null
        )
        {
            if (string.IsNullOrEmpty(savedCharacterId))
            {
                toolbarMenu.text = characters[0].characterName;
            }
            else
            {
                var savedCharacter = characters.Find(c => c.id == savedCharacterId);
                toolbarMenu.text = savedCharacter.characterName;
            }

            foreach (var character in characters)
                toolbarMenu.menu.AppendAction(
                    character.characterName,
                    action,
                    a => DropdownMenuAction.Status.Normal,
                    character);
        }

        public static ObjectField CreateObjectField<T>(
            string title = "",
            EventCallback<ChangeEvent<Object>> onValueChanged = null
        )
        {
            var objectField = new ObjectField
            {
                objectType = typeof(T),
                label = title
            };

            if (onValueChanged != null)
                objectField.RegisterCallback(onValueChanged);

            return objectField;
        }
    }
}