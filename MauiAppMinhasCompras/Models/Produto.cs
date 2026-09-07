using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                // Armazena valor trimado; não lança exceção aqui para não quebrar o binding
                _descricao = value?.Trim();
            }
        }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total { get => Quantidade * Preco; }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new InvalidOperationException("Por favor, preencha a descrição");
        }
    }
}