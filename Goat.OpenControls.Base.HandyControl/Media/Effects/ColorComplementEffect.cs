using System;
using System.Windows.Media.Effects;

namespace Goat.OpenControls.Base.HandyControl.Media.Effects
{
    public class ColorComplementEffect : EffectBase
    {
        private static readonly PixelShader Shader;

        static ColorComplementEffect()
        {
            Shader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/Goat.OpenControls.Base.HandyControl;component/Resources/Effects/ColorComplementEffect.ps")
            };
        }

        public ColorComplementEffect()
        {
            PixelShader = Shader;

            UpdateShaderValue(InputProperty);
        }
    }
}
