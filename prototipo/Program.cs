List<Usuario> usuarios = new();
List<Carrinho> carrinhos = new();
List<Notebook> notebooks = new();
List<Solicitacao> solicitacoes = new();
List<SolicitacaoNotebook> solicitacaoNotebooks = new();

while (true)
{
    Console.WriteLine("\n========= SISTEMA DE NOTEBOOKS ==========");
    Console.WriteLine("1. Cadastrar usuário");
    Console.WriteLine("2. Cadastrar carrinho");
    Console.WriteLine("3. Cadastrar notebook");
    Console.WriteLine("4. Fazer solicitação");
    Console.WriteLine("5. Aprovar solicitação");
    Console.WriteLine("6. Recusar solicitação");
    Console.WriteLine("7. Devolver notebook");
    Console.WriteLine("8. Listar tudo");
    Console.WriteLine("0. Sair");
    Console.Write("Opção: ");
    string opcao = Console.ReadLine()!;

    if (opcao == "0") break;

    else if (opcao == "1")
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine()!;
        Console.Write("Email: ");
        string email = Console.ReadLine()!;
        Console.Write("Senha: ");
        string senha = Console.ReadLine()!;
        Console.Write("Perfil (Comum/Admin): ");
        string perfilStr = Console.ReadLine()!;

        if (!Enum.TryParse<PerfilUsuario>(perfilStr, true, out var perfil))
        {
            Console.WriteLine("Perfil inválido.");
            continue;
        }

        var usuario = new Usuario
        {
            Id = usuarios.Count + 1,
            Nome = nome,
            Email = email,
            SenhaHash = senha,
            Perfil = perfil,
            Status = StatusCadastro.Ativo
        };
        usuarios.Add(usuario);
        Console.WriteLine($"Usuário '{usuario.Nome}' cadastrado com ID {usuario.Id}.");
    }

    else if (opcao == "2")
    {
        Console.Write("Nome do carrinho: ");
        string nome = Console.ReadLine()!;

        var carrinho = new Carrinho { Id = carrinhos.Count + 1, Nome = nome };
        carrinhos.Add(carrinho);
        Console.WriteLine($"Carrinho '{carrinho.Nome}' cadastrado com ID {carrinho.Id}.");
    }

    else if (opcao == "3")
    {
        if (carrinhos.Count == 0) { Console.WriteLine("Nenhum carrinho cadastrado."); continue; }

        Console.WriteLine("Carrinhos disponíveis:");
        foreach (var c in carrinhos) Console.WriteLine($"  {c.Id} - {c.Nome}");

        Console.Write("Marca: ");
        string marca = Console.ReadLine()!;
        Console.Write("Modelo: ");
        string modelo = Console.ReadLine()!;
        Console.Write("Patrimônio: ");
        string patrimonio = Console.ReadLine()!;
        Console.Write("ID do carrinho: ");
        int carrinhoId = int.Parse(Console.ReadLine()!);

        if (!carrinhos.Exists(c => c.Id == carrinhoId))
        {
            Console.WriteLine("Carrinho não encontrado.");
            continue;
        }

        var notebook = new Notebook
        {
            Id = notebooks.Count + 1,
            Marca = marca,
            Modelo = modelo,
            Patrimonio = patrimonio,
            CarrinhoId = carrinhoId,
            Status = StatusNotebook.Disponivel
        };
        notebooks.Add(notebook);
        Console.WriteLine($"Notebook '{notebook.Marca} {notebook.Modelo}' cadastrado com ID {notebook.Id}.");
    }

    else if (opcao == "4")
    {
        if (usuarios.Count == 0) { Console.WriteLine("Nenhum usuário cadastrado."); continue; }

        var disponiveis = notebooks.FindAll(n => n.Status == StatusNotebook.Disponivel);
        if (disponiveis.Count == 0) { Console.WriteLine("Nenhum notebook disponível."); continue; }

        Console.WriteLine("Usuários:");
        foreach (var u in usuarios) Console.WriteLine($"  {u.Id} - {u.Nome}");
        Console.Write("ID do usuário: ");
        int usuarioId = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Notebooks disponíveis:");
        foreach (var n in disponiveis) Console.WriteLine($"  {n.Id} - {n.Marca} {n.Modelo} (Patrimônio: {n.Patrimonio})");
        Console.Write("ID do notebook: ");
        int notebookId = int.Parse(Console.ReadLine()!);

        Console.Write("Aulas previstas: ");
        int aulas = int.Parse(Console.ReadLine()!);

        if (!usuarios.Exists(u => u.Id == usuarioId) || !disponiveis.Exists(n => n.Id == notebookId))
        {
            Console.WriteLine("Usuário ou notebook inválido.");
            continue;
        }

        var solicitacao = new Solicitacao
        {
            Id = solicitacoes.Count + 1,
            UsuarioId = usuarioId,
            AulasPrevistas = aulas,
            DataHoraSolicitacao = DateTime.Now,
            Status = StatusSolicitacao.Pendente
        };
        solicitacoes.Add(solicitacao);
        solicitacaoNotebooks.Add(new SolicitacaoNotebook { SolicitacaoId = solicitacao.Id, NotebookId = notebookId });
        Console.WriteLine($"Solicitação #{solicitacao.Id} criada.");
    }

    else if (opcao == "5")
    {
        var pendentes = solicitacoes.FindAll(s => s.Status == StatusSolicitacao.Pendente);
        if (pendentes.Count == 0) { Console.WriteLine("Nenhuma solicitação pendente."); continue; }

        Console.WriteLine("Solicitações pendentes:");
        foreach (var s in pendentes)
        {
            var u = usuarios.Find(u => u.Id == s.UsuarioId);
            Console.WriteLine($"  #{s.Id} - Usuário: {u?.Nome} | Aulas: {s.AulasPrevistas}");
        }
        Console.Write("ID da solicitação: ");
        int id = int.Parse(Console.ReadLine()!);

        var sol = solicitacoes.Find(s => s.Id == id);
        if (sol == null) { Console.WriteLine("Solicitação não encontrada."); continue; }

        try
        {
            sol.Aprovar();
            var rel = solicitacaoNotebooks.Find(sn => sn.SolicitacaoId == sol.Id);
            if (rel != null)
            {
                var nb = notebooks.Find(n => n.Id == rel.NotebookId);
                if (nb != null) nb.Status = StatusNotebook.Emprestado;
            }
            Console.WriteLine($"Solicitação #{sol.Id} aprovada. Previsão de devolução: {sol.PrevisaoDevolucao}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    else if (opcao == "6")
    {
        var pendentes = solicitacoes.FindAll(s => s.Status == StatusSolicitacao.Pendente);
        if (pendentes.Count == 0) { Console.WriteLine("Nenhuma solicitação pendente."); continue; }

        Console.WriteLine("Solicitações pendentes:");
        foreach (var s in pendentes)
        {
            var u = usuarios.Find(u => u.Id == s.UsuarioId);
            Console.WriteLine($"  #{s.Id} - Usuário: {u?.Nome}");
        }
        Console.Write("ID da solicitação: ");
        int id = int.Parse(Console.ReadLine()!);

        var sol = solicitacoes.Find(s => s.Id == id);
        if (sol == null) { Console.WriteLine("Solicitação não encontrada."); continue; }

        Console.Write("Motivo da recusa: ");
        string motivo = Console.ReadLine()!;

        try
        {
            sol.Recusar(motivo);
            Console.WriteLine($"Solicitação #{sol.Id} recusada.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    else if (opcao == "7")
    {
        var emUso = solicitacoes.FindAll(s => s.Status == StatusSolicitacao.EmUso);
        if (emUso.Count == 0) { Console.WriteLine("Nenhum notebook em uso."); continue; }

        Console.WriteLine("Notebooks em uso:");
        foreach (var s in emUso)
        {
            var u = usuarios.Find(u => u.Id == s.UsuarioId);
            var rel = solicitacaoNotebooks.Find(sn => sn.SolicitacaoId == s.Id);
            var nb = rel != null ? notebooks.Find(n => n.Id == rel.NotebookId) : null;
            string atrasado = s.EstaAtrasada ? " *** ATRASADO ***" : "";
            Console.WriteLine($"  #{s.Id} - Usuário: {u?.Nome} | Notebook: {nb?.Marca} {nb?.Modelo} | Previsão: {s.PrevisaoDevolucao}{atrasado}");
        }
        Console.Write("ID da solicitação: ");
        int id = int.Parse(Console.ReadLine()!);

        var sol = solicitacoes.Find(s => s.Id == id);
        if (sol == null) { Console.WriteLine("Solicitação não encontrada."); continue; }

        try
        {
            sol.Devolver();
            var rel = solicitacaoNotebooks.Find(sn => sn.SolicitacaoId == sol.Id);
            if (rel != null)
            {
                var nb = notebooks.Find(n => n.Id == rel.NotebookId);
                if (nb != null) nb.Status = StatusNotebook.Disponivel;
            }
            Console.WriteLine($"Notebook devolvido. Devolução registrada em: {sol.DataHoraDevolucao}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    else if (opcao == "8")
    {
        Console.WriteLine("\n--- Usuários ---");
        if (usuarios.Count == 0) Console.WriteLine("  (nenhum)");
        foreach (var u in usuarios)
            Console.WriteLine($"  [{u.Id}] {u.Nome} | {u.Email} | {u.Perfil} | {u.Status}");

        Console.WriteLine("\n--- Carrinhos ---");
        if (carrinhos.Count == 0) Console.WriteLine("  (nenhum)");
        foreach (var c in carrinhos)
            Console.WriteLine($"  [{c.Id}] {c.Nome}");

        Console.WriteLine("\n--- Notebooks ---");
        if (notebooks.Count == 0) Console.WriteLine("  (nenhum)");
        foreach (var n in notebooks)
        {
            var carrinho = carrinhos.Find(c => c.Id == n.CarrinhoId);
            Console.WriteLine($"  [{n.Id}] {n.Marca} {n.Modelo} | Patrimônio: {n.Patrimonio} | {n.Status} | Carrinho: {carrinho?.Nome}");
        }

        Console.WriteLine("\n--- Solicitações ---");
        if (solicitacoes.Count == 0) Console.WriteLine("  (nenhuma)");
        foreach (var s in solicitacoes)
        {
            var u = usuarios.Find(u => u.Id == s.UsuarioId);
            var rel = solicitacaoNotebooks.Find(sn => sn.SolicitacaoId == s.Id);
            var nb = rel != null ? notebooks.Find(n => n.Id == rel.NotebookId) : null;
            string atrasado = s.EstaAtrasada ? " *** ATRASADO ***" : "";
            Console.WriteLine($"  [#{s.Id}] Usuário: {u?.Nome} | Notebook: {nb?.Marca} {nb?.Modelo} | Status: {s.Status} | Aulas: {s.AulasPrevistas}{atrasado}");
            if (s.MotivoRecusa != null) Console.WriteLine($"         Motivo recusa: {s.MotivoRecusa}");
            if (s.PrevisaoDevolucao != null) Console.WriteLine($"         Previsão devolução: {s.PrevisaoDevolucao}");
            if (s.DataHoraDevolucao != null) Console.WriteLine($"         Devolvido em: {s.DataHoraDevolucao}");
        }
    }

    else
    {
        Console.WriteLine("Opção inválida.");
    }
}
