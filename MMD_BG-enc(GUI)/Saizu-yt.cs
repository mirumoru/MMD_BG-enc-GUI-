using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MMD_enc3
{
    public class ViewModel : INotifyPropertyChanged
    {
        // INotifyPropertyChangedインターフェースの実装
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //TextBoxの入力可否用のプロパティ(サイズ)
        private bool _IsEdit = false;
        public bool IsEdit
        {
            get { return _IsEdit; }
            set { if (_IsEdit != value) { _IsEdit = value; RaisePropertyChanged(); } }
        }

        //TextBoxの入力可否用のプロパティ(YouTube)
        private bool _IsEdit2 = false;
        public bool IsEdit2
        {
            get { return _IsEdit2; }
            set { if (_IsEdit2 != value) { _IsEdit2 = value; RaisePropertyChanged(); } }
        }
    }


    
}
