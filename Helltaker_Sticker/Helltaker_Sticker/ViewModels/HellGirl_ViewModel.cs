using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Helltaker_Sticker.Xml;

namespace Helltaker_Sticker.ViewModels
{
    class HellGirl_ViewModel : ViewModelBase
    {
        private bool m_Language;
        private HellGirl m_Window;
        private Bitmap m_Original;
        private Bitmap[] m_BitmapFrames = new Bitmap[24];
        private ImageSource[] m_ImgSourceFrames = new ImageSource[24];
        private MainWindow_ViewModel m_MainWindow_ViewModel;

        private ImageSource _finalSource;
        private string _girlsName;
        private string _musicLang;
        private string _volumeLang;
        private string _frameIntervalLang;
        private string _defaultLang;

        public ImageSource FinalSource { get => _finalSource; set { _finalSource = value; RaisePropertyChanged(nameof(FinalSource)); } }
        public string GirlsName { get => _girlsName; set { _girlsName = value; RaisePropertyChanged(nameof(GirlsName)); } }
        public string MusicLang { get => _musicLang; set { _musicLang = value; RaisePropertyChanged(nameof(MusicLang)); } }
        public string VolumeLang { get => _volumeLang; set { _volumeLang = value; RaisePropertyChanged(nameof(VolumeLang)); } }
        public string FrameIntervalLang { get => _frameIntervalLang; set { _frameIntervalLang = value; RaisePropertyChanged(nameof(FrameIntervalLang)); } }
        public string DefaultLang { get => _defaultLang; set { _defaultLang = value; RaisePropertyChanged(nameof(DefaultLang)); } }
        public int Volume { get => m_MainWindow_ViewModel.Volume; set { m_MainWindow_ViewModel.Volume = value; RaisePropertyChanged(nameof(Volume)); } }
        public int FrameInterval
        {
            get => m_MainWindow_ViewModel.FrameInterval;
            set
            {
                if (45 <= value && value <= 55)
                {
                    m_MainWindow_ViewModel.FrameInterval = value;
                    RaisePropertyChanged(nameof(FrameInterval));
                }
            }
        }
        public int SelectedMusic { get => m_MainWindow_ViewModel.SelectedMusic; set { m_MainWindow_ViewModel.SelectedMusic = value; RaisePropertyChanged(nameof(SelectedMusic)); } }

        #region name properties
        public string Who { get; set; }
        public string Dismiss { get; set; }
        public string Azazel { get; set; }
        public string Cerberus { get; set; }
        public string Judgement { get; set; }
        public string Justice { get; set; }
        public string Lucifer { get; set; }
        public string LuciferApron { get; set; }
        public string Malina { get; set; }
        public string Modeus { get; set; }
        public string Pandemonica { get; set; }
        public string Zdrada { get; set; }
        public string Beelzebub { get; set; }
        public string Helltaker { get; set; }
        public string HelltakerApron { get; set; }
        public string Skeleton { get; set; }
        public string Subject67 { get; set; }
        public string GloriousLeft { get; set; }
        public string GloriousRight { get; set; }
        #endregion

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public HellGirl_ViewModel(HellGirl window, MainWindow_ViewModel mainWindow, GirlSetting girlSetting)
        {
            m_Language = (Application.Current as App).Language;
            m_Window = window;
            m_MainWindow_ViewModel = mainWindow;

            if (girlSetting == null)
            {
                CreateSpriteCollection("Lucifer");
                window.Topmost = mainWindow.Topmost;
            }
            else
            {
                CreateSpriteCollection(girlSetting.Name);
                window.Top = girlSetting.Top;
                window.Left = girlSetting.Left;
                window.Topmost = mainWindow.Topmost;
            }

            Naming(m_Language);
        }

        internal void Window_Closing()
        {
            Dispose();
        }

        private void CreateSpriteCollection(string girl)
        {
            Dispose();

            //GirlsName = m_Language ? EnglishToKorean(girl) : girl;
            GirlsName = girl;

            string path = girl.Equals("LuciferApron") ? @"Resources\Lucifer.png" : @"Resources\" + girl + ".png";

            if (!File.Exists(path)) return;
            m_Original = new Bitmap(path);

            for (int i = 0; i < 24; i++)
            {
                m_BitmapFrames[i] = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(m_BitmapFrames[i]))
                {
                    g.DrawImage(m_Original, new Rectangle(0, 0, 100, 100), new Rectangle(i * 100, girl.Equals("LuciferApron") ? 100 : 0, 100, 100), GraphicsUnit.Pixel);
                }

                var handle = m_BitmapFrames[i].GetHbitmap();
                try
                {
                    m_ImgSourceFrames[i] = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                catch
                {
                    MessageBox.Show("Fucked");
                    break;
                }
                finally
                {
                    DeleteObject(handle);
                }
            }

            //(Application.Current as App).SaveCurrentState();
        }

