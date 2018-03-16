using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Huyn.CrossUIBinding
{
    public class CrossUIItem<T>
    {
        public event EventHandler Updated;
        private T _item;
        private bool _autoRaisePropertyChanged;
        public CrossUIItem(T property = default(T), bool autoRaisePropertyChanged = false)
        {
            _item = property;
            _autoRaisePropertyChanged = autoRaisePropertyChanged;
        }

        public T Value
        {
            get
            {
                return _item;
            }
            set
            {
                SetValue(value);
            }
        }

        public Dictionary<CoreDispatcher, CrossUIItemClone<T>> _clones = new Dictionary<CoreDispatcher, CrossUIItemClone<T>>();

        public CrossUIItemClone<T> Clone
        {
            get
            {
                CrossUIItemClone<T> val;
                var dispatcher = Window.Current.Dispatcher;
                if (_clones.TryGetValue(dispatcher, out val))
                {
                    return val;
                }
                else
                {
                    val = new CrossUIItemClone<T>(this, dispatcher);
                    _clones[dispatcher] = val;

                    ApplicationView.GetForCurrentView().Consolidated += (sender, e) =>
                      {
                          if (e.IsAppInitiated || e.IsUserInitiated)
                          {
                              CrossUIItemClone<T> cloneToUnlink;
                              if (_clones.TryGetValue(dispatcher, out cloneToUnlink))
                              {
                                  _clones.Remove(dispatcher);
                                  cloneToUnlink.Unlink();
                              }
                          }
                      };
                    return val;
                }
            }
        }


        public void SetValue(T value)
        {
            if (_item == null)
            {
                if (value == null)
                    return;
            }
            else if (_item.Equals(value))
                return;

            _item = value;
            if(_autoRaisePropertyChanged)
                Updated?.Invoke(this, null);
        }

        public void RaisePropertyChanged()
        {
            Updated?.Invoke(this, null);
        }
    }
}
