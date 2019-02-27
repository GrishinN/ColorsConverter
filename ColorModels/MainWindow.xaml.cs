using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ColorModels
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static object sliderC;
        public static object sliderM;
        public static object sliderY;
        public static object sliderK;
        bool activity = false;
        bool recalculationCMYK = true;
        bool recalculationHSV = true;
        bool recalculationLAB = true;

        public MainWindow()
        {
            InitializeComponent();
            //colorPicker.SelectedColor = Color.FromRgb(255, 255, 255);
            //currentСolor.Background = new SolidColorBrush(colorPicker.SelectedColor.Value);
            
        }

        private void CMYK_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (activity)
            {
                return;
            }
            recalculationCMYK = false;
            double valueC = Cmyk.Value / 100.00;
            double valueM = cMyk.Value / 100.00;
            double valueY = cmYk.Value / 100.00;
            double valueK = cmyK.Value / 100.00;

            int R, G, B;
            R = (int)(255.00 * (1.00 - valueC) * (1.00 - valueK));
            G = (int)(255.00 * (1.00 - valueM) * (1.00 - valueK));
            B = (int)(255.00 * (1.00 - valueY) * (1.00 - valueK));



            colorPicker.SelectedColor = Color.FromRgb(byte.Parse(R.ToString()), byte.Parse(G.ToString()), byte.Parse(B.ToString()));
            recalculationCMYK = true;
        }

        private void RGBtoCMYK(double R, double G, double B)
        {
            double C, M, Y, K;
            K = Math.Min((Math.Min((1.00 - R / 255.00), (1.00 - G / 255.00))), (1.00 - B / 255.00));
            if (K != 1)
            {
                C = (1.00 - R / 255.00 - K) / (1.00 - K);
                M = (1.00 - G / 255.00 - K) / (1.00 - K);
                Y = (1.00 - B / 255.00 - K) / (1.00 - K);
                Cmyk.Value = Math.Round(C * 100);
                cMyk.Value = Math.Round(M * 100);
                cmYk.Value = Math.Round(Y * 100);
                cmyK.Value = Math.Round(K * 100);
            }
            else
            {
                Cmyk.Value = 0;
                cMyk.Value = 0;
                cmYk.Value = 0;
                cmyK.Value = 100;
            }
        }

        private void RGBtoHSV(double R, double G, double B)
        {
            double min, max, delta;
            //double R, G, B;
            double X, Y, Z;
            double H = -1, S, V;
            double del_R, del_G, del_B;
            R = R / 255.00;
            G = G / 255.00;
            B = B / 255.00;

            min = Math.Min(Math.Min(R, G), B);
            max = Math.Max(Math.Max(R, G), B);
            delta = max - min;

            V = max;
            if (delta == 0)
            {
                H = 0;
                S = 0;
            }
            else
            {
                S = delta / max;

                del_R = (((max - R) / 6.00) + (delta / 2.00)) / delta;
                del_G = (((max - G) / 6.00) + (delta / 2.00)) / delta;
                del_B = (((max - B) / 6.00) + (delta / 2.00)) / delta;

                if (R == max)
                {
                    H = del_B - del_G;
                }
                else if (G == max)
                {
                    H = (0.33) + del_R - del_B;
                }
                else if (B == max)
                {
                    H = (0.66) + del_G - del_R;
                }

                if (H < 0)
                {
                    H += 1;
                }
                if (H > 1)
                {
                    H -= 1;
                }
            }

            HofHSV.Text = (Math.Round(H, 2) * 100).ToString();
            SofHSV.Text = (Math.Round(S, 2) * 100).ToString();
            VofHSV.Text = (Math.Round(V, 2) * 100).ToString();

        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            activity = true;
            double R = colorPicker.SelectedColor.Value.R;
            double G = colorPicker.SelectedColor.Value.G;
            double B = colorPicker.SelectedColor.Value.B;


            if (recalculationCMYK)
            {
                RGBtoCMYK(R, G, B);
            }

            if (recalculationHSV)
            {
                RGBtoHSV(R, G, B);
            }

            if (recalculationLAB)
            {
                RGBtoXYZtoLAB(R, G, B);
            }
            activity = false;


        }

        private void RGBtoXYZtoLAB(double R , double G , double B)
        {            
            double X, Y, Z;
            R = R / 255.00;
            G = G / 255.00;
            B = B / 255.00;

            if (R >= 0.04045)
            {
                R = Math.Pow(((R + 0.055) / 1.055), 2.4);
            }
            else
            {
                R = R / 12.92;
            }

            if (G >= 0.04045)
            {
                G = Math.Pow(((G + 0.055) / 1.055), 2.4);
            }
            else
            {
                G = G / 12.92;
            }

            if (B >= 0.04045)
            {
                B = Math.Pow(((B + 0.055) / 1.055), 2.4);
            }
            else
            {
                B = B / 12.92;
            }
            R = R * 100;
            G = G * 100;
            B = B * 100;

            X = R * 0.412453 + G * 0.357580 + B * 0.180423;
            Y = R * 0.212671 + G * 0.715160 + B * 0.072169;
            Z = R * 0.019334 + G * 0.119193 + B * 0.950227;

            double X2, Y2, Z2;

            X2 = X / 95.047;
            Y2 = Y / 100.00;
            Z2 = Z / 108.883;

            if (X2 >= 0.008856)
            {
                X2 = Math.Pow(X2, 0.333);
            }
            else
            {
                X2 = (7.787 * X2) + 16.00 / 116.00;
            }

            if (Y2 >= 0.008856)
            {
                Y2 = Math.Pow(Y2, 0.333);
            }
            else
            {
                Y2 = (7.787 * Y2) + 16.00 / 116.00;
            }

            if (Z2 >= 0.008856)
            {
                Z2 = Math.Pow(Z2, 0.333);
            }
            else
            {
                Z2 = (7.787 * Z2) + 16.00 / 116.00;
            }

            double CIE_L, CIE_A, CIE_B;
            CIE_L = (116.00 * Y2) - 16.00;
            CIE_A = 500.00 * (X2 - Y2);
            CIE_B = 200.00 * (Y2 - Z2);

            LofLAB.Text = Math.Round(CIE_L).ToString();
            AofLAB.Text = Math.Round(CIE_A).ToString();
            BofLAB.Text = Math.Round(CIE_B).ToString();

        }

       

        private void LAB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (activity)
            {
                return;
            }
           
            double X, Y, Z;
            double var_X, var_Y, var_Z;
            double CIE_L, CIE_A, CIE_B;
            double var_R, var_G, var_B;
            int R, G, B;

            recalculationLAB = false;

            if(LofLAB.Value == null)
            {
                LofLAB.Value = 0;
            }

            if (AofLAB.Value == null)
            {
                AofLAB.Value = 0;
            }

            if (BofLAB.Value == null)
            {
                BofLAB.Value = 0;
            }

            CIE_L = double.Parse(LofLAB.Value.ToString());
            CIE_A = double.Parse(AofLAB.Value.ToString());
            CIE_B = double.Parse(BofLAB.Value.ToString());

            var_Y = (CIE_L + 16.00) / 116.00;
            var_X = CIE_A / 500.00 + var_Y;
            var_Z = var_Y - CIE_B / 200.00;

            if (Math.Pow(var_Z, 3) > 0.008856)
            {
                var_Y = Math.Pow(var_Y, 3);
            }
            else
            {
                var_Y = (var_Y - 16.00 / 116.00) / 7.787;
            }

            if (Math.Pow(var_X, 3) > 0.008856)
            {
                var_X = Math.Pow(var_X, 3);
            }
            else
            {
                var_X = (var_X - 16.00 / 116.00) / 7.787;
            }

            if (Math.Pow(var_Z, 3) > 0.008856)
            {
                var_Z = Math.Pow(var_Z, 3);
            }
            else
            {
                var_Z = (var_Z - 16.00 / 116.00) / 7.787;
            }

            X = var_X * 95.047;
            if (X < 0)
            {
                X = 0;
            }

            Y = var_Y * 100.00;
            if (Y < 0)
            {
                Y = 0;
            }

            Z = var_Z * 108.883;
            if (Z < 0)
            {
                Z = 0;
            }
            //convert XYZ to RGB
            var_X = X / 100.00;
            var_Y = Y / 100.00;
            var_Z = Z / 100.00;

            var_R = var_X * 3.2406 + var_Y * -1.5372 + var_Z * -0.4986;
            var_G = var_X * -0.9689 + var_Y * 1.8758 + var_Z * 0.0415;
            var_B = var_X * 0.0557 + var_Y * -0.2040 + var_Z * 1.0570;

            if (var_R > 0.0031308)
            {
                var_R = 1.055 * (Math.Pow(var_R, 1.00 / 2.4)) - 0.055;
            }
            else
            {
                var_R = 12.92 * var_R;
            }

            if (var_G > 0.0031308)
            {
                var_G = 1.055 * (Math.Pow(var_G, 1.00 / 2.4)) - 0.055;
            }
            else
            {
                var_G = 12.92 * var_G;
            }

            if (var_B > 0.0031308)
            {
                var_B = 1.055 * (Math.Pow(var_B, 1.00 / 2.4)) - 0.055;
            }
            else
            {
                var_B = 12.92 * var_B;
            }

            R = (int)(var_R * 255);
            G = (int)(var_G * 255);
            B = (int)(var_B * 255);
                
            if(R < 0)
            {
                R = 0;
                Warning();               
            }
            if (R > 255)
            {
                R = 255;
                Warning();
            }

            if (G < 0)
            {
                G = 0;
                Warning();
            }
            if (G > 255)
            {
                G = 255;
                Warning();
            }

            if (B < 0)
            {
                B = 0;
                Warning();
            }
            if(B > 255)
            {
                B = 255;
                Warning();
            }

            colorPicker.SelectedColor = Color.FromRgb(byte.Parse(R.ToString()), byte.Parse(G.ToString()), byte.Parse(B.ToString()));
            recalculationLAB = true;
        }

        private void Warning()
        {
            Thread myThread = new Thread((new ThreadStart(showWarning)));
            myThread.Start();
        }

        private void HSV_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (activity)
            {
                return;
            }

            int R, G, B;
            double H, S, V;
            double var_h, var_1, var_2, var_3;
            double var_r, var_g, var_b;
            int var_i;

            if (HofHSV.Value == null)
            {
                HofHSV.Value = 0;
            }

            if (SofHSV.Value == null)
            {
                SofHSV.Value = 0;
            }

            if (VofHSV.Value == null)
            {
                VofHSV.Value = 0;
            }

            H = Math.Round(double.Parse(HofHSV.Value.ToString()) / 100, 2);
            S = Math.Round(double.Parse(SofHSV.Value.ToString()) / 100, 2);
            V = Math.Round(double.Parse(VofHSV.Value.ToString()) / 100, 2);

            recalculationHSV = false;

            if (S == 0)
            {
                R = (int)(V * 255.00);
                G = (int)(V * 255.00);
                B = (int)(V * 255.00);
                colorPicker.SelectedColor = Color.FromRgb(byte.Parse(R.ToString()), byte.Parse(G.ToString()), byte.Parse(B.ToString()));
            }
            else
            {
                var_h = H * 6;
                if (var_h == 6)
                {
                    var_h = 0;
                }
                var_i = (int)Math.Round(var_h);
                var_1 = V * (1 - S);
                var_2 = V * (1 - S * (var_h - var_i));
                var_3 = V * (1 - S * (1 - (var_h - var_i)));

                switch (var_i)
                {
                    case 0:
                        {
                            var_r = V;
                            var_g = var_3;
                            var_b = var_1;
                            break;
                        }
                    case 1:
                        {
                            var_r = var_2;
                            var_g = V;
                            var_b = var_1;
                            break;
                        }
                    case 2:
                        {
                            var_r = var_1;
                            var_g = V;
                            var_b = var_3;
                            break;
                        }
                    case 3:
                        {
                            var_r = var_1;
                            var_g = var_2;
                            var_b = V;
                            break;
                        }
                    case 4:
                        {
                            var_r = var_3;
                            var_g = var_1;
                            var_b = V;
                            break;
                        }
                    default:
                        {
                            var_r = V;
                            var_g = var_1;
                            var_b = var_2;
                            break;
                        }
                }
                R = (int)(var_r * 255.00);
                G = (int)(var_g * 255.00);
                B = (int)(var_b * 255.00);

                if (R < 0)
                {
                    R = 0;
                    Warning();
                }

                if (G < 0)
                {
                    G = 0;
                    Warning();
                }

                if (B < 0)
                {
                    B = 0;
                    Warning();
                }

                if (R > 255)
                {
                    R = 255;
                    Warning();
                }

                if (G > 255)
                {
                    G = 255;
                    Warning();
                }

                if (B > 255)
                {
                    B = 255;
                    Warning();
                }

                colorPicker.SelectedColor = Color.FromRgb(byte.Parse(R.ToString()), byte.Parse(G.ToString()), byte.Parse(B.ToString()));
                recalculationHSV = true;
            }
        }

        private void showWarning()
        {
            warning.Dispatcher.Invoke(DispatcherPriority.Background, new
            Action(() =>
            {
            warning.Visibility = Visibility.Visible
            ;
             }));            
            Thread.Sleep(3000);
            warning.Dispatcher.Invoke(DispatcherPriority.Background, new
            Action(() =>
            {
                warning.Visibility = Visibility.Hidden
                ;
            }));
        }
    }
}
