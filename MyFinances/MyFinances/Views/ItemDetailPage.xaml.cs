using MyFinances.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyFinances.Views
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