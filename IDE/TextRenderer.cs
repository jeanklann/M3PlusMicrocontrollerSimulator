using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace IDE
{
    /// <summary>
    /// Uses System.Drawing for 2d text rendering.
    /// </summary>
    public class TextRenderer:IDisposable {
        private static Dictionary<int, TextRenderer> _textRenderers = new Dictionary<int, TextRenderer>();
        private static Font _font = new Font(FontFamily.GenericMonospace, 12);
        private Bitmap _bmp;
        private Graphics _gfx;
        private Size _textSize;
        private int _texture;
        private Rectangle _dirtyRegion;
        private bool _disposed;
        

        private static int GetPowerOfTwo(int value) {
            if (value == 0) return 1;
            if (value <= 1) return 1;
            if (value <= 2) return 2;
            if (value <= 4) return 4;
            if (value <= 8) return 8;
            if (value <= 16) return 16;
            if (value <= 32) return 32;
            if (value <= 64) return 64;
            if (value <= 128) return 128;
            if (value <= 256) return 256;
            if (value <= 512) return 512;
            if (value <= 1024) return 1024;
            if (value <= 2048) return 2048;
            if (value <= 4096) return 4096;
            if (value <= 8192) return 8192;
            return 1;
        }

        private TextRenderer() {
            if (GraphicsContext.CurrentContext == null)
                throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");
        }
        

        void Dispose(bool manual) {
            if (!_disposed) {
                if (manual) {
                    _bmp.Dispose();
                    _gfx.Dispose();
                    if (GraphicsContext.CurrentContext != null)
                        GL.DeleteTexture(_texture);
                }

                _disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextRenderer() {
            Console.WriteLine("[Warning] Resource leaked: {0}.", typeof(TextRenderer));
        }
        public static void DrawText(string text, Color color, PointF point) {
            DrawText(text, color, point, _font);
        }
        public static void DrawText(string text, Color color, PointF point, Font font) {
            TextRenderer renderer;
            int width;
            int height;
            var hash = text.GetHashCode() + color.GetHashCode() + font.GetHashCode();
            if (!_textRenderers.ContainsKey(hash)) { 
                renderer = new TextRenderer();
                renderer._bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                renderer._gfx = Graphics.FromImage(renderer._bmp);
                Brush brush = new SolidBrush(color);
                renderer._gfx.DrawString(text, font, brush, 0, 0);
                var textSize = renderer._gfx.MeasureString(text, font);
                renderer._textSize = new Size((int)(textSize.Width + 0.5f), (int)(textSize.Height + 0.5f));

                renderer._gfx.Dispose();
                renderer._bmp.Dispose();
                width = GetPowerOfTwo(renderer._textSize.Width);
                height = GetPowerOfTwo(renderer._textSize.Height);
                renderer._bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                renderer._gfx = Graphics.FromImage(renderer._bmp);
                renderer._gfx.Clear(UiStatics.Circuito.ClearColor);
                renderer._gfx.DrawString(text, font, brush, 0, 0);
                renderer._dirtyRegion = new Rectangle(0, 0, renderer._bmp.Width, renderer._bmp.Height);
                GL.Enable(EnableCap.Texture2D);
                renderer._texture = GL.GenTexture();
                _textRenderers[hash] = renderer;
                GL.BindTexture(TextureTarget.Texture2D, renderer._texture);
                
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);


                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

                var data = renderer._bmp.LockBits(renderer._dirtyRegion,
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                    renderer._dirtyRegion.X, renderer._dirtyRegion.Y, renderer._dirtyRegion.Width, renderer._dirtyRegion.Height,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                renderer._bmp.UnlockBits(data);
            } else {
                renderer = _textRenderers[hash];
                GL.BindTexture(TextureTarget.Texture2D, renderer._texture);
                width = renderer._textSize.Width;
                height = renderer._textSize.Height;

                var wmax = width / (float)renderer._bmp.Width;
                var hmax = height / (float)renderer._bmp.Height;

                GL.Color3(Color.White);
                GL.Begin(PrimitiveType.Quads);

                GL.TexCoord2(0, hmax); GL.Vertex2(point.X - width / 2, point.Y - height / 2);
                GL.TexCoord2(wmax, hmax); GL.Vertex2(point.X + width / 2, point.Y - height / 2);
                GL.TexCoord2(wmax, 0); GL.Vertex2(point.X + width / 2, point.Y + height / 2);
                GL.TexCoord2(0, 0); GL.Vertex2(point.X - width / 2, point.Y + height / 2);
                GL.End();
            }
            
        }
    }
}