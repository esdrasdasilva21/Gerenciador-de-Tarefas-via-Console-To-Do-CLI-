using System.Text;

namespace TodoCli;

public class TarefaItem
{
    private int _id;
    private string _titulo = string.Empty;
    private string _categoria = "Geral";

    public int Id
    {
        get => _id;
        set => _id = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "O ID não pode ser negativo.");
    }

    public string Titulo
    {
        get => _titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(value));

            _titulo = value.Trim();
        }
    }

    public bool Concluida { get; private set; }
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;

    public string Categoria
    {
        get => _categoria;
        set => _categoria = string.IsNullOrWhiteSpace(value) ? "Geral" : value.Trim();
    }

    public DateOnly? DataLimite { get; set; }

    public bool EstaAtrasada => EstaAtrasadaEm(DateOnly.FromDateTime(DateTime.Today));

    public TarefaItem() { }

    public TarefaItem(
        int id,
        string titulo,
        PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
        string categoria = "Geral",
        DateOnly? dataLimite = null,
        bool concluida = false)
    {
        Id = id;
        Titulo = titulo;
        Prioridade = prioridade;
        Categoria = categoria;
        DataLimite = dataLimite;
        Concluida = concluida;
    }

    public bool EstaAtrasadaEm(DateOnly dataReferencia) =>
        !Concluida && DataLimite.HasValue && DataLimite.Value < dataReferencia;

    public void MarcarComoConcluida() => Concluida = true;

    public void Reabrir() => Concluida = false;

    public void AdiarPara(DateOnly novaDataLimite)
    {
        if (DataLimite.HasValue && novaDataLimite < DataLimite.Value)
            throw new ArgumentException("A nova data limite não pode ser anterior à data atual.", nameof(novaDataLimite));

        DataLimite = novaDataLimite;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(Concluida ? "[X]" : "[ ]");
        sb.Append($" #{Id} [{Prioridade}] ({Categoria}) - {Titulo}");

        if (DataLimite.HasValue)
            sb.Append($" | Prazo: {DataLimite.Value:dd/MM/yyyy}");

        if (EstaAtrasada)
            sb.Append(" [ATRASADA]");

        return sb.ToString();
    }
}