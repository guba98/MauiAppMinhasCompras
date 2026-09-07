using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            if (produto_anexado == null)
            {
                await DisplayAlert("Erro", "Produto não encontrado no contexto.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Erro", "Descrição é obrigatória.", "OK");
                return;
            }

            if (!double.TryParse(txt_quantidade.Text, out double quantidade))
            {
                await DisplayAlert("Erro", "Quantidade inválida.", "OK");
                return;
            }

            if (!double.TryParse(txt_preco.Text, out double preco))
            {
                await DisplayAlert("Erro", "Preço inválido.", "OK");
                return;
            }

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = quantidade,
                Preco = preco
            };

            p.Validate();

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}