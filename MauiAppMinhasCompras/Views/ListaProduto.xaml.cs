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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarLista();
    }

    private async Task CarregarLista(string textoBusca = null)
    {
        lista.Clear();

        List<Produto> tmp;

        if (string.IsNullOrWhiteSpace(textoBusca))
            tmp = await App.Db.GetAll();
        else
            tmp = await App.Db.Search(textoBusca);

        foreach (var item in tmp)
            lista.Add(item);
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue?.Trim();
        await CarregarLista(q);
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
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
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirmar = await DisplayAlert("Confirmar", $"Deseja deletar o item {p.Descricao}?", "Sim", "Não");
            if (confirmar)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
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

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto { BindingContext = p });

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}