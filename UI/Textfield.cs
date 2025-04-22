using UnityEngine;

namespace NastyMod_v2.UI
{
    /**
     * Textfield
     * 
     * A custom text field class for handling user input.
     * 
     * Author: nastycodes, darkness
     * Version: 1.0.0
     */
    public class Textfield
    {
        // Static field to keep track of the currently focused text field
        private static Textfield CurrentField = null;

        // Textfield properties
        private int ID;
        private string _Value;
        private bool IsFocused;
        private float LastInputTime;

        // Static ID counter for unique identification
        private static int NextID = 1000;

        // Constants
        private const float InputCooldown = .1f;
        private string LastInputCharacter = "";

        // Styles
        private GUIStyle Style;
        private GUIStyle DefaultStyle;

        // Value property
        public string Value
        {
            get => _Value;
            set => _Value = value ?? "";
        }

        /**
         * Constructor
         * 
         * Initializes a new instance of the Textfield class.
         * 
         * @param initialValue The initial value of the text field.
         * @param style The GUIStyle to use for the text field.
         * @return void
         */
        public Textfield(string initialValue = "", GUIStyle style = null)
        {
            _Value = initialValue ?? "";
            Style = style ?? GUI.skin.textField;

            DefaultStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 14,
                fixedHeight = 24,
            };

            var TextfieldBgDarkTexture = new Texture2D(1, 1);
            TextfieldBgDarkTexture.SetPixel(0, 0, new Color(0.09f, 0.09f, 0.09f));
            TextfieldBgDarkTexture.Apply();

            var TextfieldBgGrayTexture = new Texture2D(1, 1);
            TextfieldBgGrayTexture.SetPixel(0, 0, new Color(0.18f, 0.18f, 0.18f));
            TextfieldBgGrayTexture.Apply();

            DefaultStyle.normal.background = TextfieldBgDarkTexture;
            DefaultStyle.focused.background = TextfieldBgGrayTexture;
            DefaultStyle.hover.background = TextfieldBgGrayTexture;
            DefaultStyle.active.background = TextfieldBgGrayTexture;
            DefaultStyle.onNormal.background = TextfieldBgDarkTexture;
            DefaultStyle.onHover.background = TextfieldBgGrayTexture;
            DefaultStyle.onActive.background = TextfieldBgGrayTexture;
            DefaultStyle.onFocused.background = TextfieldBgGrayTexture;

            ID = NextID++;
        }

        /**
         * Implicit conversion operator
         * 
         * Allows implicit conversion from Textfield to string.
         * 
         * @param textField The Textfield instance.
         * @return The current value of the text field.
         */
        public static implicit operator string(Textfield textField)
        {
            return textField.Value;
        }

        /**
         * Draw
         * 
         * Draws the text field on the screen.
         * 
         * @param position The position to draw the text field at.
         * @return The current value of the text field.
         */
        public string Draw(Rect position)
        {
            return Draw(position, _Value, Style);
        }

        /**
         * Draw
         * 
         * Draws the text field on the screen with a specified text and style.
         * 
         * @param position The position to draw the text field at.
         * @param text The text to display in the text field.
         * @param style The GUIStyle to use for the text field.
         * @return The current value of the text field.
         */
        public string Draw(Rect position, string text, GUIStyle style)
        {
            Event current = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Keyboard);

            // Handle focus
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (position.Contains(current.mousePosition))
                    {
                        // Unfocus any previously focused field
                        if (CurrentField != null && CurrentField != this)
                        {
                            CurrentField.IsFocused = false;
                        }

                        GUIUtility.keyboardControl = controlID;

                        // Focus this field
                        IsFocused = true;
                        CurrentField = this;

                        current.Use();
                    }
                    else if (IsFocused)
                    {
                        // Clicked outside, unfocus this field
                        IsFocused = false;
                        if (CurrentField == this)
                        {
                            CurrentField = null;
                        }
                    }
                    break;

                case EventType.KeyDown:
                    if (IsFocused && GUIUtility.keyboardControl == controlID)
                    {
                        switch (current.keyCode)
                        {
                            case KeyCode.Backspace:
                                if (_Value.Length > 0)
                                {
                                    _Value = _Value.Substring(0, _Value.Length - 1);
                                    current.Use();
                                }
                                break;

                            case KeyCode.Return:
                            case KeyCode.KeypadEnter:
                            case KeyCode.Escape:
                                IsFocused = false;
                                GUIUtility.keyboardControl = 0;
                                CurrentField = null;
                                current.Use();
                                break;
                        }
                    }
                    break;

                case EventType.Layout:
                    if (IsFocused && CurrentField == this)
                    {
                        HandleTextInput(current);
                    }
                    break;
            }

            if (IsFocused)
            {
                var ActiveBgColor = new Color(0.09f, 0.09f, 0.09f);
                var ActiveBgTexture = new Texture2D(1, 1);
                ActiveBgTexture.SetPixel(0, 0, ActiveBgColor);
                ActiveBgTexture.Apply();
                style.normal.background = ActiveBgTexture;
                style.hover.background = ActiveBgTexture;
                style.active.background = ActiveBgTexture;
                style.focused.background = ActiveBgTexture;
                style.onNormal.background = ActiveBgTexture;
                style.onHover.background = ActiveBgTexture;
                style.onActive.background = ActiveBgTexture;
                style.onFocused.background = ActiveBgTexture;
            }
            else
            {
                style = DefaultStyle;
            }

            // Draw the field background
            GUI.Box(position, "", style);

            // Draw the text with cursor
            string displayText = _Value;
            if (IsFocused && (Time.time % 1f) < 0.5f)
            {
                displayText += "|"; // Blinking cursor
            }

            GUI.Label(position, displayText, style);

            return _Value;
        }

        /**
         * HandleTextInput
         * 
         * Handles text input for the text field.
         * 
         * @param current The current event.
         * @return void
         */
        private void HandleTextInput(Event current)
        {
            // Check if the character is a valid input
            if (current.character == '\0' || (!char.IsLetterOrDigit(current.character) && !char.IsWhiteSpace(current.character))) return;

            // Check for input cooldown and character
            if (((Time.time - LastInputTime) <= InputCooldown) && LastInputCharacter == current.character.ToString()) return;

            // Add the character to the value
            _Value += current.character;
            _Value = _Value.Trim();

            // Update the last input time and character
            LastInputTime = Time.time;
            LastInputCharacter = current.character.ToString();

            // Consume the event
            current.Use();
        }

        /**
         * DrawLayout
         * 
         * Draws the text field using GUILayout.
         * 
         * @param options The layout options for the text field.
         * @return The current value of the text field.
         */
        public string DrawLayout(GUILayoutOption[] options = null)
        {
            Rect rect = GUILayoutUtility.GetRect(40, 20, options ?? new GUILayoutOption[0]);
            return Draw(rect);
        }
    }
}
