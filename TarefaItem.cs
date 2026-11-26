using System.Globalization;
using System.Text;

namespace TodoCli;

public class TarefaItem : IEquatable<TarefaItem>
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    private int _id;
    private string _titulo = string.Empty;
    private string _categoria = "Geral";
    private readonly TimeProvider _timeProvider;

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

    public bool EstaAtrasada => EstaAtrasadaEm(Hoje());

    public TarefaItem(TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public TarefaItem(
        int id,
        string titulo,
        PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
        string categoria = "Geral",
        DateOnly? dataLimite = null,
        bool concluida = false,
        TimeProvider? timeProvider = null) : this(timeProvider)
    {
        Id = id;
        Titulo = titulo;
        Prioridade = prioridade;
        Categoria = categoria;
        DataLimite = dataLimite;
        Concluida = concluida;
    }

    public DateOnly Hoje() =>
        DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);

    public bool EstaAtrasadaEm(DateOnly dataReferencia) =>
        !Concluida && DataLimite.HasValue && DataLimite.Value < dataReferencia;

    public void MarcarComoConcluida() => Concluida = true;
    public void Reabrir() => Concluida = false;

    public void AtualizarDetalhes(string titulo, PrioridadeTarefa prioridade, string categoria, DateOnly? dataLimite)
    {
        Titulo = titulo;
        Prioridade = prioridade;
        Categoria = categoria;
        DataLimite = dataLimite;
    }

    public bool Equals(TarefaItem? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id != 0 && Id == other.Id;
    }

    public override bool Equals(object? obj) => Equals(obj as TarefaItem);

    public override int GetHashCode() => Id != 0 ? Id.GetHashCode() : HashCode.Combine(Titulo, Categoria, DataLimite);

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(Concluida ? "[X]" : "[ ]");
        sb.Append($" #{Id} [{Prioridade}] ({Categoria}) - {Titulo}");

        if (DataLimite.HasValue)
            sb.Append($" | Prazo: {DataLimite.Value.ToString("dd/MM/yyyy", PtBr)}");

        if (EstaAtrasada)
            sb.Append(" [ATRASADA]");

        return sb.ToString();
    }
}