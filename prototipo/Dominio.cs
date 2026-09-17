enum PerfilUsuario
{
    Comum,
    Admin
}

enum StatusCadastro
{
    PendenteAprovacao,
    Ativo,
    Recusado
}

enum StatusNotebook
{
    Disponivel,
    Emprestado,
    Manutencao
}

enum StatusSolicitacao
{
    Pendente,
    Recusada, 
    EmUso,
    Devolvida
}

class Usuario
{
    public int Id {get; set;}
    public required string Nome {get; set;}
    public required string Email {get; set;}
    public required string SenhaHash {get; set;}

    public PerfilUsuario Perfil {get; set;}

    public StatusCadastro Status { get; set; }
}

class Carrinho
{
    public int Id {get; set;}
    public required string Nome { get; set; }
}

class Notebook
{
    public int Id { get; set; }
    public required string Marca { get; set; }
    public required string Modelo { get; set; }
    public required string Patrimonio { get; set; }
    public StatusNotebook Status { get; set; }
    public int CarrinhoId {get; set;}
}

class Solicitacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataHoraSolicitacao { get; set; }
    public int AulasPrevistas { get; set; }
    public StatusSolicitacao Status { get; set; }
    public string? MotivoRecusa { get; set; }
    public DateTime? DataHoraRetirada { get; set; }
    public DateTime? DataHoraDevolucao { get; set; }
    public DateTime? PrevisaoDevolucao => DataHoraRetirada == null ? null : DataHoraRetirada.Value.AddMinutes(AulasPrevistas * 50);
    public bool EstaAtrasada => PrevisaoDevolucao != null && Status == StatusSolicitacao.EmUso && DateTime.Now > PrevisaoDevolucao.Value;

    public void Aprovar()
    {
        if (Status == StatusSolicitacao.Pendente)
        {
            Status = StatusSolicitacao.EmUso;
            DataHoraRetirada = DateTime.Now;
        }
        else
        {
            throw new InvalidOperationException("A transção não pode ser concluída, por que o computador já está em uso");
        }
    }

    public void Devolver()
    {
        if (Status == StatusSolicitacao.EmUso)
        {
            Status = StatusSolicitacao.Devolvida;
            DataHoraDevolucao = DateTime.Now;
        }
        else
        {
            throw new InvalidOperationException("O computador está em uso, não é possível devolver a máquina");
        }
    }

    public void Recusar(string motivo)
    {
        if (Status == StatusSolicitacao.Pendente)
        {
            Status = StatusSolicitacao.Recusada;
            MotivoRecusa = motivo;
        }
        else
        {
            throw new InvalidOperationException("A solicitação não pode ser recusada, pois já foi aprovada ou recusada.");
        }
    }
}
    class SolicitacaoNotebook
    {
        public int SolicitacaoId { get; set; }
        public int NotebookId { get; set; }
    }



