using Huyn.CrossUIBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossUIBindingSample.ViewModels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        #region Standard Binding
        public int Counter { get; set; } = 0;
        public void IncrementCounter()
        {
            ++Counter;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Counter)));
        }
        #endregion

        #region CrossUIBinding
        public CrossUIBindingProvider<int> CounterCrossUI { get; set; } = new CrossUIBindingProvider<int>(0);

        public void IncrementCounterCrossUI()
        {
            ++CounterCrossUI.Clone.Item;
        }

        #endregion

        #region CrossIndirectUIBinding
        public CrossUIBindingProvider<CounterContainer> CounterContainerCrossUI { get; set; } = new CrossUIBindingProvider<CounterContainer>(new CounterContainer());
        public void IncrementCounterContainerCrossUI()
        {
            CounterContainerCrossUI.Clone.Item = new CounterContainer() { Value = CounterContainerCrossUI.GetValue().Value + 1 };
        }

        #endregion

        public CrossUIBindingProvider<double> CrossUISliderValue = new CrossUIBindingProvider<double>(0);
        public CrossUIBindingProvider<bool> CrossUICheckboxValue = new CrossUIBindingProvider<bool>(false);
        public CrossUIBindingProvider<string> CrossUITextboxValue = new CrossUIBindingProvider<string>("Nodo, Mango, Apollo");


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
