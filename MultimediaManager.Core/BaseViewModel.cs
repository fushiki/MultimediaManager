using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core
{
    public abstract class BaseViewModel : IDisposable, INotifyPropertyChanged
    {
        


        protected virtual void OnDisposable()
        {

        }

        [Conditional("DEBUG")]
        private void veryfyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string exception = "Infalid property name: " + propertyName;
                Debug.Fail(exception);
            }
        }

        #region IDisposable members
        public void Dispose()
        {
            OnDisposable();
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            veryfyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var arg = new PropertyChangedEventArgs(propertyName);
                handler(this, arg);
            }
        }

        #endregion

       
    }
}
