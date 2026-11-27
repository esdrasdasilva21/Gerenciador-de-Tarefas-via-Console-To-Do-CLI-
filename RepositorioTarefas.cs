public async Task<IReadOnlyList<TarefaItem>> CarregarAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_caminhoArquivo)) return [];

        await using FileStream fluxo = new(
            _caminhoArquivo,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true);

        if (fluxo.Length == 0) return [];

        return await JsonSerializer.DeserializeAsync<List<TarefaItem>>(fluxo, _opcoesJson, ct) ?? [];
    }

    public async Task SalvarAsync(IEnumerable<TarefaItem> tarefas, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(tarefas);
        GarantirDiretorioExiste();

        string caminhoTemp = $"{_caminhoArquivo}.{Guid.NewGuid():N}.tmp";

        await using (FileStream fluxo = new(
            caminhoTemp,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true))
        {
            await JsonSerializer.SerializeAsync(fluxo, tarefas, _opcoesJson, ct);
            await fluxo.FlushAsync(ct);
        }

        File.Move(caminhoTemp, _caminhoArquivo, overwrite: true);
    }