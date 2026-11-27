PrioridadeTarefa prioridade = entradaPrioridade?.Trim() switch
{
    "" or "2" => PrioridadeTarefa.Media,
    "1" => PrioridadeTarefa.Baixa,
    "3" => PrioridadeTarefa.Alta,
    _ => PrioridadeTarefa.Media
};

if (!string.IsNullOrWhiteSpace(entradaData))
{
    if (DateOnly.TryParseExact(
        entradaData.Trim(),
        ["dd/MM/yyyy", "yyyy-MM-dd"],
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateOnly dataConvertida))
    {
        dataLimite = dataConvertida;
    }
    else
    {
        Console.WriteLine("Data inválida. A tarefa será salva sem data limite.");
    }
}