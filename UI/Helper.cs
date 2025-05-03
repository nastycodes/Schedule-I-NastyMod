using System.Collections.Generic;
using UnityEngine;

namespace NastyMod_v2.UI
{
    /**
     * Helper
     * 
     * This is the helper class for the ui elements.
     * 
     * Author: nastycodes
     * Version: 1.0.0
     */
    public class Helper
    {
        /**
         * AddLabel
         * 
         * Adds a label to the UI.
         * 
         * @param Text The text to display on the label.
         * @param LabelStyle The style of the label.
         * @return void
         */
        public static void AddLabel(string Text, GUIStyle LabelStyle)
        {
            GUILayout.Label(Text, LabelStyle);
        }

        /**
         * AddButton
         * 
         * Adds a button to the UI.
         * 
         * @param Text The text to display on the button.
         * @param ButtonStyle The style of the button.
         * @param Function The function to call when the button is clicked.
         * @return void
         */
        public static void AddButton(string Text, GUIStyle ButtonStyle, System.Action Function)
        {
            if (GUILayout.Button(Text, ButtonStyle)) Function.Invoke();
        }

        /**
         * AddIntSlider
         * 
         * Adds an integer slider to the UI.
         * 
         * @param Value The value of the slider.
         * @param Min The minimum value of the slider.
         * @param Max The maximum value of the slider.
         * @param SliderStyle The style of the slider.
         * @param Function The function to call when the slider is changed.
         * @return void
         */
        public static void AddIntSlider(ref int Value, int Min, int Max, int Width, int Height, System.Action Function)
        {
            
            Value = (int)GUILayout.HorizontalSlider(Value, Min, Max, GUILayout.Width(Width), GUILayout.Height(Height));
            Function.Invoke();
        }

        /**
         * AddFloatSlider
         * 
         * Adds a float slider to the UI.
         * 
         * @param Value The value of the slider.
         * @param Min The minimum value of the slider.
         * @param Max The maximum value of the slider.
         * @param Function The function to call when the slider is changed.
         * @return void
         */
        public static void AddFloatSlider(ref float Value, float Min, float Max, int Width, int Height, System.Action Function)
        {
            Value = GUILayout.HorizontalSlider(Value, Min, Max, GUILayout.Width(Width), GUILayout.Height(Height));

            // round to 1 decimal places
            Value = Mathf.Round(Value * 10f) / 10f;

            Function.Invoke();
        }

        /**
         * AddInput
         * 
         * Adds an input field to the UI.
         * 
         * @param Textfields The dictionary of text fields.
         * @param Value The value of the input field.
         * @param ID The ID of the input field.
         * @param X The x position of the input field.
         * @param Y The y position of the input field.
         * @param Width The width of the input field.
         * @param Height The height of the input field.
         * @param Label The style of the label.
         * @param Function The function to call when the input is changed.
         * @return void
         */
        public static void AddInput(ref Dictionary<string, Textfield> Textfields, ref string Value, string ID, float X, float Y, int Width, int Height, GUIStyle LabelStyle, GUIStyle TextfieldStyle, System.Action Function)
        {
            // Make a label take its place
            GUILayout.Label("", GUILayout.Width(Width), GUILayout.Height(Height));

            // Build the text field
            Rect InputRect = new Rect(X, Y, Width, Height);
            if (!Textfields.TryGetValue(ID, out var TextField))
            {
                TextField = new Textfield(Value, TextfieldStyle);
                Textfields[ID] = TextField;
            }
            Value = TextField.Draw(InputRect);

            // Invoke the function
            Function.Invoke();
        }

        /**
         * AddTextfield
         * 
         * Adds a text field to the UI.
         * 
         * @param Textfields The dictionary of text fields.
         * @param Value The value of the text field.
         * @param ID The ID of the text field.
         * @param X The x position of the text field.
         * @param Y The y position of the text field.
         * @param Width The width of the text field.
         * @param Height The height of the text field.
         * @param Label The style of the label.
         * @param Function The function to call when the text is changed.
         * @return void
         */
        public static void AddTextfield(ref Dictionary<string, Textfield> Textfields, ref string Value, string ID, float X, float Y, int Width, int Height, GUIStyle Label, System.Action Function)
        {
            // Make a label take its place
            GUILayout.Label("", Label, GUILayout.Width(Width), GUILayout.Height(Height));

            // Build the text field
            Rect InputRect = new Rect(X, Y, Width, Height);
            if (!Textfields.TryGetValue(ID, out var TextField))
            {
                TextField = new Textfield(Value, GUI.skin.textField);
                Textfields[ID] = TextField;
            }
            Value = TextField.Draw(InputRect);

            // Invoke the function
            Function.Invoke();
        }

        /**
         * AddSpace
         * 
         * Adds a space to the UI.
         * 
         * @param Height The height of the space.
         * @return void
         */
        public static void AddSpace(int Height)
        {
            GUILayout.Space(Height);
        }
    }
}
