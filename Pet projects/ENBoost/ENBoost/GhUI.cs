using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ENBoost
{
	public static class GhUI
	{
		static Dictionary<string, Texture2D> coloredTextures = new Dictionary<string, Texture2D>();
		public static Dictionary<string, Color> Colors = new Dictionary<string, Color>();
		
		public static void PrepeareTexture(string colorName_, Color color_)
		{
			Texture2D bufferT = new Texture2D(1, 1);
			bufferT.SetPixel(0, 0, color_);
			bufferT.wrapMode = 0;
			bufferT.Apply();
			
			coloredTextures.Add(colorName_, bufferT);
			
			Colors.Add(colorName_, color_);
		}
		
		public static void DestroyTexture(string colorName_)
		{
			UnityEngine.Object.Destroy(coloredTextures[colorName_]);
			
			coloredTextures.Remove(colorName_);
			Colors.Remove(colorName_);
		}
		
		public static void oDrawLine(Vector2 lineStart, Vector2 lineEnd, string colorName_)
		{
			DrawLineStretchedIN(lineStart, lineEnd, coloredTextures[colorName_], 1);
		}
		
		public static void oDrawBox(float x, float y, float w, float h, string colorName_)
		{
			oDrawLine(new Vector2(x, y), new Vector2(x + w, y), colorName_);
			oDrawLine(new Vector2(x, y), new Vector2(x, y + h), colorName_);
			oDrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), colorName_);
			oDrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), colorName_);
		}

		static void DrawLineStretchedIN(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
		{
			Vector2 vector = lineEnd - lineStart;
			float pivot = 57.29578f * Mathf.Atan(vector.y / vector.x);
			if (vector.x < 0f) {
				pivot += 180f;
			}

			if (thickness < 1) {
				thickness = 1;
			}

			int yOffset = (int)Mathf.Ceil((float)(thickness / 2));

			GUIUtility.RotateAroundPivot(pivot, lineStart);
			GUI.DrawTexture(new Rect(lineStart.x, lineStart.y - (float)yOffset, vector.magnitude, (float)thickness), texture);
			GUIUtility.RotateAroundPivot(-pivot, lineStart);
		}
	}
}
