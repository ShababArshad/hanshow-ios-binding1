using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NFCDemo.Models;
using NFCDemo.ViewModels;

namespace NFCDemo.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }
        Services.IESLService eSLService;
        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
             eSLService =   DependencyService.Resolve<Services.IESLService>();
           if(eSLService != null)
            {
                eSLService.OnESLIdDetected += Service_OnESLIdDetected;
                eSLService.FakeESL();
                
            }
        }

        private void Service_OnESLIdDetected(object sender, int e)
        {
            _ = Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Title", $"ESL Id {e}","OK");
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            eSLService.StartScan();
        }

        void ButtonStop_Clicked(System.Object sender, System.EventArgs e)
        {
            eSLService.StopScan();
        }
    }
}
