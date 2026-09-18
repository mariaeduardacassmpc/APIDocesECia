namespace ApiDoces.Helpers;

public static class ApiMessages
{
    public const string RequiredField = "Campo obrigatório.";
    public const string InvalidData = "Dados inválidos.";
    public const string UnexpectedError = "Ocorreu um erro inesperado.";
    public const string OperationNotAllowed = "Operação não permitida.";

    public static string NotFound(string entity)
        => $"{entity} não encontrado.";

    public static string Created(string entity)
        => $"{entity} criado com sucesso.";

    public static string Updated(string entity)
        => $"{entity} atualizado com sucesso.";

    public static string Deleted(string entity)
        => $"{entity} excluído com sucesso.";

    public static string AlreadyExists(string entity)
        => $"{entity} já cadastrado.";

    public static class Email
    {
        public const string Invalid = "O e-mail informado não é válido.";
        public const string MaxLength = "O e-mail deve ter no máximo 100 caracteres.";
        public const string AlreadyExists = "Já existe um cadastro com esse e-mail.";
    }

    public static class Password
    {
        public const string MinLength = "Senha deve ter no mínimo 8 caracteres.";
        public const string MaxLength = "Senha deve ter no máximo 50 caracteres.";
        public const string Uppercase = "Senha deve conter pelo menos uma letra maiúscula.";
        public const string Lowercase = "Senha deve conter pelo menos uma letra minúscula.";
        public const string Number = "Senha deve conter pelo menos um número.";
        public const string SpecialCharacter = "Senha deve conter pelo menos um caractere especial.";
    }

    public static class Customer
    {
        public const string NameMaxLength = "O nome deve ter no máximo 150 caracteres.";
        public const string PhoneInvalid = "O telefone informado não é válido.";
        public const string AddressMaxLength = "O endereço deve ter no máximo 200 caracteres.";
        public const string CityMaxLength = "A cidade deve ter no máximo 100 caracteres.";
        public const string ObsMaxLength = "As observações devem ter no máximo 500 caracteres.";
    }

    public static class Product
    {
        public const string NameMaxLength = "O nome deve ter no máximo 150 caracteres.";
        public const string DescriptionMaxLength = "A descrição deve ter no máximo 500 caracteres.";
        public const string PurchasePriceNotNegative = "O preço de compra não pode ser negativo.";
        public const string SalePriceGreaterThanZero = "O preço de venda deve ser maior que zero.";
        public const string SalePriceNotLessThanPurchase = "O preço de venda não pode ser menor que o preço de compra.";
        public const string StockNotNegative = "O estoque não pode ser negativo.";
        public const string ImageUrlInvalid = "A URL da imagem informada não é válida.";
    }

    public static class Category
    {
        public const string NameMaxLength = "O nome da categoria deve ter no máximo 100 caracteres.";
    }

    public static class Expense
    {
        public const string DescriptionMaxLength = "A descrição deve ter no máximo 200 caracteres.";
        public const string ValueGreaterThanZero = "O valor deve ser maior que zero.";
        public const string DateNotFuture = "A data não pode ser futura.";
    }

    public static class Sale
    {
        public const string PaymentMethodMaxLength = "Forma de pagamento deve ter no máximo 50 caracteres.";
        public const string ItemsRequired = "A venda deve possuir pelo menos um item.";
        public const string QuantityGreaterThanZero = "A quantidade deve ser maior que zero.";
        public const string UnitPriceGreaterThanZero = "O preço unitário deve ser maior que zero.";
    }

    public static class Auth
    {
        public const string InvalidCredentials = "E-mail ou senha inválidos.";
        public const string Unauthorized = "Não autorizado.";
        public const string Forbidden = "Acesso negado.";
    }
}