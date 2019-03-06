namespace Licitar {
    
    public class ItensOrcamentoDb
    {

        public int idOrcOrcamento { get; set; }

        public int idOrcInsumos { get; set; }

        public int idOrcBdis { get; set; }

        public int idOrcOrcamentoPai { get; set; }

        public int Sequencia { get; set; }

        public string CodBaseRef { get; set; }

        public string Itemizacao { get; set; }

        public string Descricao { get; set; }

        public string Unidade { get; set; }

        public double Quantidade { get; set; }

        public double ValorUnitarioVenda { get; set; }

        public bool Titulo { get; set; }

    }
}
