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
    public string nomeUsuario {get; set;}
    public string emailUsuario {get; set;}
    public string senhaHash {get; set;}

    public PerfilUsuario perfilUsuario {get; set;}

    public StatusCadastro statusCadastro {get; set;}
}