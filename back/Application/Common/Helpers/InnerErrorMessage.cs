namespace Application.Helpers;

public class InnerErrorMessage
{
    public static string ObterMensagemErro(Exception ex)
    {
        if (ex.InnerException is Npgsql.PostgresException pg)
            return $"{pg.ConstraintName}: {pg.MessageText}";

        return ex.InnerException?.Message ?? ex.Message;
    }
}