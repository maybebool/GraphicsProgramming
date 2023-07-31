using Silk.NET.OpenGL;

namespace SAE._5300S1.Utils.ModelHelpers {
    public class Texture : IDisposable {
        private uint _handle;
        private GL _gl;

        public unsafe Texture(GL gl, string fileName) {
            _gl = gl;


            _handle = _gl.GenTexture();
            Bind();

            using (var img = Image.Load<Rgba32>($"textures/{fileName}")) {
                gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, null);

                img.ProcessPixelRows(accessor => {
                    for (int y = 0; y < accessor.Height; y++) {
                        fixed (void* data = accessor.GetRowSpan(y)) {
                            gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1,
                                PixelFormat.Rgba, PixelType.UnsignedByte, data);
                        }
                    }
                });
            }

            SetParameters();
        }
        
        public unsafe Texture(GL gl,
            List<string> textureNames) {
            _gl = gl;

            _handle = _gl.GenTexture();
            _gl.BindTexture(TextureTarget.TextureCubeMap, _handle);

            for (int i = 0; i < textureNames.Count; i++) {
                using (var img = Image.Load<Rgba32>("textures/" + textureNames[i])) {
                    var target = TextureTarget.TextureCubeMapPositiveX + i;
                    gl.TexImage2D(target, 0, InternalFormat.Rgba, (uint)img.Width,
                        (uint)img.Height, 0,
                        PixelFormat.Rgba, PixelType.UnsignedByte, null);

                    img.ProcessPixelRows(accessor => {
                        for (int y = 0; y < accessor.Height; y++) {
                            fixed (void* data = accessor.GetRowSpan(y)) {
                                gl.TexSubImage2D(target, 0, 0, y, (uint)accessor.Width, 1,
                                    PixelFormat.Rgba,
                                    PixelType.UnsignedByte, data);
                            }
                        }
                    });
                }
            }

            _gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            _gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            _gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)GLEnum.ClampToEdge);
        }

        public unsafe Texture(GL gl, Span<byte> data, uint width, uint height) {
            _gl = gl;

            _handle = _gl.GenTexture();
            Bind();

            fixed (void* d = &data[0]) {
                _gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba,
                    PixelType.UnsignedByte, d);
                SetParameters();
            }
        }

        private void SetParameters() {
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)GLEnum.LinearMipmapLinear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0) {
            _gl.ActiveTexture(textureSlot);
            _gl.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Dispose() {
            _gl.DeleteTexture(_handle);
        }
    }
}