using FluentValidation;

public class OrderValidator : AbstractValidator<CreateOrderRequest>
{
    public OrderValidator()
    {
        RuleFor(CreateOrderRequest => CreateOrderRequest.CustomerId).NotEmpty().WithMessage("O ID do cliente não pode ser vazio.")
                                          .Length(5, 50).WithMessage("O ID do cliente deve ter entre 5 e 50 caracteres.");

        RuleFor(CreateOrderRequest => CreateOrderRequest.Currency).NotEmpty().WithMessage("Currency não pode ser vazio.")
                                          .Length(3).WithMessage("Currency deve conter 3 caracteres.");

        RuleFor(CreateOrderRequest => CreateOrderRequest.Items).NotNull().WithMessage("A lista de itens não pode ser nula.")
                                     .NotEmpty().WithMessage("A lista de itens não pode ser vazia.");

        RuleForEach(CreateOrderRequest => CreateOrderRequest.Items).SetValidator(new OrderItemValidator());

    }
}