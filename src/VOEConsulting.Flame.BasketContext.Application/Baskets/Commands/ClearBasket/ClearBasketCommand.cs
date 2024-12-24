using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.ClearBasket
{
    public record ClearBasketCommand(Guid BasketId) : IRequest;

}
