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

        #region CrossUIBinding Manual
        public CrossUIItem<int> CounterCrossUIManual { get; set; } = new CrossUIItem<int>(0, false);

        public void IncrementCounterCrossUIManual()
        {
            ++CounterCrossUIManual.Clone.Value;
            CounterCrossUIManual.RaisePropertyChanged();
        }

        #endregion


        #region CrossUIBinding
        public CrossUIItem<int> CounterCrossUI { get; set; } = new CrossUIItem<int>(0, true);

        public void IncrementCounterCrossUI()
        {
            ++CounterCrossUI.Clone.Value;
        }

        #endregion

        #region CrossIndirectUIBinding
        public CrossUIItem<CounterContainer> CounterContainerCrossUI { get; set; } = new CrossUIItem<CounterContainer>(new CounterContainer(), true);
        public void IncrementCounterContainerCrossUI()
        {
            CounterContainerCrossUI.Clone.Value = new CounterContainer() { Value = CounterContainerCrossUI.Value.Value + 1 };
        }

        #endregion

        public CrossUIItem<double> CrossUISliderValue { get; set; } = new CrossUIItem<double>(0d, true);
        public CrossUIItem<bool> CrossUICheckboxValue { get; set; } = new CrossUIItem<bool>(false, true);
        public CrossUIItem<string> CrossUITextboxValue { get; set; } = new CrossUIItem<string>("Nodo, Mango, Apollo", true);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
