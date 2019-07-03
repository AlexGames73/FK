using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fk.Models
{
    public class AuthForm : INotifyPropertyChanged
    {
        private bool isEmailValid;
        private bool isEmailSend;
        private bool isActivated;

        public bool IsEmailValid
        {
            get { return isEmailValid; }
            set
            {
                isEmailValid = value;
                OnPropertyChanged();
            }
        }
        public bool IsEmailSend
        {
            get { return isEmailSend; }
            set
            {
                isEmailSend = value;
                OnPropertyChanged();
            }
        }
        public bool IsActivated
        {
            get { return isActivated; }
            set
            {
                isActivated = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
