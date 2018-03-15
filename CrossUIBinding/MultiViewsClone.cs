using System;
using System.ComponentModel;
using Windows.UI.Core;

namespace Huyn.CrossUIBinding
{
    
    public class CrossUIBindingClone<T> : INotifyPropertyChanged
    {
        public T Item
        {
            get
            {
                return _source.GetValue();
            }
            set
            {
          //      _source.Updated -= Source_Updated;
                _source.UpdateValue(value);
            //    _source.Updated += Source_Updated;
            }
        }


        private readonly CoreDispatcher _dispatcher;
        private readonly CrossUIBindingProvider<T> _source;

        public CrossUIBindingClone(CrossUIBindingProvider<T> source, CoreDispatcher dispatcher)
        {
            _source = source;
            _source.Updated += Source_Updated;
            _dispatcher = dispatcher;
        }

        internal void Unlink()
        {
            this._source.Updated -= Source_Updated;
        }

        private void Source_Updated(object sender, object s)
        {
            try
            {
                _dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item)));
                });
            }
            catch(Exception)
            {
                _source.Updated -= Source_Updated;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
