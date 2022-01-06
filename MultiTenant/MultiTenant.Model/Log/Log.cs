namespace MultiTenant.Model;

public class Log
{
    public long Id { get; set; }

    public DateTime DataHora { get; set; }
    
    public string Schema { get; set; }

    public string Acontecimento { get; set; }

    public string Mensagem { get; set; }

    public string StackTrace { get; set; }
    
}