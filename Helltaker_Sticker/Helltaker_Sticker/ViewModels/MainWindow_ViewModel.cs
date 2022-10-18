using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Application = System.Windows.Application;
using WMPLib;
using Helltaker_Sticker.Xml;

namespace Helltaker_Sticker.ViewModels
{
    public class MainWindow_ViewModel : ViewModelBase
    {
        private List<HellGirl> m_Girls;
        private bool m_Language;
        private NotifyIcon Noti;
        private MainWindow m_Window;
        private CreditWindow credit;
        private WMPLib.WindowsMediaPlayer m_Player;
        private System.Windows.Forms.Timer timer;
        private Xml_Parser m_Xml;

        private int _frame;
        private int _volume;
        private int _frameInterval;
        private int _selectedMusic;

        public int Frame { get => _frame; set { _frame = value; RaisePropertyChanged(); } }
        public bool Topmost { get => m_Xml.settings.Topmost; set { m_Xml.settings.Topmost = value; RaisePropertyChanged(); } }
        public int Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                RaisePropertyChanged(nameof(Volume));
                if (m_Player != null) m_Player.settings.volume = _volume;
                foreach (var girl in m_Girls) (girl.DataContext as HellGirl_ViewModel).RefreshAll();
            }
        }

        public int FrameInterval
        {
            get => _frameInterval;
            set
            {
                _frameInterval = value;
                if (timer != null) timer.Interval = FrameInterval;
                RaisePropertyChanged(nameof(FrameInterval));
                foreach (var girl in m_Girls) (girl.DataContext as HellGirl_ViewModel).RefreshAll();
            }
        }
        public int SelectedMusic
        {
            get => _selectedMusic;
            set
            {
                _selectedMusic = value;
                RaisePropertyChanged(nameof(SelectedMusic));
                foreach (var girl in m_Girls) (girl.DataContext as HellGirl_ViewModel).RefreshAll();
            }
        }

        public MainWindow_ViewModel(MainWindow window)
        {
            m_Window = window;
            m_Language = (Application.Current as App).Language;
            m_Girls = (Application.Current as App).Girls;
            m_Xml = (Application.Current as App).xml_Parser;
            Volume = 50;
            m_Player = new WindowsMediaPlayer();
            m_Player.settings.volume = Volume;

            FrameInterval = 49;
            Frame = -1;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = FrameInterval;
            timer.Tick += NextFrame;

            timer.Start();

            GenerateNotifyIcon();

            //로드
            if (m_Xml.settings.GirlSettings.Count == 0)
            {
                m_Girls.Add(new HellGirl(this, null));
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            }
            else
            {
                if (Screen.AllScreens.Length == 1)
                {
                    int width = Screen.PrimaryScreen.Bounds.Width;
                    int height = Screen.PrimaryScreen.Bounds.Height;

                    foreach (GirlSetting girl in m_Xml.settings.GirlSettings)
                    {
                        if (girl.Left < 0 || girl.Left > width - 100) girl.Left = 100;
                        if (girl.Top < 0 || girl.Top > height - 100) girl.Top = 100;
                    }
                }

                foreach (GirlSetting girl in m_Xml.settings.GirlSettings)
                {
                    m_Girls.Add(new HellGirl(this, girl));
                    m_Girls.Last().Show();
                }

                (Application.Current as App).SaveCurrentState();
            }

        }

        internal void Window_Closing()
        {
            if (credit != null) credit.Close();
            Noti.Visible = false;
            Noti.Icon = null;
        }

        private void GenerateNotifyIcon()
        {
            if (Noti != null) Noti.Dispose();

            ContextMenu Menu = new ContextMenu();
            Noti = new NotifyIcon
            {
                Icon = new Icon(@"Resources\icon.ico", new Size(10, 10)),
                Visible = true,
                Text = m_Language ? "헬테이커" : "Helltaker",
                ContextMenu = Menu
            };

            m_Window.Topmost = Topmost;
            foreach (var girl in (Application.Current as App).Girls)
                girl.Topmost = Topmost;

            MenuItem TopMostItem = new MenuItem
            {
                Text = m_Language ? "최상위고정" : "Top Most",
                Checked = Topmost
            };
            TopMostItem.Click += (object o, EventArgs e) =>
            {
                Topmost = !Topmost;
                m_Window.Topmost = Topmost;
                foreach (var girl in (Application.Current as App).Girls)
                    girl.Topmost = Topmost;

                TopMostItem.Checked = Topmost;
                (Application.Current as App).SaveCurrentState();
            };

            #region Girls menuItem
            MenuItem SummonGirls = new MenuItem()
            {
                Text = m_Language ? "소환" : "Summon"
            };

            MenuItem AzazelItem = new MenuItem() { Text = m_Language ? "아자젤" : "Azazel" };
            AzazelItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Azazel_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(AzazelItem);

            MenuItem CerberusItem = new MenuItem() { Text = m_Language ? "케르베로스" : "Cerberus" };
            CerberusItem.Click += (object o, EventArgs e) =>
            {
                for (int i = 0; i < 3; i++)
                {
                    m_Girls.Add(new HellGirl(this, null));
                    (m_Girls.Last().DataContext as HellGirl_ViewModel).Cerberus_button_Click();
                    m_Girls.Last().Show();
                }
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(CerberusItem);

            MenuItem JudgementItem = new MenuItem() { Text = m_Language ? "저지먼트" : "Judgement" };
            JudgementItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Judgement_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(JudgementItem);

            MenuItem JusticeItem = new MenuItem() { Text = m_Language ? "저스티스" : "Justice" };
            JusticeItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Justice_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(JusticeItem);

            MenuItem LuciferItem = new MenuItem() { Text = m_Language ? "루시퍼" : "Lucifer" };
            LuciferItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Lucifer_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(LuciferItem);

            MenuItem LuciferApronItem = new MenuItem() { Text = m_Language ? "앞치마 루시퍼" : "Lucifer Apron" };
            LuciferApronItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).LuciferApron_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(LuciferApronItem);

            MenuItem MalinaItem = new MenuItem() { Text = m_Language ? "말리나" : "Malina" };
            MalinaItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Malina_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(MalinaItem);

            MenuItem ModeusItem = new MenuItem() { Text = m_Language ? "모데우스" : "Modeus" };
            ModeusItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Modeus_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(ModeusItem);

            MenuItem PandemonicaItem = new MenuItem() { Text = m_Language ? "판데모니카" : "Pandemonica" };
            PandemonicaItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Pandemonica_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(PandemonicaItem);

            MenuItem ZdradaItem = new MenuItem() { Text = m_Language ? "즈드라다" : "Zdrada" };
            ZdradaItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Zdrada_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(ZdradaItem);

            //MenuItem Beelzebub = new MenuItem() { Text = m_Language ? "베엘제붑" : "Beelzebub" };
            //Beelzebub.Click += (object o, EventArgs e) =>
            //{
            //    m_Girls.Add(new HellGirl(this, null));
            //    (m_Girls.Last().DataContext as HellGirl_ViewModel).Beelzebub_button_Click();
            //    m_Girls.Last().Show();
            //    (Application.Current as App).SaveCurrentState();
            //};
            //SummonGirls.MenuItems.Add(Beelzebub);

            MenuItem SkeletonItem = new MenuItem() { Text = m_Language ? "스켈레톤" : "Skeleton" };
            SkeletonItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Skeleton_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(SkeletonItem);

            MenuItem HelltakerItem = new MenuItem() { Text = m_Language ? "헬테이커" : "Helltaker" };
            HelltakerItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Helltaker_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(HelltakerItem);

            MenuItem HelltakerApronItem = new MenuItem() { Text = m_Language ? "앞치마 헬테이커" : "Helltaker Apron" };
            HelltakerApronItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).HelltakerApron_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(HelltakerApronItem);

            //MenuItem Subject67 = new MenuItem() { Text = m_Language ? "실험체 67" : "Subject 67" };
            //Subject67.Click += (object o, EventArgs e) =>
            //{
            //    m_Girls.Add(new HellGirl(this, null));
            //    (m_Girls.Last().DataContext as HellGirl_ViewModel).Subject67_button_Click();
            //    m_Girls.Last().Show();
            //    (Application.Current as App).SaveCurrentState();
            //};
            //SummonGirls.MenuItems.Add(Subject67);

            MenuItem GloriousLeftItem = new MenuItem() { Text = m_Language ? "Glorious 왼쪽" : "Glorious Left" };
            GloriousLeftItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).GloriousLeft_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(GloriousLeftItem);

            MenuItem GloriousRightItem = new MenuItem() { Text = m_Language ? "Glorious 오른쪽" : "Glorious Right" };
            GloriousRightItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).GloriousRight_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(GloriousRightItem);

            MenuItem SummonAllItem = new MenuItem() { Text = m_Language ? "전부소환" : "Summon all" };
            SummonAllItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Azazel_button_Click();
                m_Girls.Last().Show();
                for (int i = 0; i < 3; i++)
                {
                    m_Girls.Add(new HellGirl(this, null));
                    (m_Girls.Last().DataContext as HellGirl_ViewModel).Cerberus_button_Click();
                    m_Girls.Last().Show();
                }
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Judgement_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Justice_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Lucifer_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).LuciferApron_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Malina_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Modeus_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Pandemonica_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Zdrada_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Skeleton_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).Helltaker_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).HelltakerApron_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).GloriousLeft_button_Click();
                m_Girls.Last().Show();
                m_Girls.Add(new HellGirl(this, null));
                (m_Girls.Last().DataContext as HellGirl_ViewModel).GloriousRight_button_Click();
                m_Girls.Last().Show();
                (Application.Current as App).SaveCurrentState();
            };
            SummonGirls.MenuItems.Add(SummonAllItem);
            #endregion

            #region Language setting menuItem
            MenuItem LangItem = new MenuItem()
            {
                Text = m_Language ? "언어" : "Language"
            };

            MenuItem LangKoreanItem = new MenuItem()
            {
                Text = "한국어"
            };
            LangKoreanItem.Click += (object o, EventArgs e) =>
            {
                if (!m_Language)
                {
                    m_Xml.settings.Language = "Korean";
                    m_Xml.SaveSettings();
                    m_Language = true;
                    GenerateNotifyIcon();
                    foreach (HellGirl girl in m_Girls) (girl.DataContext as HellGirl_ViewModel).Naming(m_Language);
                }
            };
            LangItem.MenuItems.Add(LangKoreanItem);

            MenuItem LangEnglishItem = new MenuItem()
            {
                Text = "English"
            };
            LangEnglishItem.Click += (object o, EventArgs e) =>
            {
                if (m_Language)
                {
                    m_Xml.settings.Language = "English";
                    m_Xml.SaveSettings();
                    m_Language = false;
                    GenerateNotifyIcon();
                    foreach (HellGirl girl in m_Girls) (girl.DataContext as HellGirl_ViewModel).Naming(m_Language);
                }
            };
            LangItem.MenuItems.Add(LangEnglishItem);

            if (m_Language) LangKoreanItem.Checked = true;
            else LangEnglishItem.Checked = true;
            #endregion

            #region etc.
            MenuItem ResetItem = new MenuItem()
            {
                Text = m_Language ? "초기화" : "Reset"
            };
            ResetItem.Click += (object o, EventArgs e) =>
            {
                m_Girls.Insert(0, new HellGirl(this, null));
                (m_Girls[0].DataContext as HellGirl_ViewModel).Lucifer_button_Click();
                m_Girls[0].Show();

                for (int i = 0; i < m_Girls.Count; i++)
                {
                    if (i == m_Girls.Count - 1) break;
                    m_Girls[i + 1].Close();
                }

                int cnt = m_Girls.Count;
                for (int i = 1; i < cnt; i++)
                {
                    m_Girls.RemoveAt(1);
                }

                (Application.Current as App).SaveCurrentState();
            };

            MenuItem CreditItem = new MenuItem()
            {
                Text = m_Language ? "크레딧" : "Credit"
            };
            CreditItem.Click += (object o, EventArgs e) =>
            {
                credit = new CreditWindow();
                credit.Show();
            };

            MenuItem ExitItem = new MenuItem()
            {
                Text = m_Language ? "탈출" : "Exit"
            };
            ExitItem.Click += (object o, EventArgs e) =>
            {
                (Application.Current as App).SaveCurrentState();
                (Application.Current as App).IsOnClose = true;

                foreach (var girl in m_Girls)
                {
                    girl.Close();
                }
                m_Girls.Clear();
                m_Window.Close();
            };
            #endregion

            Menu.MenuItems.Add(SummonGirls);
            Menu.MenuItems.Add(TopMostItem);
            Menu.MenuItems.Add(LangItem);
            Menu.MenuItems.Add(ResetItem);
            Menu.MenuItems.Add(CreditItem);
            Menu.MenuItems.Add(ExitItem);
            Noti.ContextMenu = Menu;
        }

        public void PlayApropos()
        {
            m_Player.URL = @"Resources\Mittsies - Apropos.mp3";
            m_Player.controls.play();
            m_Player.settings.setMode("loop", true);
        }

        public void PlayVitality()
        {
            m_Player.URL = @"Resources\Mittsies - Vitality.mp3";
            m_Player.controls.play();
            m_Player.settings.setMode("loop", true);
        }

        public void PlayEpitomize()
        {
            m_Player.URL = @"Resources\Mittsies - Epitomize.mp3";
            m_Player.controls.play();
            m_Player.settings.setMode("loop", true);
        }

        public void PlayLuminescent()
        {
            m_Player.URL = @"Resources\Mittsies - Luminescent.mp3";
            m_Player.controls.play();
            m_Player.settings.setMode("loop", true);
        }

        public void StopPlayer()
        {
            m_Player.controls.stop();
        }

        private void NextFrame(object sender, EventArgs e)//EventArgs
        {
            Frame++;
            if (Frame == 24) Frame = 0;

            foreach (var girl in m_Girls) (girl.DataContext as HellGirl_ViewModel).NextFrame(Frame);
        }

    }

}
