namespace TodoCli;

public enum OrdenacaoTarefa
{
    CriacaoDesc,
    CriacaoAsc,
    PrioridadeDesc,
    TituloAsc
}

public sealed record FiltroTarefas(
    bool? ApenasConcluidas = null,
    PrioridadeTarefa? PrioridadeMinima = null,
    string? TermoBusca = null,
    OrdenacaoTarefa OrdenarPor = OrdenacaoTarefa.CriacaoDesc,
    int? Limite = null);