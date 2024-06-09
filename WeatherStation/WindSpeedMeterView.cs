using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace WeatherStation
{
    public class WindSpeedMeterView: ContentView
    {
        private float _currentSpeed;

        public static readonly BindableProperty SpeedProperty =
            BindableProperty.Create(nameof(Speed), typeof(float), typeof(WindSpeedMeterView), 0f, propertyChanged: OnSpeedChanged);

        public float Speed
        {
            get => (float)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        private static void OnSpeedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((WindSpeedMeterView)bindable).InvalidateLayout();
        }

        public WindSpeedMeterView()
        {
            var canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnPaintSurface;
            Content = canvasView;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Black);

            var width = e.Info.Width;
            var height = e.Info.Height;
            var centerX = width / 2;
            var centerY = height / 2;
            var radius = Math.Min(width, height) / 2 - 20;

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 10,
                IsAntialias = true
            };

            // Draw the outer circle
            canvas.DrawCircle(centerX, centerY, radius, paint);

            // Draw the ticks
            paint.StrokeWidth = 5;
            for (int i = 0; i < 240; i += 10)
            {
                var angle = Math.PI * i / 180;
                var startX = centerX + (radius - 20) * (float)Math.Cos(angle);
                var startY = centerY + (radius - 20) * (float)Math.Sin(angle);
                var endX = centerX + radius * (float)Math.Cos(angle);
                var endY = centerY + radius * (float)Math.Sin(angle);
                canvas.DrawLine(startX, startY, endX, endY, paint);
            }

            // Draw the speed text
            paint.Style = SKPaintStyle.Fill;
            paint.TextSize = 60;
            paint.Color = SKColors.White;
            var speedText = Speed.ToString("0");
            var textBounds = new SKRect();
            paint.MeasureText(speedText, ref textBounds);
            canvas.DrawText(speedText, centerX - textBounds.MidX, centerY + textBounds.MidY, paint);

            // Draw the needle
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 5;
            paint.Color = SKColors.Red;
            var needleAngle = Math.PI * (Speed / 240) * 180 / 180;
            var needleX = centerX + (radius - 40) * (float)Math.Cos(needleAngle);
            var needleY = centerY + (radius - 40) * (float)Math.Sin(needleAngle);
            canvas.DrawLine(centerX, centerY, needleX, needleY, paint);
        }
    }
}
