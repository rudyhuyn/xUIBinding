using CrossUIBindingSample.ViewModels;
using System;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace CrossUIBindingSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private MainViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode==NavigationMode.New || e.NavigationMode == NavigationMode.Refresh)
            {
                ViewModel = (MainViewModel)e.Parameter;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
                
            }
        }

        private async void CreateWindow_Click(object sender, RoutedEventArgs e)
        {
            var vm = ViewModel;
            var newAV = CoreApplication.CreateNewView();
            Window newWindow = null;
            Frame frame = null;
            ApplicationView newAppView = null;
            var currentAV = ApplicationView.GetForCurrentView();

            await newAV.Dispatcher.RunAsync(
                            CoreDispatcherPriority.High,
                            () =>
                            {
                                newWindow = Window.Current;
                                newAppView = ApplicationView.GetForCurrentView();
                                frame = new Frame();
                                frame.Navigate(typeof(MainPage), vm);
                                newWindow.Content = frame;
                                newWindow.Activate();


                                ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                                    newAppView.Id,
                                    ViewSizePreference.UseMinimum,
                                    currentAV.Id,
                                    ViewSizePreference.UseMinimum);

                            });


        }

        private void IncrementCounter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IncrementCounter();
        }

        private void IncrementCrossUICounter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IncrementCounterCrossUI();
        }

        private void IncrementCrossUICounterManual_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IncrementCounterCrossUIManual();
        }
        private void IncrementCounterContainer_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IncrementCounterContainerCrossUI();
        }
          
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
