using System;
using System.Windows.Forms;

namespace Emu12864
{
    public partial class Launcher : Form
    {
        private Game GameObj;
        public static readonly string GameTitle = "Emu12864";

        public Launcher()
        {
            InitializeComponent();
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            this.Hide();

            /* 显示一个‘加载中’的窗口
             */
            Loading Load_Obj = new Loading();
            Load_Obj.Show();

            /* 分辨率设置以及窗口加载
             */
            Control.ControlCollection DModes = DMPanel.Controls;
            foreach (RadioButton r in DModes)
            {
                if (r.Checked)
                {
                    if (!Core.DxCS.DxInit(this.Icon.Handle, GameTitle,
                        FullScreen.Checked,
                        Convert.ToInt32(r.Text.Split('×')[0]), Convert.ToInt32(r.Text.Split('×')[1]))
                        ) goto ExitFlag;
                    break;
                }
            }

            /* 实例化游戏类
             */
            GameObj = new Game();

            /* 关闭Loading窗体
             */
            Load_Obj.Dispose();

            /* 主游戏循环
             */
            while (!((DxDLL.DX.ProcessMessage() == -1) | Core.Base.GetKey(Keys.KeyESCAPE)))
            {
                GameObj.Work();
            }

        ExitFlag:
            
            Core.DxCS.DxEnd();
            this.Dispose();
        }

        private void Launcher_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("NSDN(TM) 2014 - 2017\nCopyright the WDJ 2005 - 2017", "Something...");
        }
    }
}
