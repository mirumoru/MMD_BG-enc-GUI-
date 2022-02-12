using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
// ファイル選択ダイアログの名前空間をusing Microsoft.Win32
// System.IOのusingを追加


namespace MMD_enc3
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        // ファイルを開くダイアログボックスを表示		 
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // テキストボックスのリセット
            TextBox_1.Text = "";

            MessageBox.Show("変換(デコード)する動画を選択して下さい。", "説明");

            // ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            // ファイルの設定
            dialog.Filter = "動画ファイル (*.mp4)|*.mp4|全てのファイル(*.*)|*.* ";


            // ダイアログを表示
            if (dialog.ShowDialog() == true)
            {
                //テキストボックスにパスを代入
                TextBox_1.Text = dialog.FileName;

                // 選択されたファイルパスをメッセージボックスに表示
                MessageBox.Show(dialog.FileName, "パスを表示");
                

            }
            MessageBox.Show("動画の保存先を設定して下さい。", "説明");

            var dialog2 = new System.Windows.Forms.SaveFileDialog();

            // ファイルの設定
            dialog2.Filter = "動画ファイル (*.avi)|*.avi|全てのファイル (*.*)|*.*"; ;

            if (dialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog2.FileName, "パスを表示");
                MessageBox.Show("変換(デコード)を開始します。", "通知");

            }
            // パスを代入
            string input = dialog.FileName;
            string output = dialog2.FileName;
            
            // ffmpegのコマンドライン
            var arguments = string.Format("-i \"{0}\" -r 30 -b:v 1500K -vcodec rawvideo -y -an \"{1}\"", input, output);

            // ffmpegの実行
            var process = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
            });
            
            process.WaitForExit();

            // テキストボックスのリセット
            TextBox_1.Text = "";

            MessageBox.Show("変換(デコード)が終了しました。", "通知");
            
        }


        private void version_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MMD_BC-enc(GUI)\nv1.0\nby mirumoru", "バージョン情報");
        }
    }
    }
         


 
    

   
















