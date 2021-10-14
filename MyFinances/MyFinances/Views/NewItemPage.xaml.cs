using MyFinances.Core.Dtos;
using MyFinances.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyFinances.Views
{
    public partial class NewItemPage : ContentPage
    {
        public OperationDto Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}