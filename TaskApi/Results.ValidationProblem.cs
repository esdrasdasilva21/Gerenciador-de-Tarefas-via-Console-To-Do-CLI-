static Dictionary<string, string[]> ValidarTarefa(string titulo, PrioridadeTarefa prioridade)
{
    Dictionary<string, string[]> erros = new();

    if (string.IsNullOrWhiteSpace(titulo))
    {
        erros["Titulo"] = new[] { "O título da tarefa é obrigatório." };
    }
    else if (titulo.Trim().Length > 120)
    {
        erros["Titulo"] = new[] { "O título deve ter no máximo 120 caracteres." };
    }

    if (!Enum.IsDefined(typeof(PrioridadeTarefa), prioridade))
    {
        erros["Prioridade"] = new[] { "Prioridade inválida. Use Baixa (1), Media (2) ou Alta (3)." };
    }

    return erros;
}