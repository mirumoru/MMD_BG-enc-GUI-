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
        {  //comboBoxの追加
            InitializeComponent();
            Enccombo.Items.Add("rawvideo");
            Enccombo.Items.Add("h264_nvenc");
            Enccombo.Items.Add("h264_qsv");

            //comboBoxのデフォルト設定
            Enccombo.SelectedIndex = 0;

            // テキストボックスのセット
            TextBox_1.Text = "ここにパスが入力されます";
            YT_pas.Text = "違法動画のダウンロード禁止です。";
            saizu_BT.Text = "";
            saizu_BH.Text = "";

            // チェックボックスの制御
            DataContext = new ViewModel();
            

        }

        


        // ファイルを開くダイアログボックスを表示		 
        private void button1_Click(object sender, RoutedEventArgs e)
        {


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
            else if (dialog.ShowDialog() == false)
            {
                Console.WriteLine("操作が中止されました。");

            }


            MessageBox.Show("動画の保存先を設定して下さい。", "説明");

            // 保存ダイアログ設定
            var dialog2 = new System.Windows.Forms.SaveFileDialog();

            // ファイルの設定
            dialog2.Filter = "動画ファイル (*.avi)|*.avi|全てのファイル (*.*)|*.*"; ;

            if (dialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog2.FileName, "パスを表示");
                MessageBox.Show("音声の保存先を設定して下さい。", "通知");

            }

            else if (dialog2.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                Console.WriteLine("キャンセルボタンが押されました。");
            }
            dialog2.Dispose();

            // 保存ダイアログ設定
            var dialog3 = new System.Windows.Forms.SaveFileDialog();

            // ファイルの設定
            dialog3.Filter = "音声ファイル (*.wav)|*.wav|全てのファイル (*.*)|*.*"; ;
            if (dialog3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog3.FileName, "パスを表示");
                MessageBox.Show("変換(デコード)を開始します。\n映像のデコードの後に\n音声のデコードが始まります。", "通知");

            }
            else if (dialog3.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                Console.WriteLine("エラーが発生しました。");
            }
            dialog3.Dispose();

            
            //コモンズ設定
            var sb = new StringBuilder();
            sb.AppendLine(Enccombo.Text);

            // パスを代入
            string input = dialog.FileName;
            string output = dialog2.FileName;
            string vcodec = Enccombo.Text;
            string muoutput = dialog3.FileName;
            string saizu = saizu_BH.Text +"*"+ saizu_BT.Text;

            // チェックボックスが選択されているか
            if ((bool)saizu_CheckBox.IsChecked) 
            {
                // 動画ffmpegのコマンドライン
                var arguments = string.Format("-i \"{0}\" -s \"{1}\" -r 30 -b:v 1500K -vcodec \"{2}\" -y -an \"{3}\"", input, saizu, vcodec, output);

                // 音声ffmpegのコマンドライン
                var arguments2 = string.Format("-i \"{0}\" -sample_fmt s16 -ar 48000 -y \"{1}\"", input, muoutput);

                // ffmpegの実行
                var process = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments)

                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                });

                process.WaitForExit();

                var process2 = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments2)

                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                });

                process2.WaitForExit();

                MessageBox.Show("変換(デコード)が終了しました。", "通知");

            }
            else
            {


                // 動画ffmpegのコマンドライン
                var arguments = string.Format("-i \"{0}\" -r 30 -b:v 1500K -vcodec \"{1}\" -y -an \"{2}\"", input, vcodec, output);

                // 音声ffmpegのコマンドライン
                var arguments2 = string.Format("-i \"{0}\" -sample_fmt s16 -ar 48000 -y \"{1}\"", input, muoutput);

                // ffmpegの実行
                var process = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments)

                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                });

                process.WaitForExit();

                var process2 = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments2)

                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                });

                process2.WaitForExit();

                MessageBox.Show("変換(デコード)が終了しました。", "通知");

            }

        //エクスプローラーで出力ファイルを開く
        string PathString = output;
            string dirName = System.IO.Path.GetDirectoryName(PathString);
            PathString += dirName + "\r\n";
            System.Diagnostics.Process.Start("explorer.exe", dirName);

            // テキストボックスのセット
            TextBox_1.Text = dirName;

        }

        private void version_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MMD_BC-encGUIv1.3(β1.6)\nby mirumoru", "バージョン情報");
        }


        private void ComboBoxMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //空白

        }

        private void exp_Click(object sender, RoutedEventArgs e)
        {
            string Enc = TextBox_1.Text;

            //エクスプローラーを開く
            System.Diagnostics.Process.Start("explorer.exe", Enc);
        }

        private void NVE_enc_Click(object sender, RoutedEventArgs e)
        {


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
            else if (dialog.ShowDialog() == false)
            {
                Console.WriteLine("操作が中止されました。");

            }


            MessageBox.Show("動画の保存先を設定して下さい。", "説明");

            // 保存ダイアログ設定
            var dialog2 = new System.Windows.Forms.SaveFileDialog();

            // ファイルの設定
            dialog2.Filter = "動画ファイル (*.avi)|*.avi|全てのファイル (*.*)|*.*"; ;

            if (dialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog2.FileName, "パスを表示");
                MessageBox.Show("音声の保存先を設定して下さい。", "通知");

            }

            else if (dialog2.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                Console.WriteLine("キャンセルボタンが押されました。");
            }
            dialog2.Dispose();

            // 保存ダイアログ設定
            var dialog3 = new System.Windows.Forms.SaveFileDialog();

            // ファイルの設定
            dialog3.Filter = "音声ファイル (*.wav)|*.wav|全てのファイル (*.*)|*.*"; ;
            if (dialog3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog3.FileName, "パスを表示");
                MessageBox.Show("変換(デコード)を開始します。\n映像のデコードの後に\n音声のデコードが始まります。", "通知");

            }
            else if (dialog3.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                Console.WriteLine("キャンセルボタンが押されました。");
            }
            dialog3.Dispose();


            //コモンズ設定
            var sb = new StringBuilder();
            sb.AppendLine(Enccombo.Text);

            // パスを代入
            string input = dialog.FileName;
            string output = dialog2.FileName;
            string muoutput = dialog3.FileName;
            string saizu2 = saizu_BH.Text + "x" + saizu_BT.Text;

            if ((bool)saizu_CheckBox.IsChecked)
            {
                // 動画NVEncのコマンドライン
                var arguments = string.Format("--fps 30 --cbr 1500 --output-res \"{0}\" --avsync vfr -i \"{1}\" -o \"{2}\"", saizu2 , input, output);

                // NVEncの実行
                var process = Process.Start(new ProcessStartInfo("Tool/NVEncC64.exe", arguments)

                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                });
                process.WaitForExit();

                // 音声ffmpegのコマンドライン
                var arguments2 = string.Format("-i \"{0}\" -sample_fmt s16 -ar 48000 -y \"{1}\"", input, muoutput);

                // ffmpegの実行
                var process2 = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments2)

                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                });

                process2.WaitForExit();

                MessageBox.Show("変換(デコード)が終了しました。", "通知");
            }

            else { 

            // 動画NVEncのコマンドライン
                var arguments = string.Format("--fps 30 --cbr 1500 --avsync vfr -i \"{0}\" -o \"{1}\"", input, output);

            // NVEncの実行
            var process = Process.Start(new ProcessStartInfo("Tool/NVEncC64.exe", arguments)

            {
                CreateNoWindow = false,
                UseShellExecute = false,
            });
            process.WaitForExit();

            // 音声ffmpegのコマンドライン
            var arguments2 = string.Format("-i \"{0}\" -sample_fmt s16 -ar 48000 -y \"{1}\"", input, muoutput);

            // ffmpegの実行
            var process2 = Process.Start(new ProcessStartInfo("Tool/ffmpeg.exe", arguments2)

            {
                CreateNoWindow = false,
                UseShellExecute = false,
            });

            process2.WaitForExit();

            MessageBox.Show("変換(デコード)が終了しました。", "通知");

            }

            //エクスプローラーで出力ファイルを開く
            string PathString = output;
            string dirName = System.IO.Path.GetDirectoryName(PathString);
            PathString += dirName + "\r\n";
            System.Diagnostics.Process.Start("explorer.exe", dirName);

            // テキストボックスのセット
            TextBox_1.Text = dirName;
        }



            //ここからYTのコマンドライン
        private void YTdl_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("動画の保存先を設定して下さい。", "説明");

            // 保存ダイアログ設定
            var dialog4 = new System.Windows.Forms.SaveFileDialog();

            // ファイルの設定
            dialog4.Filter = "動画ファイル (*.mp4)|*.mp4|全てのファイル (*.*)|*.*"; ;

            if (dialog4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(dialog4.FileName, "パスを表示");
                MessageBox.Show("ダウンロードを開始します。", "通知");

            }
            else if (dialog4.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                Console.WriteLine("キャンセルボタンが押されました。");
            }
            dialog4.Dispose();


            //コモンズ設定
            var sb = new StringBuilder();
            sb.AppendLine(Enccombo.Text);

            // パスを代入
            string URL = YT_pas.Text;
            string output = dialog4.FileName;
            string muoutput = dialog4.FileName;
            string saizu2 = saizu_BH.Text + "x" + saizu_BT.Text; //不明

            
            
         // yt-dlpのコマンドライン
          var arguments = string.Format("-f bv[ext=mp4]+ba[ext=m4a] --merge-output-format mp4 -o \"{0}\" \"{1}\"", output, URL);

         // yt-dlpの実行
          var process = Process.Start(new ProcessStartInfo("Tool/yt-dlp.exe", arguments)

         　{
              CreateNoWindow = false,
              UseShellExecute = false,
           });
         process.WaitForExit();

            //エクスプローラーで出力ファイルを開く
            string PathString = output;
            string dirName = System.IO.Path.GetDirectoryName(PathString);
            PathString += dirName + "\r\n";
            System.Diagnostics.Process.Start("explorer.exe", dirName);

            // テキストボックスのセット
            TextBox_1.Text = dirName;
        }


        private void saizu_Checked(object sender, RoutedEventArgs e)
        {
            //空白
        }
        

        private void saizu_BT_TextChanged(object sender, TextChangedEventArgs e)
        {
            //空白
        }

        private void saizu_BH_TextChanged(object sender, TextChangedEventArgs e)
        {
            //空白
        }

        private void LIcense_Click(object sender, RoutedEventArgs e)
        {
            var process = Process.Start(new ProcessStartInfo("notepad.exe",@"""Tool/license.txt"""));
            process.WaitForExit();
        }

        private void Readme_Click(object sender, RoutedEventArgs e)
        {
            var process = Process.Start(new ProcessStartInfo("notepad.exe", @"""Readme.txt"""));
            process.WaitForExit();
        }

        

        private void YT_dl_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
    
}




















