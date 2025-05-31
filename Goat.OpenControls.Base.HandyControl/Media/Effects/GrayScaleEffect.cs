using System;
using System.Windows;
using System.Windows.Media.Effects;
using Goat.OpenControls.Base.HandyControl.Data;

namespace Goat.OpenControls.Base.HandyControl.Media.Effects
{
    public class GrayScaleEffect : EffectBase
    {
        private static readonly PixelShader Shader;

        static GrayScaleEffect()
        {
            Shader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/Goat.OpenControls.Base.HandyControl;component/Resources/Effects/GrayScaleEffect.ps")
            };
        }

        public GrayScaleEffect()
        {
            PixelShader = Shader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ScaleProperty);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(GrayScaleEffect), new PropertyMetadata(ValueBoxes.Double1Box, PixelShaderConstantCallback(0)));

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }
    }
}
