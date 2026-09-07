using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
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
				Descricao = txt_descricao.Text,
				Quantidade = quantidade,
				Preco = preco
			};

			// validações do modelo
			p.Validate();

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso!", "Registro Inserido", "OK");
		}
		catch(Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "ok");
		}
    }
}