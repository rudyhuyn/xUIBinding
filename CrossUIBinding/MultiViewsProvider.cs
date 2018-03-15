using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Huyn.CrossUIBinding
{
    public class CrossUIBindingProvider<T>
    {
        public event EventHandler Updated;
        private T _item;
        public CrossUIBindingProvider(T property = default(T))
        {
            _item = property;
        }

        public T GetValue()
        {
            return _item;
        }

        public Dictionary<CoreDispatcher, CrossUIBindingClone<T>> _clones = new Dictionary<CoreDispatcher, CrossUIBindingClone<T>>();

        public CrossUIBindingClone<T> Clone
        {
            get
            {
                CrossUIBindingClone<T> val;
                var dispatcher = Window.Current.Dispatcher;
                if (_clones.TryGetValue(dispatcher, out val))
                {
                    return val;
                }
                else
                {
                    val = new CrossUIBindingClone<T>(this, dispatcher);
                    _clones[dispatcher] = val;

                    ApplicationView.GetForCurrentView().Consolidated += (sender, e) =>
                      {
                          if (e.IsAppInitiated || e.IsUserInitiated)
                          {
                              CrossUIBindingClone<T> cloneToUnlink;
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


        public void UpdateValue(T value)
        {
            if (_item == null)
            {
                if (value == null)
                    return;
            }
            else if (_item.Equals(value))
                return;

            _item = value;
            Updated?.Invoke(this, null);
        }
    }
}
