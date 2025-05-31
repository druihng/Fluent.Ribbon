using System;
using System.Windows;
using System.Windows.Media.Effects;
using Goat.OpenControls.Base.HandyControl.Data;

namespace Goat.OpenControls.Base.HandyControl.Media.Effects
{
    public class ContrastEffect : EffectBase
    {
        private static readonly PixelShader Shader;

        static ContrastEffect()
        {
            Shader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/Goat.OpenControls.Base.HandyControl;component/Resources/Effects/ContrastEffect.ps")
            };
        }

        public ContrastEffect()
        {
            PixelShader = Shader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ContrastProperty);
        }

        public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register(
            "Contrast", typeof(double), typeof(ContrastEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(0)));

        public double Contrast
        {
            get => (double) GetValue(ContrastProperty);
            set => SetValue(ContrastProperty, value);
        }
    }
}
