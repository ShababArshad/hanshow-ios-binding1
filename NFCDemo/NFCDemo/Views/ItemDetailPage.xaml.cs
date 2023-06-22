using System.ComponentModel;
using Xamarin.Forms;
using NFCDemo.ViewModels;

namespace NFCDemo.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
