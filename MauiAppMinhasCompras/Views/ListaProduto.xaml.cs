using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
    }

	protected async override void OnAppearing()
	{
		List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i));

    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		string q = e.NewTextValue;

		lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));






    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

		string msg = $"O total é: {soma:C}";

		DisplayAlert("Total dos produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            var menuItem = sender as MenuItem;
            var produto = menuItem?.CommandParameter as Produto ?? menuItem?.BindingContext as Produto;
            if (produto == null)
            {
                await DisplayAlert("Erro", "Produto não encontrado.", "OK");
                return;
            }
    
            bool confirmar = await DisplayAlert("Confirmar", "Deseja deletar o item selecionado?", "Sim", "Não");
            if (!confirmar)
                return;

            
            int rows = await App.Db.Delete(produto.Id);

            if (rows > 0)
            {
                lista.Remove(produto);
                await DisplayAlert("Sucesso!", "Registro Deletado", "OK");
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível deletar o registro.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}