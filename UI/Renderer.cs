using UnityEngine;

namespace NastyMod_v2.UI
{
    /**
     * Renderer
     * 
     * This class handles the rendering of game overlay ui elements.
     * 
     * Author: nastycodes
     * Version: 1.0.0
     */
    public class Renderer
    {
        // String style for drawing text
        public static GUIStyle StringStyle { get; set; } = new GUIStyle(UnityEngine.GUI.skin.label);

        // Texture for drawing lines
        public static Texture2D LineTexture;

        // Color struct for drawing
        public static Color Color
        {
            get { return UnityEngine.GUI.color; }
            set { UnityEngine.GUI.color = value; }
        }

        /**
         * DrawString
         * 
         * Draws a string on the screen.
         * 
         * @param position The position to draw the string at.
         * @param label The string to draw.
         * @param centered Whether to center the string.
         */
        public void DrawString(Vector2 position, string label, bool centered = true)
        {
            var content = new GUIContent(label);
            var size = StringStyle.CalcSize(content);
            var upperLeft = centered ? position - size / 2f : position;
            UnityEngine.GUI.Label(new Rect(upperLeft, size), content);
        }

        /**
         * DrawLine
         * 
         * Draws a line between two points.
         * 
         * @param pointA The start point of the line.
         * @param pointB The end point of the line.
         * @param color The color of the line.
         * @param width The width of the line.
         */
        public void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            Matrix4x4 matrix = UnityEngine.GUI.matrix;
            if (!LineTexture) LineTexture = new Texture2D(1, 1);

            Color color2 = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;
            float num = Vector3.Angle(pointB - pointA, Vector2.right);

            if (pointA.y > pointB.y)
                num = -num;

            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(num, pointA);
            UnityEngine.GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), LineTexture);
            UnityEngine.GUI.matrix = matrix;
            UnityEngine.GUI.color = color2;
        }

        /**
         * DrawBox
         * 
         * Draws a box on the screen.
         * 
         * @param x The x position of the box.
         * @param y The y position of the box.
         * @param w The width of the box.
         * @param h The height of the box.
         * @param color The color of the box.
         * @param thickness The thickness of the box lines.
         */
        public void DrawBox(float x, float y, float w, float h, Color color, float thickness)
        {
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), color, thickness);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), color, thickness);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color, thickness);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color, thickness);
        }

        /**
         * DrawSkewedBox
         * 
         * Draws a skewed box on the screen.
         * 
         * @param footpos The position of the foot.
         * @param headpos The position of the head.
         * @param color The color of the box.
         */
        public void DrawSkewedBox(Vector3 footpos, Vector3 headpos, Color color)
        {
            float npcHeight = headpos.y - footpos.y;
            float npcWidth = npcHeight / 2.7f;

            Vector3 screen_topLeft = new Vector3(headpos.x - npcWidth / 2, headpos.y, headpos.z);
            Vector3 screen_topRight = new Vector3(headpos.x + npcWidth / 2, headpos.y, headpos.z);
            Vector3 screen_bottomLeft = new Vector3(footpos.x - npcWidth / 2, footpos.y, footpos.z);
            Vector3 screen_bottomRight = new Vector3(footpos.x + npcWidth / 2, footpos.y, footpos.z);

            DrawLine(new Vector2(screen_topLeft.x, Screen.height - screen_topLeft.y), new Vector2(screen_topRight.x, Screen.height - screen_topRight.y), color, 2f);
            DrawLine(new Vector2(screen_topRight.x, Screen.height - screen_topRight.y), new Vector2(screen_bottomRight.x, Screen.height - screen_bottomRight.y), color, 2f);
            DrawLine(new Vector2(screen_bottomRight.x, Screen.height - screen_bottomRight.y), new Vector2(screen_bottomLeft.x, Screen.height - screen_bottomLeft.y), color, 2f);
            DrawLine(new Vector2(screen_bottomLeft.x, Screen.height - screen_bottomLeft.y), new Vector2(screen_topLeft.x, Screen.height - screen_topLeft.y), color, 2f);
        }
    }
}
