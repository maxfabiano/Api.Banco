using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public record AssignRoleCommand(int ContaId, int RoleId) : IRequest<bool>;

    public class AssignRoleHandler : IRequestHandler<AssignRoleCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public AssignRoleHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(AssignRoleCommand request, CancellationToken ct)
        {
            var mapping = new ContaCorrenteRole
            {
                IdContaCorrente = request.ContaId,
                IdRole = request.RoleId
            };

            _context.Set<ContaCorrenteRole>().Add(mapping);
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