        internal void Luminescent_button_Click()
        {
            if (SelectedMusic != 4)
            {
                SelectedMusic = 4;
                m_MainWindow_ViewModel.PlayLuminescent();
            }
            else
            {
                SelectedMusic = 0;
                m_MainWindow_ViewModel.StopPlayer();
            }
        }

        internal void Window_SizeChanged(HellGirl hellGirl)
        {
            hellGirl.Width = 100;
            hellGirl.Height = 100;
        }

        internal void Epitomize_button_Click()
        {
            if (SelectedMusic != 3)
            {
                SelectedMusic = 3;
                m_MainWindow_ViewModel.PlayEpitomize();
            }
            else
            {
                SelectedMusic = 0;
                m_MainWindow_ViewModel.StopPlayer();
            }
        }

        internal void Vitality_button_Click()
        {
            if (SelectedMusic != 2)
            {
                SelectedMusic = 2;
                m_MainWindow_ViewModel.PlayVitality();
            }
            else
            {
                SelectedMusic = 0;
                m_MainWindow_ViewModel.StopPlayer();
            }
        }

        internal void Apropos_button_Click()
        {
            if (SelectedMusic != 1)
            {
                SelectedMusic = 1;
                m_MainWindow_ViewModel.PlayApropos();
            }
            else
            {
                SelectedMusic = 0;
                m_MainWindow_ViewModel.StopPlayer();
            }
        }

        internal void FrameIntervaleDefault_button_Click()
        {
            FrameInterval = 49;
        }

        private void Dispose()
        {
            if (m_Original != null) m_Original.Dispose();
            foreach (var bitmap in m_BitmapFrames) if (bitmap != null) bitmap.Dispose();
            for (int i = 0; i < 24; i++)
            {
                m_ImgSourceFrames[i] = null;
            }
        }

        internal void Dismiss_button_Click()
        {
            m_Window.Close();
            (Application.Current as App).Girls.Remove(m_Window);
            (Application.Current as App).SaveCurrentState();
        }

        public void NextFrame(int frame)
        {
            FinalSource = m_ImgSourceFrames[frame];
        }

        #region 노가다
        internal void Helltaker_button_Click()
        {
            CreateSpriteCollection("Helltaker");
        }

        internal void HelltakerApron_button_Click()
        {
            CreateSpriteCollection("HelltakerApron");
        }

        internal void Subject67_button_Click()
        {
            CreateSpriteCollection("Subject67");
        }

        internal void Lucifer_button_Click()
        {
            CreateSpriteCollection("Lucifer");
        }

        internal void LuciferApron_button_Click()
        {
            CreateSpriteCollection("LuciferApron");
        }

        internal void Malina_button_Click()
        {
            CreateSpriteCollection("Malina");
        }

        internal void Modeus_button_Click()
        {
            CreateSpriteCollection("Modeus");
        }

        internal void Justice_button_Click()
        {
            CreateSpriteCollection("Justice");
        }

        internal void Pandemonica_button_Click()
        {
            CreateSpriteCollection("Pandemonica");
        }

        internal void Zdrada_button_Click()
        {
            CreateSpriteCollection("Zdrada");
        }

        internal void Judgement_button_Click()
        {
            CreateSpriteCollection("Judgement");
        }

        internal void Beelzebub_button_Click()
        {
            CreateSpriteCollection("Beelzebub");
        }

        internal void GloriousLeft_button_Click()
        {
            CreateSpriteCollection("Glorious_success_left");
        }

        internal void GloriousRight_button_Click()
        {
            CreateSpriteCollection("Glorious_success_right");
        }

        internal void Cerberus_button_Click()
        {
            CreateSpriteCollection("Cerberus");
        }

        internal void Azazel_button_Click()
        {
            CreateSpriteCollection("Azazel");
        }
        internal void Skeleton_button_Click()
        {
            CreateSpriteCollection("Skeleton");
        }

        private string EnglishToKorean(string girl)
        {
            string koreanName = "";
            switch (girl)
            {
                case "Azazel": koreanName = "아자젤"; break;
                case "Cerberus": koreanName = "케르베로스"; break;
                case "Lucifer": koreanName = "루시퍼"; break;
                case "LuciferApron": koreanName = "앞치마 루시퍼"; break;
                case "Malina": koreanName = "말리나"; break;
                case "Modeus": koreanName = "모데우스"; break;
                case "Justice": koreanName = "저스티스"; break;
                case "Judgement": koreanName = "저지먼트"; break;
                case "Pandemonica": koreanName = "판데모니카"; break;
                case "Zdrada": koreanName = "즈드라다"; break;
                case "Skeleton": koreanName = "스켈레톤"; break;
                case "Helltaker": koreanName = "헬테이커"; break;
                case "HelltakerApron": koreanName = "앞치마 헬테이커"; break;
                default: koreanName = girl; break;
            }
            return koreanName;
        }

