using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.Models;
using MyFinances.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyFinances.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<OperationDto> Operations { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command PreviousPageCommand { get; }
        public Command NextPageCommand { get; }
        public Command DeleteItemCommand { get; }
        public Command<OperationDto> ItemTapped { get; }

        private bool _ifPrevBtnVisible;
        private bool _ifNextBtnVisible;

        public bool IfPrevBtnVisible
        {
            get { return _ifPrevBtnVisible; }
            set 
            {
                _ifPrevBtnVisible = value;
                OnPropertyChanged();
            }
        }   
        public bool IfNextBtnVisible
        {
            get { return _ifNextBtnVisible; }
            set
            {
                _ifNextBtnVisible = value;
                OnPropertyChanged();
            }
        }

        public int PageCounter { get; set; }

        public ItemsViewModel()
        {
            Title = "Operacje";

            IfNextBtnVisible = true;

            PageCounter = 1;

            Operations = new ObservableCollection<OperationDto>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OperationDto>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            PreviousPageCommand = new Command(async () => await OnPreviousPage());

            NextPageCommand = new Command(async () => await OnNextPage());

            DeleteItemCommand = new Command<OperationDto>(async (x) => await OnDeleteItem(x));
        }

        private async Task OnNextPage()
        {
            IfPrevBtnVisible = true;

            PageCounter += 2;

            await ExecuteLoadItemsCommand();

            if (Operations.Count == 0)
            {
                IfNextBtnVisible = false;
            }

            PageCounter--;

            await ExecuteLoadItemsCommand();                       

        }

        private async Task OnPreviousPage()
        {
            PageCounter -= 1;

            IfNextBtnVisible = true;

            if (PageCounter == 1)            
                IfPrevBtnVisible = false;            

            await ExecuteLoadItemsCommand();
        }

        private async Task OnDeleteItem(OperationDto operation)
        {
            if (operation == null)
                return;

            var dialog = await Shell.Current.DisplayAlert("Usuwanie", $"Czy napewno chcesz usunąć operację: {operation.Name}?", "Tak", "Nie");

            if (!dialog)
                return;

            var response = await OperationService.DeleteAsync(operation.Id);

            if (!response.IsSuccess)
                await ShowErrorAlert(response);

            Operations.Remove(operation);

            if (Operations.Count == 0)
                PageCounter--;
            

            await ExecuteLoadItemsCommand();
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var response = await OperationService.GetPaginatedAsync(PageCounter);

                if (!response.IsSuccess)
                    await ShowErrorAlert(response);

                Operations.Clear();

                foreach (var item in response.Data)
                {
                    Operations.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                        "Wystąpił Błąd!",
                        string.Join(". ", ex.Message),
                        "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(OperationDto operation)
        {
            if (operation == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={operation.Id}");
        }
    }
}