        public void Naming(bool lang)
        {
            Who = lang ? "누구?" : "Who?";
            Dismiss = lang ? "소환 해제" : "Dismiss";
            Azazel = lang ? "아자젤" : "Azazel";
            Cerberus = lang ? "케르베로스" : "Cerberus";
            Judgement = lang ? "저지먼트" : "Judgement";
            Justice = lang ? "저스티스" : "Justice";
            Lucifer = lang ? "루시퍼" : "Lucifer";
            LuciferApron = lang ? "앞치마 루시퍼" : "Lucifer Apron";
            Malina = lang ? "말리나" : "Malina";
            Modeus = lang ? "모데우스" : "Modeus";
            Pandemonica = lang ? "판데모니카" : "Pandemonica";
            Zdrada = lang ? "즈드라다" : "Zdrada";
            Beelzebub = lang ? "베엘제붑" : "Beelzebub";
            Helltaker = lang ? "헬테이커" : "Helltaker";
            HelltakerApron = lang ? "앞치마 헬테이커" : "Helltaker Apron";
            Skeleton = lang ? "스켈레톤" : "Skeleton";
            Subject67 = lang ? "실험체 67" : "Subject 67";
            GloriousLeft = lang ? "Glorious 왼쪽" : "Glorious Left";
            GloriousRight = lang ? "Glorious 오른쪽" : "Glorious Right";

            MusicLang = lang ? "배경음악" : "BGM";
            VolumeLang = lang ? "볼륨" : "Volume";
            FrameIntervalLang = lang ? "프레임간격" : "Frame Interval";
            DefaultLang = lang ? "기본값" : "Default";

            RaisePropertyChanged(nameof(Who));
            RaisePropertyChanged(nameof(Dismiss));
            RaisePropertyChanged(nameof(Azazel));
            RaisePropertyChanged(nameof(Cerberus));
            RaisePropertyChanged(nameof(Judgement));
            RaisePropertyChanged(nameof(Justice));
            RaisePropertyChanged(nameof(Lucifer));
            RaisePropertyChanged(nameof(LuciferApron));
            RaisePropertyChanged(nameof(Malina));
            RaisePropertyChanged(nameof(Modeus));
            RaisePropertyChanged(nameof(Pandemonica));
            RaisePropertyChanged(nameof(Zdrada));
            RaisePropertyChanged(nameof(Beelzebub));
            RaisePropertyChanged(nameof(Subject67));
            RaisePropertyChanged(nameof(Helltaker));
            RaisePropertyChanged(nameof(HelltakerApron));
            RaisePropertyChanged(nameof(Skeleton));
            RaisePropertyChanged(nameof(GloriousLeft));
            RaisePropertyChanged(nameof(GloriousRight));

            RaisePropertyChanged(nameof(MusicLang));
            RaisePropertyChanged(nameof(VolumeLang));
            RaisePropertyChanged(nameof(FrameIntervalLang));
            RaisePropertyChanged(nameof(DefaultLang));
        }

        public void RefreshAll()
        {
            RaisePropertyChanged(nameof(GirlsName));

            RaisePropertyChanged(nameof(Who));
            RaisePropertyChanged(nameof(Dismiss));
            RaisePropertyChanged(nameof(Azazel));
            RaisePropertyChanged(nameof(Cerberus));
            RaisePropertyChanged(nameof(Judgement));
            RaisePropertyChanged(nameof(Justice));
            RaisePropertyChanged(nameof(Lucifer));
            RaisePropertyChanged(nameof(LuciferApron));
            RaisePropertyChanged(nameof(Malina));
            RaisePropertyChanged(nameof(Modeus));
            RaisePropertyChanged(nameof(Pandemonica));
            RaisePropertyChanged(nameof(Zdrada));
            RaisePropertyChanged(nameof(Beelzebub));
            RaisePropertyChanged(nameof(Subject67));
            RaisePropertyChanged(nameof(Helltaker));
            RaisePropertyChanged(nameof(HelltakerApron));
            RaisePropertyChanged(nameof(Skeleton));
            RaisePropertyChanged(nameof(GloriousLeft));
            RaisePropertyChanged(nameof(GloriousRight));

            RaisePropertyChanged(nameof(MusicLang));
            RaisePropertyChanged(nameof(VolumeLang));
            RaisePropertyChanged(nameof(FrameIntervalLang));
            RaisePropertyChanged(nameof(DefaultLang));

            RaisePropertyChanged(nameof(Volume));
            RaisePropertyChanged(nameof(FrameInterval));
            RaisePropertyChanged(nameof(SelectedMusic));
        }
        #endregion
    }

    public class MusicOneToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int val)
            {
                if (val == 1) return true;
                else return false;
            }
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MusicTwoToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int val)
            {
                if (val == 2) return true;
                else return false;
            }
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MusicThreeToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int val)
            {
                if (val == 3) return true;
                else return false;
            }
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MusicFourToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int val)
            {
                if (val == 4) return true;
                else return false;
            }
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
